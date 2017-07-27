using UnityEngine;

public class CurvedTrajectoryState : ATrajectoryState
{
    private Vector3[] controlPoints;
    private float curveLength;
    private float previousCompletion;
    private float speed;

    public Vector3 CurrentDirection
    {
        get
        {
            float curveProportion = speed / curveLength;
            return (BezierMaths.Bezier3(controlPoints, previousCompletion + curveProportion) - BezierMaths.Bezier3(controlPoints, previousCompletion)).normalized;
        }
    }

    public CurvedTrajectoryState(Vector3[] controlPoints, float speed)
    {
        this.speed = speed;
        this.curveLength = BezierMaths.LengthBezier3(controlPoints, 10);
        this.controlPoints = controlPoints;
        previousCompletion = 0;
    }
    public override ATrajectoryState NextPosition(float deltaTime, Vector3 currentPosition, out Vector3? newPosition, out Quaternion? newRotation)
    {
        float increment = (speed * deltaTime) / curveLength;
        float newCompletion = previousCompletion + increment;

        if (newCompletion <= 1)
        {
            newPosition = BezierMaths.Bezier3(controlPoints, newCompletion);
            newPosition = new Vector3(
                newPosition.Value.x,
                currentPosition.y,
                newPosition.Value.z
                );
            newRotation = Quaternion.Euler(90, 0, 90 + Mathf.Rad2Deg * Mathf.Atan2(CurrentDirection.z, CurrentDirection.x));

            previousCompletion = newCompletion;

            return this;
        }
        else
        {
            return new StraightTrajectoryStrategy(CurrentDirection, speed).NextPosition(deltaTime,currentPosition, out newPosition, out newRotation);
        }

    }
}

