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
    public static class IssuedVisas
    {
        public class IssuedVisasDTO
        {
            public int Id { get; set; }
            public string VisaName { get; set; }
            public string IssuedFrom { get; set; }
            public string IssuedTo { get; set; }
            public string Status { get; set; }

            public string ClientFullName { get; set; }
            public string ClientEmail { get; set; }
            public string ClientPhoneNumber { get; set; }



        }
        public static async Task<List<IssuedVisasDTO>> GetIssuedVisasAsync(string token, int ownerId)
        {
            try
            {
                // تحقق من وجود التوكن ومعرف المالك
                if (string.IsNullOrEmpty(token) || ownerId <= 0)
                {
                    Console.WriteLine("Invalid token or ownerId.");
                    return null;
                }

                var response = await Settings.HttpClient.GetAsync($"IssuedVisa/GetIssuedVisas?Token={Uri.EscapeDataString(token)}&OwnerId={ownerId}");

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(responseData))
                    {
                        var visas = JsonConvert.DeserializeObject<List<IssuedVisasDTO>>(responseData);
                        return visas;
                    }
                    else
                    {
                        Console.WriteLine("No data received from the API.");
                        return null;
                    }
                }
                else
                {
                    // قراءة محتوى الخطأ
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to get issued visas: {response.ReasonPhrase}. Details: {errorContent}");
                    return null;
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