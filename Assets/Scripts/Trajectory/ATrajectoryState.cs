using UnityEngine;

public abstract class ATrajectoryState
{
    public abstract ATrajectoryState NextPosition(float deltaTime, Vector3 currentPosition, out Vector3? newPosition, out Quaternion? newRotation);
}
