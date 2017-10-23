using UnityEngine;

public class UserControl : MonoBehaviour
{
    public GameObject gameOverPanel;
    private UserSoldier soldierControl;
    private CameraControl cameraControl;
    private Vector3 aimPosition;
    private Rect touchRect;
    private float h, v, x, y, z = 0;
    private bool isEquiped = false;

    private void Start()
    {
        soldierControl = GetComponent<UserSoldier>();
        cameraControl = GetComponent<CameraControl>();
        aimPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        touchRect = new Rect(Screen.width * 0.4f, Screen.height * 0.3f, Screen.width * 0.5f, Screen.height * 0.7f);
    }

    private void Update()
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
#if UNITY_STANDALONE || UNITY_WEBPLAYER
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");
        z = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetKeyDown(KeyCode.C)) //change equip
        {
            isEquiped = !isEquiped;
            soldierControl.SetEquipment(isEquiped);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))//crouch
        {
            soldierControl.SetCrouch();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            soldierControl.Fire(aimPosition);
        }
#else
        x = 0;
        y = 0;
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.touches[i].phase == TouchPhase.Moved && touchRect.Contains(Input.touches[i].position))
            {
                x = Input.touches[i].deltaPosition.x;
                y = Input.touches[i].deltaPosition.y;
                break;
            }
        }
        
#endif
        x = x * soldierControl.turnSpeed * Time.fixedDeltaTime;
        soldierControl.Turn(x);

        cameraControl.Control(x, y, z);

        soldierControl.Move(h, v);

    }

    public void TouchInput(float h, float v)
    {
        this.h = h;
        this.v = v;
    }

    public void Equip()
    {
        isEquiped = !isEquiped;
        soldierControl.SetEquipment(isEquiped);
    }

    public void Crouch()
    {
        soldierControl.SetCrouch();
    }

    public void Fire()
    {
        soldierControl.Fire(aimPosition);
    }
}
