using UnityEngine;
using UnityEngine.UI;

public class DrawAim : MonoBehaviour
{
    public Image image;
    public UserSoldier soldier;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (soldier.isDead)
        {
            image.enabled = false;
            return;
        }
        if (Cursor.lockState!=CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        image.rectTransform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
    }
    
}
