using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SalamaTravelDTA;

namespace SalamaTravelBL
{
    public static class UsersBL
    {
        public  class LoginRequest
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }
        public class VerifyPasswordRequest
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        public class PutRequest
        {
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Token { get; set; }
            public int ClientId { get; set; }


        }
        public static  async Task<bool> ValidateUserPasswordAsync(string email, string password)
        {
            return await SalamaTravelDTA.UsersDAL.IsPasswordValidAsync(email, password);
        }
        public  static async Task<UsersDAL.UserDTO>loginASync(string Email,string Passowrd)
        {
            try
            {
                UsersDAL.UserDTO UserInfo = await UsersDAL.LoginAsync(Email, Passowrd);
                return UserInfo != null ? UserInfo : null;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
           
        }
        public static async Task<bool> UpdateUserInfoAsync(string UserName,string Password,string Token,int ClientId)
        {
            return  await UsersDAL.UpdateUserInfoAsync(ClientId, Token, UserName, Password);
            
        }
}
}
