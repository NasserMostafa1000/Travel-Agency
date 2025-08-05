using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SalamaTravelDTA;
using SalamaTravelBL;
using Microsoft.AspNetCore.SignalR;

namespace SalamaTravelApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisasController : ControllerBase
    {
        private readonly IHubContext<VisasPricesHub> _hubContext;

        public VisasController(IHubContext<VisasPricesHub> hubContext)
        {
            _hubContext = hubContext;
        }
        [HttpGet("GetAllVisas")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<VisasDAL.VisaDTO>>> GetVisas()
        {
         
            var visas = await SalamaTravelBL.VisasBL.GetVisasAsync();
            if (visas == null || visas.Count == 0)
                return NotFound("No visas available for issuance.");
            return Ok(visas);
        }
        [HttpPost("PostVisa")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> AddNewVisa([FromBody] VisasDAL.VisaDTO dto,  string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Invalid token.");
            }

            if (dto == null || !ModelState.IsValid)
            {
                return BadRequest("Invalid data.");
            }

            // استدعاء الدالة غير المتزامنة من طبقة الـ Business Logic لإضافة الفيزا
            var visaId = await SalamaTravelBL.VisasBL.AddNewVisaAsync(dto, token);

            if (visaId > 0) // إذا تم إضافة الفيزا بنجاح
            {
                return Ok(new { message = "Visa added successfully.", visaId });
            }

            return BadRequest("Failed to add visa.");
        }
        [HttpPut("UpdateVisaInfo")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> UpdateVisa([FromBody] VisasDAL.VisaDTO dto,  string token)
        {
            try
            {

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("invalid Token");
            }
            if (dto == null || dto.VisaId <= 0)
                return BadRequest("Invalid data.");

            var result = await SalamaTravelBL.VisasBL.PutVisaAsync(dto, token);
            if (result)
            {
                // 🔔 إرسال إشعار عبر SignalR لكل العملاء
                await _hubContext.Clients.Group(VisasPricesHub.ClientsGroup).SendAsync(
                    "ReceiveVisaPriceUpdate",
                    dto.Name,           // اسم الفيزا
                    dto.IssuancePrice,  // سعر الإصدار
                    dto.RenewalPrice,   // سعر التجديد
                    dto.VisaId       
                    // ID لو حبيت تستخدمه في الكلاينت
                );
                return Ok("Visa updated successfully.");

            }
            return BadRequest("Failed to update visa.");
            }catch(Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
        [HttpPost("UploadVisaImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UploadVisaImage(IFormFile imageFile, string Token)
        {
            // 1. Validate the Token
            if (string.IsNullOrEmpty(Token))
            {
                return Unauthorized("Invalid Token");
            }

            // 2. Check if no file is uploaded
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // 3. Define the upload directory (inside wwwroot/VisaImages)
            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "VisaImages");

            // Ensure the uploads directory exists, create if it doesn't
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            // 4. Generate a unique filename for the uploaded file
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadDirectory, fileName);

            // 5. Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // 6. Generate a relative URL for accessing the file
            var fileUrl = $"/VisaImages/{fileName}";

            // 7. Return the file URL as a response
            return Ok( fileUrl );
        }

  
        [HttpPut("UpdateVisaImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateVisaImage(IFormFile imageFile, string Token, int VisaId)
        {
            // Validate Token
            if (string.IsNullOrEmpty(Token))
            {
                return Unauthorized("Invalid Token");
            }

            // Delete previous visa image
            await VisasBL.DeleteLastVisaImageAsync(VisaId);
           

            // Check if no file is uploaded
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Directory where files will be uploaded (inside wwwroot/VisaImages)
            var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "VisaImages");

            // Ensure the uploads directory exists, create if it doesn't
            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            // Generate a unique filename
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

            // Full path to save the file
            var filePath = Path.Combine(uploadDirectory, fileName);

            // Save the file to the server
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Generate the URL for accessing the image
            string fileUrl = $"/VisaImages/{fileName}";

            // Update the database with the new image URL
            bool isUpdated = await VisasDAL.UpdateVisaImagePathAsync(VisaId, fileUrl);

            // Return the response
            return isUpdated ? Ok(new { FileUrl = fileUrl }) : BadRequest("An error occurred while updating the image.");
        }

    }
}
    