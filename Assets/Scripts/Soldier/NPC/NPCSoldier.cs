using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCSoldier : Soldier
{
    public float idleTurnTime = 2f;

    public event Action<Soldier> Damaged;

    private NavMeshAgent nav;
    private CapsuleCollider capsuleCollider;
    private float idleTurnTimer;

    private void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        anim.SetBool(HashIDs.notEquipedBool, !isEquiped);
    }


    public void Move(Vector3 destination)
    {

        float moveSpeed = speed;
        if (isCrouch)
        {
            moveSpeed = crouchSpeed;
        }
        if (!isEquiped)
        {
            moveSpeed *= notEquipedRatio;
        }

        nav.speed = moveSpeed;
        if (!nav.pathPending)
        {
            if (nav.isStopped)
            {
                nav.isStopped = false;
            }
            nav.destination = destination;
        }

        //if (nav.remainingDistance>0.05f)
        //{
        anim.SetFloat(HashIDs.speedFloat, nav.velocity.magnitude);
        if (!isCrouch && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        //}
        //else
        //{
        //    nav.isStopped = true;
        //    anim.SetFloat(HashIDs.speedFloat, 0);
        //    audioSource.Stop();
        //}


    }

    public void Stop()
    {
        nav.isStopped = true;
        anim.SetFloat(HashIDs.speedFloat, 0);
        audioSource.Stop();
    }

    public override void Turn(float angle)
    {
        base.Turn(angle);
        Stop();
    }
    
    public void SetCrouch()
    {
        isCrouch = !isCrouch;
        anim.SetBool(HashIDs.crouchBool, isCrouch);

        if (isCrouch)
        {
            audioSource.Pause();
            capsuleCollider.height = 2f;
            capsuleCollider.center = new Vector3(capsuleCollider.center.x, 1f, capsuleCollider.center.z);
        }
        else
        {
            capsuleCollider.height = 3f;
            capsuleCollider.center = new Vector3(capsuleCollider.center.x, 1.5f, capsuleCollider.center.z);
        }

        if (isCrouch)
        {
            SetEquipment(true);
        }
    }

    public override void Fire(Vector3 position)
    {
        Stop();
        transform.LookAt(position);
        base.Fire(position);
    }
    
    public void TakeDamage(float damage, Soldier shooter = null)
    {
        if (isDead)
        {
            return;
        }

        health -= damage;
        getHurtAudioSource.Play();

        if (shooter)//not take because not in safe zone
        {
            Damaged?.Invoke(shooter);
        }

        if (health <= 0)
        {
            isDead = true;
            nav.isStopped = true;
            audioSource.Stop();
            anim.SetTrigger(HashIDs.deadTrigger);
        }
    }

    public void Idle()
    {
        if (idleTurnTimer > idleTurnTime)
        {
            transform.rotation = transform.rotation * Quaternion.Euler(0, 30f, 0);
            idleTurnTimer = 0;
        }
        else
        {
            idleTurnTimer += Time.fixedDeltaTime;
        }
    }
}
