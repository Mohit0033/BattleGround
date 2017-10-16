using UnityEngine;

public class GunShoot : MonoBehaviour
{
    public float maxBulletDistance = 300f;
    public float bulletDamage = 23f;
    public float fireColdTime = 0.9f;
    public AudioSource fireAudioSource;
    public AudioClip audioClip;

    private float fireTimer;
    private Camera mainCamera;

    private void OnEnable()
    {
        fireAudioSource.clip = audioClip;
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        fireTimer -= Time.deltaTime;
        if (fireTimer < 0)
        {
            fireTimer = 0;
        }
    }

    public bool FireBullet(Soldier soldier)//for player
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        return DoFireBullet(ray, soldier);
    }

    public bool FireBullet(Vector3 position, Soldier soldier)//for npc
    {
        var dir = position - transform.position;
        var ray = new Ray(transform.position, dir);
        return DoFireBullet(ray, soldier);
    }

    private bool DoFireBullet(Ray ray, Soldier soldier)
    {
        if (fireTimer > 0)
        {
            return false;
        }

        fireTimer = fireColdTime;
        if (fireAudioSource.isPlaying)
        {
            fireAudioSource.Stop();
        }
        fireAudioSource.Play();

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxBulletDistance))
        {
            var obj = hit.collider.gameObject;
            if (obj.tag == Tags.NPC)
            {
                var npc = obj.GetComponent<NPCSoldier>();
                npc.TakeDamage(bulletDamage, soldier);
            }
            else if (obj.tag == Tags.Player)
            {
                var player = obj.GetComponent<UserSoldier>();
                player.TakeDamage(bulletDamage);
            }

        }

        return true;
    }
}
