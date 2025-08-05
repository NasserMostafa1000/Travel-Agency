using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace SalamaTravelDTA
{
    public class Settings
    {
        public static string ConnectionString { get; private set; }

        public static void LoadConfiguration(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public static async Task SendEmail(string Subject, string Body, string EmailToSend)
        {
            try
            {
                // إعدادات البريد الإلكتروني
                string fromAddress = "commercialprokerskaramalsalama@gmail.com"; // بريدك الإلكتروني
                string appPassword = "tabr lqzr mavn omvc"; // كلمة مرور التطبيق أو كلمة المرور العادية إذا كان "الوصول للتطبيقات الأقل أمانًا" مفعلاً
                string toAddress = EmailToSend; // البريد الإلكتروني للمستلم
                string subject = Subject;
                string fromName = "Karam Al-Salama Commercial Prokers";
                string body = Body;

                // تكوين SMTP
                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress, appPassword)
                };

                MailAddress from = new MailAddress(fromAddress, fromName);
                MailMessage message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    From = from,
                    Body = body
                };

                // إرسال الرسالة
                smtp.Send(message);
            }
            catch (Exception)
            {
            }
        }

    }
}
