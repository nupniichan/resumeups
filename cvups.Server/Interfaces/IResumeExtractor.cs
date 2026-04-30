namespace resumeups.Server.Interfaces
{
    public interface IResumeExtractor
    {
        bool extensionType(string extension);
        Task<string> ExtractAsync(Stream fileStream);
    }
}