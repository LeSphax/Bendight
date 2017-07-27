using UnityEngine;

public class BattleriteCamera : MonoBehaviour
{
    public GameObject target;

    public float speed;

    private Vector3 startPosition;
    private Vector3 previousBasePosition;

    public float YOffset
    {
        get
        {
            return transform.position.y * Mathf.Tan(Mathf.Deg2Rad * (Camera.main.fieldOfView / 2));
        }
    }

    private static float HorizontalFOV
    {
        get
        {
            var radAngle = Camera.main.fieldOfView * Mathf.Deg2Rad;
            var radHFOV = 2 * Mathf.Atan(Mathf.Tan(radAngle / 2) * Camera.main.aspect);
            var horizontalFOV = Mathf.Rad2Deg * radHFOV;
            return horizontalFOV;
        }
    }

    private float XOffset
    {
        get
        {
            return transform.position.y * Mathf.Tan(Mathf.Deg2Rad * (HorizontalFOV / 2));
        }
    }


    private Vector3 BasePosition
    {
        get
        {
            if (target != null)
                return startPosition + Vector3.forward * target.transform.position.z + Vector3.right * target.transform.position.x;
            else
                return startPosition;
        }
    }

    private void Awake()
    {
        startPosition = transform.localPosition;
        previousBasePosition = transform.localPosition;
    }

    private void FixedUpdate()
    {
        PositionCamera();
    }

    public void PositionCamera()
    {
        Vector3 previousPosition = transform.position;
        Vector2 mouseProportion = MouseProportionOnScreen();

        Vector3 targetPosition = new Vector3(BasePosition.x + mouseProportion.x * XOffset, BasePosition.y, BasePosition.z + mouseProportion.y * YOffset);

        transform.position = transform.position + BasePosition - previousBasePosition;

        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.fixedDeltaTime);

        previousBasePosition = BasePosition;
    }

    private Vector2 MouseProportionOnScreen()
    {
        Vector3 clampedMousePosition = new Vector3(Mathf.Clamp(Input.mousePosition.x, 0, Screen.width),
            Mathf.Clamp(Input.mousePosition.y, 0, Screen.height));

        float xDistanceToCenter = clampedMousePosition.x - Screen.width / 2.0f;
        float yDistanceToCenter = clampedMousePosition.y - Screen.height / 2.0f;

        float xProportion = xDistanceToCenter / (Screen.width / 2.0f);
        float yProportion = yDistanceToCenter / (Screen.height / 2.0f);

        return new Vector2(xProportion, yProportion);
    }
}
