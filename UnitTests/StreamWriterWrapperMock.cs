using LogComponent;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTests
{
    internal class StreamWriterWrapperMock : IStreamWriterWrapper
    {
        public StreamWriterWrapperMock(DateTime dateTime)
        {
            CurrentFileName = CreateFileName(dateTime);
        }

        public bool AutoFlush { get; set; }
        public List<string?> LinesInCurrentFile { get; set; } = new List<string?>();
        public string CurrentFileName { get; set; }

        public async Task WriteAsync(string? value)
        {
            await Task.Run(() =>
            {
                LinesInCurrentFile.Add(value);
            });
        }

        public async Task StartNewFileAsync(DateTime fileDate)
        {
            await Task.Run(() =>
            {
                CurrentFileName = CreateFileName(fileDate);
            });
        }
        public async Task FlushAsync()
        {
            await Task.Run(() =>
            {
            });
        }

        public async Task CloseAsync()
        {
            await Task.Run(() =>
            {
            });
        }

        private string CreateFileName(DateTime dateTime)
        {
            return @"C:\Logs\Log_" + dateTime.ToString("ddMMyyyy") + ".log";
        }
    }
}
