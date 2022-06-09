using API.Enums;

namespace API.ExternalServices.Email; 

public interface IEmailService {
    
    void SendEmail(string emailTo, string name,string body, string subject);
}