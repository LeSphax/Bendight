public enum AvatarType
{
    DODGER = 0,
    SHOOTER = 1
}

public static class AvatarTypeMethods
{
    public static AvatarType OtherType(this AvatarType avatarType)
    {
        switch (avatarType)
        {
            case AvatarType.DODGER:
                return AvatarType.SHOOTER;
            case AvatarType.SHOOTER:
                return AvatarType.DODGER;
            default:
                throw new UnhandledEnumCase(avatarType);
        }
    }

    public static AAvatarTypeSettings Settings(this AvatarType avatarType)
    {
        switch (avatarType)
        {
            case AvatarType.DODGER:
                return new DodgerAvatarSetttings();
            case AvatarType.SHOOTER:
                return new ShooterAvatarSetttings();
            default:
                throw new UnhandledEnumCase(avatarType);
        }
    }
}
