using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalamaTravelDTA;

namespace SalamaTravelBL
{
  public static  class OrdersBL
    {
        public class FindOrdersRequest
        {
            public string Token { get; set; }
            public int ClientId { get; set; }
        }
        public class AddOrderRequest
        {
            public string Token { get; set;}
            public int ClientId { get; set; }
            public int VisaId { get; set; }
            public int OrderTypeId { get; set; }
        }
        public class RejectOrBadOrderRequest
        {
            public int OrderId { get; set; }
            public string Reason { get; set; }
        }
        public static async Task<List<SalamaTravelDTA.OrdersDAL.OrderDTO>>GetOrdersAsync(string Token,int ClientId)
        {
            return await SalamaTravelDTA.OrdersDAL.GetOrdersAsync(Token, ClientId);
        }
        public static async Task<bool>AddOrderASync(string Token, int ClientId, int VisaId, int OrderTypeId)
        {
                        return await SalamaTravelDTA.OrdersDAL.PostOrderAsync(Token, ClientId, VisaId, OrderTypeId);
        }
        public static async Task<bool> AcceptOrderAsync(int OrderId, int OwnerId)
        {
            return await SalamaTravelDTA.OrdersDAL.AcceptOrderAsync(OwnerId, OrderId);
        }
        public static async Task<bool> RejectOrderAsync(string Token, int OrderId,string Reason)
        {
            return await SalamaTravelDTA.OrdersDAL.RejectOrderASync(Token, OrderId,Reason);
        }
        public static async Task<bool> BadOrderAsync(string Token, int OrderId, string Reason)
        {
            return await SalamaTravelDTA.OrdersDAL.BadOrderAsync(Token, OrderId, Reason);
        }
        public static async Task<OrdersDAL.OrderDTO> FindOrderByIdAsync(string Token, int ClientId, int OrderId)
        {
            // التحقق من صحة المدخلات (إذا لزم الأمر)
            if (string.IsNullOrWhiteSpace(Token) || ClientId <= 0 || OrderId <= 0)
                throw new ArgumentException("Invalid input parameters.");

            // استدعاء طبقة البيانات
            return await OrdersDAL.FindOrderByIdAsync(Token, ClientId, OrderId);
        }
    }
}
