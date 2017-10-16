using UnityEngine;

public class UserControl : MonoBehaviour
{
    public GameObject gameOverPanel;
    private UserSoldier soldierControl;
    private CameraControl cameraControl;

    private void Start()
    {
        soldierControl = GetComponent<UserSoldier>();
        cameraControl = GetComponent<CameraControl>();
    }

    private void FixedUpdate()
    {
        if (soldierControl.isDead)
        {
            Cursor.lockState = CursorLockMode.None;
            if (!gameOverPanel.activeSelf)
            {
                gameOverPanel.SetActive(true);
            }
            return;
        }

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float angle = Input.GetAxis("Mouse X");
        soldierControl.Turn(angle);

        var y = Input.GetAxis("Mouse Y");

        var z = Input.GetAxis("Mouse ScrollWheel");

        cameraControl.Control(angle * soldierControl.turnSpeed * Time.deltaTime, y, z);

        if (Input.GetKeyDown(KeyCode.C)) //change equip
        {
            soldierControl.SetEquipment();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))//crouch
        {
            soldierControl.SetCrouch();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            soldierControl.Fire();
        }

        soldierControl.Move(h, v);

    }
}
