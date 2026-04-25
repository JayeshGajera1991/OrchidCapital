using Microsoft.AspNetCore.Mvc;

namespace OrchidCapital.Controllers
{
    public class DashboardController:BaseController
    {
         private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public DashboardController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public IActionResult Dashboard()
        {
            return View();
        }
    }
}