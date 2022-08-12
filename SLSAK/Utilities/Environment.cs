using Underlying = System.Environment;

namespace SLSAK.Utilities;

public static class Environment
{
    public static IDictionary<string, string> GetEnvironmentVariables(bool caseSensitive)
    {
        var comparer = caseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase;
        var result = new Dictionary<string, string>(comparer);

        var env = Underlying.GetEnvironmentVariables();
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