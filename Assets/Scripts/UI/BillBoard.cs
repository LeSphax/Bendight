using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BillBoard : MonoBehaviour
{
    private Camera mCamera;

    [SerializeField]
    private Text text;

    public Color Color
    {
        set
        {
            text.color = value;
        }
    }

    public string Text
    {

        set
        {
            text.text = value;
        }
    }

    void Start()
    {
        mCamera = Camera.main;
    }

    public void SetHeight(float height)
    {
        transform.localPosition = transform.localPosition - Vector3.up * transform.localPosition.y + height * Vector3.up;
    }

    void Update()
    {
        transform.LookAt(transform.position + mCamera.transform.rotation * Vector3.forward, mCamera.transform.rotation * Vector3.up);
    }
}