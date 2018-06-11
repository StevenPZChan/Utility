using System;
using System.Windows.Forms;
using TsRemoteLib;

namespace TCPTester
{
    public partial class Form1 : Form
    {
        private TsRemoteV controller;
        private TsRemoteS cont;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            controller = new TsRemoteV();
            controller.SetIPaddr(1, "192.168.0.124", 1000);
            controller.Connect(1);
            cont = new TsRemoteS();
            cont.Client = controller.Client;
            cont.SetIPaddr(1, "192.168.0.124", 1000);
            cont.Connect(1);
            cont.WatchDogStart(100, 0, 0, null);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            controller.Disconnect();
        }

        private void lblServo_Click(object sender, EventArgs e)
        {
            try
            {
                //cont.ServoOn();
                //cont.ServoOff();
                TsPointS p = cont.GetPsnFbkWork();
                cont.Move(p, 0, 0, -2, 0, 0);
                TsPointV p2 = controller.GetPsnFbkWork();
            }
            catch { }
        }

        public void testTSStatusEvent(TsStatusMonitor para)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new TsRemote.TSStatusEvent(testTSStatusEvent), para);
            }
            else
            {
            }
        }
    }
}
