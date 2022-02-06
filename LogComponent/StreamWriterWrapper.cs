namespace LogComponent
{
    public class StreamWriterWrapper : IStreamWriterWrapper
    {
        private StreamWriter writer;

        public StreamWriterWrapper(DateTime dateTime)
        {
            writer = new StreamWriter(CreateFileName(dateTime));
            writer.AutoFlush = false;
        }

        public bool AutoFlush { get { return writer.AutoFlush; } set { writer.AutoFlush = value; } }
        public List<string?> LinesInCurrentFile { get; set; } = new List<string?>();

        public async Task WriteAsync(string? value)
        {
            LinesInCurrentFile.Add(value);
            await writer.WriteAsync(value);
        }

        public async Task FlushAsync()
        {
            await writer.FlushAsync();
        }

        public async Task CloseAsync()
        {
            await Task.Run(() =>
            {
                writer.Close();
            });
        }

        public async Task StartNewFileAsync(DateTime fileDate)
        {
            await Task.Run(() =>
            {
                LinesInCurrentFile = new List<string?>();
                string filename = CreateFileName(fileDate);
                writer = new StreamWriter(filename);
                writer.AutoFlush = false;
            });
        }

        private string CreateFileName(DateTime fileDate)
        {
            return @"C:\Logs\Log_" + fileDate.ToString("ddMMyyyy") + ".log";
        }
    }
}
