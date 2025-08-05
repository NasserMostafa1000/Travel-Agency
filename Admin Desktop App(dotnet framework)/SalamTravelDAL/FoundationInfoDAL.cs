using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SalamaTravelDAL;
using static SalamTravelDAL.FoundationInfoDAL;

namespace SalamTravelDAL
{
    public static  class FoundationInfoDAL
    {
        public  class FoundationInfoDTO
        {
            public string CallNumber { get; set; }
            public string WhastAppNumber { get; set; }
            public string Email { get; set; }
            public string About { get; set; }
            public string Insta { get; set; }
            public string FaceBook { get; set; }
            public string TikTok { get; set; }
        }
        public static async Task<bool> UpdateFoundationInformationAsync(FoundationInfoDTO foundationInfo)
        {
            try
            {
                // بناء الـ URL الخاص بالـ API
                string url = "FoundationInfo/UpdateFoundationInfo";

                // تحويل البيانات إلى JSON
                var jsonContent = JsonConvert.SerializeObject(foundationInfo);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // إرسال الطلب PUT
                var response = await Settings.HttpClient.PutAsync(url, content);

                // التحقق من حالة الاستجابة
                if (response.IsSuccessStatusCode)
                {
                    return true; // العملية ناجحة
                }
                else
                {
                    Console.WriteLine($"Failed to update foundation information: {response.ReasonPhrase}");
                    return false; // العملية فشلت
                }
            }
            catch (Exception ex)
            {
                // معالجة الأخطاء
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
        public static async Task<FoundationInfoDTO> GetFoundationInformationAsync()
        {
            try
            {
                // بناء الـ URL الخاص بالـ API
                string url = "FoundationInfo/FoundationInformation";

                // إرسال الطلب GET
                var response = await Settings.HttpClient.GetAsync(url);

                // التحقق من حالة الاستجابة
                if (response.IsSuccessStatusCode)
                {
                    // قراءة البيانات من الاستجابة وتحويلها إلى الكائن المطلوب
                    var responseData = await response.Content.ReadAsStringAsync();
                    var foundationInfo = JsonConvert.DeserializeObject<FoundationInfoDTO>(responseData);

                    return foundationInfo; // إرجاع الكائن
                }
                else
                {
                    return null; // إذا لم تنجح العملية
                }
            }
            catch (Exception ex)
            {
                // معالجة الأخطاء
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

    }
}
