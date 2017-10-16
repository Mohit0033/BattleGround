using System.Collections;
using UnityEngine;

public class UserSoldier : Soldier
{



    private CharacterController controller;
    private bool isSettingEquip = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        currGun = guns[0];
        currGun.SetActive(true);
        gunShoot = currGun.GetComponent<GunShoot>();
    }

    public void Move(float h, float v)
    {
        var moveDirection = new Vector3(h, 0, v);
        moveDirection = transform.TransformDirection(moveDirection);
        var length = moveDirection.magnitude;
        anim.SetFloat(HashIDs.speedFloat, length);
        if (length > 0)
        {
            if (!isCrouch && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }

        float moveSpeed = speed;
        if (isCrouch)
        {
            moveSpeed = crouchSpeed;
        }
        if (!isEquiped)
        {
            moveSpeed *= notEquipedRatio;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    public void Turn(float angle)
    {
        transform.Rotate(Vector3.up, angle * turnSpeed * Time.deltaTime, Space.World);
    }

    public void SetEquipment()
    {
        if (isCrouch || isSettingEquip)
        {
            return;
        }
        anim.SetBool(HashIDs.notEquipedBool, isEquiped);
        isEquiped = !isEquiped;
        anim.SetTrigger(HashIDs.grabGunTrigger);
        StartCoroutine(GrabGun(isEquiped));
        isSettingEquip = true;
    }


    private IEnumerator GrabGun(bool equip)
    {
        yield return waitGrab;
        currGun.SetActive(equip);
        isSettingEquip = false;
    }

    public void SetCrouch()
    {
        isCrouch = !isCrouch;
        anim.SetBool(HashIDs.crouchBool, isCrouch);

        if (isCrouch)
        {
            audioSource.Pause();
            controller.height = 2f;
            controller.center = new Vector3(controller.center.x, 1f, controller.center.z);
        }
        else
        {
            controller.height = 3f;
            controller.center = new Vector3(controller.center.x, 1.5f, controller.center.z);
        }

        if (isCrouch && !isEquiped)
        {
            SetEquipment();
        }
    }

    public void Fire()
    {
        if (isEquiped)
        {


            if (gunShoot.FireBullet(this) && !isCrouch)
            {
                anim.SetTrigger(HashIDs.shootTrigger);
            }
        }
    }


    public void TakeDamage(float damage)
    {
        if (isDead)
        {
            return;
        }

        health -= damage;
        getHurtAudioSource.Play();
        var velocity = controller.velocity;
        controller.Move(new Vector3(-velocity.x / 2f, -velocity.y / 2f, -velocity.z / 2f) * Time.deltaTime);

        if (health <= 0)
        {
            isDead = true;
            audioSource.Stop();
            anim.SetTrigger(HashIDs.deadTrigger);
        }
    }
}
