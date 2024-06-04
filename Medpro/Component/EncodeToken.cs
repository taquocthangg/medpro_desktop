using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Component
{
    internal class EncodeToken
    {

public class AuthManager
    {
        public static string AccessToken { get; private set; }
        public static User CurrentUser { get; private set; }

        public static void SetAccessToken(string token)
        {
            AccessToken = token;
            CurrentUser = TokenDecoder.DecodeToken(token);
        }

        public static void ClearAccessToken()
        {
            AccessToken = null;
            CurrentUser = null;
        }
    }

    public class User
    {
        public string id { get; set; }
        public string email { get; set; }
        public string role_Id { get; set; }
        public string id_chuyenKhoa { get; set; }
        }

    public class TokenDecoder
    {
        public static User DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken != null)
            {
                var user = new User
                {
                    id = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "id")?.Value,
                    email = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "email")?.Value,
                    role_Id = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "role_Id")?.Value,
                    id_chuyenKhoa = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "id_chuyenKhoa")?.Value,
                };
                return user;
            }

            return null;
        }
    }

}
}
