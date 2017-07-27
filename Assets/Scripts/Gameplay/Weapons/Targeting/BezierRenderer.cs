using System.Linq;
using UnityEngine;

public class BezierRenderer : MonoBehaviour
{
    public Vector3?[] ControlPoints;

    private LineRenderer lineRenderer;

    private int layerOrder = 0;
    private int SEGMENT_COUNT = 50;

    private bool activated;
    public bool Activated
    {
        get
        {
            return activated;
        }
        set
        {
            if (!value)
                Reset();
            activated = value;
        }
    }

    public Color Color
    {
        set
        {
            lineRenderer.startColor = value;
            lineRenderer.endColor = value;
        }
    }

    private void Awake()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.sortingLayerID = layerOrder;
        Activated = false;
        Reset();
    }

    public void DrawCurve()
    {
        if (activated && ControlPoints.Where(point => point == null).Count() == 0)
            for (int i = 1; i <= SEGMENT_COUNT; i++)
            {
                float t = i / (float)SEGMENT_COUNT;
                int nodeIndex = 0;
                Vector3 pixel = BezierMaths.Bezier3(ControlPoints[nodeIndex].Value,
                    ControlPoints[nodeIndex + 1].Value, ControlPoints[nodeIndex + 2].Value, t);
                lineRenderer.positionCount = i;
                lineRenderer.SetPosition((i - 1), pixel);
            }
    }

    public void Reset()
    {
        ControlPoints = new Vector3?[3];
        lineRenderer.positionCount = 0;
    }
}
