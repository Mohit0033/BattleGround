using UnityEditor;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public static GameObject fence;
    public static GameObject tree;
    public static GameObject rock;
    public static int EnviromentObjectCount = 100;
    

    [MenuItem("GameObject/Environment")]
    public static void GenerateEnviroment()
    {
        fence = Resources.Load("Fence", typeof(GameObject)) as GameObject;
        tree = Resources.Load("Tree", typeof(GameObject)) as GameObject;
        rock = Resources.Load("Rock", typeof(GameObject)) as GameObject;
        
        var range = 248 / 2;
        for (int i = 0; i < EnviromentObjectCount; i++)
        {
            var pos = new Vector3(Random.Range(-range, range), 0f, Random.Range(-range, range));
            var rot = Quaternion.Euler(0, Random.Range(0, 360), 0);
            Instantiate(tree, pos, rot).transform.localScale *= Random.Range(0.3f, 1);

            pos = new Vector3(Random.Range(-range, range), 0f, Random.Range(-range, range));
            rot = Quaternion.Euler(0, Random.Range(0, 360), 0);
            Instantiate(rock, pos, rot);

            pos = new Vector3(Random.Range(-range, range), 0f, Random.Range(-range, range));
            rot = Quaternion.Euler(0, Random.Range(0, 360), 0);
            Instantiate(fence, pos, rot);
            pos = new Vector3(Random.Range(-range, range), 0f, Random.Range(-range, range));
            rot = Quaternion.Euler(0, Random.Range(0, 360), 0);
            Instantiate(fence, pos, rot);
        }
    }
}
