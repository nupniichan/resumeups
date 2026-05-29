using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using resumeups.Server.Interfaces;
using resumeups.Server.Models;
using resumeups.Server.Utils;

namespace resumeups.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("api/resume-extractions")]
    public class ResumeExtractorController : ControllerBase
    {
        private readonly IEnumerable<IResumeExtractorService> _extractors;
        private readonly IPiiMaskerService _piiMasker;
        private readonly SecuritySettings _security;

        public ResumeExtractorController(IEnumerable<IResumeExtractorService> extractors, IPiiMaskerService piiMasker, IOptions<SecuritySettings> security)
        {
            _extractors = extractors;
            _piiMasker = piiMasker;
            _security = security.Value;
        }

        [HttpPost]
        [HttpPost("extract")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<string>> ExtractAndMaskPII(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is required");

            if (file.Length > _security.MaxUploadBytes)
                return BadRequest("File exceeds size limit");

            var extension = Path.GetExtension(file.FileName);
            var normalizedExt = NormalizeExt(extension);

            var allowedExtensions = AllowedExtension(_security.AllowedExtensions);
            if (!allowedExtensions.Contains(normalizedExt))
                return BadRequest("Only .pdf, .doc, .docx are supported");

            var extractor = _extractors.FirstOrDefault(x => x.extensionType(extension));
            if (extractor == null)
                return BadRequest($"'{extension}' is not supported.");

            await using var stream = file.OpenReadStream();

            var extractedText = await extractor.ExtractAsync(stream);
            var normalizedText = TextNormalizeHelper.NormalizeText(extractedText);
            var maskedText = _piiMasker.MaskAll(normalizedText);

            return Ok(maskedText);
        }

        private static HashSet<string> AllowedExtension(string[] configured)
        {
            var list = configured is { Length: > 0 } ? configured : [".pdf", ".doc", ".docx"];
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var e in list)
                set.Add(NormalizeExt(e));
            return set;
        }

        private static string NormalizeExt(string ext)
        {
            ext = ext.Trim().ToLowerInvariant();
            return ext.StartsWith('.') ? ext : "." + ext;
        }
    }
}