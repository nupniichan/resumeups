using cvups.Server.Interfaces;
using NPOI.XWPF.UserModel;

namespace cvups.Server.Services
{
    public class WordCVExtractorService : ICVExtractor
    {
        public bool extensionType(string extension)
        {
            return extension.Equals(".docx", StringComparison.OrdinalIgnoreCase) || extension.Equals(".doc", StringComparison.OrdinalIgnoreCase);
        }
        public async Task<string> ExtractAsync(Stream fileStream)
        {
            // I use NPOI library to convert
            return await Task.Run(() =>
            {
                var doc = new XWPFDocument(fileStream);
                var sb = new System.Text.StringBuilder();

                foreach (var paragraph in doc.Paragraphs)
                {
                    sb.AppendLine(paragraph.Text);
                }

                return sb.ToString();
            });
        }
    }
}