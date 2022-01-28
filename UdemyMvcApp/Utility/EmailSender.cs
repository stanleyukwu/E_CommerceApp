
using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace UdemyMvcApp.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _Config;
        private Mailjet mailjet { get; set; }

        public EmailSender(IConfiguration Configuration)
        {
            _Config = Configuration;
            
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string Body)
        {
            mailjet = _Config.GetSection("MailSender").Get<Mailjet>();
            MailjetClient client = new MailjetClient(mailjet.ApiKey, mailjet.SecretKey)
            {
                Version = ApiVersion.V3_1
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      {
       "From",
       new JObject {
        {"Email", "stanleyjekwu16@gmail.com"},
        {"Name", "Stanley"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
          email
         }, {
          "Name",
          "ECommerceApp"
         }
        }
       }
      }, {
       "Subject",
       subject
      },
         {
       "HTMLPart",
         Body
      }
     }
             });
          await client.PostAsync(request);
        }
    }
}
