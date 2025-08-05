using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SalamaTravelDTA
{
   
    public class VisasDAL
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
        public static async Task<List<VisaDTO>> GetVisasAsync()
        {
            List<VisaDTO> visas = new List<VisaDTO>(); ;
            try
            {
                using (SqlConnection conn = new SqlConnection(Settings.ConnectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("GetVisas", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                visas.Add(new VisaDTO
                                {
                                    VisaId = reader.GetInt32(reader.GetOrdinal("VisaId")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    IssuancePrice = reader.GetDecimal(reader.GetOrdinal("IssuancePrice")),
                                    RenewalPrice = reader.GetDecimal(reader.GetOrdinal("renewalPrice")),
                                    ImagePath =  reader.GetString(reader.GetOrdinal("VisaImage")),
                                    Period = reader.GetByte(reader.GetOrdinal("Period")),
                                    MoreDetails = reader.GetString(reader.GetOrdinal("MoreDetails"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // معالجة الأخطاء
                Console.WriteLine($"Error: {ex.Message}");
            }

            return visas;
        }
        public static async Task<int> PostVisaAsync(string Token, string Name, decimal IssuancePrice, decimal RenewalPrice, string ImagePath, byte Period, string MoreDetails)
        {
            int visaId = -1; // تعيين قيمة افتراضية في حال فشل العملية

            // نص الاستعلام
            string query = "EXEC PostVisa @Token, @Name, @IssuancePrice, @Period, @ImagePath, @RenewalPrice, @MoreDetails";

            try
            {
                // الاتصال بقاعدة البيانات باستخدام سلسلة الاتصال المخزنة في Settings
                using (SqlConnection conn = new SqlConnection(Settings.ConnectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // تمرير المعاملات
                        cmd.Parameters.AddWithValue("@Token", Token);
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@IssuancePrice", IssuancePrice);
                        cmd.Parameters.AddWithValue("@RenewalPrice", RenewalPrice);
                        cmd.Parameters.AddWithValue("@ImagePath", ImagePath);
                        cmd.Parameters.AddWithValue("@Period", Period);
                        cmd.Parameters.AddWithValue("@MoreDetails", MoreDetails);

                        // تنفيذ الاستعلام وجلب القيمة التي تم إرجاعها (VisaId)
                        visaId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                    }
                }
            }
            catch (Exception ex)
            {
                // معالجة الأخطاء
                Console.WriteLine($"Error: {ex.Message}");
            }

            return visaId; // إرجاع رقم الفيزا المضاف أو -1 إذا فشلت العملية
        }
        public static async Task<bool> UpdateVisaASync(int VisaId, string Token, string Name, decimal IssuancePrice, decimal RenewalPrice,  byte Period, string MoreDetails)
        {
            bool isUpdated = false;

            // نص الاستعلام
            string query = "EXEC PutVisa @Token, @VisaId, @Name, @IssuancePrice, @Period,@RenewalPrice, @MoreDetails";

            try
            {
                // الاتصال بقاعدة البيانات
                using (SqlConnection conn = new SqlConnection(Settings.ConnectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // تمرير المعاملات
                        cmd.Parameters.AddWithValue("@Token", Token);
                        cmd.Parameters.AddWithValue("@VisaId", VisaId);
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@IssuancePrice", IssuancePrice);
                        cmd.Parameters.AddWithValue("@RenewalPrice", RenewalPrice);
                        cmd.Parameters.AddWithValue("@Period", Period);
                        cmd.Parameters.AddWithValue("@MoreDetails", MoreDetails);

                        // تنفيذ الأمر والحصول على عدد الصفوف المتأثرة
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        // إذا تم تعديل صف واحد على الأقل، تكون العملية ناجحة
                        isUpdated = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // معالجة الأخطاء
                throw;
            }

            return isUpdated;
        }
        public static async Task<string> GetVisaImageWithVisaIdAsync(int VisaId)
        {
            string ImagePath = string.Empty;
            string query = "SELECT VisaImage FROM Visas WHERE VisaId = @VisaId";

            try
            {
                using (SqlConnection conn = new SqlConnection(Settings.ConnectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // تمرير المعاملات
                        cmd.Parameters.AddWithValue("@VisaId", VisaId);

                        // تنفيذ الاستعلام
                        var result = await cmd.ExecuteScalarAsync();

                        // التحقق من النتيجة
                        if (result != null && result != DBNull.Value)
                        {
                            ImagePath = result.ToString(); // تحويل القيمة النصية إلى String
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // معالجة الأخطاء
                Console.WriteLine($"Error: {ex.Message}");
            }

            return ImagePath; // إرجاع مسار الصورة أو قيمة فارغة إذا لم يتم العثور على الصورة
        }
        public static async Task<bool> UpdateVisaImagePathAsync(int VisaId, string ImagePath)
        {
            bool isUpdated = false;
            string query = "UPDATE Visas SET VisaImage = @ImagePath WHERE VisaId = @VisaId";

            try
            {
                // الاتصال بقاعدة البيانات باستخدام سلسلة الاتصال المخزنة في Settings
                using (SqlConnection conn = new SqlConnection(Settings.ConnectionString))
                {
                    await conn.OpenAsync();

                    // إنشاء الأمر
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // إضافة المعاملات
                        cmd.Parameters.AddWithValue("@ImagePath", ImagePath);
                        cmd.Parameters.AddWithValue("@VisaId", VisaId);

                        // تنفيذ الاستعلام
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        // إذا كانت العملية ناجحة وتم تحديث صف واحد أو أكثر
                        isUpdated = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // معالجة الأخطاء
                Console.WriteLine($"Error: {ex.Message}");
            }

            return isUpdated; // إرجاع true إذا تم التحديث بنجاح، false إذا فشلت العملية
        }


    }
}
