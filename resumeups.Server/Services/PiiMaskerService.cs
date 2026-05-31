using resumeups.Server.Interfaces;
using System.Text.RegularExpressions;

namespace resumeups.Server.Services
{
    public partial class PiiMaskerService : IPiiMaskerService
    {
        [GeneratedRegex(@"\b[A-Za-z0-9._%+\-]+(@[A-Za-z0-9.\-]+\.[A-Za-z]{2,})\b", RegexOptions.None, 250)]
        private static partial Regex EmailRegex();

        [GeneratedRegex(@"\+?\d{1,3}[\s\-.]?\(?\d{1,4}\)?[\s\-.]?\d[\d\s\-\.]{4,10}\d", RegexOptions.None, 250)]
        private static partial Regex PhoneRegex();

        [GeneratedRegex(@"\b\d{1,4}[\/\-\.]\d{1,2}[\/\-\.]\d{2,4}\b", RegexOptions.None, 250)]
        private static partial Regex DobNumericRegex();

        [GeneratedRegex(@"\b\d{1,2}\s+(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)[a-z]*\.?\s+\d{4}\b", RegexOptions.IgnoreCase, 250)]
        private static partial Regex DobTextRegex();

        [GeneratedRegex(@"(?i)(address|addr|location|địa\s*chỉ|dirección|adresse|indirizzo|住所|주소)\s*[:\-]?\s*(.{8,120}?)(?=\r?\n|$)", RegexOptions.None, 250)]
        private static partial Regex AddressRegex();

        [GeneratedRegex(@"(linkedin\.com/in/)[A-Za-z0-9\-]+", RegexOptions.None, 250)]
        private static partial Regex LinkedInRegex();

        [GeneratedRegex(@"(github\.com/)[A-Za-z0-9\-]+", RegexOptions.None, 250)]
        private static partial Regex GitHubRegex();

        [GeneratedRegex(@"https?://(?!(?:www\.)?linkedin\.com)(?!(?:www\.)?github\.com)[A-Za-z0-9\-\.\/?=&%_]+", RegexOptions.None, 250)]
        private static partial Regex PortfolioRegex();

        [GeneratedRegex(@"(?i)(full\s*name|first\s*name|last\s*name|given\s*name|surname|name|họ\s*(?:và?\s*)?tên|nombre|nom|nome|名前|성명|名字)\s*[:\-]?\s*[\p{L}][\p{L}\s\-'\.]{1,60}", RegexOptions.None, 250)]
        private static partial Regex FullNameRegex();

        [GeneratedRegex(@"(?i)(passport\s*(?:no\.?|number|#)?|national\s*id|id\s*(?:no\.?|number|card)|cccd|cmnd|nric|sin|dni|nif|bsn|pesel|aadhaar)\s*[:\-#]?\s*[A-Z0-9]{6,18}\b", RegexOptions.None, 250)]
        private static partial Regex IdDocumentsRegex();

        public string MaskEmail(string input) =>
            EmailRegex().Replace(input, "****$1");

        // Phone (E.164, US, UK, VN, EU...)
        public string MaskPhoneNumber(string input) =>
            PhoneRegex().Replace(input, "****");

        public string MaskDateOfBirth(string input)
        {
            // dd/MM/yyyy or MM/dd/yyyy or yyyy-MM-dd
            input = DobNumericRegex().Replace(input, "****");

            // dd Month yyyy  (04 December 2006)
            input = DobTextRegex().Replace(input, "****");

            return input;
        }

        public string MaskAddress(string input) =>
            AddressRegex().Replace(input, m => m.Groups[1].Value + ": ****");

        public string MaskLinkedIn(string input) =>
            LinkedInRegex().Replace(input, "$1****");

        public string MaskGitHub(string input) =>
            GitHubRegex().Replace(input, "$1****");

        public string MaskPortfolioWebsite(string input) =>
            PortfolioRegex().Replace(input, "https://****");

        // Full name - but it might not work for all name
        public string MaskFullName(string input) =>
            FullNameRegex().Replace(input, m => m.Groups[1].Value + ": ****");

        public string MaskIdDocuments(string input) =>
            IdDocumentsRegex().Replace(input, m => m.Groups[1].Value + ": ****");

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