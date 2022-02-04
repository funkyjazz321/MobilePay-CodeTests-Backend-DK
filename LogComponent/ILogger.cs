using System.Linq;
using System.Threading.Tasks;

namespace LogTest
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
        /// <param name="s">The s to written to the log</param>
        Task WriteLogAsync(string s);
    }
}
