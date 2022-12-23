using System.Net;

namespace WinHook
{
    public partial class ConnectionInfo : Form
    {
        public ConnectionInfo()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (IPAddress.TryParse(ipBox.Text, out _) && $"{portBox.Text}".All(char.IsDigit) && portBox.Text != String.Empty)
                this.Hide();
            else if (ipBox.Text == String.Empty && portBox.Text == String.Empty)
                MessageBox.Show("The IP and the port box are empty!", "Error");
            else if (!IPAddress.TryParse(ipBox.Text, out _) && ipBox.Text != String.Empty && int.TryParse(portBox.Text, out _))
                MessageBox.Show("The IP box syntax error!", "IP error");
            else if (IPAddress.TryParse(ipBox.Text, out _) && portBox.Text != String.Empty && !int.TryParse(portBox.Text, out _))
                MessageBox.Show("The port box syntax error!", "Port error");
            else if (!IPAddress.TryParse(ipBox.Text, out _) && ipBox.Text != String.Empty && portBox.Text != String.Empty && !int.TryParse(portBox.Text, out _))
                MessageBox.Show("The IP and the port box syntax error!", "Error");
            else if (!IPAddress.TryParse(ipBox.Text, out _) && ipBox.Text != String.Empty && portBox.Text == String.Empty)
                MessageBox.Show("The IP box syntax error and the port box is empty!", "Error");
            else if (IPAddress.TryParse(ipBox.Text, out _) && ipBox.Text != String.Empty && portBox.Text == String.Empty)
                MessageBox.Show("The port box is empty!", "Port error");
            else if (!int.TryParse(portBox.Text, out _) && ipBox.Text == String.Empty && portBox.Text != String.Empty)
                MessageBox.Show("The port box syntax error and the IP box is empty!", "Error");
            else if (int.TryParse(portBox.Text, out _) && ipBox.Text == String.Empty && portBox.Text != String.Empty)
                MessageBox.Show("The IP box is empty!", "IP error");
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show("Do you really want to close the program?", "Program closing", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    Environment.Exit(0);
                else
                    e.Cancel = true;
            }
            else
                e.Cancel = true;
        }
    }
}