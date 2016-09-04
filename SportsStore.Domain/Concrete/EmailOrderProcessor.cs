using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;


namespace SportsStore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "5509993810@163.com";
        public string MailFromAddress = "5509993810@163.com";
        public bool UseSsl = true;
        public string Username = "5509993810@163.com";
        public string Password = "yx20080118150";
        public string Servername = "smtp.163.com";
        public int ServerPort = 25;
        public bool WriteAsFile = false;
        public string FileLocation = @"c:\sports_store_emails";

    }
    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }

        public void ProcessOrder(Cart cart, ShoppingDetails shoppingInfo)
        {
            using(var smtpClient=new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.Servername;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                    .AppendLine("A new order has been submitted")
                    .AppendLine("---")
                    .AppendLine("Items:");
                foreach(var line in cart.Lines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0}×{1}(subtotal:{2:c}", line.Quantity, line.Product.Name, subtotal);
                }
                body.AppendFormat("Total order value:{0:c}", cart.ComputeTotalValue())
                    .AppendLine("---")
                    .AppendLine("Ship to:")
                    .AppendLine(shoppingInfo.Name)
                    .AppendLine(shoppingInfo.line1)
                    .AppendLine(shoppingInfo.line2 ?? "")
                    .AppendLine(shoppingInfo.line3 ?? "")
                    .AppendLine(shoppingInfo.City)
                    .AppendLine(shoppingInfo.Country)
                    .AppendLine(shoppingInfo.Zip)
                    .AppendLine("---")
                    .AppendFormat("Gift wrap:{0}", shoppingInfo.Giftwrap ? "Yes" : "No");
                MailMessage mailMessage = new MailMessage(
                    emailSettings.MailFromAddress,
                    emailSettings.MailToAddress,
                    "New order submitted!",
                    body.ToString());
                if (emailSettings.WriteAsFile){
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }
                smtpClient.Send(mailMessage);

            }
            
        }
    }

}
