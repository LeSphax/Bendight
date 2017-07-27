using Photon;
using UnityEngine;

public class Bullet : PunBehaviour
{

    public float speed;
    public Team team;

    private ATrajectoryState trajectory;

    private void Awake()
    {
        Init((Vector3[])photonView.instantiationData[0], (Team)photonView.instantiationData[1]);
    }

    public void Init(Vector3[] controlPoints, Team team)
    {
        this.team = team;
        GetComponent<Renderer>().material.color = team.Color();
        if (controlPoints.Length == 1)
            trajectory = new StraightTrajectoryStrategy(controlPoints[0], speed);
        else
            trajectory = new CurvedTrajectoryState(controlPoints, speed);

        UpdatePositionAndRotation();
    }

    private void FixedUpdate()
    {
        UpdatePositionAndRotation();
    }

    private void UpdatePositionAndRotation()
    {
        Vector3? nextPosition;
        Quaternion? nextRotation;

        trajectory = trajectory.NextPosition(transform.position, out nextPosition, out nextRotation);

        if (nextPosition != null)
            transform.position = nextPosition.Value;
        if (nextRotation != null)
            transform.rotation = nextRotation.Value;
    }

    private void OnCollisionEnter(Collision collision)
    {
        BulletCollision(collision.collider, collision.contacts[0].point);
    }

    private void BulletCollision(Collider otherCollider, Vector3 contactPoint)
    {
        HittableByBullet effect = otherCollider.GetComponent<HittableByBullet>();

        if (effect != null)
        {
            effect.ApplyHitEffect(this, contactPoint);
        }
    }
}
