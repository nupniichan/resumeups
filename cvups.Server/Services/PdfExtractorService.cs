using cvups.Server.Interfaces;
using UglyToad.PdfPig;

namespace cvups.Server.Services
{
    public class PDFCVExtractorService : IResumeExtractor
    {
        public bool extensionType(string extension)
        {
            return extension.Equals(".pdf", StringComparison.OrdinalIgnoreCase);
        }
        public async Task<string> ExtractAsync(Stream fileStream)
        {
            return await Task.Run(() =>
            {
                using var document = PdfDocument.Open(fileStream);
                var sb = new System.Text.StringBuilder();

                foreach (var page in document.GetPages())
                {
                    var words = page.GetWords();

                    if (words != null)
                    {
                        var pageContent = string.Join(" ", words.Select(w => w.Text));
                        sb.AppendLine(pageContent);
                    }
                }

                return sb.ToString();
            });
        }
    }
}
