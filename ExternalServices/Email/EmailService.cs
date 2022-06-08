using API.Enums;
using FluentEmail.Core;

namespace API.ExternalServices.Email {
    
    public class EmailService : IEmailService{
        private readonly IConfiguration _config;
        private readonly IServiceProvider _serviceProvider;
        public  EmailService(IConfiguration config,IServiceProvider serviceProvider) {
            _config = config;
            _serviceProvider = serviceProvider;
        }
  
        
      // Send Email with HTML template from wwwroot  
      // TODO: Could use refactor with some models etc and other forms of sending emails (options)
      public async void SendEmail(string emailTo,string name,string body,string subject)
      {
          
          // Directory.GetCurrentDirectory()}/wwwroot/EmailTemplates/ConfirmEmailTemplate.cshtml
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