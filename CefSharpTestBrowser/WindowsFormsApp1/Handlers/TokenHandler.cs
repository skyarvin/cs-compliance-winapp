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
        public static bool shouldRefresh()
        {
            if (Globals.UserToken == null)
            {
                return false;
            }
            int TimeLimitInMinute = 60;
            var jwthandler = new JwtSecurityTokenHandler();
            var token = jwthandler.ReadToken(Globals.UserToken.access_token) as JwtSecurityToken;
            long issuedAt = Int64.Parse(token.Payload[TokenClaim.iat.ToString()].ToString());
            if (GetTokenAgeMinutes(issuedAt) >= TimeLimitInMinute)
            {
                return true;
            }
            return false;
        }

        private static double GetTokenAgeMinutes(long issuedAt)
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
        user_id,
        username,
        tier_level,
        role,
        type,
        jti,
        iat,
        exp,
    }
}
