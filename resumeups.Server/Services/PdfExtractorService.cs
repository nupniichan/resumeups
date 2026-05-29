using resumeups.Server.Interfaces;
using UglyToad.PdfPig;

namespace resumeups.Server.Services
{
    public class PdfExtractorService : IResumeExtractorService
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
                        foreach (var word in words)
                            sb.Append(word.Text).Append(' ');
                        sb.AppendLine();
                    }
                }

                return sb.ToString();
            });
        }
    }
}
