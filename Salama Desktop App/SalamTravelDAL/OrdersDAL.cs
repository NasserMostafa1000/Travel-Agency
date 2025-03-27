using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SalamaTravelDAL
{
    public static class OrdersDAL
    {
        public class RejectOrBadOrderRequest
        {
            public int OrderId { get; set; }
            public string Reason { get; set; }
        }
        public class OrderDTO
        {
            public int OrderID { get; set; }
            public string Visa_Name { get; set; }
            public string Order_Status { get; set; }
            public string OrderType { get; set; }
            public string FullName { get; set; }
            public string PhoneNumber { get; set; }
            public string RejectionReason { get; set; }
            public string Email { get; set; }
            public string Date { get; set; }
        }

        public static async Task<List<OrderDTO>> GetOrdersAsync(string token, int clientId)
        {
            try
            {
                string url = $"Orders/GetOrders?Token={Uri.EscapeDataString(token)}&ClientId={clientId}";

                var response = await Settings.HttpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var ordersInfo = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<List<OrderDTO>>(ordersInfo);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
        public static async Task<bool> AcceptOrderAsync(int orderId, int ownerId)
        {
            try
            {
              
                var response = await Settings.HttpClient.PutAsync($"Orders/AcceptOrder?OrderId={orderId}&OwnerId={ownerId}",null);

                // التحقق من حالة الاستجابة
                if (response.IsSuccessStatusCode)
                {
                    // إذا كان الطلب ناجحًا
                    return true;
                }
                else
                {
                    // إذا فشل الطلب
                    Console.WriteLine($"Failed to accept order: {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // التعامل مع الأخطاء
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
        public static async Task<bool> RejectOrderAsync(RejectOrBadOrderRequest R)
        {
            try
            {
                // URL API
                string url = $"Orders/RejectOrder?Token={Uri.EscapeDataString(CurrentUser.Token)}";

                // إعداد البيانات (DTO) لإرسالها
                var requestData = new
                {
                    OrderId = R.OrderId,
                    Reason = R.Reason
                };

                // تحويل البيانات إلى JSON باستخدام JsonConvert
                var jsonContent = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // إرسال الطلب PUT إلى الـ API
                var response = await Settings.HttpClient.PutAsync(url, content);

                // التحقق من حالة الاستجابة
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // التعامل مع الأخطاء
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
        public static async Task<bool> IsItBeenBadOrderSuccessAsync(RejectOrBadOrderRequest R,string token)
        {
            try
            {
                // URL API
                string url = $"Orders/BadOrder?Token={Uri.EscapeDataString(token)}";

                // إعداد البيانات (DTO) لإرسالها
                var requestData = new
                {
                    OrderId = R.OrderId,
                    Reason = R.Reason
                };

                // تحويل البيانات إلى JSON باستخدام JsonConvert
                var jsonContent = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // إرسال الطلب PUT إلى الـ API
                var response = await Settings.HttpClient.PutAsync(url, content);

                // التحقق من حالة الاستجابة
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // التعامل مع الأخطاء
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
        public static async Task<OrderDTO> GetOrderByIdAsync(string token, int clientId, int orderId)
        {
            try
            {
                string url = $"Orders/FindById?token={Uri.EscapeDataString(token)}&clientId={clientId}&orderId={orderId}";

                // إعداد طلب GET
                var request = new HttpRequestMessage(HttpMethod.Get, url);

                // إرسال الطلب واستلام الاستجابة
                var response = await Settings.HttpClient.SendAsync(request);

                // التحقق من حالة الاستجابة
                if (response.IsSuccessStatusCode)
                {
                    // قراءة البيانات من الاستجابة
                    var responseData = await response.Content.ReadAsStringAsync();
                    // تحويل البيانات إلى كائن OrderDTO
                    var order = JsonConvert.DeserializeObject<OrderDTO>(responseData);
                    return order;
                }
                else
                {
                    Console.WriteLine($"Failed to get order: {response.ReasonPhrase}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

    }
}

