namespace Publicizer;

public enum MemberLifetime
{
    Static = 1 << 1,
    Instance = 1 << 2,
    All = Static | Instance
}

public enum MemberVisibility
{
    Public = 1 << 1,
    NonPublic = 1 << 2,
    All = Public | NonPublic
}
