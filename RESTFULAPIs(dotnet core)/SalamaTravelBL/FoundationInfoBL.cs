using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalamaTravelDTA;

namespace SalamaTravelBL
{
    
  public   class FoundationInfoBL
    {
        public static async Task<bool>UpdateFoundationInfoAsync(FoundationInfoDAL.FoundationInfoDTO Dt)
        {
            return await FoundationInfoDAL.UpdateFoundationInfoAsync(Dt.CallNumber, Dt.WhastAppNumber, Dt.Email, Dt.About,Dt.Insta,Dt.FaceBook,Dt.TikTok);
        }
        public static async Task<FoundationInfoDAL.FoundationInfoDTO>GetFoundationInfoAsync()
        {
            FoundationInfoDAL.FoundationInfoDTO Information =await FoundationInfoDAL.GetFoundationInfoAsync();
            return Information;
        }
    }
}






