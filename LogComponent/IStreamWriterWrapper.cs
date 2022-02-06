namespace LogComponent
{
    public interface IStreamWriterWrapper
    {
        Task WriteAsync(string? value);
        bool AutoFlush { get; set; }
        List<string?> LinesInCurrentFile { get; set; }
        Task StartNewFileAsync(DateTime fileDate);
        Task FlushAsync();
        Task CloseAsync();
    }
}
