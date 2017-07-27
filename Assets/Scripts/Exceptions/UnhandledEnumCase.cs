
using System;

public class UnhandledEnumCase : Exception
{
    public UnhandledEnumCase(object enumCase) : base("This enum case isn't handled " + enumCase + " we should surely add it to the switch statement")
    {

    }
}
