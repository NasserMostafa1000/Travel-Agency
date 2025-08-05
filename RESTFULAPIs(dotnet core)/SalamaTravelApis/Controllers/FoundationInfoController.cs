using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalamaTravelBL;
using SalamaTravelDTA;

namespace SalamaTravelApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]  
    public class FoundationInfoController : ControllerBase
    {
        [HttpPut("UpdateFoundationInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateFoundationInformation(FoundationInfoDAL.FoundationInfoDTO dt)
        {
            // التحقق من صحة البيانات المدخلة
            if (string.IsNullOrEmpty(dt.WhastAppNumber) ||
                string.IsNullOrEmpty(dt.CallNumber) ||
                string.IsNullOrEmpty(dt.About) ||
                string.IsNullOrEmpty(dt.Email))
            {
                return BadRequest("Bad Request: Maybe there is some missing information.");
            }
            if (string.IsNullOrEmpty(dt.Email) || !dt.Email.EndsWith("@gmail.com"))
            {
                return BadRequest("البريد الإلكتروني يجب أن ينتهي بـ @gmail.com");
            }

            try
            {
                // استدعاء وظيفة التحديث
                bool result = await FoundationInfoBL.UpdateFoundationInfoAsync(dt);

                // التحقق من نتيجة التحديث
                return result ? Ok("Foundation information updated successfully.")
                              : StatusCode(StatusCodes.Status500InternalServerError, "Update failed.");
            }
            catch (Exception ex)
            {
                // معالجة الأخطاء
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }


        [HttpGet("FoundationInformation")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFoundationInformation()
        {
            try
            {
                // استدعاء الدالة لجلب المعلومات
                var foundationInfo = await FoundationInfoBL.GetFoundationInfoAsync();

                // تحقق من وجود بيانات قبل الإرجاع
                if (foundationInfo == null)
                {
                    return NotFound("No foundation information found.");
                }

                return Ok(foundationInfo);
            }
            catch (Exception ex)
            {
                // تسجيل الخطأ إذا لزم الأمر
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching foundation information.");
            }
        }


    }
}
