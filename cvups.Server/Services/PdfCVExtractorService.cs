using cvups.Server.Interfaces;
using UglyToad.PdfPig;

namespace cvups.Server.Services
{
    public class PDFCVExtractorService : ICVExtractor
    {
        public bool extensionType(string extension)
        {
            return extension.Equals(".pdf", StringComparison.OrdinalIgnoreCase);
        }
        public async Task<string> ExtractAsync(Stream fileStream)
        {
            // I use PdfPig library to convert
            return await Task.Run(() =>
            {
                using var document = PdfDocument.Open(fileStream);
                var sb = new System.Text.StringBuilder();

                foreach (var page in document.GetPages())
                {
                    sb.Append(page.Text);
                }

                return sb.ToString();
            });
        }
    }
}
