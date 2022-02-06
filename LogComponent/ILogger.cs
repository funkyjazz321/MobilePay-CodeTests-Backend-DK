namespace LogComponent
{
    public interface ILogger
    {
        /// <summary>
        /// Stop the logging. If any outstadning logs theses will not be written to Log
        /// </summary>
        Task StopWithoutFlushAsync();

        /// <summary>
        /// Stop the logging. The call will not return until all all logs have been written to Log.
        /// </summary>
        Task StopWithFlushAsync();

        /// <summary>
        /// WriteLog a message to the Log.
        /// </summary>
        /// <param name="message">The message to written to the log</param>
        /// <param name="logTime">The timing of the event to be written to the log</param>
        Task WriteLogAsync(string message, DateTime logTime);
    }
}
