using Bogus.Premium;

namespace Bogus.JsonWebToken;

/// <summary>
/// Extension class for the <see cref="JsonWebToken"/>
/// </summary>
public static class JsonWebTokenExtensions
{
    /// <summary>
    /// Configures the faker property to be generated as a jwt string
    /// </summary>
    /// <param name="faker"></param>
    /// <returns></returns>
    public static JsonWebToken JsonWebToken(this Faker faker)
    {
        return ContextHelper.GetOrSet(faker, () => new JsonWebToken());
    }
}