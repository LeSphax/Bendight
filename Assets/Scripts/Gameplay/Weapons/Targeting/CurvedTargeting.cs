using System.Linq;
using UnityEngine;

public class CurvedTargeting : MonoBehaviour, ITargeting
{
    private BezierRenderer bezierRenderer;

    private bool curving;

    private bool isActivated;
    private bool IsActivated
    {
        get
        {
            return isActivated;
        }
        set
        {
            bezierRenderer.Activated = value;
            isActivated = value;
        }
    }

    public Color Color
    {
        set
        {
            bezierRenderer.Color = value;
        }
    }

    private void Awake()
    {
        bezierRenderer = gameObject.GetComponent<BezierRenderer>();
    }

    public void StartAiming()
    {
        IsActivated = true;
        bezierRenderer.ControlPoints[2] = MouseUtils.MouseWorldPositionAtLevel(transform.position.y);
        Update();
    }

    private void Update()
    {
        if (IsActivated)
        {
            bezierRenderer.ControlPoints[0] = transform.position;
            bezierRenderer.ControlPoints[1] = MouseUtils.MouseWorldPositionAtLevel(transform.position.y);
            bezierRenderer.DrawCurve();
        }
    }

    public void StopAiming()
    {
        IsActivated = false;
    }

    public Vector3[] ControlPoints()
    {
        if (isActivated)
            return bezierRenderer.ControlPoints.Select(point => point.Value).ToArray();
        else
            return new Vector3[1] { (MouseUtils.MouseWorldPositionAtLevel(transform.position.y) - transform.position).normalized };
    }
}

