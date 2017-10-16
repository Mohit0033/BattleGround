using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float maxTime;
    public GameObject npc;
    public int npcMaxNum = 10;
    public float[] safeZoneRadiuses;
    public float safeZoneMoveSpeed = 4f;
    public float notSafeDamage = 5f;
    public float mapWidth;
    public Transform safeZoneCircle;

    [HideInInspector]
    public float currRadius;
    [HideInInspector]
    public float nextRadius;
    [HideInInspector]
    public Vector3 nextSafeZone;
    [HideInInspector]
    public NPCSoldier[] allNPC;
    [HideInInspector]
    public Vector3[] allCoverPos;

    private int nextRadiusIndex;
    private float changeTimer;
    private bool isChangingCircle;
    private float distance;
    private float radiusDistance;
    private bool isLastRadius;


    private void Awake()
    {
        GetEnvironmentPostion();
        GenerateNPC();

        safeZoneCircle.localScale = new Vector3(mapWidth * 1.5f, 100f, mapWidth * 1.5f);
        currRadius = nextRadius = safeZoneCircle.localScale.x / 2;

    }

    private void GetEnvironmentPostion()
    {
        var fenceObjects = GameObject.FindGameObjectsWithTag(Tags.Fence);
        var treeObjects = GameObject.FindGameObjectsWithTag(Tags.Tree);
        allCoverPos = new Vector3[fenceObjects.Length * 2 + treeObjects.Length * 4];

        int index = 0;
        Vector3 fenceScale = new Vector3(1.35f, 0, 1.35f);

        for (int i = 0; i < fenceObjects.Length; i++)
        {
            var trans = fenceObjects[i].transform;
            var center = trans.position - Vector3.Scale(trans.right, fenceScale);
            allCoverPos[index++] = center - trans.forward;
            allCoverPos[index++] = center + trans.forward;
        }

        for (int i = 0; i < treeObjects.Length; i++)
        {
            var trans = treeObjects[i].transform;
            allCoverPos[index++] = trans.position - trans.forward;
            allCoverPos[index++] = trans.position + trans.forward;
            allCoverPos[index++] = trans.position - trans.right;
            allCoverPos[index++] = trans.position + trans.right;
        }


    }
    
    private void GenerateNPC()
    {
        allNPC = new NPCSoldier[npcMaxNum];
        var range = mapWidth / 2;
        for (int i = 0; i < npcMaxNum; i++)
        {
            var pos = new Vector3(Random.Range(-range, range), 0f, Random.Range(-range, range));
            allNPC[i] = Instantiate(npc, pos, Random.rotation).GetComponent<NPCSoldier>();
        }
    }

    private void Update()
    {
        if (isLastRadius)
        {
            return;
        }

        if (isChangingCircle)
        {
            if (radiusDistance == 0)//first round
            {
                SetNextSafeZone();
                isChangingCircle = false;
                return;
            }
            if (!ChangeCircle())
            {
                if (!isLastRadius)
                {
                    isChangingCircle = false;
                    SetNextSafeZone();
                }

            }
            return;
        }

        if (changeTimer >= maxTime)
        {
            isChangingCircle = true;
            changeTimer = 0;
        }
        else
        {
            changeTimer += Time.deltaTime;
        }


    }

    private bool ChangeCircle()
    {
        safeZoneCircle.position = Vector3.MoveTowards(safeZoneCircle.position, nextSafeZone, safeZoneMoveSpeed * Time.deltaTime);
        var dis = Vector3.Distance(safeZoneCircle.position, nextSafeZone);
        var scale = dis / distance;
        currRadius = nextRadius + radiusDistance * scale;
        safeZoneCircle.localScale = new Vector3(currRadius * 2, 100f, currRadius * 2);

        if (dis < 0.1f)
        {
            if (nextRadiusIndex >= safeZoneRadiuses.Length)
            {
                isLastRadius = true;
            }
            return false;
        }

        return true;
    }

    private void SetNextSafeZone()
    {
        float rangeRadius;
        nextRadius = safeZoneRadiuses[nextRadiusIndex];
        if (nextRadiusIndex == 0)
        {
            rangeRadius = mapWidth / 2 - nextRadius - 1;
        }
        else
        {
            rangeRadius = currRadius - nextRadius - 1;
        }

        var random = Random.insideUnitCircle;
        nextSafeZone = new Vector3(random.x * rangeRadius, 0f, random.y * rangeRadius);
        //nextSafeZoneCircle.position = nextSafeZone;
        //nextSafeZoneCircle.localScale = new Vector3(nextRadius * 2, 1f, nextRadius * 2);
        distance = Vector3.Distance(safeZoneCircle.position, nextSafeZone);
        radiusDistance = currRadius - nextRadius;
        nextRadiusIndex++;

    }

    public void Resart()
    {
        SceneManager.LoadSceneAsync("Main");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
