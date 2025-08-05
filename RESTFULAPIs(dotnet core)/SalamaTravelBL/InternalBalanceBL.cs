using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalamaTravelDTA;

namespace SalamaTravelBL
{
   public class InternalBalanceBL
    {
        public static async Task<bool>PayWithInternalBalanceAsync(SalamaTravelDTA.InternalBalanceDAL.PaymentService.PayInternalBalanceRequest Req)
        {
            try
            {

            return await SalamaTravelDTA.InternalBalanceDAL.PaymentService.PayWithInternalBalanceAsync(Req.Token,Req.ClientId,Req.Amount);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public static async Task<bool> PayDues(SalamaTravelDTA.InternalBalanceDAL.PaymentService.PayDuesRequest Req)
        {
            try
            {

            return await SalamaTravelDTA.InternalBalanceDAL.PaymentService.PayDuesAsync(Req.Token, Req.ClientId);
            }catch(Exception ex)
            {
                throw new Exception(ex.Message.ToString());

            }
        }
        public static async Task<bool>UpdateInternalBalanceAsync(InternalBalanceDAL.PaymentService.PayInternalBalanceRequest request)
        {
            try
            {

            return await InternalBalanceDAL.PaymentService.UpdateInternalBalanceAsync(request.Token, request.ClientId, request.Amount);
            }catch(Exception ex)
            {
                throw new Exception(ex.Message.ToString());

            }
        }
    }
}
