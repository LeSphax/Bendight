using Photon;
using UnityEngine;

public class Bullet : PunBehaviour
{

    public float speed;
    public Team team;

    private ATrajectoryState trajectory;

    private void Awake()
    {
        Init((Vector3[])photonView.instantiationData[0], (Team)photonView.instantiationData[1], (int)photonView.instantiationData[2]);
    }

    public void Init(Vector3[] controlPoints, Team team, int instantiatorID)
    {
        this.team = team;
        GetComponent<Renderer>().material.color = team.Color();

        if (controlPoints.Length == 1)
            trajectory = new StraightTrajectoryStrategy(controlPoints[0], speed);
        else
            trajectory = new CurvedTrajectoryState(controlPoints, speed);

        if (PhotonNetwork.player.ID != instantiatorID)
            UpdatePositionAndRotation(PlayersLatency.OneWayTripOtherPlayer());
    }

    private void FixedUpdate()
    {
        UpdatePositionAndRotation(Time.fixedDeltaTime);
    }

    private void UpdatePositionAndRotation(float deltaTime)
    {
        Vector3? nextPosition;
        Quaternion? nextRotation;

        trajectory = trajectory.NextPosition(deltaTime, transform.position, out nextPosition, out nextRotation);

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

        if (effect != null && photonView.isMine)
        {
            effect.ApplyHitEffect(this, contactPoint);
        }
    }
}
