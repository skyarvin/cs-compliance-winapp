using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using WindowsFormsApp1;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace CSTool.Handlers
{
    internal static class TokenHandler
    {
        public static bool IsTokenAboutToExpire()
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadToken(Globals.UserToken.access_token) as JwtSecurityToken;
            long issuedAt = Int64.Parse(token.Payload[TokenClaim.iat.ToString()].ToString());
            if (GetTokenExpirationTimeRemaining(issuedAt) >= 60)
            {
                return true;
            }
            return false;
        }

        private static double GetTokenExpirationTimeRemaining(long issuedAt)
        {
            DateTime dateTimeNow = DateTime.Now;
            DateTime dateTimeOffset = UnixTimeStampToDateTime(issuedAt);
            return (dateTimeNow - dateTimeOffset).TotalMinutes;
        }

        private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }

    enum TokenClaim
    {
        Type,
        user_i,
        username,
        tier_level,
        role,
        type,
        jti,
        iat,
        exp,
    }
}
