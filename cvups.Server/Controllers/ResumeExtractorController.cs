using Microsoft.AspNetCore.Mvc;
using resumeups.Server.Interfaces;
using resumeups.Server.Utils;

namespace resumeups.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResumeExtractorController : ControllerBase
    {
        private readonly IEnumerable<IResumeExtractor> _extractors;
        private readonly IPiiMasker _piiMasker;

        public ResumeExtractorController(IEnumerable<IResumeExtractor> extractors, IPiiMasker piiMasker)
        {
            _extractors = extractors;
            _piiMasker = piiMasker;
        }

        [HttpPost("extract")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<string>> ExtractAndMaskPII(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("File is required");
            }

            var extension = Path.GetExtension(file.FileName);
            var extractor = _extractors.FirstOrDefault(x => x.extensionType(extension));
            if (extractor == null)
            {
                return BadRequest("Only .pdf, .doc, .docx are supported");
            }

            await using var stream = file.OpenReadStream();

            var extractedText = await extractor.ExtractAsync(stream);
            var normalizedText = TextNormalizeHelper.NormalizeText(extractedText);
            var maskedText = _piiMasker.MaskAll(normalizedText);

            return Ok(maskedText);
        }
    }
}