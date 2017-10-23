using System.Collections;
using UnityEngine;

public class UserSoldier : Soldier
{
    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        anim.SetBool(HashIDs.notEquipedBool, !isEquiped);
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

    public override void SetCrouch()
    {
        base.SetCrouch();

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
