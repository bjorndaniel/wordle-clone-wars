namespace WordleCloneWars.Helpers;

[AttributeUsage(AttributeTargets.Field)]
public class GameUrlAttribute : Attribute
{
    public string Url { get; set; } = string.Empty;
}

public static class GameTypeExtensions
{
    public static string GetUrl(this GameType type)
        => type.GetCustomAttribute<GameUrlAttribute>()?.Url ?? string.Empty;
}
