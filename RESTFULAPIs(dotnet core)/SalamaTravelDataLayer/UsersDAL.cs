using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SalamaTravelDTA
{
    public class UsersDAL
    {
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

        public static async Task<UserDTO> LoginAsync(string userName, string password)
        {
            // الاتصال بقاعدة البيانات
            string query = "EXEC [LogIn] @UserName, @Password";
            UserDTO User = null;
            // إنشاء كائن SqlConnection
            using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
            {
                try
                {
                    // فتح الاتصال
                    await connection.OpenAsync();

                    // إعداد الكود SqlCommand مع المعاملات
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        // إضافة المعاملات لتجنب SQL Injection
                        cmd.Parameters.AddWithValue("@UserName", userName);
                        cmd.Parameters.AddWithValue("@Password", password);

                        // تنفيذ الاستعلام واسترجاع النتيجة
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            // التحقق إذا كانت هناك نتائج
                            if (await reader.ReadAsync())
                            {
                                // إنشاء كائن UserDTO وملء القيم
                                UserDTO user = new UserDTO
                                {
                                    ClientID = reader.GetInt32(reader.GetOrdinal("ClientID")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    MidName = reader.IsDBNull(reader.GetOrdinal("MidName")) ? null : reader.GetString(reader.GetOrdinal("MidName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    Token = reader.GetString(reader.GetOrdinal("Token")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    PassportImagePath = reader.IsDBNull(reader.GetOrdinal("PassportImage")) ? "" : reader.GetString(reader.GetOrdinal("PassportImage")),
                                    PersonalImagePath = reader.IsDBNull(reader.GetOrdinal("PersonalImagePath")) ? "" : reader.GetString(reader.GetOrdinal("PersonalImagePath")),
                                    Balance = reader.GetDecimal(reader.GetOrdinal("Balance"))
                                };

                                User = user;
                            }
                            else
                            {
                                return null; // إذا لم يتم العثور على بيانات
                            }
                        }
                    }
                    return User;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                     
                }


            }
        }
        public static async Task<bool> UpdateUserInfoAsync(int ClientId, string Token, string UserName, string Password)
        {
            bool IsUpdated = false;

            // التحقق من المعاملات
            if (string.IsNullOrEmpty(ClientId.ToString()) || string.IsNullOrEmpty(Token) || string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                return false; 
            }

          
            using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync(); 

                    // استعلام SQL لتحديث معلومات المستخدم
                    string query = "EXEC PutUser @ClientId, @Token, @UserName, @Password";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // تمرير المعاملات للاستعلام
                        command.Parameters.AddWithValue("@ClientId", ClientId);
                        command.Parameters.AddWithValue("@Token", Token);
                        command.Parameters.AddWithValue("@UserName", UserName);
                        command.Parameters.AddWithValue("@Password", Password);

                        // تنفيذ الاستعلام
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        // إذا تم تحديث السجلات بنجاح
                        IsUpdated = rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while updating user info: {ex.Message}");
                    // التعامل مع الأخطاء بشكل مناسب (مثل تسجيل الأخطاء)
                }
            }

            return IsUpdated; // إرجاع true إذا تم التحديث بنجاح، false إذا فشل
        }
        public static async Task<bool> IsPasswordValidAsync(string email, string password)
        {
            using (var connection = new SqlConnection(Settings.ConnectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("IsThisPasswordBelongsToThisEmail", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // إضافة المعاملات للإجراء المخزن
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Email", email);

                    try
                    {
                        // تنفيذ الإجراء المخزن
                        await command.ExecuteNonQueryAsync();
                        // إذا تم تنفيذ الأمر بنجاح، تعني كلمة المرور صحيحة
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        // إذا تم رفع استثناء بسبب فشل التحقق من كلمة المرور
                        // يمكننا التحقق من رمز الخطأ أو الرسالة
                        if (ex.Number == 50004) // الكود الذي يتم إرجاعه في حالة كلمة مرور غير صحيحة
                        {
                            return false; // كلمة المرور غير صحيحة
                        }
                        else
                        {
                            throw; // إعادة رفع الاستثناء إذا كان سبب آخر
                        }
                    }
                }
            }
        }
        public static async Task<bool> ForgetPasswordAsync( string UserName, string Password)
        {
            bool IsUpdated = false;

            if ( string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
            {
                return false;
            }


            using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string query = "EXEC ForgetPassword  @UserName, @Password";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
            
                        command.Parameters.AddWithValue("@UserName", UserName);
                        command.Parameters.AddWithValue("@Password", Password);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        IsUpdated = rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
            }

            return IsUpdated;
        }


    }
}




