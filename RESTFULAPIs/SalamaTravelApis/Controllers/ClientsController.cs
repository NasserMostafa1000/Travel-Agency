 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalamaTravelBA;
using SalamaTravelDTA;
namespace SalamaTravel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
 

        [HttpGet("FetchClientsData", Name = "FetchClientsData)")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllClients( string Token, int ClientId)
        {
            // التحقق من صحة المعاملات المدخلة
            if (string.IsNullOrEmpty(Token))
            {
                return Unauthorized(); // إذا كان التوكن غير صالح
            }

            try
            {
                // استدعاء الدالة GetAllClients من الطبقة الخاصة بك
                var clients = await SalamaTravelBA.ClientsBL.GetAllClientsAsync(Token, ClientId);

                if (clients == null)
                {
                    return NotFound("there are no Clients"); // إذا لم يوجد عملاء
                }

                return Ok(clients); // إذا كانت البيانات موجودة، إرجاعها مع حالة 200 OK
            }
            catch (Exception ex)
            {
                // التعامل مع الأخطاء (يمكنك تسجيل الخطأ هنا)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("PostNewClient", Name = "PostNewClient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostClient(SalamaTravelBA.ClientsBL.ClientUserDTO Dto)
        {
            if (string.IsNullOrEmpty(Dto.Email) || !Dto.Email.EndsWith("@gmail.com"))
            {
                return BadRequest("البريد الإلكتروني يجب أن ينتهي بـ @gmail.com");
            }

            try
            {
                int ID = await SalamaTravelBA.ClientsBL.PostClientAsync(Dto);



                if (ID != -1)
                {
                    return Ok(new { ID });
                }
                else
                {
                    return BadRequest("internal server error");
                }
            }
            catch (Exception ex)
            {
                string ErrorMessage = ex.Message.ToString(); ;
             
                return BadRequest(new {ErrorMessage});
            }
        }
       [HttpPut("PutClient", Name = "PutClient")]
       [ProducesResponseType(StatusCodes.Status200OK)]
       [ProducesResponseType(StatusCodes.Status400BadRequest)]
       [ProducesResponseType(StatusCodes.Status401Unauthorized)]
       public async Task<IActionResult> PutClient(SalamaTravelBA.ClientsBL.PutClient Dto)
       {
                // التحقق من صحة التوكن
                if (string.IsNullOrEmpty(Dto.Token))
                {
                    return Unauthorized(); // إذا كان التوكن غير صالح
                }


            if (Dto.ClientID <= 0 )
                {
                    return BadRequest("البيانات المدخلة غير صالحة");
                }

                try
                {
                    bool isUpdated = await SalamaTravelBA.ClientsBL.PutClientAsync(Dto);

                    if (isUpdated)
                    {
                        return Ok("تم تحديث بيانات العميل بنجاح");
                    }
                    else
                    {
                        return BadRequest("لم يتم تحديث بيانات العميل");
                    }
                }
                catch (Exception ex)
                {
                    // التعامل مع الأخطاء (يمكنك تسجيل الخطأ هنا)
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        [HttpPost("UploadPassportImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadPassportImage(IFormFile imageFile,int ClientId)
        {
            // Check if no file is uploaded
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("No file uploaded.");
            await ClientsBL.DeleteLastPassportImageAsync(ClientId);

            // Directory where files will be uploaded
            // var uploadDirectory = @"C:\PassportImages";
            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PassportImages");

             // Generate a unique filename
             var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadDirectory, fileName);

            // Ensure the uploads directory exists, create if it doesn't
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            // Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            string FileUrl = $"/PassportImages/{fileName}";

            // Return the file path as a response
            return   await SalamaTravelDTA.Clients.UpdatePassportImageASync(ClientId, FileUrl) ? Ok(filePath):BadRequest("!!!");
        }
        [HttpPost("UploadPersonalImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadPersonalImage(IFormFile imageFile, int ClientId)
        {
            // Check if no file is uploaded
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("No file uploaded.");
            await ClientsBL.DeleteLastPersonalImageAsync(ClientId);

            // Directory where files will be uploaded
            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PersonalImages");
            //var uploadDirectory = @"C:\PersonalImages";

            // Generate a unique filename
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadDirectory, fileName);

            // Ensure the uploads directory exists, create if it doesn't
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            // Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            string FileUrl = $"/PersonalImages/{fileName}";

            // Return the file path as a response
            return await SalamaTravelDTA.Clients.UpdatePersonalImageAsync(ClientId, FileUrl) ? Ok(filePath) : BadRequest("!!!");
        }

        [HttpPost("FindClient")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetClientById(ClientsBL.GetByIdReq req)
        {
            try
            {
                // تحقق من صحة التوكن
                if (string.IsNullOrWhiteSpace(req.Token))
                {
                    return Unauthorized(new { message = "Token is required" });
                }

                // استدعاء الفانكشن لجلب بيانات العميل
                var clientData = await Clients.GetClientByIdAsync(req.ClientId, req.Token);

                // التحقق إذا كان العميل موجودًا
                if (clientData == null)
                {
                    return NotFound(new { message = "Client not found or invalid token" });
                }

                // إرجاع البيانات إذا تم العثور عليها
                return Ok(clientData);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message.ToString());
            }
        }

        [HttpPost("FindClientByName")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetClientByName(ClientsBL.GetByNameReq req)
        {
            try
            {
                // تحقق من صحة التوكن
                if (string.IsNullOrWhiteSpace(req.Token))
                {
                    return Unauthorized(new { message = "Token is required" });
                }
                var clientData = await Clients.GetClientByNameAsync(req.ClientNamePref, req.Token);
                if (clientData == null)
                {
                    return NotFound(new { message = "Client not found or invalid token" });
                }
                return Ok(clientData);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message.ToString());
            }
        }
    }

}

   