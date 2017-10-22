using System.Collections;
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
    [HideInInspector]
    public bool isHaveWeapon = false;
    [HideInInspector]
    public bool isEquiped = false;

    protected Animator anim;
    protected AudioSource audioSource;
    protected GameObject currGun;
    protected GunShoot gunShoot;
    protected bool isSettingEquip = false;
    protected WaitForSeconds waitGrab = new WaitForSeconds(0.6f);

    private Coroutine currCoroutine;

    public virtual void Turn(float angle)
    {
        transform.Rotate(Vector3.up, angle, Space.World);
    }

    public virtual void PickUpWeapon(GameObject weapon)
    {
        if (weapon.name == "m4a1(Clone)")
        {
            currGun = guns[0];
            guns[1].SetActive(false);
        }
        else if (weapon.name == "ump45(Clone)")
        {
            currGun = guns[1];
            guns[0].SetActive(false);
        }
        currGun.SetActive(true);
        gunShoot = currGun.GetComponent<GunShoot>();
        isHaveWeapon = true;
        weapon.SetActive(false);
    }

    public void SetEquipment(bool equip)
    {
        if (!isHaveWeapon)
        {
            return;
        }
        if (isEquiped==equip)
        {
            if (isSettingEquip)
            {
                StopCoroutine(currCoroutine);
            }
            return;
        }
        if (isCrouch && !equip)    //have not animation about crouch with not equip
        {
            return;
        }
        if (isSettingEquip)
        {
            return;
        }
        anim.SetTrigger(HashIDs.grabGunTrigger);
        currCoroutine = StartCoroutine(GrabGun(equip));
    }

    private IEnumerator GrabGun(bool equip)
    {
        isSettingEquip = true;
        yield return waitGrab;
        currGun.SetActive(equip);
        isEquiped = equip;
        anim.SetBool(HashIDs.notEquipedBool, !equip);
        isSettingEquip = false;
    }

    /// <summary>
    /// fire bullet
    /// </summary>
    /// <param name="position">if this soldier is user,this is the aim position. if is npc, this is the position of npc being shot</param>
    public virtual void Fire(Vector3 position)
    {
        if (isEquiped)
        {
            if (gunShoot.FireBullet(position, this) && !isCrouch)
            {
                anim.SetTrigger(HashIDs.shootTrigger);
            }
        }
    }
}
