using Photon;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Avatar : PunBehaviour
{
    [SerializeField]
    private Text title;
    [SerializeField]
    private Slider healthBar;

    private const int START_HEALTH = 5;
    private GameObject avatarModel;
    private IWeapon weapon;
    private int playerId;

    [HideInInspector]
    public Team Team;

    public AAvatarTypeSettings AvatarTypeSettings;

    public event EmptyDelegate OutOfHealth;

    private int _healthAmount;
    public int HealthAmount
    {
        get
        {
            return _healthAmount;
        }
        private set
        {
            _healthAmount = value;
            if (photonView.isMine)
            {
                photonView.RPC("SetHealth", PhotonTargets.Others, value);
            }
            healthBar.value = (float)HealthAmount / START_HEALTH;
            if (PhotonNetwork.isMasterClient)
            {
                if (_healthAmount <= 0)
                {
                    if (OutOfHealth != null)
                        OutOfHealth.Invoke();
                }
            }
        }
    }

    [PunRPC]
    private void SetHealth(int health)
    {
        HealthAmount = health;
    }

    public void LoseHealth(int healthLoss)
    {
        HealthAmount -= healthLoss;
    }

    public void Awake()
    {
        playerId = (int)photonView.instantiationData[0];
        MatchManager.RegisterAvatar(playerId, this);
        gameObject.SetActive(false);
        this.Team = (Team)photonView.instantiationData[1];

        if (!photonView.isMine)
            GetComponent<Rigidbody>().isKinematic = true;
    }

    public void CreateAvatar(AvatarType avatarType, Vector3 spawnPosition)
    {
        int idModel = PhotonNetwork.AllocateViewID();
        int idWeapon = PhotonNetwork.AllocateViewID();

        photonView.RPC("CreateModel", PhotonTargets.All, avatarType, idModel, idWeapon);
        photonView.RPC("InitAvatar", PhotonTargets.All, spawnPosition, avatarType);
    }

    [PunRPC]
    private void InitAvatar(Vector3 spawnPosition, AvatarType avatarType)
    {
        switch (avatarType)
        {
            case AvatarType.DODGER:
                HealthAmount = START_HEALTH;
                healthBar.gameObject.SetActive(true);
                break;
            case AvatarType.SHOOTER:
                healthBar.gameObject.SetActive(false);
                break;
            default:
                break;
        }

        foreach (Renderer renderer in avatarModel.transform.GetComponentsInChildren<Renderer>())
        {
            if (renderer.tag == Tags.TeamColored)
            {
                renderer.material.color = Team.Color();
            }
        }
        avatarModel.transform.SetParent(transform, false);

        AvatarTypeSettings = avatarType.Settings();
        title.text = AvatarTypeSettings.Name;

        weapon.Init(this);
        if (photonView.isMine)
        {
            AvatarControl avatarControl = gameObject.AddComponent<AvatarControl>();
            avatarControl.Init(this, avatarModel, weapon);
        }

        transform.position = spawnPosition;
        gameObject.SetActive(true);
    }

    [PunRPC]
    public void RemoveAvatar()
    {
        Destroy(avatarModel);
        Destroy(GetComponent<AvatarControl>());
        gameObject.SetActive(false);
    }


    [PunRPC]
    private void CreateModel(AvatarType avatarType, int idModel, int idWeapon)
    {
        switch (avatarType)
        {
            case AvatarType.DODGER:
                avatarModel = Instantiate(Resources.Load<GameObject>(ResourcePaths.DodgerAvatar));
                weapon = new NoWeapon();
                break;
            case AvatarType.SHOOTER:
                avatarModel = Instantiate(Resources.Load<GameObject>(ResourcePaths.ShooterAvatar));
                GameObject weaponGO = Instantiate(Resources.Load<GameObject>(ResourcePaths.Weapon));
                SetupSharedGameObject(idWeapon, weaponGO);
                weapon = weaponGO.GetComponent<Weapon>();
                break;
            default:
                throw new UnhandledEnumCase(avatarType);
        }
        SetupSharedGameObject(idModel, avatarModel);
    }

    private void SetupSharedGameObject(int viewID, GameObject instantiatedObject)
    {
        PhotonView objectPhotonView = instantiatedObject.GetComponent<PhotonView>();
        objectPhotonView.viewID = viewID;
        if (photonView.isMine)
        {
            objectPhotonView.TransferOwnership(PhotonNetwork.player.ID);
        }
        instantiatedObject.transform.SetParent(avatarModel.transform, false);
    }

    private void OnDestroy()
    {
        MatchManager.RemoveAvatar(playerId);
    }
}
