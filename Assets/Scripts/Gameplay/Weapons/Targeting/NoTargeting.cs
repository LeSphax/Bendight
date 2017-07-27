using System;
using UnityEngine;

class NoTargeting : ITargeting
{
    public Color Color
    {
        set
        {
            //DoNothing
        }
    }

    public void StopAiming()
    {
        //DoNothing
    }

    public void StartAiming()
    {
        //DoNothing
    }
}
