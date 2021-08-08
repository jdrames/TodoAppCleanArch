using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedServices
{
    public class EmailSenderService : IEmailSender
    {
        private readonly IConfiguration Configuration;
        private readonly ILogger _logger;

        public EmailSenderService(IConfiguration configuration, ILogger<EmailSenderService> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var apiConfig = new sib_api_v3_sdk.Client.Configuration();
                apiConfig.AddApiKey("api-key", Configuration.GetValue<string>("sib:apikey"));
                var apiInstance = new TransactionalEmailsApi(apiConfig);
                var to = new List<SendSmtpEmailTo>() {
                new SendSmtpEmailTo(email)
            };
                var sender = new SendSmtpEmailSender("No-Reply", "no-reply@forcemx.com");
                var smtpEmail = new SendSmtpEmail(sender, to, subject: subject, templateId: 1, _params: new { LINK = htmlMessage });
                var result = await apiInstance.SendTransacEmailAsync(smtpEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

        }
    }
}
