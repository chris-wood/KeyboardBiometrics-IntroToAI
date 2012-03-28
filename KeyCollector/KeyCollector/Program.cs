using System;
using System.Runtime.InteropServices;

namespace KeyCollector
{
    class Program
    {
        static Logger logger;

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
            logger = new Logger(logFileName);


            SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);

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
                        // we're going down
                        break;
                    }
                }
            } 
            // we're done
            shutdown();
        }

        // announces the end of logging and closes the program neatly
        private static void shutdown()
        {
            // shutdown the logger thread
            Console.WriteLine("[Stopping logger thread]");
            logger.close();
            Console.WriteLine("[Logger Stopped]");

            // final delay to allow last message to be read
            System.Threading.Thread.Sleep(500);
        }

        // special event control handler
        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            switch (ctrlType)
            {
                // in the case of any special events, shutdown
                case CtrlTypes.CTRL_C_EVENT:
                case CtrlTypes.CTRL_BREAK_EVENT:
                case CtrlTypes.CTRL_CLOSE_EVENT:
                case CtrlTypes.CTRL_LOGOFF_EVENT:
                case CtrlTypes.CTRL_SHUTDOWN_EVENT:
                    shutdown();
                    break;
            }
            return true;
        }

        #region unmanaged
        // Declare the SetConsoleCtrlHandler function
        // as external and receiving a delegate.

        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

        // A delegate type to be used as the handler routine
        // for SetConsoleCtrlHandler.
        public delegate bool HandlerRoutine(CtrlTypes CtrlType);

        // An enumerated type for the control messages
        // sent to the handler routine.
        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        #endregion
    }
}
