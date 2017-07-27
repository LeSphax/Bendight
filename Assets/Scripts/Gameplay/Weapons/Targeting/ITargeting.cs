using UnityEngine;

interface ITargeting
{

    Color Color { set; }

    void StartAiming();
    void StopAiming();
}
