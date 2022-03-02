namespace WordleCloneWars.Helpers;

public static class Extensions
{
    public static bool IsDevOrLocal(this IHostEnvironment hostEnvironment) =>
        hostEnvironment.EnvironmentName.ToLower().Equals("local") || 
        hostEnvironment.EnvironmentName.ToLower().Equals("development");
    
    public static bool IsLocal(this IHostEnvironment hostEnvironment) =>
        hostEnvironment.EnvironmentName.ToLower().Equals("local");
}