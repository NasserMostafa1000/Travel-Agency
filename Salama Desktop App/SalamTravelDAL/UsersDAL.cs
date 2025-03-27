using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SalamaTravelDAL;

namespace SalamTravelDAL
{
    public static class UsersDAL
    {
        // إعداد HttpClient
        private static readonly HttpClient client = Settings.HttpClient;

        public class LoginRequest
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

        public class UserDTO
        {
            public int ClientID { get; set; }
            public string FirstName { get; set; }
            public string MidName { get; set; }
            public string LastName { get; set; }
            public string Token { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string PassportImagePath { get; set; }
            public string PersonalImagePath { get; set; }
            public decimal Balance { get; set; }
        }

        public static async Task<bool> LoginAsync(string userName, string password)
        {
            try
            {
                string url = "Users/Login";

                // إعداد بيانات الطلب
                var loginRequest = new
                {
                    UserName = userName,
                    Password = password
                };

                var jsonContent = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");

                // إرسال الطلب
                var response = await Settings.HttpClient.PostAsync(url, jsonContent);

                // التحقق من نجاح الطلب
                if (response.IsSuccessStatusCode)
                {
                    // قراءة محتوى الرد وتحويله إلى كائن
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var currentUser = JsonConvert.DeserializeObject<UserDTO>(responseContent);

                    // تحديث بيانات المستخدم الحالي
                    CurrentUser.Email = userName;
                    CurrentUser.Token = currentUser.Token;
                    CurrentUser.OwnerId = currentUser.ClientID;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login failed: {ex.Message}");
                return false;
            }
        }


        public static async Task<bool> UpdateUserInfoAsync(int clientId, string token, string userName, string password)
        {
            try
            {
                string url = "Users/PutInfo";

                var putRequest = new
                {
                    ClientId = clientId,
                    Token = token,
                    UserName = userName,
                    Password = password
                };

                var jsonContent = new StringContent(JsonConvert.SerializeObject(putRequest), Encoding.UTF8, "application/json");

                var response = await client.PutAsync(url, jsonContent);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // التعامل مع الأخطاء مثل مشكلات الشبكة
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public static async Task<bool> VerifyPasswordAsync(string email, string password)
        {
            try
            {
                string url = "Users/VerifyPassword";

                var verifyRequest = new
                {
                    Email = email,
                    Password = password
                };

                var jsonContent = new StringContent(JsonConvert.SerializeObject(verifyRequest), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, jsonContent);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // التعامل مع الأخطاء مثل مشكلات الشبكة
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}
