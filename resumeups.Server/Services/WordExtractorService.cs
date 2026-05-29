using resumeups.Server.Interfaces;
using NPOI.XWPF.UserModel;

namespace resumeups.Server.Services
{
    public class WordExtractorService : IResumeExtractorService
    {
        public bool extensionType(string extension)
        {
            return extension.Equals(".docx", StringComparison.OrdinalIgnoreCase) || extension.Equals(".doc", StringComparison.OrdinalIgnoreCase);
        }
        public async Task<string> ExtractAsync(Stream fileStream)
        {
            return await Task.Run(() =>
            {
                if (fileStream.CanSeek) fileStream.Position = 0;

                using var doc = new XWPFDocument(fileStream);
                var sb = new System.Text.StringBuilder();

                foreach (var paragraph in doc.Paragraphs)
                {
                    var text = paragraph.Text?.Trim();
                    if (!string.IsNullOrEmpty(text))
                    {
                        sb.AppendLine(text);
                    }
                }

                foreach (var table in doc.Tables)
                {
                    foreach (var row in table.Rows)
                    {
                        foreach (var cell in row.GetTableCells())
                        {
                            var cellText = cell.GetText()?.Trim();
                            if (!string.IsNullOrEmpty(cellText))
                            {
                                sb.Append(cellText + "\t");
                            }
                        }
                        sb.AppendLine();
                    }
                }

                return sb.ToString();
            });
        }
    }
}