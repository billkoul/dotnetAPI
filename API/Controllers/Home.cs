using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// API homepage
    /// </summary>
	[Route("api/[controller]")]
	[ApiController]

	public class HomeController : ControllerBase
	{

        /// <summary>
        /// API homepage
        /// </summary>
		[HttpGet]
		public IActionResult Index()
		{
            return Ok("API online");
		}

	}
}
