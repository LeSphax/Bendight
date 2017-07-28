using Photon;
using UnityEngine;
using UnityEngine.Assertions;

public class Weapon : PunBehaviour, IWeapon
{

    private Avatar owner;
    public float castTime;
    public float reloadTime;

    private float castTimer = 0;
    private float reloadTimer = 0;

    private ITargeting targeting;

    private Color BaseColor
    {
        get
        {
            return owner.Team.Color();
        }
    }
    private Color Color
    {
        set
        {
            GetComponent<Renderer>().material.color = value;
            if (targeting != null)
                targeting.Color = value;
        }
    }

    private Vector3 tipLocalPosition = new Vector3(0f, 1f, 0f);
    private Vector3 TipPosition
    {
        get
        {
            return transform.position + (Vector3)(transform.localToWorldMatrix * tipLocalPosition);
        }
    }

    private enum WeaponState
    {
        IDLE,
        AIMING,
        RELOADING
    }

    private WeaponState currentState;
    private WeaponState CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            if (photonView.isMine)
            {
                photonView.RPC("SetState", PhotonTargets.Others, value);
                switch (value)
                {
                    case WeaponState.IDLE:
                        break;
                    case WeaponState.AIMING:
                        castTimer = 0;
                        break;
                    case WeaponState.RELOADING:
                        reloadTimer = 0;
                        break;
                    default:
                        throw new UnhandledEnumCase(value);
                }
            }
            else
            {
                switch (value)
                {
                    case WeaponState.IDLE:
                        break;
                    case WeaponState.AIMING:
                        castTimer = PlayersLatency.OneWayTripOtherPlayer();
                        break;
                    case WeaponState.RELOADING:
                        reloadTimer = PlayersLatency.OneWayTripOtherPlayer();
                        break;
                    default:
                        throw new UnhandledEnumCase(value);
                }
            }
            currentState = value;
        }
    }

    [PunRPC]
    private void SetState(WeaponState state)
    {
        CurrentState = state;
    }

    public void Init(Avatar owner)
    {
        this.owner = owner;
        if (photonView.isMine)
        {
            GameObject targetingGO = Instantiate(Resources.Load<GameObject>(ResourcePaths.WeaponTargeting));
            targetingGO.transform.SetParent(transform, false);
            targeting = targetingGO.GetComponent<CurvedTargeting>();
        }
        else
        {
            targeting = new NoTargeting();
        }
    }

    void Update()
    {
        UpdateTimers();

        UpdateAppearance();
    }

    private void Shoot(Vector3[] controlPoints)
    {
        Assert.IsTrue(photonView.isMine);

        //The shooter instantiates the bullet but the dodger manage the collisions and destroy the bullets
        GameObject bullet = PhotonNetwork.Instantiate(ResourcePaths.Bullet, TipPosition, Quaternion.identity, 0, new object[] { controlPoints, owner.Team, PhotonNetwork.player.ID });
        if (MatchManager.OtherPlayerID != -1)
            bullet.GetComponent<PhotonView>().TransferOwnership(MatchManager.OtherPlayerID);
    }

    private void UpdateAppearance()
    {
        switch (currentState)
        {
            case WeaponState.IDLE:
                Color = Color.white;
                break;
            case WeaponState.AIMING:
                float proportionOfCastTime = castTimer / castTime;
                Color = new Color(BaseColor.r + 1 - proportionOfCastTime, BaseColor.g + 1 - proportionOfCastTime, BaseColor.b + 1 - proportionOfCastTime);
                break;
            case WeaponState.RELOADING:
                float proportionOfReloadTime = reloadTimer / reloadTime;
                Color = new Color(proportionOfReloadTime, proportionOfReloadTime, proportionOfReloadTime);
                break;
            default:
                throw new UnhandledEnumCase(currentState);
        }
    }

    private void UpdateTimers()
    {
        switch (currentState)
        {
            case WeaponState.IDLE:
                //Do nothing
                break;
            case WeaponState.AIMING:
                castTimer += Time.deltaTime;
                if (castTimer >= castTime)
                {
                    if (photonView.isMine)
                    {
                        Vector3[] controlPoints = ((CurvedTargeting)targeting).ControlPoints();
                        Shoot(controlPoints);
                    }
                    CurrentState = WeaponState.RELOADING;
                }
                break;
            case WeaponState.RELOADING:
                reloadTimer += Time.deltaTime;
                if (reloadTimer >= reloadTime)
                {
                    CurrentState = WeaponState.IDLE;
                }
                break;
            default:
                throw new UnhandledEnumCase(currentState);
        }
    }

    public void ShootPressed()
    {
        switch (currentState)
        {
            case WeaponState.IDLE:
                CurrentState = WeaponState.AIMING;
                break;
            case WeaponState.AIMING:
            case WeaponState.RELOADING:
                //Do nothing
                break;
            default:
                throw new UnhandledEnumCase(currentState);
        }
    }

    public void CancelShootPressed()
    {
        switch (currentState)
        {
            case WeaponState.AIMING:
                CurrentState = WeaponState.RELOADING;
                break;
            case WeaponState.IDLE:
            case WeaponState.RELOADING:
                //Do nothing
                break;
            default:
                throw new UnhandledEnumCase(currentState);
        }
    }

    public void StartAiming()
    {
        targeting.StartAiming();
    }

    public void StopAiming()
    {
        targeting.StopAiming();
    }
}
