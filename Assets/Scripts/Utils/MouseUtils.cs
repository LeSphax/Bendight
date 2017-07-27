using UnityEngine;

public static class MouseUtils
{

    public static Vector3 MouseWorldPositionAtLevel(float yPosition)
    {
        return new Vector3(
            MouseWorldPosition().x,
            yPosition,
            MouseWorldPosition().z
        );
    }

    public static Vector3 MouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask layerMask = (1 << Layers.Ground);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            return hit.point;
        return Vector3.zero;
    }
}
