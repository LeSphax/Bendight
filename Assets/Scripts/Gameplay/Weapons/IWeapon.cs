public interface IWeapon
{
    void Init(Avatar player);
    void StartAiming();
    void StopAiming();
    void ShootPressed();
    void CancelShootPressed();
}