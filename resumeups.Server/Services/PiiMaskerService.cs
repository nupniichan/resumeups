using resumeups.Server.Interfaces;
using System.Text.RegularExpressions;

namespace resumeups.Server.Services
{
    public class PiiMaskerService : IPiiMasker
    {
        public string MaskEmail(string input) =>
            Regex.Replace(input,
                @"\b[A-Za-z0-9._%+\-]+(@[A-Za-z0-9.\-]+\.[A-Za-z]{2,})\b",
                "****$1");

        // Phone (E.164, US, UK, VN, EU...)
        public string MaskPhoneNumber(string input) =>
            Regex.Replace(input,
                @"\+?\d{1,3}[\s\-.]?\(?\d{1,4}\)?[\s\-.]?\d[\d\s\-\.]{4,10}\d",
                "****");

        public string MaskDateOfBirth(string input)
        {
            // dd/MM/yyyy or MM/dd/yyyy or yyyy-MM-dd
            input = Regex.Replace(input,
                @"\b\d{1,4}[\/\-\.]\d{1,2}[\/\-\.]\d{2,4}\b",
                "****");

            // dd Month yyyy  (04 December 2006)
            input = Regex.Replace(input,
                @"\b\d{1,2}\s+(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[a-z]*\.?\s+\d{4}\b",
                "****",
                RegexOptions.IgnoreCase);

            return input;
        }

        public string MaskAddress(string input) =>
            Regex.Replace(input,
                @"(?i)(address|addr|location|địa\s*chỉ|dirección|adresse|indirizzo|住所|주소)\s*[:\-]?\s*"
              + @"(.{8,120}?)(?=\r?\n|$)",
                m => m.Groups[1].Value + ": ****");

        public string MaskLinkedIn(string input) =>
            Regex.Replace(input,
                @"(linkedin\.com/in/)[A-Za-z0-9\-]+",
                "$1****");

        public string MaskGitHub(string input) =>
            Regex.Replace(input,
                @"(github\.com/)[A-Za-z0-9\-]+",
                "$1****");

        public string MaskPortfolioWebsite(string input) =>
            Regex.Replace(input,
                @"https?://(?!(?:www\.)?linkedin\.com)(?!(?:www\.)?github\.com)[A-Za-z0-9\-\.\/?=&%_]+",
                "https://****");

        // Full name - but it might not work for all name
        public string MaskFullName(string input) =>
            Regex.Replace(input,
                @"(?i)(full\s*name|first\s*name|last\s*name|given\s*name|surname|name"
              + @"|họ\s*(?:và?\s*)?tên|nombre|nom|nome|名前|성명|名字)\s*[:\-]?\s*"
              + @"[\p{L}][\p{L}\s\-'\.]{1,60}",
                m => m.Groups[1].Value + ": ****");

        public string MaskIdDocuments(string input) =>
            Regex.Replace(input,
                @"(?i)(passport\s*(?:no\.?|number|#)?|national\s*id|id\s*(?:no\.?|number|card)"
              + @"|cccd|cmnd|nric|sin|dni|nif|bsn|pesel|aadhaar)\s*[:\-#]?\s*[A-Z0-9]{6,18}\b",
                m => m.Groups[1].Value + ": ****");

        public string MaskAll(string resumeText)
        {
            resumeText = MaskEmail(resumeText);
            resumeText = MaskPhoneNumber(resumeText);
            resumeText = MaskDateOfBirth(resumeText);
            resumeText = MaskAddress(resumeText);
            resumeText = MaskLinkedIn(resumeText);
            resumeText = MaskGitHub(resumeText);
            resumeText = MaskPortfolioWebsite(resumeText);
            resumeText = MaskIdDocuments(resumeText);
            resumeText = MaskFullName(resumeText);
            return resumeText;
        }
    }
}