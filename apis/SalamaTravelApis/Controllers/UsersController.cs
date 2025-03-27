using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SalamaTravelBL;
using SalamaTravelDTA;

namespace SalamaTravelApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> LogIn([FromBody] UsersBL.LoginRequest LoginInfo)
        {
            // التحقق من صحة البيانات المدخلة
            if (LoginInfo == null || string.IsNullOrEmpty(LoginInfo.UserName) || string.IsNullOrEmpty(LoginInfo.Password))
            {
                return BadRequest("UserName and Password are required.");
            }
            try
            {


                // استدعاء الخدمة للتحقق من صحة بيانات الدخول
                UsersDAL.UserDTO UserInfo = await UsersBL.loginASync(LoginInfo.UserName, LoginInfo.Password);

                // إذا كانت البيانات صحيحة، نعيد استجابة 200 مع بيانات المستخدم
                if (UserInfo != null)
                {
                    return Ok(UserInfo);
                }
            }catch(Exception ex)
            {
                string errorMessage = ex.Message.ToString();
                return BadRequest(new{ errorMessage });
            }

            // إذا كانت البيانات غير موجودة، نعيد استجابة 404 مع رسالة مناسبة
            return NotFound("There is no user with this email in the system.");
        }

        [HttpPut("PutInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutUser([FromBody] UsersBL.PutRequest PutInfo)
        {
            // التحقق من أن البيانات المدخلة غير فارغة
            if (PutInfo == null || string.IsNullOrEmpty(PutInfo.UserName) || string.IsNullOrEmpty(PutInfo.Password)  || string.IsNullOrEmpty(PutInfo.Token))
            {
                if (string.IsNullOrEmpty(PutInfo.UserName) || !PutInfo.UserName.EndsWith("@gmail.com"))
                {
                    return BadRequest("البريد الإلكتروني يجب أن ينتهي @gmail.comبـ ");
                }
                return BadRequest("Missing Information");
            }

            // محاولة تحديث معلومات المستخدم باستخدام دالة UpdateUserInfo
            bool isUpdated = await UsersDAL.UpdateUserInfoAsync(PutInfo.ClientId, PutInfo.Token, PutInfo.UserName, PutInfo.Password);

            // إذا كانت عملية التحديث ناجحة، نعيد استجابة 200 OK
            if (isUpdated)
            {
                return Ok("User information updated successfully.");
            }
            else
            {
                // إذا لم يتم العثور على المستخدم أو فشل التحديث، نرجع 404
                return NotFound("User not found or update failed.");
            }
        }
        [HttpPost("VerifyPassword")]
        public async Task<IActionResult> VerifyPasswordAsync([FromBody] UsersBL.VerifyPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Email and Password are required.");

            var isValid = await SalamaTravelBL.UsersBL.ValidateUserPasswordAsync(request.Email, request.Password);

            if (isValid)
            {
                return Ok("Password matches!");
            }
            else
            {
                return Unauthorized("Incorrect password.");
            }
        }

        [HttpPut("ForgetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ForgetPassword([FromBody] UsersBL.LoginRequest LoginInfo )
        {
            if ( string.IsNullOrEmpty(LoginInfo.UserName) || string.IsNullOrEmpty(LoginInfo.Password) )
            {
                if (string.IsNullOrEmpty(LoginInfo.UserName) || !LoginInfo.UserName.EndsWith("@gmail.com"))
                {
                    return BadRequest("البريد الإلكتروني يجب أن ينتهي @gmail.comبـ ");
                }
                return BadRequest("Missing Information");
            }
           await Settings.SendEmail("Your New Password", $" We Create a Random Password For You,\n Please do not share this Password with any one, your new Password is (  {LoginInfo.Password}  ) \n please go to Your Account And Change The Random Password into your Privacy one ",LoginInfo.UserName);
            bool isUpdated = await UsersDAL.ForgetPasswordAsync(LoginInfo.UserName, LoginInfo.Password);

            if (isUpdated)
            {
                return Ok("User information updated successfully.");
            }
            else
            {
                return NotFound("User not found or update failed.");
            }
        }

    }

   

}

