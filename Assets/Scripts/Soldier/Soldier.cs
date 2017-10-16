using UnityEngine;

public abstract class Soldier : MonoBehaviour
{
    public float speed = 4f;
    public float crouchSpeed = 2f;
    public float notEquipedRatio = 1.2f;
    public float turnSpeed = 60f;
    public float gravity = 20f;
    public float health = 100f;
    public AudioSource getHurtAudioSource;
    public GameObject[] guns;

    [HideInInspector]
    public bool isDead = false;
    [HideInInspector]
    public bool isCrouch = false;

    protected Animator anim;
    protected AudioSource audioSource;
    protected GameObject currGun;
    protected GunShoot gunShoot;
    protected bool isEquiped = true;
    protected WaitForSeconds waitGrab = new WaitForSeconds(0.6f);


}
