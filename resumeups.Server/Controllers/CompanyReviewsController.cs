using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using resumeups.Server.Interfaces;
using resumeups.Server.Models;

namespace resumeups.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("api/company-reviews")]
    public class CompanyReviewsController : ControllerBase
    {
        private readonly ICompanyReviewsService _reviewsService;

        public CompanyReviewsController(ICompanyReviewsService reviewsService)
        {
            _reviewsService = reviewsService;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search([FromBody] CompanyReviewRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.CompanyName))
            {
                return BadRequest("Company name is required.");
            }

            var result = await _reviewsService.GetCompanyReviewsAsync(request.CompanyName, request.Language);
            return Ok(result);
        }
    }
}
