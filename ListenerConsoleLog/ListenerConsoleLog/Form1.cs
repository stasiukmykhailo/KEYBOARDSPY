using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ListenerConsoleLog
{
    public partial class Form1 : Form
    {
        public delegate void TextDelegate(string str);
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    ipBox.Text = ip.ToString();
            }
        }
        Socket receiverSocket;
        Task listenerTask;
        private void button1_Click(object sender, EventArgs e)
        {
            if(int.TryParse(portBox.Text, out _) && portBox.Text != String.Empty)
            {
                if (listenerTask == null)
                {
                    portBox.ReadOnly = true;
                    listenerTask = Task.Run(async () =>
                    {
                        IPAddress localIP = IPAddress.Parse(ipBox.Text);
                        IPEndPoint localEP = new IPEndPoint(localIP, int.Parse(portBox.Text));
                        receiverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
                        receiverSocket.Bind(localEP);
                        MessageBox.Show("The local receiving point was succesfully created!", "Connection info");
                        while (true)
                        {
                            EndPoint remoteEP = new IPEndPoint(IPAddress.Any, int.Parse(portBox.Text));
                            byte[] buff = new byte[1024];
                            var receiveResult = await receiverSocket.ReceiveFromAsync(new ArraySegment<byte>(buff), SocketFlags.None, remoteEP);
                            string keyPressed = Encoding.Default.GetString(buff, 0, receiveResult.ReceivedBytes);
                            textBox1.BeginInvoke(
                                new TextDelegate(UpdateTextBox3), $"{receiveResult.RemoteEndPoint} Time: {DateTime.Now} Key: {keyPressed}");
                        }
                    });
                }
                else
                {
                    MessageBox.Show("The local receiving point was already created!", "Connection info");
                }
            }
            else if(portBox.Text == String.Empty)
                MessageBox.Show("The port box is empty!", "Port error");
            else
                MessageBox.Show("The port box syntax error!", "Port error");
        }
        private void UpdateTextBox3(string str)
        {
            StringBuilder builder = new StringBuilder(textBox1.Text);
            builder.AppendLine(str);
            textBox1.Text = builder.ToString();
            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show("Do you really want to close the program?", "Program closing", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    if (receiverSocket != null)
                        receiverSocket.Close();
                    Environment.Exit(0);
                }
                else
                    e.Cancel = true;
            }
        }
    }
}