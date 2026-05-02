namespace resumeups.Server.Interfaces
{
    public interface IResumeExtractorService
    {
        bool extensionType(string extension);
        Task<string> ExtractAsync(Stream fileStream);
    }
}