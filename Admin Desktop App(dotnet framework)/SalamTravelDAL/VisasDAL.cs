using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SalamaTravelDAL;

namespace SalamTravelDAL
{
    public static class VisasDAL
    {
        public class VisaDTO
        {
            public int VisaId { get; set; }
            public string Name { get; set; }
            public decimal IssuancePrice { get; set; }
            public decimal RenewalPrice { get; set; }
            public string ImagePath { get; set; } = "";
            public byte Period { get; set; }
            public string MoreDetails { get; set; }
        }
        public static async Task<bool> AddNewVisaAsync(VisaDTO dto, string token)
        {
            try
            {
                string url = $"Visas/PostVisa?token={Uri.EscapeDataString(token)}";



                var jsonContent = JsonConvert.SerializeObject(dto);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await Settings.HttpClient.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to add visa: {response.ReasonPhrase}");
                    return false;  // إذا فشلت العملية
                }
            }
            catch (Exception ex)
            {
                // التعامل مع الأخطاء
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
        public static async Task<bool> UpdateVisaAsync(VisaDTO dto, string token)
        {
            try
            {
                string url = $"Visas/UpdateVisaInfo?token={Uri.EscapeDataString(token)}";

                var jsonContent = JsonConvert.SerializeObject(dto);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await Settings.HttpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorMsg = await response.Content.ReadAsStringAsync();
                    throw new Exception($" {errorMsg.ToString()}");
                }
            }
            catch (Exception ex)
            {
                // ممكن ترجع الرسالة الأصلية أو تغلفها برسالة أوضح
                throw new Exception($"{ex.Message}");
            }
        }
        public static async Task<string> UploadVisaImageAsync(MemoryStream imageFile, string token)
        {
            try
            {
                // تحقق من وجود التوكن
                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("Token is required.");
                    return "";
                }

                string url = $"Visas/UploadVisaImage?token={Uri.EscapeDataString(token)}";

                // التحقق من وجود الملف
                if (imageFile == null || imageFile.Length == 0)
                {
                    Console.WriteLine("No file uploaded.");
                    return "";
                }

                // إعداد محتوى MultipartFormDataContent
                var content = new MultipartFormDataContent();

                // تحويل الصورة إلى مصفوفة بايت وإضافتها للمحتوى
                var byteArray = imageFile.ToArray();
                var byteArrayContent = new ByteArrayContent(byteArray);
                byteArrayContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                content.Add(byteArrayContent, "imageFile", "visa_image.png");

                // إرسال الطلب
                var response = await Settings.HttpClient.PostAsync(url, content);

                // التحقق من حالة الاستجابة
                if (response.IsSuccessStatusCode)
                {
                    // قراءة محتوى الرد كـ JSON أو نص
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // قراءة رسالة الخطأ من الرد
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to upload visa image: {response.ReasonPhrase}, Details: {errorContent}");
                    return "";
                }
            }
            catch (Exception ex)
            {
                // التعامل مع الأخطاء
                return "";
            }
        }


            
        public static async Task<bool> UpdateVisaImageAsync(MemoryStream imageFile, string token, int visaId)
        {
            try
            {
                string url = $"Visas/UpdateVisaImage?token=${Uri.EscapeDataString(token)}&visaId={visaId}";

                // التحقق من وجود الملف
                if (imageFile == null || imageFile.Length == 0)
                {
                    return false;
                }

                // إعداد الـ HttpContent لإرسال الصورة
                var content = new MultipartFormDataContent();

                // تحويل الصورة إلى byte[]
                var byteArray = imageFile.ToArray();

                // إضافة الصورة إلى المحتوى
                var byteArrayContent = new ByteArrayContent(byteArray);
                byteArrayContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                content.Add(byteArrayContent, "imageFile", "visa_image.png");



                var response = await Settings.HttpClient.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    return true; // إذا كانت العملية ناجحة
                }
                else
                {
                    Console.WriteLine($"Failed to update visa image: {response.ReasonPhrase}");
                    return false; // إذا فشلت العملية
                }
            }
            catch (Exception ex)
            {
                // التعامل مع الأخطاء
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
        public static async Task<List<VisasDAL.VisaDTO>> GetVisasAsync()
        {
            try
            {
                string url = $"Visas/GetAllVisas?token={Uri.EscapeUriString(CurrentUser.Token)}";

                // إنشاء طلب GET
                var request = new HttpRequestMessage(HttpMethod.Get, url);

                var response = await Settings.HttpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    // قراءة البيانات من الاستجابة
                    var responseData = await response.Content.ReadAsStringAsync();
                    // تحويل البيانات إلى قائمة من VisaDTO
                    var visas = JsonConvert.DeserializeObject<List<VisasDAL.VisaDTO>>(responseData);
                    return visas;
                }
                else
                {
                    Console.WriteLine($"Failed to get visas: {response.ReasonPhrase}");
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
