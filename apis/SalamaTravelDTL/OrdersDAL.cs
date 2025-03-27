using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SalamaTravelDTA
{
    public class OrdersDAL
    {
        public class OrderDTO
        {
            public int OrderID { get; set; }
            public string Visa_Name { get; set; }
            public string Order_Status { get; set; }
            public string OrderType { get; set; }
            public string FullName { get; set; }
            public string PhoneNumber { get; set; }
            public string RejectionReason { get; set; }
            public string Email { get; set; }
            public string Date { get; set; }
        }
        public static async Task<List<OrderDTO>> GetOrdersAsync(string Token, int ClientId)
        {
            string query = "EXEC GetOrders @Token, @ClientId";

            var orders = new List<OrderDTO>();

            using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Token", Token);
                    command.Parameters.AddWithValue("@ClientId", ClientId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            orders.Add(new OrderDTO
                            {
                                OrderID = reader.GetInt32(0),
                                Visa_Name = reader.GetString(1),
                                Order_Status = reader.GetString(2),
                                OrderType = reader.GetString(3),
                                FullName = reader.GetString(4),
                                PhoneNumber = reader.GetString(5),
                                Email = reader.GetString(6),
                                RejectionReason = reader.IsDBNull(7) ? "" : reader.GetString(7),
                                Date = reader.GetDateTime(8).ToString()
                            });
                        }
                    }
                }
            }

            return orders;
        }
        public static async Task<OrderDTO> FindOrderByIdAsync(string Token, int ClientId, int OrderId)
        {
            OrderDTO order = null; // القيمة الافتراضية إذا لم يتم العثور على الطلب.
            string query = "EXEC FindOrderById @Token, @ClientId, @OrderId";

            try
            {
                using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // تمرير المعاملات إلى الإجراء المخزن
                        command.Parameters.AddWithValue("@Token", Token);
                        command.Parameters.AddWithValue("@ClientId", ClientId);
                        command.Parameters.AddWithValue("@OrderId", OrderId);

                        // قراءة النتائج باستخدام SqlDataReader
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                // إنشاء كائن OrderDTO من النتائج
                                order = new OrderDTO
                                {
                                    OrderID = reader.GetInt32(0),
                                    Visa_Name = reader.GetString(1),
                                    Order_Status = reader.GetString(2),
                                    OrderType = reader.GetString(3),
                                    FullName = reader.GetString(4),
                                    PhoneNumber = reader.GetString(5),
                                    Email = reader.GetString(6),
                                    RejectionReason = reader.IsDBNull(7) ? "" : reader.GetString(7)
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                // معالجة الأخطاء المتعلقة بقاعدة البيانات
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // معالجة الأخطاء العامة
                Console.WriteLine($"Error: {ex.Message}");
            }

            return order;
        }
        public static async Task<bool> PostOrderAsync(string Token, int ClientId, int VisaId, int OrderTypeId)
        {
            bool IsUpdated = false;
            string query = "EXEC PostOrder @Token, @ClientId, @VisaId, @OrderTypeId;";

            try
            {
                using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // إضافة المعاملات إلى الاستعلام
                        command.Parameters.AddWithValue("@Token", Token);
                        command.Parameters.AddWithValue("@ClientId", ClientId);
                        command.Parameters.AddWithValue("@VisaId", VisaId);
                        command.Parameters.AddWithValue("@OrderTypeId", OrderTypeId);


                        int rowsAffected = await command.ExecuteNonQueryAsync();


                        IsUpdated = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // يمكن إضافة معالجة للأخطاء هنا (مثل تسجيل الخطأ)
                Console.WriteLine($"Error: {ex.Message}");
            }

            return IsUpdated;
        }
        public static async Task<bool> AcceptOrderAsync(int OwnerId, int OrderId)
        {
            bool IsAccepted = false;
            string storedProcedure = "EXEC [FinishOrder] @OrderId, @OwnerId";

            try
            {
                using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(storedProcedure, connection))
                    {
                        // إضافة المعاملات إلى الاستعلام
                        command.Parameters.AddWithValue("@OwnerId", OwnerId);
                        command.Parameters.AddWithValue("@OrderId", OrderId);

                        // تنفيذ الاستعلام
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        // إذا تم تنفيذ الإجراء بنجاح
                        IsAccepted = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // معالجة الأخطاء العامة
                Console.WriteLine($"Error: {ex.Message}");
            }

            return IsAccepted;
        }
        public static async Task<bool> RejectOrderASync(string Token, int OrderId, string Reason)
        {
            bool IsRejected = false;
            string Procedure = "EXEC [RejectOrder] @Token, @OrderId, @Reason";

            try
            {
                using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(Procedure, connection))
                    {
                        // إضافة المعاملات إلى الإجراء المخزن
                        command.Parameters.AddWithValue("@Token", Token);
                        command.Parameters.AddWithValue("@OrderId", OrderId);
                        command.Parameters.AddWithValue("@Reason", Reason);

                        // تنفيذ الإجراء المخزن
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        // إذا تم تحديث صف واحد أو أكثر، العملية ناجحة
                        IsRejected = rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                // معالجة الأخطاء المتعلقة بقاعدة البيانات
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // معالجة الأخطاء العامة
                Console.WriteLine($"Error: {ex.Message}");
            }

            return IsRejected;
        }
        public static async Task<bool> BadOrderAsync(string Token, int OrderId, string Reason)
        {
            bool IsRejected = false;
            string Procedure = "EXEC [BadOrder] @Token, @OrderId, @Reason";

            try
            {
                using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(Procedure, connection))
                    {
                        // إضافة المعاملات إلى الإجراء المخزن
                        command.Parameters.AddWithValue("@Token", Token);
                        command.Parameters.AddWithValue("@OrderId", OrderId);
                        command.Parameters.AddWithValue("@Reason", Reason);

                        // تنفيذ الإجراء المخزن
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        // إذا تم تحديث صف واحد أو أكثر، العملية ناجحة
                        IsRejected = rowsAffected > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                // معالجة الأخطاء المتعلقة بقاعدة البيانات
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // معالجة الأخطاء العامة
                Console.WriteLine($"Error: {ex.Message}");
            }

            return IsRejected;
        }
        public static async Task<List<OrderDTO>> FindOrdersBelongsToSpecificClient(string Token, int ClientId)
        {
            string query = "EXEC [GetOrdersBelongsToSpecificClient] @Token, @ClientId";

            var orders = new List<OrderDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(Settings.ConnectionString))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Token", Token);
                        command.Parameters.AddWithValue("@ClientId", ClientId);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                orders.Add(new OrderDTO
                                {
                                    OrderID = reader.GetInt32(0),
                                    Visa_Name = reader.GetString(1),
                                    Order_Status = reader.GetString(2),
                                    OrderType = reader.GetString(3),
                                    FullName = reader.GetString(4),
                                    PhoneNumber = reader.GetString(5),
                                    Email = reader.GetString(6),
                                    RejectionReason = reader.IsDBNull(7) ? "" : reader.GetString(7),
                                    Date = reader.GetDateTime(8).ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // هنا نرمي استثناء جديد مع رسالة الخطأ من الاستثناء الحالي
                throw new Exception(ex.Message.ToString());
            }

            return orders;
        }
    }
}