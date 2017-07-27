using UnityEngine;

public enum Team
{
    Blue = 0,
    Red = 1
}

public static class TeamEnum
{
    private static Color BlueTeamColor = new Color(0, 149 / 255f, 1);
    private static Color RedTeamColor = new Color(1, 53 / 255f, 0);

    public static Color Color(this Team team)
    {
        switch (team)
        {
            case Team.Blue:
                return BlueTeamColor;
            case Team.Red:
                return RedTeamColor;
            default:
                throw new UnhandledEnumCase(team);
        }
    }
}