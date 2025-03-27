using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SalamaTravelDTA
{
    public class IssuedVisaDAL
    {
        public class IssuedVisasDTO
        {
          public  int Id { get; set; }
          public  string VisaName { get; set; }
          public  string IssuedFrom { get; set; }
          public  string IssuedTo { get; set; }
          public  string Status { get; set; }
          
          public  string ClientFullName { get; set; }
          public  string ClientEmail { get; set; }
          public  string ClientPhoneNumber { get; set; }



        }
        public static async Task<List<IssuedVisasDTO>> GetIssuedVisasAsync(string Token, int OwnerId)
        {
            List<IssuedVisasDTO> issuedVisas = new List<IssuedVisasDTO>();

            string Query = "EXEC [GetIssuedVisa] @Token, @OwnerId";

            try
            {
                using (SqlConnection conn = new SqlConnection(Settings.ConnectionString))
                {
                    await conn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand(Query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Token", Token);
                        cmd.Parameters.AddWithValue("@OwnerId", OwnerId);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                issuedVisas.Add(new IssuedVisasDTO
                                {
                                    Id = reader.GetInt32(0),
                                    VisaName = reader.GetString(1),
                                    IssuedFrom = reader.GetDateTime(2).ToString("yyyy-MM-dd"),
                                    IssuedTo = reader.GetDateTime(3).ToString("yyyy-MM-dd"),
                                    Status = reader.GetString(4),
                                    ClientFullName = reader.GetString(5),
                                    ClientEmail = reader.GetString(6),
                                    ClientPhoneNumber = reader.GetString(7)
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error: {ex.Message}");
            }

            return issuedVisas;
        }
    }
}
