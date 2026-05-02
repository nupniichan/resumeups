namespace resumeups.Server.Interfaces
{
    public interface IPiiMaskerService
    {
        public string MaskEmail(string input);
        public string MaskPhoneNumber(string input);
        public string MaskDateOfBirth(string input);
        public string MaskAddress(string input);
        public string MaskLinkedIn(string input);
        public string MaskPortfolioWebsite(string input);
        public string MaskGitHub(string input);

        public string MaskAll(string resumetext);
    }
}