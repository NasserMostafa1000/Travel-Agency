using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalamaTravelBL
{
    public class IssuedVisasBL
    {
        public static async Task<List<SalamaTravelDTA.IssuedVisaDAL.IssuedVisasDTO>> GetIssuedVisasAsync(string Token, int OwnerId)
        {
            // يمكن إضافة تحقق أو معالجة إضافية هنا
            if (string.IsNullOrEmpty(Token) || OwnerId <= 0)
                throw new ArgumentException("Invalid token or owner ID.");

            return await SalamaTravelDTA.IssuedVisaDAL.GetIssuedVisasAsync(Token, OwnerId);
        }
    }
}
