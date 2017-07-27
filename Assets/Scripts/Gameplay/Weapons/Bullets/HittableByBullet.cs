using UnityEngine;

public class HittableByBullet : MonoBehaviour
{
    public delegate void EffectOnBullet(Bullet bullet);

    //Instead of this enum we could use inheritance and have a different class for each case (HittableWall, HittablePlayer, ...).
    //Inheritance would be more flexible but splitting the code would make it harder to see all the hitting behavior at one glance.
    //Since I don't need anything complex for now, I chose this solution.
    public enum Type
    {
        WALL,
        PLAYER,
        BULLET
    }

    public Type type;

    public void ApplyHitEffect(Bullet bullet, Vector3 contactPoint)
    {
        switch (type)
        {
            case Type.WALL:
                //Nothing happening to the wall
                DestroyBullet(bullet, contactPoint);
                break;
            case Type.PLAYER:
                Avatar player = GetComponent<Avatar>();
                if (player.Team != bullet.team)
                {
                    GetComponent<Avatar>().LoseHealth(1);
                    DestroyBullet(bullet, contactPoint);
                }
                break;
            case Type.BULLET:
                if (bullet.team != GetComponent<Bullet>().team)
                    //Both bullets will destroy the other one
                    DestroyBullet(bullet, contactPoint);
                break;
            default:
                throw new UnhandledEnumCase(type);
        }
    }

    private void DestroyBullet(Bullet bullet, Vector3 contactPoint)
    {
        PhotonNetwork.Destroy(bullet.photonView);
        PhotonNetwork.Instantiate(ResourcePaths.Impact, contactPoint, Quaternion.identity, 0);
    }
}
