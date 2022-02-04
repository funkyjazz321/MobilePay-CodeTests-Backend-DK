namespace LogTest
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Threading;

    public class Logger : ILogger
    {
        private Thread runThread;
        private List<LogLine> lines = new List<LogLine>();

        private StreamWriter writer; 

        private bool exit;
        private bool quitWithFlush = false;
        DateTime curDate = DateTime.Now;

        public Logger()
        {
            if (!Directory.Exists(@"C:\LogTest")) 
                Directory.CreateDirectory(@"C:\LogTest");

            writer = File.AppendText(@"C:\LogTest\Log" + DateTime.Now.ToString("yyyyMMdd HHmmss fff") + ".log");
            
            writer.Write("Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ') + "\t" + Environment.NewLine);

            writer.AutoFlush = true;

            runThread = new Thread(MainLoop);
            runThread.Start();
        }

        private void MainLoop()
        {
            while (!exit)
            {
                if (lines.Count > 0)
                {
                    int f = 0;
                    List<LogLine> _handled = new List<LogLine>();

                    foreach (LogLine logLine in lines)
                    {
                        f++;

                        if (f > 5)
                            continue;
                        
                        if (!exit || quitWithFlush)
                        {
                            _handled.Add(logLine);

                            StringBuilder stringBuilder = new StringBuilder();

                            if ((DateTime.Now - curDate).Days != 0)
                            {
                                curDate = DateTime.Now;

                                writer = File.AppendText(@"C:\LogTest\Log" + DateTime.Now.ToString("yyyyMMdd HHmmss fff") + ".log");

                                writer.Write("Timestamp".PadRight(25, ' ') + "\t" + "Data".PadRight(15, ' ') + "\t" + Environment.NewLine);

                                stringBuilder.Append(Environment.NewLine);

                                writer.Write(stringBuilder.ToString());

                                writer.AutoFlush = true;
                            }

                            stringBuilder.Append(logLine.Timestamp.ToString("yyyy-MM-dd HH:mm:ss:fff"));
                            stringBuilder.Append("\t");
                            stringBuilder.Append(logLine.LineText());
                            stringBuilder.Append("\t");

                            stringBuilder.Append(Environment.NewLine);

                            writer.Write(stringBuilder.ToString());
                        }
                    }

                    for (int y = 0; y < _handled.Count; y++)
                    {
                        lines.Remove(_handled[y]);   
                    }

                    if (quitWithFlush == true && lines.Count == 0) 
                        exit = true;

                    Thread.Sleep(50);
                }
            }
        }

        public async Task StopWithoutFlushAsync()
        {
            await Task.Run(() =>
            {
                exit = true;
            });
        }

        public async Task StopWithFlushAsync()
        {
            await Task.Run(() =>
            {
                quitWithFlush = true;
            });
        }

        public async Task WriteLogAsync(string s)
        {
            await Task.Run(() =>
            {
                lines.Add(new LogLine() { Text = s, Timestamp = DateTime.Now });
            });
        }
    }
}