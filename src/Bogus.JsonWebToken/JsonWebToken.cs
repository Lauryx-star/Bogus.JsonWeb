using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Bogus.JsonWebToken;

public class JsonWebToken : DataSet
{
    private const string SigningKey = "7766397EB6094E27B420EF3A7D4DA345467F37E2F6BF4E72BEC68E2A016DD453";

    private static readonly TimeSpan DefaultTokenDuration = TimeSpan.FromMinutes(10);
    private static readonly ReadOnlyDictionary<string, string> SampleClaims = new(new Dictionary<string, string>
    {
        { "scope", "read" },
        { "email", "some@body.com" },
    });

    /// <summary>
    /// Generates a valid JSON Web Token
    /// </summary>
    /// <returns>
    /// e.g. eyJXXX.eyXXXX.SfXXXXX5c 
    /// </returns>
    public string ValidToken()
    {
        return ValidToken(SampleClaims, DefaultTokenDuration);
    }
    
    /// <summary>
    /// Generates a valid JSON Web Token
    /// </summary>
    /// <param name="validUntil">
    /// Defines the timespan when this token will be expired
    /// </param>
    /// <returns>
    /// e.g. eyJXXX.eyXXXX.SfXXXXX5c 
    /// </returns>
    public string ValidToken(TimeSpan validUntil)
    {
        return ValidToken(SampleClaims, validUntil);
    }

    /// <summary>
    /// Generates a valid JSON Web Token
    /// </summary>
    /// <param name="claims">
    /// Adds custom claims to the Json Web Token
    /// </param>
    /// <returns>
    /// e.g. eyJXXX.eyXXXX.SfXXXXX5c 
    /// </returns>
    public string ValidUntil(ReadOnlyDictionary<string, string> claims)
    {
        return ValidToken(claims, DefaultTokenDuration);
    }
    
    /// <summary>
    /// Generates a valid JSON Web Token
    /// </summary>
    /// <param name="claims">
    /// Adds custom claims to the Json Web Token
    /// </param>
    /// <param name="validUntil">
    /// Defines the timespan when this token will be expired
    /// </param>
    /// <returns>
    /// e.g. eyJXXX.eyXXXX.SfXXXXX5c 
    /// </returns>
    public string ValidToken(ReadOnlyDictionary<string, string> claims, TimeSpan validUntil)
    {
        var identity = CreateIdentity(claims);
             
        var token = CreateJwtToken(identity, validUntil);
        return token;
    }
    
    private static ClaimsIdentity CreateIdentity(ReadOnlyDictionary<string, string> claims)
    {
        const string fakeAuthenticationType = "Bogus.JsonWebToken";
        var claimsIdentity = new ClaimsIdentity(fakeAuthenticationType);
        AddClaims(claims, claimsIdentity);
        return claimsIdentity;
    }
    
    private static void AddClaims(ReadOnlyDictionary<string, string> claims, ClaimsIdentity identity)
    {
        foreach (var claim in claims)
        {
            identity.AddClaim(new Claim(claim.Key, claim.Value));
        }
    }
    
    private static string CreateJwtToken(ClaimsIdentity identity, TimeSpan duration)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = identity,
            Expires = DateTime.UtcNow + duration,
            SigningCredentials = signingCredentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(descriptor);
        var tokenString = tokenHandler.WriteToken(token);
        return tokenString;
    }
}