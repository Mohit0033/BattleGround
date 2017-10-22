using UnityEngine;

public class ShowInteraction : MonoBehaviour
{
    private GameObject ui;

    [HideInInspector]
    public GameObject UI
    {
        get { return ui; }
        set
        {
            ui = value;
            pickUp = ui.GetComponent<PickUpWeapon>();
        }
    }
    
    private PickUpWeapon pickUp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == Tags.Player)
        {
            pickUp.gameObject.SetActive(true);
            pickUp.weapon = this.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == Tags.Player)
        {
            pickUp.gameObject.SetActive(false);
        }
    }
}
