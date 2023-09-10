using System.Net.Mail;

namespace Entities.Static
{
    public class EmailHelper
    {
		
		public bool SendEmailPasswordReset(string userEmail, string link)
		{
			MailMessage mailMessage = new MailMessage();
			mailMessage.From = new MailAddress("info@examsystem.com");
			mailMessage.To.Add(new MailAddress(userEmail));

			mailMessage.Subject = "Password Reset";
			mailMessage.IsBodyHtml = true;
			mailMessage.Body = link;

			SmtpClient client = new SmtpClient();

            //client.Credentials = new System.Net.NetworkCredential("mkkb917@yahoo.com", "c5fj!B6mA@r");
            client.Credentials = new System.Net.NetworkCredential("kamran.9002@gmail.com", "orvnqqlaakmugjug");
            client.Host = "smtp.gmail.com";
            client.Port = 587;    // google 587  , 465
            client.EnableSsl = true;

            //// mailjet api configurations
            //client.Credentials = new System.Net.NetworkCredential("804b4a498b67d7e41496e084c56d1639", "6d7446bde04fbd5550e8d825626a7683");
            //client.Host = "in-v3.mailjet.com";
            //client.EnableSsl = true;
            //client.Port = 587;


            //// Ethereal api configurations (temporary email account for smtp)
            //client.Credentials = new System.Net.NetworkCredential("rachelle.jerde52@ethereal.email", "bVWmz5nbn5MGFsSvyV");
            //client.Host = "smtp.ethereal.email";
            //client.EnableSsl = true;
            //client.Port = 587;


   //         try
			//{
			//	client.Send(mailMessage);
			//	return true;
			//}
			//catch (Exception ex)
			//{
				
			//	// log exception
				
				
			//}
			return false;
		}
	}
}
