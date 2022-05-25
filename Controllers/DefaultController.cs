using Microsoft.AspNetCore.Mvc;

namespace API.Controllers; 

[Route("[controller]")]
// [Route("v{version:apiVersion}/[controller]")]
[ApiController]
public class DefaultController : ControllerBase {
    public DefaultController() {
    }
}