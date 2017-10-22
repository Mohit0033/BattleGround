using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.UI;

public class NPCControl : MonoBehaviour
{
    public List<Transform> enemies = new List<Transform>(3);
    public float sightDistance = 30f;
    public float sightAngle = 60f;
    public float rememberTime = 10f;
    public float fallBackTime = 8f;
    public Image image;

    private UserSoldier player;
    private Soldier shooter;

    private BlackBoard blackBoard = new BlackBoard();
    private const string safeZoneKey = "safeZone";
    private const string enemyPosKey = "enemyPos";
    private const string coverPosKey = "coverPos";
    private const string fallBackPosKey = "fallBackPos";
    private const string shooterPosKey = "shooterPos";
    private const string weaponPosKey = "weaponPos";
    private const string weaponKey = "weapon";

    private NPCSoldier npc;
    private GameManager gameManager;
    private float outOfSafeZoneTimer;
    private float nextRadius;
    private Vector3 nextPos;
    private float rememberTimer;
    private float fallBackTimer;
    private bool isArrivedSafeZone = true;

    private SelectionNode root = new SelectionNode();
    private SelectionNode haveEnemy, beingShoot, idle;
    private SequenceNode toSafeZone, stayInCover, seekWeapon;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<UserSoldier>();

        gameManager = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameManager>();
        npc = GetComponent<NPCSoldier>();
        npc.Damaged += Damaged;
        nextRadius = gameManager.nextRadius;
        SetCoverPos(transform.position);

        var notInSafeZoneCond = new ConditionNode(IsInSafeZone, false);
        var haveEnemyCond = new ConditionNode(HaveEnemyInSight, true);
        var beingShootCond = new ConditionNode(BeingShoot, true);
        var haveWeaponCond = new ConditionNode(HaveWeapon, true);
        var seekWeaponCond = new ConditionNode(SeekWeapon, true);

        var equipAction = new SetEquipmentAction(npc, true);
        var notEquipAction = new SetEquipmentAction(npc, false);
        var crouchAction = new SetCrouchAction(npc, true);
        var notCrouchAction = new SetCrouchAction(npc, false);
        var toSafeZoneAction = new MoveAction(npc, blackBoard, safeZoneKey);
        var goCoverAction = new MoveAction(npc, blackBoard, coverPosKey);
        var FallBackAction = new MoveAction(npc, blackBoard, fallBackPosKey);
        var goWeaponAction = new MoveAction(npc, blackBoard, weaponPosKey);
        var pickUpWeaponAction = new PickUpWeaponAction(npc, blackBoard, weaponKey);
        var shootAction = new ShootAction(npc, blackBoard, enemyPosKey);
        var turnToShooterAction = new TurnAction(npc, blackBoard, shooterPosKey);
        var idleAction = new IdleAction(npc);

        var combat = new SequenceNode();
        combat.AddCondition(haveWeaponCond);
        combat.AddNode(equipAction);
        combat.AddNode(shootAction);

        var fallBack = new SequenceNode();
        fallBack.AddNode(FallBackAction);

        haveEnemy = new SelectionNode();
        haveEnemy.AddCondition(haveEnemyCond);
        haveEnemy.AddNode(combat);
        haveEnemy.AddNode(fallBack);

        var counter = new SequenceNode();
        counter.AddCondition(haveWeaponCond);
        counter.AddNode(turnToShooterAction);

        beingShoot = new SelectionNode();
        beingShoot.AddCondition(beingShootCond);
        beingShoot.AddNode(counter);
        beingShoot.AddNode(fallBack);

        toSafeZone = new SequenceNode();
        toSafeZone.AddCondition(notInSafeZoneCond);
        toSafeZone.AddNode(notCrouchAction);
        toSafeZone.AddNode(notEquipAction);
        toSafeZone.AddNode(toSafeZoneAction);

        stayInCover = new SequenceNode();
        stayInCover.AddCondition(haveWeaponCond);
        stayInCover.AddNode(goCoverAction);
        stayInCover.AddNode(crouchAction);
        stayInCover.AddNode(idleAction);

        seekWeapon = new SequenceNode();
        seekWeapon.AddCondition(seekWeaponCond);
        seekWeapon.AddNode(goWeaponAction);
        seekWeapon.AddNode(pickUpWeaponAction);

        idle = new SelectionNode();
        idle.AddNode(stayInCover);
        idle.AddNode(seekWeapon);

        root.AddNode(haveEnemy);
        root.AddNode(beingShoot);
        root.AddNode(toSafeZone);
        root.AddNode(idle);
    }

    private void Damaged(Soldier shooter)
    {
        blackBoard.SetValue(shooterPosKey, shooter.transform.position);
        this.shooter = shooter;
        rememberTimer = rememberTime;
        if (!npc.isHaveWeapon)
        {
            SetFallBackPos(shooter.transform.position);
        }
    }

    private void FixedUpdate()
    {
        if (npc.isDead)
        {
            return;
        }

        if (Vector3.Distance(transform.position, gameManager.safeZoneCircle.position) > gameManager.currRadius)
        {
            if (outOfSafeZoneTimer > 3f)
            {
                npc.TakeDamage(gameManager.notSafeDamage);
                outOfSafeZoneTimer = 0;
            }
            else
            {
                outOfSafeZoneTimer += Time.deltaTime;
            }
        }

        root.CheckCondition();
        root.Tick();

        if (root.currNode == haveEnemy)
        {
            image.color = Color.red;
        }
        else if (root.currNode == toSafeZone)
        {
            image.color = Color.yellow;
        }
        else if (idle.currNode == stayInCover)
        {
            image.color = Color.green;
        }
        else if (idle.currNode == seekWeapon)
        {
            image.color = Color.blue;
        }
        else if (root.currNode == beingShoot)
        {
            image.color = Color.black;
        }
    }

    private bool HaveEnemyInSight()
    {
        var allNPC = gameManager.allNPC;
        Vector3 dir;
        RaycastHit hit;
        for (int i = 0; i < allNPC.Length; i++)
        {
            if (allNPC[i].isDead) continue;
            if (allNPC[i].transform == transform) continue;
            var enemyPos = allNPC[i].transform.position;
            dir = enemyPos - transform.position;
            if (Vector3.Distance(transform.position, enemyPos) < sightDistance &&
                Vector3.Angle(transform.forward, dir) < sightAngle / 2 &&
                Physics.Raycast(transform.position + Vector3.up, dir, out hit, sightDistance))
            {
                if (hit.collider.gameObject.tag == Tags.NPC)
                {
                    blackBoard.SetValue(enemyPosKey, enemyPos);
                    if (!npc.isHaveWeapon)
                    {
                        SetFallBackPos(enemyPos);
                    }
                    return true;
                }
            }
        }
        if (!player.isDead)
        {
            dir = player.transform.position - transform.position;
            if (Vector3.Distance(transform.position, player.transform.position) < sightDistance &&
                Vector3.Angle(transform.forward, dir) < sightAngle / 2 &&
                Physics.Raycast(transform.position + Vector3.up, dir, out hit, sightDistance))
            {
                if (hit.collider.gameObject.tag == Tags.Player)
                {
                    blackBoard.SetValue(enemyPosKey, player.transform.position);
                    if (!npc.isHaveWeapon)
                    {
                        SetFallBackPos(player.transform.position);
                    }
                    return true;
                }
            }
        }
        if (fallBackTimer > 0)
        {
            fallBackTimer -= Time.fixedDeltaTime;
            return true;
        }


        return false;
    }

    private void SetFallBackPos(Vector3 enemyPos)
    {
        var dir = enemyPos - transform.position;
        var length = dir.magnitude;
        dir = Quaternion.Euler(0, 90, 0) * dir.normalized * (sightDistance + 1 - length);
        var fallBackPos = transform.position + dir;
        blackBoard.SetValue(fallBackPosKey, fallBackPos);
        fallBackTimer = fallBackTime * (sightDistance - length) / sightDistance;
    }

    private bool IsInSafeZone()
    {
        if (nextRadius != gameManager.nextRadius)
        {
            nextRadius = gameManager.nextRadius;
            if (Vector3.Distance(transform.position, gameManager.nextSafeZone) > gameManager.nextRadius)
            {
                var random = Random.insideUnitCircle;
                var safeZone = gameManager.nextSafeZone + new Vector3(gameManager.nextRadius * random.x, 0f, gameManager.nextRadius * random.y);
                blackBoard.SetValue(safeZoneKey, safeZone);
                isArrivedSafeZone = false;
                nextPos = safeZone;
                SetCoverPos(safeZone);
                return false;
            }
        }
        else
        {
            if (isArrivedSafeZone)
            {
                return true;
            }
            if (Vector3.Distance(transform.position, nextPos) > 0.2)
            {
                return false;
            }
            else
            {
                isArrivedSafeZone = true;
            }
        }
        return true;
    }

    private void SetCoverPos(Vector3 pos)
    {
        var distance = float.MaxValue;
        var coverPos = Vector3.zero;
        var allPos = gameManager.allCoverPos;
        for (int i = 0; i < allPos.Length; i++)
        {
            if (Vector3.Distance(allPos[i], gameManager.nextSafeZone) > gameManager.nextRadius)
            {
                continue;
            }

            var tempDistance = Vector3.Distance(pos, allPos[i]);
            if (tempDistance < distance)
            {
                distance = tempDistance;
                coverPos = allPos[i];
            }
        }
        blackBoard.SetValue(coverPosKey, coverPos);

    }

    private bool BeingShoot()
    {
        if (rememberTimer > 0 && !shooter.isDead)
        {
            rememberTimer -= Time.fixedDeltaTime;
            return true;
        }

        if (fallBackTimer > 0)
        {
            fallBackTimer -= Time.fixedDeltaTime;
            return true;
        }
        return false;
    }

    private bool HaveWeapon()
    {
        return npc.isHaveWeapon;
    }

    private bool SeekWeapon()
    {
        var allWeapon = gameManager.allWeapon;
        Transform weaponPos = null;
        float distance = 100000;
        for (int i = 0; i < allWeapon.Length; i++)
        {
            if (!allWeapon[i].gameObject.activeSelf) continue;
            var dis = Vector3.Distance(transform.position, allWeapon[i].position);
            if (dis < distance)
            {
                weaponPos = allWeapon[i];
                distance = dis;
            }
        }
        blackBoard.SetValue(weaponPosKey, weaponPos.position);
        blackBoard.SetValue(weaponKey, weaponPos.gameObject);
        return true;
    }
}
