using SalamaTravelDTA;

namespace SalamaTravelBA
{

    public class ClientsBL
    {
        public class GetByIdReq
        {
            public int ClientId { get; set; }
            public string Token { get; set; }
        }
        public class GetByNameReq
        {
            public string ClientNamePref { get; set; }
            public string Token { get; set; }
        }
        public static async Task<bool> DeleteImageAsync(string imagePath)
        {
            try
            {
                if (imagePath.StartsWith("/"))
                {
                    imagePath = imagePath.Substring(1); 
                }

                // تحديد المسار الكامل للصورة
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath);

                // التحقق من وجود الملف
                if (File.Exists(fullPath))
                {
                    await Task.Run(() => File.Delete(fullPath));
                    return true; 
                }
                else
                {
                    Console.WriteLine("File not found at path: " + fullPath);
                    return false; 
                }
            }
            catch (Exception ex)
            {
                // تسجيل الأخطاء التي تحدث أثناء الحذف
                Console.WriteLine($"Error deleting file: {ex.Message}");
                return false;
            }
        }

        //this class to post Client and the stored procedure will insert the user bu default into the DB 
        public class ClientUserDTO
        {
            public int ClientID { get; set; }
            public string FirstName { get; set; }
            public string MidName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string PhoneNumber { get; set; }
        }
        public class PutClient
        {
            public int ClientID { get; set; }
            public string FirstName { get; set; }
            public string MidName { get; set; }
            public string LastName { get; set; }
            public string PhoneNumber { get; set; }
            public string Token { get; set; }

        }

        public static async Task<List<SalamaTravelDTA.Clients.ClientsDTO>>  GetAllClientsAsync(string Token,int ClientId)
        {
            List<SalamaTravelDTA.Clients.ClientsDTO> Clients =await SalamaTravelDTA.Clients.GetAllClientsAsync(Token,ClientId);
            
            return Clients.Count>0?Clients:null;
        }
        public static async Task<int>PostClientAsync(ClientUserDTO Client)
        {
            try
            {
                int Result = await SalamaTravelDTA.Clients.PostClientAsync(Client.FirstName, Client.MidName, Client.Email, Client.PhoneNumber, Client.LastName, Client.Password);
                return Result;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        public static async Task<bool>PutClientAsync(ClientsBL.PutClient Client)
        {
            return await SalamaTravelDTA.Clients.PutClientASync(Client.ClientID, Client.Token, Client.FirstName, Client.MidName, Client.LastName, Client.PhoneNumber);
        }
        public static async Task<bool>DeleteLastPersonalImageAsync(int ClientId)
        {
            string FilePath = await Clients.GetPersonalImageWithClientIdAsync(ClientId);
            if(string.IsNullOrEmpty(FilePath))
            {
                return true;
            }
            return await DeleteImageAsync(FilePath);

        }
        public static async Task<bool> DeleteLastPassportImageAsync(int ClientId)
        {
            string FilePath = await Clients.GetPassportImageWithClientIdAsync(ClientId);
            if (string.IsNullOrEmpty(FilePath))
            {
                return true;
            }
            return await DeleteImageAsync(FilePath);

        }
    }
}
