using UnityEngine;
using UnityEngine.EventSystems;

public class PickUpWeapon : MonoBehaviour, IPointerDownHandler
{
    [HideInInspector]
    public GameObject weapon;

    private UserSoldier user;

    private void Start()
    {
        user = FindObjectOfType<UserSoldier>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (weapon != null)
        {
            user.PickUpWeapon(weapon);
            user.SetEquipment(true);
            gameObject.SetActive(false);
        }

    }
#if UNITY_STANDALONE
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && weapon != null)
        {
            user.PickUpWeapon(weapon);
            user.SetEquipment(true);
            gameObject.SetActive(false);
        }
    }
#endif
}
