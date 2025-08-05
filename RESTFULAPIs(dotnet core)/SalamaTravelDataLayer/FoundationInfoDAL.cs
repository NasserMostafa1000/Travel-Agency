using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SalamaTravelDTA
{
    public class FoundationInfoDAL
    {
        public class FoundationInfoDTO
        {
            public string CallNumber { get; set; }
            public string WhastAppNumber { get; set; }
            public string Email { get; set; }
            public string About { get; set; }
            public string Insta { get; set; }
            public string FaceBook { get; set; }
            public string TikTok { get; set; }


        }

        public static async Task<bool> UpdateFoundationInfoAsync(string PhoneNumber, string WhatsappNumber, string Email, string About,string Insta,string FaceBook,string TikTok)
        {
            bool IsUpdated = false;

            // SQL query for calling the stored procedure
            string Query = "EXEC UpdateFoundationInfo @WhatsappNumber, @PhoneNumber, @Email,@TikTok,@FaceBook,@Insta, @About";

            // Using block to ensure proper disposal of SqlConnection
            using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
            {
                try
                {
                    // Open the connection
                    await connection.OpenAsync();

                    // Create the SQL command
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        // Add parameters to the command
                        command.Parameters.AddWithValue("@WhatsappNumber", WhatsappNumber);
                        command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                        command.Parameters.AddWithValue("@Email", Email);
                        command.Parameters.AddWithValue("@About", About);
                        command.Parameters.AddWithValue("@Insta", Insta);
                        command.Parameters.AddWithValue("@FaceBook", FaceBook);
                        command.Parameters.AddWithValue("@TikTok", TikTok);


                        // Execute the command
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        // Check if the update was successful
                        IsUpdated = rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                  
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            return IsUpdated;
        }
        public static async Task<FoundationInfoDTO> GetFoundationInfoAsync()
        {
            FoundationInfoDTO foundationInfo = null;

            // SQL query to retrieve foundation info
            string Query = "SELECT * FROM FoundationInfo";

            // Using block to ensure proper disposal of SqlConnection
            using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
            {
                try
                {
                    // Open the connection
                    await connection.OpenAsync();

                    // Create the SQL command
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        // Execute the command and read the data
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                while (await reader.ReadAsync())
                                {
                                    foundationInfo = new FoundationInfoDTO
                                    {
                                        CallNumber = reader["CallNumber"] as string,
                                        WhastAppNumber = reader["WhatsAppNumber"] as string,
                                        Email = reader["Email"] as string,
                                        About = reader["Information"] as string,
                                        Insta = reader["Insta"] as string,
                                        FaceBook = reader["FaceBook"] as string,
                                        TikTok = reader["Tiktok"] as string



                                    };
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle the exception
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            return foundationInfo;
        }

    }
}
