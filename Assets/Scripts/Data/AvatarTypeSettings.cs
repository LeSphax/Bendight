using System;

[Serializable]
//Not abstract to be able to tweak settings in the inspector
//Don't instantiate this class
public class AAvatarTypeSettings
{
    public AvatarType AvatarType { get; protected set; } //Properties are not tweakable in the inspector but can be seen in debug mode
    public string Name { get; protected set; }
    public string TimerDescription { get; protected set; }
    public float Speed;

    public AAvatarTypeSettings()
    {
        InitSettings();
    }

    protected virtual void InitSettings()
    {
    }
}

public class ShooterAvatarSetttings : AAvatarTypeSettings
{

    protected override void InitSettings()
    {
        Name = "Shooter";
        TimerDescription = "Kill him before :";
        AvatarType = AvatarType.SHOOTER;
        Speed = 0.15f;
    }
}

public class DodgerAvatarSetttings : AAvatarTypeSettings
{

    protected override void InitSettings()
    {
        Name = "Dodger";
        TimerDescription = "Survive for :";
        AvatarType = AvatarType.DODGER;
        Speed = 0.25f;
    }
}