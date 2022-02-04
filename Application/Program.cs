using LogTest;

ILogger logger = new Logger();

for (int i = 0; i < 15; i++)
{
    await logger.WriteLogAsync("Number with Flush: " + i.ToString());
    Thread.Sleep(50);
}

await logger.StopWithFlushAsync();

ILogger logger2 = new Logger();

for (int i = 50; i > 0; i--)
{
    await logger2.WriteLogAsync("Number with No flush: " + i.ToString());
    Thread.Sleep(20);
}

await logger2.StopWithoutFlushAsync();

Console.ReadLine();