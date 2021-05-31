using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rocky.Utility
{
	public class EmailSender : IEmailSender
	{
		public Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			return Execute(email, subject, htmlMessage);
		}

		public async Task Execute(string email, string subject, string body)
		{
            MailjetClient client = new MailjetClient("fb24fc8e4e7e8893165beed59708641a", "6fdd9e28a6ab9b783d86c4710b05a973");
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      {
       "From",
       new JObject {
        {"Email", "sgsdbs@gmail.com"},
        {"Name", "Siraj"}
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
          "DotNetMastery"
         }
        }
       }
      }, {
       "Subject",
       subject
      }, {
       "HTMLPart",
       body
      },
     }
             });
            await client.PostAsync(request);
        }
	}
}
