using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SalamaTravelDTA
{
    public class InternalBalanceDAL
    {
  
        public static class PaymentService
        {
            public class PayInternalBalanceRequest
            {
                public string Token { get; set; }
                public int VisaId { get; set; }
                public int OrderTypeId { get; set; }
                public int ClientId { get; set; }
                public decimal Amount { get; set; }
            }
            public static async Task<bool> PayWithInternalBalanceAsync(string Token, int ClientId, decimal Amount)
            {
                bool IsPayedSuccessfully = false;
                string StoredProcedure = "EXEC [PayWithInternalBalance] @Token, @ClientId, @Amount";

                try
                {
                    using (SqlConnection conn = new SqlConnection(Settings.ConnectionString))
                    {
                        await conn.OpenAsync();

                        using (SqlCommand cmd = new SqlCommand(StoredProcedure, conn))
                        {
                            // إضافة المعاملات كمعلمات مخزنة
                            cmd.Parameters.AddWithValue("@Token", Token);
                            cmd.Parameters.AddWithValue("@ClientId", ClientId);
                            cmd.Parameters.AddWithValue("@Amount", Amount);

                            // تنفيذ الإجراء المخزن
                            int rowsAffected = await cmd.ExecuteNonQueryAsync();

                            // تحقق من أن العملية أثرت على البيانات (مثلاً تحديث أو إدخال)
                            IsPayedSuccessfully = rowsAffected > 0;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }

                return IsPayedSuccessfully;
            }
            public class PayDuesRequest
            {
                public string Token { get; set; }
                public int ClientId { get; set; }
            }

            public static async Task<bool> PayDuesAsync(string Token, int ClientId)
            {
                bool IsPayedSuccessfully = false;
                string StoredProcedure = "EXEC [dbo].[PayDues] @Token, @ClientId";

                try
                {
                    using (SqlConnection conn = new SqlConnection(Settings.ConnectionString))
                    {
                        await conn.OpenAsync();

                        using (SqlCommand cmd = new SqlCommand(StoredProcedure, conn))
                        {
                            // إضافة المعاملات كمعلمات مخزنة
                            cmd.Parameters.AddWithValue("@Token", Token);
                            cmd.Parameters.AddWithValue("@ClientId", ClientId);

                            // تنفيذ الإجراء المخزن
                            int rowsAffected = await cmd.ExecuteNonQueryAsync();

                            // تحقق من أن العملية أثرت على البيانات (مثلاً تحديث أو إدخال)
                            IsPayedSuccessfully = rowsAffected > 0;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }

                return IsPayedSuccessfully;
            }
            public static async Task<bool> UpdateInternalBalanceAsync(string Token, int ClientId, decimal Amount)
            {
                bool IsUpdatedSuccessfully = false;
                string StoredProcedure = "EXEC [dbo].[UpdateInternalBalance] @Token, @ClientId, @Amount";

                try
                {
                    using (SqlConnection conn = new SqlConnection(Settings.ConnectionString))
                    {
                        await conn.OpenAsync();

                        using (SqlCommand cmd = new SqlCommand(StoredProcedure, conn))
                        {
                            // إضافة المعاملات كمعلمات مخزنة
                            cmd.Parameters.AddWithValue("@Token", Token);
                            cmd.Parameters.AddWithValue("@ClientId", ClientId);
                            cmd.Parameters.AddWithValue("@Amount", Amount);

                            // تنفيذ الإجراء المخزن
                            int rowsAffected = await cmd.ExecuteNonQueryAsync();

                            // تحقق من أن العملية أثرت على البيانات
                            IsUpdatedSuccessfully = rowsAffected > 0;
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }

                return IsUpdatedSuccessfully;
            }

        }


    }
}
