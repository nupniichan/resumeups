using Microsoft.AspNetCore.Mvc;
using cvups.Server.Interfaces;
using cvups.Server.Utils;

namespace cvups.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CVExtractorController : ControllerBase
    {
        private readonly IEnumerable<ICVExtractor> _extractors;
        private readonly IPiiMasker _piiMasker;

        public CVExtractorController(IEnumerable<ICVExtractor> extractors, IPiiMasker piiMasker)
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