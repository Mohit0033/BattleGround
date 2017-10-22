using UnityEngine;
using UnityEngine.UI;

public class DrawAim : MonoBehaviour
{
    public Image image;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        image.rectTransform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }

    private void Update()
    {
        //if (Cursor.lockState!=CursorLockMode.Locked)
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //}
    }
    
}
