using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orchid.DataModels;
using OrchidCapitalCoreAPI.Interface;

namespace OrchidCapitalCoreAPI.Controllers
{
    [Route("api/ORCHIDCAPITAL")]
    [ApiController]
    [ApiVersion(APIVersions.Version1)]
    public class LoginController : Controller
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IConfiguration _configuration;
        public LoginController(ILoginRepository loginRepository, IConfiguration configuration)
        {
            _loginRepository = loginRepository;
            _configuration = configuration;
        }

        [MapToApiVersion(APIVersions.Version1)]
        [AllowAnonymous]
        [HttpPost("v{version:apiVersion}/AuthenticateUser")]
        public async Task<IActionResult> AuthenticateUser([FromBody] LoginRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response = await _loginRepository.AuthenticateUser(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Ok(new CommonResponse { Message = "Error occurred", Response = null, StatusCode = 500 });
            }
        }
    }
}
