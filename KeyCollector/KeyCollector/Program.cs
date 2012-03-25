using System;

namespace KeyCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            string logFileName = "";
            if (args.Length > 0) {
                logFileName = args[0];
            }
            else {
                string dateString = DateTime.Now.ToString("MM-dd-yy_HH-mm-ss");
                logFileName = string.Format("keylog_{0}.txt", dateString);
            }

            Console.WriteLine("[Starting logger to: {0}]", logFileName);
            Logger logger = new Logger(logFileName);

            // loop until exit is requested
            Console.WriteLine("Press [esc] to exit");
            while (true)
            {
                // don't use too much CPU
                System.Threading.Thread.Sleep(100);

                // check if [esc] was pressed
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.Escape)
                    {
                        break;
                    }
                }
            } 

            // shutdown the logger thread
            Console.WriteLine("[Stopping logger thread]");
            logger.close();
            Console.WriteLine("[Logger Stopped]");

            // final delay to allow last message to be read
            System.Threading.Thread.Sleep(500);
        }
    }
}
