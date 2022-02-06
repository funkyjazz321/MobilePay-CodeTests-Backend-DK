using LogComponent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace UnitTests.LoggerTests
{
    [TestClass]
    public class LoggerTests
    {
        [TestMethod]
        public async Task WriteAsync_Successfully()
        {
            var startDate = DateTime.Now;
            var streamWriterWrapper = new StreamWriterWrapperMock(startDate);
            var logger = new Logger(streamWriterWrapper, startDate);

            var linesBeforeAdding = await logger.GetLinesInCurrentFile();
            var numberOfLinesBeforeAdding = linesBeforeAdding.Count;

            // The start date is used again here to prevent writing to a new file
            await logger.WriteLogAsync("Test", startDate);

            var linesAfterAdding = await logger.GetLinesInCurrentFile();
            var numberOfLinesAfterAdding = linesAfterAdding.Count;

            Assert.AreEqual(numberOfLinesBeforeAdding + 1, numberOfLinesAfterAdding);
        }

        [TestMethod]
        public async Task WriteAsyncToNewFileAfterMidnight_Successfully()
        {
            var earlierDate = new DateTime(2022, 1, 1, 23, 59, 59);
            var laterDate = new DateTime(2022, 1, 2, 0, 0, 0);
            var laterDateLaterInDay = new DateTime(2022, 1, 2, 0, 0, 1);

            var streamWriterWrapper = new StreamWriterWrapperMock(earlierDate);
            var logger = new Logger(streamWriterWrapper, earlierDate);

            // Check that original date is correct
            Assert.AreEqual(earlierDate.Date, logger.CurrentDate.Date);

            await logger.WriteLogAsync("Test", laterDate);

            // Check that new file was started after inserting from a later day
            Assert.AreEqual(laterDate.Date, logger.CurrentDate.Date);

            await logger.WriteLogAsync("Test", laterDateLaterInDay);

            // Check that a new file has not been started later in the same day
            Assert.AreNotEqual(laterDateLaterInDay, logger.CurrentDate);
        }
    }
}