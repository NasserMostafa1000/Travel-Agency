using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SalamaTravelDAL;
using static SalamTravelDAL.ClientsDAL;

namespace SalamTravelDAL
{
    public static class ClientsDAL
    {
        public class ClientDTO
        {
            public int ClientID { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public decimal balance { get; set; }
            public string PhoneNumber { get; set; }
            public string PersonalImagePath { get; set; }
            public string PassportImagePath { get; set; }

        }
        public static async Task<List<ClientDTO>> FetchClientsDataAsync(string token, int clientId)
        {
            try
            {
                // تحديد عنوان URL الخاص بـ API
                string url = $"Clients/FetchClientsData?Token={Uri.EscapeDataString(token)}&ClientId={clientId}";

                // إرسال طلب GET للحصول على بيانات العملاء
                var response = await Settings.HttpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    // قراءة البيانات المسترجعة من API
                    var content = await response.Content.ReadAsStringAsync();

                    // تحويل البيانات من JSON إلى قائمة من الكائنات ClientsDTO
                    return JsonConvert.DeserializeObject<List<ClientDTO>>(content);
                }
                else
                {
                    // إذا كانت استجابة API غير ناجحة
                    return null;
                }
            }
            catch (Exception ex)
            {
                // التعامل مع الأخطاء
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
        public static async Task<int> PostClientAsync(ClientDTO dto)
        {
            try
            {
                // تحديد عنوان URL
                string url = "Clients/PostNewClient";

                // تحويل الـ DTO إلى JSON
                var jsonContent = JsonConvert.SerializeObject(dto);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // إرسال طلب POST مع بيانات العميل
                var response = await Settings.HttpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var id = JsonConvert.DeserializeObject<int>(result);
                    return id; // إرجاع الـ ID
                }
                else
                {
                    // إذا كانت الاستجابة غير ناجحة
                    return -1;
                }
            }
            catch (Exception ex)
            {
                // التعامل مع الأخطاء
                Console.WriteLine($"Error: {ex.Message}");
                return -1;
            }
        }
        public static async Task<bool> PutClientAsync(ClientDTO dto, string token)
        {
            try
            {
                string url = "Clients/PutClient";

                var requestData = new
                {
                    Dto = dto,
                    Token = token
                };

                var jsonContent = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await Settings.HttpClient.PutAsync(url, content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // التعامل مع الأخطاء
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }


        }
        public class GetByNameRequest
        {
            public string ClientNamePref { get; set; }
            public string Token { get; set; }
        }

        /// <summary>
        /// جلب بيانات العميل بالاسم باستخدام API
        /// </summary>
        /// <param name="clientNamePref">جزء من اسم العميل</param>
        /// <param name="token">التوكن للتحقق من الصلاحية</param>
        /// <returns>كائن ClientDTO أو null إذا لم يتم العثور على العميل</returns>
        /// <summary>
        /// جلب بيانات العميل بالاسم باستخدام API
        /// </summary>
        /// <param name="clientNamePref">جزء من اسم العميل</param>
        /// <param name="token">التوكن للتحقق من الصلاحية</param>
        /// <returns>كائن ClientDTO أو null إذا لم يتم العثور على العميل</returns>
        public static async Task<List<ClientDTO>> FindClientByNameAsync(string clientNamePref, string token)
        {
            try
            {
                // إعداد نقطة النهاية وبيانات الطلب
                string endpoint = "Clients/FindClientByName";
                var requestData = new GetByNameRequest
                {
                    ClientNamePref = clientNamePref,
                    Token = token
                };

                // تحويل بيانات الطلب إلى JSON
                var jsonContent = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // إرسال طلب POST إلى واجهة API
                var response = await Settings.HttpClient.PostAsync(endpoint, content);

                // التحقق من نجاح الاستجابة
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();

                    // محاولة تحويل الاستجابة إلى قائمة من ClientDTO
                    try
                    {
                        return JsonConvert.DeserializeObject<List<ClientDTO>>(responseData);
                    }
                    catch (JsonException jsonEx)
                    {
                        throw new Exception("Failed to deserialize response data. Ensure the API response matches the expected format.", jsonEx);
                    }
                }
                else
                {
                    // قراءة محتوى الاستجابة لتوضيح سبب الفشل
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API call failed. Status code: {response.StatusCode}. Response: {errorContent}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                throw new Exception("Failed to send the HTTP request. Check the network connection or API endpoint.", httpEx);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while processing the request.", ex);
            }
        }
    }
}