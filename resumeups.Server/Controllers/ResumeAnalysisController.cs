using Microsoft.AspNetCore.Mvc;
using resumeups.Server.Interfaces;
using resumeups.Server.Models;

namespace resumeups.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeAnalysisController : ControllerBase
    {
        private readonly IAnalyzeService _analyzeService;
        public ResumeAnalysisController(IAnalyzeService analyzeService)
        {
            _analyzeService = analyzeService;
        }
        [HttpPost("analyze")]
        public async Task<IActionResult> Analyze([FromBody] AnalyzeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Resume) || string.IsNullOrWhiteSpace(request.JobDescription))
            {
                return BadRequest("Missing data");
            }
            var result = await _analyzeService.Analyze(request);

            return Ok(result);
        }
    }
}
