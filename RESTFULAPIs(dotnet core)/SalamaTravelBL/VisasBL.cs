using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalamaTravelBA;
using SalamaTravelDTA;

namespace SalamaTravelBL
{
  public  class VisasBL
    {
       public static async Task<List<SalamaTravelDTA.VisasDAL.VisaDTO>> GetVisasAsync()
        {
            return await VisasDAL.GetVisasAsync();
        }
     
        public static async Task<int>AddNewVisaAsync(VisasDAL.VisaDTO DTO,string Token)
        {
            return await  VisasDAL.PostVisaAsync(Token, DTO.Name, DTO.IssuancePrice, DTO.RenewalPrice, DTO.ImagePath, DTO.Period, DTO.MoreDetails);
        }
        public static async Task<bool> PutVisaAsync(VisasDAL.VisaDTO DTO, string Token)
        {
            try
            {
              return await VisasDAL.UpdateVisaASync(DTO.VisaId,Token, DTO.Name, DTO.IssuancePrice, DTO.RenewalPrice,  DTO.Period, DTO.MoreDetails);

            }catch
            {
                throw;
            }
        }
        public static async Task<bool>DeleteLastVisaImageAsync(int VisaId)
        {
            string FilePath = await VisasDAL.GetVisaImageWithVisaIdAsync(VisaId);
            if (string.IsNullOrEmpty(FilePath))
            {
                return true;
            }
            return await ClientsBL.DeleteImageAsync(FilePath);
        }
     

    }
}
