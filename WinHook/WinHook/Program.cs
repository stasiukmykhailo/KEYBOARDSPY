using System.Net.Sockets;
using System.Net;

namespace WinHook
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
       
            if (DuplicateLaunch.Default.Detect())
            {
                NamedEvent.Deafult.Set();
            }
            else
            {
                Application.Run(new WinHookContext());
            }
        }
    }
}