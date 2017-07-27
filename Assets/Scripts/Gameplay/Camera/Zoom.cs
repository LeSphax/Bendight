using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Zoom : MonoBehaviour
{
    private Camera mCamera;

    public float speed = 3;
    public float fovMin = 20;
    public float fovMax = 70;

    private void Start()
    {
        mCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            mCamera.fieldOfView = Mathf.Clamp(mCamera.fieldOfView - Input.GetAxis("Mouse ScrollWheel") * speed, fovMin, fovMax);
        }
    }

}