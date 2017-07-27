using UnityEngine;

public class StraightTrajectoryStrategy : ATrajectoryState
{

    private Vector3 direction;
    private float speed;
    private Quaternion rotation;

    public StraightTrajectoryStrategy(Vector3 direction, float speed)
    {
        this.direction = direction;
        this.speed = speed;
        rotation = Quaternion.Euler(90, 0, 90 + Mathf.Rad2Deg * Mathf.Atan2(direction.z, direction.x));
    }

    public override ATrajectoryState NextPosition(Vector3 currentPosition, out Vector3? newPosition, out Quaternion? newRotation)
    {
        newRotation = rotation;
        newPosition = currentPosition + direction * speed;
        return this;
    }
}
