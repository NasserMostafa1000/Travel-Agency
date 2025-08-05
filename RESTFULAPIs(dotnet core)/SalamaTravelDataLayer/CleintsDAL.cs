using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




using Microsoft.Data.SqlClient;

namespace SalamaTravelDTA
{
  
    public static class Clients


    {
        //Clients Dto Refers to The data transfer ogbject 
        public class ClientsDTO
        {
            public int ClientID { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string PassportImagePath { get; set; }
            public string PersonalImagePath { get; set; }
            public decimal Balance { get; set; }
        }
        public static async Task<int> PostClientAsync(string FirstName, string MidName, string Email, string PhoneNumber, string LastName, string Password, string PassportImagePath = "")
        {
            int clientId = -1;  

            try
            {
                // Establish a connection to the database
                using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
                {
                    // SQL query to execute the stored procedure and get the identity of the inserted client
                    string query = "EXEC [PostClient] @FirstName, @MidName, @LastName, @PhoneNumber, @PassportImagePath, @Email, @Password; SELECT CAST(SCOPE_IDENTITY() AS INT)";

                    // Create a new SQL command object
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to the SQL command
                        command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = FirstName;
                        command.Parameters.Add("@MidName", SqlDbType.NVarChar).Value = MidName;
                        command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Email;
                        command.Parameters.Add("@PhoneNumber", SqlDbType.NVarChar).Value = PhoneNumber;
                        command.Parameters.Add("@PassportImagePath", SqlDbType.NVarChar).Value = PassportImagePath;
                        command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = LastName;
                        command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = Password;

                        // Open the connection
                        await connection.OpenAsync();

                        // Execute the stored procedure and get the scalar result (the inserted client ID)
                        var resultScalar = await command.ExecuteScalarAsync();

                        // If the result is not null, parse it as an integer
                        if (resultScalar != null && int.TryParse(resultScalar.ToString(), out int id))
                        {
                            clientId = id;  
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return clientId;  // Return the client ID or null if the operation failed
        }

        public static async Task<List<ClientsDTO>> GetAllClientsAsync(string Token, int ClientID)
        {
            List<ClientsDTO> clients = new List<ClientsDTO>();

            // Define the SQL query
            string query = "EXEC GetAllClients @Token, @ClientID";

            try
            {
                // Create a new SQL connection using your connection string
                using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
                {
                    // Create the SQL command to execute
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters to the SQL command
                        command.Parameters.AddWithValue("@Token", Token);
                        command.Parameters.AddWithValue("@ClientID", ClientID);

                        // Open the connection
                        await connection.OpenAsync();

                        // Execute the query and get the result set (assuming it returns rows)
                        using (SqlDataReader reader =await command.ExecuteReaderAsync())
                        {
                            // Loop through the rows in the result set
                            while ( await reader.ReadAsync())
                            {
                                // Map the data to a ClientsDTO object
                                ClientsDTO client = new ClientsDTO
                                {
                                    ClientID = reader.GetInt32(reader.GetOrdinal("ClientId")),
                                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    PassportImagePath = reader.GetString(reader.GetOrdinal("PassportImage")),
                                    PersonalImagePath = reader.IsDBNull(reader.GetOrdinal("PersonalImagePath"))
                            ? ""
                            : reader.GetString(reader.GetOrdinal("PersonalImagePath")),
                                    Balance = reader.GetDecimal(reader.GetOrdinal("balance"))
                                    ,Email= reader.GetString(reader.GetOrdinal("Email"))
                                };

                                // Add the client to the list
                                clients.Add(client);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new List<ClientsDTO>();
            }

            return clients;
        }
        public static async Task<bool> PutClientASync(int ClientId, string Token, string FirstName, string MidName, string LastName, string PhoneNumber)
        {
            bool IsUpdated = false; // المبدئي هو false، يعني لم يتم التحديث بعد

            try
            {
                // التأكد من صحة المعاملات المدخلة
                if (ClientId <= 0 || string.IsNullOrEmpty(Token))
                {
                    throw new ArgumentException("Invalid input parameters.");
                }

                // الاتصال بقاعدة البيانات
                using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
                {
                    // استعلام SQL لتنفيذ الإجراء المخزن
                    string query = "EXEC PutClient @ClientId, @Token, @FirstName, @MidName, @LastName, @PhoneNumber";

                    // إنشاء أمر SQL جديد
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // إضافة المعاملات إلى الأمر
                        command.Parameters.AddWithValue("@ClientId", ClientId);
                        command.Parameters.AddWithValue("@Token", Token);
                        command.Parameters.AddWithValue("@FirstName", FirstName);
                        command.Parameters.AddWithValue("@MidName", MidName);
                        command.Parameters.AddWithValue("@LastName", LastName);
                        command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);

                        await connection.OpenAsync();

                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        IsUpdated = rowsAffected > 0; 
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                IsUpdated = false;
            }

            return IsUpdated;
        }
        public static async Task<List<ClientsDTO>> GetClientByNameAsync(string Name,string Token)
        {
           List<ClientsDTO> client = new List<ClientsDTO>(); 
            string query = "EXEC FindClientByName @Name ,@Token"; 

            try
            {
                using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name;
                        command.Parameters.Add("@Token", SqlDbType.NVarChar).Value = Token;

                        // فتح الاتصال
                        await connection.OpenAsync();

                        // تنفيذ الاستعلام واسترجاع النتائج
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            // التأكد من وجود بيانات للعميل
                            while (await reader.ReadAsync())
                            {
                                // تعيين قيم بيانات العميل إلى الكائن ClientDTO
                                client.Add ( new ClientsDTO
                                {
                                    ClientID = reader.GetInt32(reader.GetOrdinal("ClientId")),
                                    FullName = reader.IsDBNull(reader.GetOrdinal("fullName"))
             ? "Unknown"
             : reader.GetString(reader.GetOrdinal("FullName")),
                                    Email = reader.IsDBNull(reader.GetOrdinal("Email"))
             ? "no-email@example.com"
             : reader.GetString(reader.GetOrdinal("Email")),
                                    PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber"))
             ? "0000000000"
             : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    PersonalImagePath = reader.IsDBNull(reader.GetOrdinal("PersonalImagePath"))
             ? "/default-image.jpg"
             : reader.GetString(reader.GetOrdinal("PersonalImagePath")),
                                    PassportImagePath = reader.IsDBNull(reader.GetOrdinal("PassportImage"))
             ? "/default-image.jpg"
             : reader.GetString(reader.GetOrdinal("PersonalImagePath")),
                                    Balance = reader.IsDBNull(reader.GetOrdinal("Balance"))
             ? 0.0m
             : reader.GetDecimal(reader.GetOrdinal("Balance"))
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
   
                throw new Exception(ex.Message.ToString());
            }

            return client; 
        }
        public static async Task<ClientsDTO> GetClientByIdAsync(int Id, string Token)
        {
            ClientsDTO client = null; 
           
            string query = "EXEC FindClientById @Id ,@Token";

            try
            {
                using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@Id", SqlDbType.NVarChar).Value = Id;
                        command.Parameters.Add("@Token", SqlDbType.NVarChar).Value = Token;
                        await connection.OpenAsync();
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                client = new ClientsDTO
                                {
                                    ClientID = reader.GetInt32(reader.GetOrdinal("ClientId")),
                                    FullName = reader.IsDBNull(reader.GetOrdinal("fullName"))
             ? "Unknown"
             : reader.GetString(reader.GetOrdinal("FullName")),
                                    Email = reader.IsDBNull(reader.GetOrdinal("Email"))
             ? "no-email@example.com"
             : reader.GetString(reader.GetOrdinal("Email")),
                                    PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber"))
             ? "0000000000"
             : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    PersonalImagePath = reader.IsDBNull(reader.GetOrdinal("PersonalImagePath"))
             ? "/default-image.jpg"
             : reader.GetString(reader.GetOrdinal("PersonalImagePath")),
                                    Balance = reader.IsDBNull(reader.GetOrdinal("Balance"))
             ? 0.0m
             : reader.GetDecimal(reader.GetOrdinal("Balance"))
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message.ToString());
            }

            return client;
        }

        public static async Task<bool> UpdatePassportImageASync(int ClientId, string ImagePath)
        {
            bool isUpdated = false;

            try
            {
                // التحقق من المعاملات المدخلة
                if (ClientId <= 0  || string.IsNullOrEmpty(ImagePath))
                {
                    throw new ArgumentException("معاملات غير صالحة.");
                }

                // الاتصال بقاعدة البيانات
                using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
                {
                    // استعلام SQL لتحديث مسار الصورة
                    string query = "EXEC UpdatePassportImage @ImagePath, @ClientId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // إضافة المعاملات إلى الأمر
                        command.Parameters.Add(new SqlParameter("@ClientId", SqlDbType.Int) { Value = ClientId });
                        command.Parameters.Add(new SqlParameter("@ImagePath", SqlDbType.NVarChar) { Value = ImagePath });

                        await connection.OpenAsync();

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        isUpdated = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                isUpdated = false;
            }

            return isUpdated;
        }
        public static async Task<bool> UpdatePersonalImageAsync(int ClientId, string ImagePath)
        {
            bool isUpdated = false;

            try
            {
                // التحقق من المعاملات المدخلة
                if (ClientId <= 0 ||  string.IsNullOrEmpty(ImagePath))
                {
                    throw new ArgumentException("معاملات غير صالحة.");
                }

                // الاتصال بقاعدة البيانات
                using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
                {
                    // استعلام SQL لتحديث مسار الصورة
                    string query = "EXEC [UpdatePersonlaImage] @ImagePath, @ClientId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // إضافة المعاملات إلى الأمر
                        command.Parameters.Add(new SqlParameter("@ClientId", SqlDbType.Int) { Value = ClientId });
                        command.Parameters.Add(new SqlParameter("@ImagePath", SqlDbType.NVarChar) { Value = ImagePath });

                        await connection.OpenAsync();

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        isUpdated = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                isUpdated = false;
            }

            return isUpdated;
        }
        public static async Task<string> GetPassportImageWithClientIdAsync(int ClientId)
        {
            string ImagePath = string.Empty; // تهيئة المتغير كقيمة فارغة
            string query = "SELECT PassportImage FROM Clients WHERE ClientId = @ClientId";

            try
            {
                using (SqlConnection conn = new SqlConnection(Settings.ConnectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // تمرير المعاملات
                        cmd.Parameters.AddWithValue("@ClientId", ClientId);

                        // تنفيذ الاستعلام
                        var result = await cmd.ExecuteScalarAsync();

                        // التحقق إذا كانت النتيجة ليست null
                        if (result != null && result != DBNull.Value)
                        {
                            ImagePath = result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // معالجة الأخطاء
                Console.WriteLine($"Error: {ex.Message}");
            }

            return ImagePath; // إرجاع المسار (أو قيمة فارغة إذا لم يتم العثور على صورة)
        }
        public static async Task<string> GetPersonalImageWithClientIdAsync(int ClientId)
        {
            string ImagePath = string.Empty; // تهيئة المتغير كقيمة فارغة
            string query = "SELECT PersonalImagePath FROM Clients WHERE ClientId = @ClientId";

            try
            {
                using (SqlConnection conn = new SqlConnection(Settings.ConnectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // تمرير المعاملات
                        cmd.Parameters.AddWithValue("@ClientId", ClientId);

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



    }

}
