using System.IdentityModel.Tokens.Jwt;

namespace IceSync.Application.Utils;

public static class JwtValidator
{
    public static bool IsTokenExpired(string token)
    {
        if (string.IsNullOrEmpty(token))
            return true;

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            // Read the token without validation (we only want to check expiration)
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Check if the token has an expiration time
            if (jwtToken.ValidTo == DateTime.MinValue)
                return false; // No expiration time set

            // Compare expiration time with current time
            return jwtToken.ValidTo < DateTime.UtcNow;
        }
        catch (Exception)
        {
            // If there's an error reading the token, consider it expired
            return true;
        }
    }
}
