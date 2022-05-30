using System.Net;
using System.Net.Mail;
using FluentEmail.Core;
using FluentEmail.Smtp;
using Microsoft.AspNetCore.Mvc;


namespace API.ExternalServices {
    
    public class EmailService : IEmailService{
        private readonly IConfiguration _config;
        private readonly IServiceProvider _serviceProvider;
        public  EmailService(IConfiguration config,IServiceProvider serviceProvider) {
            _config = config;
            _serviceProvider = serviceProvider;
        }
      //  Environment.GetEnvironmentVariable("FRONTEND_URL");
      public async void SendEmail(string emailTo,string name,string body,string subject)
      {
          using var scope = _serviceProvider.CreateScope();
          var mailer = scope.ServiceProvider.GetRequiredService<IFluentEmail>();

          var email = mailer
              .To(emailTo)
              .Subject(subject)
              .UsingTemplateFromFile(
                  $"{Directory.GetCurrentDirectory()}/wwwroot/EmailTemplates/ConfirmEmailTemplate.cshtml",new
                  {
                      Name = name,
                      Token = body
                  });
                  
              
          await email.SendAsync();
      }
    }

}