namespace LogComponent
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading;

    public class Logger : ILogger
    {
        private IStreamWriterWrapper writer;
        private DateTime currentDate;

        public Logger(IStreamWriterWrapper writer, DateTime currentDate)
        {
            this.writer = writer;
            this.currentDate = currentDate;

            if (!Directory.Exists(@"C:\Logs"))
                Directory.CreateDirectory(@"C:\Logs");
        }

        public DateTime CurrentDate { get { return currentDate; } }

        public async Task<List<string?>> GetLinesInCurrentFile()
        {
            return await Task.Run(() =>
            {
                return writer.LinesInCurrentFile;
            });
        }

        public async Task StopWithoutFlushAsync()
        {
            writer.AutoFlush = false;
            await writer.CloseAsync();
        }

        public async Task StopWithFlushAsync()
        {
            await writer.FlushAsync();
        }

        public async Task WriteLogAsync(string message, DateTime logTime)
        {
            await Task.Run(() =>
            {
                if(logTime.Date > currentDate.Date)
                {
                    currentDate = logTime;
                    writer.StartNewFileAsync(logTime);
                }
                writer.WriteAsync(FormatLogEntry(message, logTime));
            });
        }

        private string FormatLogEntry(string message, DateTime logTime)
        {
            return $"{logTime:yyyy-MM-dd HH:mm:ss:fff} {message}{Environment.NewLine}";
        }
    }
}