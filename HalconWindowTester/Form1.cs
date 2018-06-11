using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;

namespace HalconWindowTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //HRegion ho_Region;
            //double row1, column1, row2, column2;
            //hWindowControl1.HalconWindow.SetPart(0, 0, 1023, 1279);
            //hWindowControl1.Focus();
            //hWindowControl1.HalconWindow.DrawRectangle1(out row1, out column1, out row2, out column2);
            ho_Region = new HRegion(row1, column1, row2, column2);

            HSystem.SetSystem("width", 1280);
            HSystem.SetSystem("height", 1024);
            HOperatorSet.SetPart(hWindowControl1.HalconWindow, 0, 0, 1023, 1279);
            HOperatorSet.SetDraw(hWindowControl1.HalconWindow, "margin");
            HOperatorSet.SetColor(hWindowControl1.HalconWindow, "blue");
            HOperatorSet.SetLineWidth(hWindowControl1.HalconWindow, 2);
            HOperatorSet.ClearWindow(hWindowControl1.HalconWindow);
            HObject ho_DetectRegion;
            HTuple hv_Row, hv_Column, hv_Phi, hv_Length1, hv_Length2;
            hWindowControl1.Focus();
            HOperatorSet.DrawRectangle2(hWindowControl1.HalconWindow, out hv_Row, out hv_Column, out hv_Phi, out hv_Length1, out hv_Length2);
            HOperatorSet.GenRectangle2(out ho_DetectRegion, hv_Row, hv_Column, hv_Phi, hv_Length1, hv_Length2);
            HOperatorSet.DispObj(ho_DetectRegion, hWindowControl1.HalconWindow);
            ho_DetectRegion.Dispose();
        }
    }
}
