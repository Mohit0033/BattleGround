using UnityEngine;

public static class HashIDs
{
    public static readonly int speedFloat;
    public static readonly int crouchBool;
    public static readonly int notEquipedBool;
    public static readonly int shootTrigger;
    public static readonly int grabGunTrigger;
    public static readonly int deadTrigger;

    static HashIDs()
    {
        speedFloat = Animator.StringToHash("Speed");
        crouchBool = Animator.StringToHash("Crouch");
        notEquipedBool = Animator.StringToHash("NotEquiped");
        deadTrigger = Animator.StringToHash("Dead");
        shootTrigger = Animator.StringToHash("Shoot");
        grabGunTrigger = Animator.StringToHash("GrabGun");
    }
}
