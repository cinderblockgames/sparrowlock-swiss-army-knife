namespace SLSAK.Extensions;

public static class EnvironmentExtensions
{
    public static IDictionary<string, string> GetEnvironmentVariables(bool caseSensitive)
    {
        var comparer = caseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;
        var result = new Dictionary<string, string>(comparer);

        var env = Environment.GetEnvironmentVariables();
        foreach (string key in env.Keys)
        {
            var obj = env[key];
            if (obj is string value)
            {
                result.Add(key, value);
            }
        }

        return result;
    }
}