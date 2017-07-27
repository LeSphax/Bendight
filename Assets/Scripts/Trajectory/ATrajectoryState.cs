using UnityEngine;

//Controls the trajectory of a bullet
public abstract class ATrajectoryState
{
    //Gives the next position, rotation and ATrajectoryState of the bullet
    public abstract ATrajectoryState NextPosition(float deltaTime, Vector3 currentPosition, out Vector3? newPosition, out Quaternion? newRotation);
}
