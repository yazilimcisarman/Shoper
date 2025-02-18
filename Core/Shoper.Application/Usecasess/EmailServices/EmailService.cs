using Shoper.Application.Dtos.EMailDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Usecasess.EmailServices
{
    public class EmailService : IEmailService
    {
        public bool SendEmailAsync(SendEmailDto dto)
        {
			try
			{
				//mailmessage olusturduk
				MailMessage message = new MailMessage(dto.SenderEmail,dto.ReciverEmail,dto.Subject,dto.Message);

                //smtp ayarlarini yaptik
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                smtpClient.Port = 587;
				smtpClient.UseDefaultCredentials = false;
				//uygulama sifresi kullanacgiz
				smtpClient.Credentials = new NetworkCredential(dto.SenderEmail,dto.SenderPassword);
				smtpClient.EnableSsl = true;

				smtpClient.Send(message);
				return true;

			}
			catch (Exception)
			{
				return false;
			}
        }
    }
}
