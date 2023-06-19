using SE1614_Group4_Project_API.Utils.Interfaces;
using System.Net;
using System.Net.Mail;

namespace SE1614_Group4_Project_API.Utils
{
    public class LogicHandler : ILogicHandler
    {
        public string GeneratePassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            Random random = new Random();
            char[] chars = new char[length];

            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }

            return new string(chars);
        }

        public async Task<bool> SendEmailAsync(string recipient, string subject, string body)
        {
            using var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("prn231.gr4@gmail.com", "kltekzfrsqvfuiin");

            var message = new MailMessage
            {
                From = new MailAddress("prn231.gr4@gmail.com"),
                To = { recipient },
                Subject = subject,
                Body = body
            };
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;
            message.ReplyToList.Add(new MailAddress("prn231.gr4@gmail.com"));
            message.Sender = new MailAddress("prn231.gr4@gmail.com");

            try
            {
                await client.SendMailAsync(message);
                return true;
            }
            catch (SmtpException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
    }
}