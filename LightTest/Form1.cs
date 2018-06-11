using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utility.IOs;

namespace LightTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            serialPortUtil1.OpenPort();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            serialPortUtil1.ClosePort();
        }

        private void serialPortUtil1_DataReceived(object sender, Utility.IOs.DataReceivedEventArgs e)
        {
            Match match = Regex.Match(e.DataReceived, "(\\d+)channel");
            if (!match.Success)
                return;

            int ind = int.Parse(match.Groups[1].Value);
            label1.BackColor = (ind & 1) > 0 ? Color.Green : SystemColors.Control;
            label2.BackColor = (ind & 2) > 0 ? Color.Green : SystemColors.Control;
            label3.BackColor = (ind & 4) > 0 ? Color.Green : SystemColors.Control;
            label4.BackColor = (ind & 8) > 0 ? Color.Green : SystemColors.Control;
            this.Invoke(new Action(() => this.Update()));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] message = SerialPortUtil.HexToByte(textBox1.Text);

        }
    }
}
