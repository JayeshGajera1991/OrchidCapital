using Microsoft.AspNetCore.Mvc;
namespace OrchidCapital.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public AdminController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Pages()
        {
            return View();
        }
        public IActionResult Roles()
        {
            return View();
        }
        public IActionResult SystemSettings()
        {
            return View();
        }
    }
}