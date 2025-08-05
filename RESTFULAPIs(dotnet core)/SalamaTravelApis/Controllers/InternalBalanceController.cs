using Microsoft.AspNetCore.Mvc;
using SalamaTravelBL;
using SalamaTravelDTA;
using System.Threading.Tasks;

namespace SalamaTravelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InternalBalanceController : ControllerBase
    {
        /// <summary>
        ///  انشاء طلب باستخدام الرصيد الداخلي 
        /// </summary>
        /// <param name="request">طلب الدفع يحتوي على Token و ClientId و Amount</param>
        /// <returns>نتيجة الدفع (نجاح/فشل)</returns>
        [HttpPost("PayWithInternalBalance")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)] 
        [ProducesResponseType(500)] 
        public async Task<IActionResult> PayWithInternalBalanceAsync([FromBody] SalamaTravelDTA.InternalBalanceDAL.PaymentService.PayInternalBalanceRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Token) || request.ClientId <= 0 || request.Amount <= 0)
            {
                return BadRequest("Invalid request data. Please ensure all fields are correctly provided.");
            }

            try
            {
                bool isSuccess = await SalamaTravelBL.InternalBalanceBL.PayWithInternalBalanceAsync(request);
                if (isSuccess)
                {
                    bool result = await SalamaTravelBL.OrdersBL.AddOrderASync(request.Token, request.ClientId, request.VisaId, request.OrderTypeId);
                    return Ok(true); 
                }
                else
                {
                    return BadRequest("Payment failed. Please check your balance or input data.");
                }
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }
        [HttpPost("payDues")]
        [ProducesResponseType(typeof(bool), 200)] 
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> PayDues([FromBody]InternalBalanceDAL.PaymentService.PayDuesRequest request)
        {
            if (request == null)
                return BadRequest("Invalid data.");

            bool isPaymentSuccessful = await InternalBalanceBL.PayDues(request);

            if (isPaymentSuccessful)
            {
                return Ok("Payment processed successfully.");
            }
            else
            {
                return StatusCode(500, "Payment failed, please try again later.");
            }
        }

        /// <summary>
        /// تحديث الرصيد الداخلي للعميل
        /// </summary>
        /// <param name="request">طلب التحديث يحتوي على Token و ClientId و Amount</param>
        /// <returns>نتيجة التحديث (نجاح/فشل)</returns>
        [HttpPut("UpdateInternalBalance")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateInternalBalance([FromBody] SalamaTravelDTA.InternalBalanceDAL.PaymentService.PayInternalBalanceRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Token) || request.ClientId <= 0 || request.Amount <= 0)
            {
                return BadRequest("Invalid request data. Please ensure all fields are correctly provided.");
            }

            try
            {
                bool isUpdatedSuccessfully = await SalamaTravelBL.InternalBalanceBL.UpdateInternalBalanceAsync(request);

                if (isUpdatedSuccessfully)
                {
                    return Ok(true);
                }
                else
                {
                    return BadRequest("Update failed. Please check your input data.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing your request: {ex.Message}");
            }
        }
    }

}

