using Microsoft.AspNetCore.Mvc;

namespace API.ExternalServices; 

public interface IEmailService {
    
    void SendEmail(string emailTo, string name,string body, string subject);
}