namespace cvups.Server.Interfaces
{
    public interface ICVExtractor
    {
        bool extensionType(string extension);
        Task<string> ExtractAsync(Stream fileStream);
    }
}