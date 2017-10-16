using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform cameraRig;
    public float yMinRot = -60f;
    public float yMaxRot = 60f;
    public float zMin = -5f;
    public float zMax = -1f;

    private Transform mainCamera;
    private float xRot = 0f;
    private float yRot = 0f;
    private float zDis=-3;

    private void Start()
    {
        mainCamera = Camera.main.transform;
    }

    public void Control(float x,float y,float z)
    {
        xRot += x;
        yRot -= y;
        yRot = Mathf.Clamp(yRot, yMinRot, yMaxRot);
        zDis += z;
        zDis = Mathf.Clamp(zDis, zMin, zMax);

        var rotation = Quaternion.Euler(yRot, xRot, 0f);
        var position = rotation * new Vector3(0f, 0f, zDis) + cameraRig.position;
        mainCamera.rotation = rotation;
        mainCamera.position = position;

        UpdatePosition();
    }

    void UpdatePosition()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraRig.position, mainCamera.position - cameraRig.position, out hit, -zMin))
        {
            if (hit.collider.transform != mainCamera)
            {
                mainCamera.position = hit.point;
            }
        }
    }
}
