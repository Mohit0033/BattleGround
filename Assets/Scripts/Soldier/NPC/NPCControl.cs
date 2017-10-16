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
    public Image image;

    private UserSoldier player;
    private Soldier shooter;

    private BlackBoard blackBoard = new BlackBoard();
    private const string safeZoneKey = "safeZone";
    private const string enemyPosKey = "enemyPos";
    private const string coverPosKey = "coverPos";
    private const string shooterPosKey = "shooterPos";

    private NPCSoldier npc;
    private GameManager gameManager;
    private float outOfSafeZoneTimer;
    private float nextRadius;
    private Vector3 nextPos;
    private float rememberTimer;

    private SelectionNode root = new SelectionNode();
    private SequenceNode combat, toSafeZone, idle, counter;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<UserSoldier>();

        gameManager = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameManager>();
        npc = GetComponent<NPCSoldier>();
        npc.Damaged += Damaged;
        nextRadius = gameManager.nextRadius;
        nextPos = SetCoverPos(transform.position);

        var notInSafeZoneCond = new ConditionNode(IsInSafeZone, false);
        var haveEnemyCond = new ConditionNode(HaveEnemyInSight, true);
        var beingShootCond = new ConditionNode(BeingShoot, true);

        var equipAction = new SetEquipmentAction(npc, true);
        var notEquipAction = new SetEquipmentAction(npc, false);
        var crouchAction = new SetCrouchAction(npc, true);
        var notCrouchAction = new SetCrouchAction(npc, false);
        var toSafeZoneAction = new MoveAction(npc, blackBoard, safeZoneKey);
        var goCoverAction = new MoveAction(npc, blackBoard, coverPosKey);
        var shootAction = new ShootAction(npc, blackBoard, enemyPosKey);
        var turnToShooterAction = new TurnAction(npc, blackBoard, shooterPosKey);
        var idleAction = new IdleAction(npc);

        counter = new SequenceNode();
        counter.AddCondition(beingShootCond);
        counter.AddNode(turnToShooterAction);

        combat = new SequenceNode();
        combat.AddCondition(haveEnemyCond);
        combat.AddNode(equipAction);
        combat.AddNode(shootAction);

        toSafeZone = new SequenceNode();
        toSafeZone.AddCondition(notInSafeZoneCond);
        toSafeZone.AddNode(notCrouchAction);
        toSafeZone.AddNode(notEquipAction);
        toSafeZone.AddNode(toSafeZoneAction);
        toSafeZone.AddNode(goCoverAction);

        idle = new SequenceNode();
        idle.AddNode(crouchAction);
        idle.AddNode(idleAction);

        root.AddNode(combat);
        root.AddNode(counter);
        root.AddNode(toSafeZone);
        root.AddNode(idle);
    }

    private void Damaged(Soldier shooter)
    {
        blackBoard.SetValue(shooterPosKey, shooter.transform.position);
        this.shooter = shooter;
        rememberTimer = rememberTime;
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
                npc.TakeDamage(gameManager.notSafeDamage, npc);
                outOfSafeZoneTimer = 0;
            }
            else
            {
                outOfSafeZoneTimer += Time.deltaTime;
            }
        }

        root.CheckCondition();
        root.Tick();

        if (root.currNode == combat)
        {
            image.color = Color.red;
        }
        else if (root.currNode == toSafeZone)
        {
            image.color = Color.yellow;
        }
        else if (root.currNode == idle)
        {
            image.color = Color.green;
        }
        else if (root.currNode == counter)
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
            dir = allNPC[i].transform.position - transform.position;
            if (Vector3.Distance(transform.position, allNPC[i].transform.position) < sightDistance &&
                Vector3.Angle(transform.forward, dir) < sightAngle / 2 &&
                Physics.Raycast(transform.position + Vector3.up, dir, out hit, sightDistance))
            {
                if (hit.collider.gameObject.tag == Tags.NPC)
                {
                    blackBoard.SetValue(enemyPosKey, allNPC[i].transform.position);
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
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsInSafeZone()
    {
        if (nextRadius != gameManager.nextRadius)
        {
            nextRadius = gameManager.nextRadius;
            if (Vector3.Distance(transform.position, gameManager.nextSafeZone) > gameManager.nextRadius)
            {
                var random = Random.insideUnitCircle;
                var pos = gameManager.nextSafeZone + new Vector3(gameManager.nextRadius * random.x, 0f, gameManager.nextRadius * random.y);
                blackBoard.SetValue(safeZoneKey, pos);
                nextPos = SetCoverPos(pos);
                return false;
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, nextPos) > 0.2)
            {
                return false;
            }
        }
        npc.Stop();
        return true;
    }

    private Vector3 SetCoverPos(Vector3 pos)
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
        return coverPos;

    }

    private bool BeingShoot()
    {
        if (rememberTimer > 0 && !shooter.isDead)
        {
            rememberTimer -= Time.fixedDeltaTime;
            return true;
        }

        return false;
    }
}
