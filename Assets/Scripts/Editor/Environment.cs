using UnityEditor;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public static GameObject fence;
    public static GameObject tree;
    public static GameObject rock;
    public static int EnviromentObjectCount = 100;
    public static GameObject weaponPosition;
    public static int WeaponPositionCount = 100;

    [MenuItem("GameObject/Environment")]
    public static void GenerateEnviroment()
    {
        fence = Resources.Load<GameObject>("Fence");
        tree = Resources.Load<GameObject>("Tree");
        rock = Resources.Load<GameObject>("Rock");
        
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

    [MenuItem("GameObject/Weapon")]
    public static void GenerateWeapon()
    {
        weaponPosition = Resources.Load<GameObject>("WeaponPosition");

        var range = 248 / 2;
        for (int i = 0; i < WeaponPositionCount; i++)
        {
            var pos = new Vector3(Random.Range(-range, range), 0.1f, Random.Range(-range, range));
            var rot = Quaternion.Euler(0, Random.Range(0, 360), 0);
            Instantiate(weaponPosition, pos, rot);
            
        }
    }
}
