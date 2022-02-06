using LogComponent;

var startDate = DateTime.Now;
IStreamWriterWrapper wrapper = new StreamWriterWrapper(startDate);
ILogger logger = new Logger(wrapper, startDate);

for (int i = 0; i < 15; i++)
{
    try
    {
        await logger.WriteLogAsync("Number with Flush: " + i.ToString(), DateTime.Now);
    }
    catch (Exception)
    {
        // Nothing is done in response to the exception to let the application recover
    }
    Thread.Sleep(50);
}

await logger.StopWithFlushAsync();

var startDate2 = DateTime.Now.AddDays(1);
IStreamWriterWrapper wrapper2 = new StreamWriterWrapper(startDate2);
ILogger logger2 = new Logger(wrapper2, startDate2);

for (int i = 50; i > 0; i--)
{
    try
    {
        await logger2.WriteLogAsync("Number with No flush: " + i.ToString(), DateTime.Now);
    }
    catch (Exception)
    {
        // Nothing is done in response to the exception to let the application recover
    }
    Thread.Sleep(20);
}

await logger2.StopWithoutFlushAsync();

Console.ReadLine();