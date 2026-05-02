namespace resumeups.Server.Models
{
    public sealed class SecuritySettings
    {
        public long MaxRequestBodyBytes { get; set; } = 5 * 1024 * 1024;
        public long MaxUploadBytes { get; set; } = 5 * 1024 * 1024;
        public string[] AllowedExtensions { get; set; } = [".pdf", ".doc", ".docx"];
    }
}
