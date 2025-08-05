using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SalamaTravelDAL;

namespace SalamTravelDAL
{
    public static class InternalBalance
    {
        public class PayAndUpdateInternalBalanceRequest
        {
            public string Token { get; set; }
            public int ClientId { get; set; }
            public decimal Amount { get; set; }
        }
        public class PayDuesRequest
        {
            public string Token { get; set; }
            public int ClientId { get; set; }
        }
        public static async Task<bool> UpdateInternalBalanceAsync(PayAndUpdateInternalBalanceRequest requestData)
        {
            try
            {
                // بناء الـ URL الخاص بالـ API
                string url = "InternalBalance/UpdateInternalBalance";
              

                var jsonContent = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await Settings.HttpClient.PutAsync(url, content);

                // التحقق من حالة الاستجابة
                if (response.IsSuccessStatusCode)
                {
                    return true; // العملية ناجحة
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                // معالجة الأخطاء
                return false;
            }
        }
        public static async Task<bool> PayDuesAsync(PayDuesRequest requestData)
        {
            try
            {
                // بناء الـ URL الخاص بالـ API
                string url = "InternalBalance/payDues";

              

                // تحويل البيانات إلى JSON
                var jsonContent = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // إرسال الطلب POST
                var response = await Settings.HttpClient.PostAsync(url, content);

                // التحقق من حالة الاستجابة
                if (response.IsSuccessStatusCode)
                {
                    return true; // العملية ناجحة
                }
                else
                {
                    Console.WriteLine($"Failed to pay dues: {response.ReasonPhrase}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                // معالجة الأخطاء
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}
