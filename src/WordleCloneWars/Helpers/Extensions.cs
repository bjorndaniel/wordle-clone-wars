namespace WordleCloneWars.Helpers;

public static class Extensions
{
    public static bool IsDevOrLocal(this IHostEnvironment hostEnvironment) =>
        hostEnvironment.EnvironmentName.ToLower().Equals("local") || 
        hostEnvironment.EnvironmentName.ToLower().Equals("development");
    
    public static bool IsLocal(this IHostEnvironment hostEnvironment) =>
        hostEnvironment.EnvironmentName.ToLower().Equals("local");
    
    public static TAttribute? GetCustomAttribute<TAttribute>(this Enum target) where TAttribute : Attribute
    {
        var type = target.GetType();
        var memInfo = type.GetMember(target.ToString());
        var attributes = memInfo[0].GetCustomAttributes(typeof(TAttribute), false);
        if (attributes != null && attributes.Any())
        {
            return (TAttribute)attributes.First();
        }

        return null;
    }
}