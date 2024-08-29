using DEFRA.NE.BNG.Integration.Domain.Constants;
using JWT.Algorithms;
using JWT.Builder;
using System.Text;

namespace DEFRA.NE.BNG.Integration.Infrastructure.Utilities
{
    public class TokenManager
    {
        public static string GenerateAccessToken()
        {
            return new JwtBuilder()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationSecret)))
                .AddClaim(GovNotificationConstants.GovNot_Token_IssuedAt, DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                .AddClaim(GovNotificationConstants.GovNot_Token_ServiceId, Environment.GetEnvironmentVariable(EnvironmentConstants.NotificationServiceId))
                .Encode();
        }
    }
}