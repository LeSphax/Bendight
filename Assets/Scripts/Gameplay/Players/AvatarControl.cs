using UnityEngine;
using UnityEngine.Assertions;

public class AvatarControl : Photon.PunBehaviour
{
    private Avatar avatar;
    private IWeapon weapon;
    private GameObject avatarModel;

    private Vector3 currentDirection;

    public void Init(Avatar avatar, GameObject avatarModel, IWeapon weapon)
    {
        Assert.IsTrue(photonView.isMine);
        GameObject.FindGameObjectWithTag(Tags.BattleriteCamera).GetComponent<BattleriteCamera>().target = gameObject;
        this.weapon = weapon;
        this.avatar = avatar;
        this.avatarModel = avatarModel;
    }

    void Update()
    {
        currentDirection = Vector3.zero;

        if (Input.GetButton(InputButtons.Up))
        {
            currentDirection += Vector3.forward;
        }
        if (Input.GetButton(InputButtons.Left))
        {
            currentDirection += Vector3.left;
        }
        if (Input.GetButton(InputButtons.Down))
        {
            currentDirection += Vector3.back;
        }
        if (Input.GetButton(InputButtons.Right))
        {
            currentDirection += Vector3.right;
        }
        if (Input.GetButton(InputButtons.Shoot))
        {
            weapon.ShootPressed();
        }
        if (Input.GetButtonDown(InputButtons.Cancel))
        {
            weapon.CancelShootPressed();
        }
        if (Input.GetButtonDown(InputButtons.Aim))
        {
            weapon.StartAiming();
        }
        if (Input.GetButtonUp(InputButtons.Aim))
        {
            weapon.StopAiming();
        }
    }

    private void FixedUpdate()
    {
        transform.position += currentDirection.normalized * avatar.AvatarTypeSettings.Speed;
        avatarModel.transform.LookAt(MouseUtils.MouseWorldPositionAtLevel(transform.position.y));
    }
}
