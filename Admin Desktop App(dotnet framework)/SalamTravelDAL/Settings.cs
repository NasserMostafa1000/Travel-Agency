using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
namespace SalamaTravelDAL
{
    public static class Settings
    {

        public static readonly HttpClient HttpClient = new HttpClient
        {
            BaseAddress = new Uri("http://nasermostafa-001-site1.rtempurl.com/api/")
           //BaseAddress = new Uri("https://localhost:7244/api/")

        };
//        public static string ServerPath = "https://localhost:7244";
      public static string ServerPath = "http://nasermostafa-001-site1.rtempurl.com";

        public static async Task SendEmail(string subject, string body, string emailToSend)
        {
            try
            {
                string fromAddress = "commercialprokerskaramalsalama@gmail.com";
                string appPassword = "tabr lqzr mavn omvc";
                string toAddress = emailToSend;
                string fromName = "Salama Travel";

                using (var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress, appPassword)
                })
                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(fromAddress, fromName);
                    message.To.Add(toAddress);
                    message.Subject = subject;
                    message.Body = body;

                    await smtp.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static IEnumerable<TSource>Paginate<TSource>(this IEnumerable<TSource> source,int PageNum)
        {
            if(PageNum>=0&&!source.Any())
            {
                throw new Exception("Page Number Less Than 1 OR No More Data To Show");
            }

            return source.Skip((PageNum - 1) * 1).Take(1);
        }
    }
}
