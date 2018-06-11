using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using HalconDotNet;
using TsRemoteLib;
using Utility.Files;

namespace Utility.Form
{
    /// <summary>
    /// 四轴SCARA机器人标定助手
    /// </summary>
    public partial class ScaraCalibTool : UserControl
    {
        /// <summary>
        /// Halcon窗口
        /// </summary>
        public HWindowControl HWControl
        {
            get { return hWindowControl1; }
        }
        /// <summary>
        /// 显示的图像
        /// </summary>
        public HImage Image
        {
            set
            {
                if (!processing)
                {
                    int width, height;
                    if (ho_Image.IsInitialized())
                        ho_Image.Dispose();
                    ho_Image = value;
                    ho_Image.GetImageSize(out width, out height);
                    HSystem.SetSystem("width", width);
                    HSystem.SetSystem("height", height);
                    hWindowControl1.HalconWindow.SetPart(0, 0, height - 1, width - 1);
                    hWindowControl1.HalconWindow.DispObj(ho_Image);
                }
            }
        }
        /// <summary>
        /// 设定机器人通信IP地址
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// 设定机器人通信端口号
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// 标定误差
        /// </summary>
        public double Error
        {
            get { return error; }
        }
        /// <summary>
        /// 标定数据
        /// </summary>
        public DataTable calData;
        /// <summary>
        /// 相机内参
        /// </summary>
        public HTuple hv_CameraParam;
        /// <summary>
        /// 相机外参
        /// </summary>
        public HPose hv_Pose;

        private HImage ho_Image = new HImage();
        private HTuple hv_WorldX, hv_WorldY, hv_WorldZ, hv_ImageRow, hv_ImageColumn;
        private double error;
        private int count = 1;
        private bool processing = false;
        /// <summary>
        /// 构造函数
        /// </summary>
        public ScaraCalibTool()
        {
            InitializeComponent();
            calData = new DataTable();
            calData.Columns.Add("ID", typeof(int));
            calData.Columns.Add("r", typeof(double));
            calData.Columns.Add("c", typeof(double));
            calData.Columns.Add("x", typeof(double));
            calData.Columns.Add("y", typeof(double));
            calData.Columns.Add("z", typeof(double));
            dataGridView1.DataSource = calData;
            this.Disposed += (sender, e) => {
                if (ho_Image.IsInitialized())
                    ho_Image.Dispose();
            };
        }

        private void btnReadPos_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = "csv";
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "坐标数据文件 | *.csv";
            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                DataTable oldData = CsvFile.OpenCSV(openFileDialog1.FileName);
                calData.Clear();
                foreach (DataRow dr in oldData.Rows)
                {
                    DataRow newrow = calData.NewRow();
                    newrow["ID"] = int.Parse(dr["ID"].ToString());
                    newrow["r"] = double.Parse(dr["r"].ToString());
                    newrow["c"] = double.Parse(dr["c"].ToString());
                    newrow["x"] = double.Parse(dr["x"].ToString());
                    newrow["y"] = double.Parse(dr["y"].ToString());
                    newrow["z"] = double.Parse(dr["z"].ToString());
                    calData.Rows.Add(newrow);
                }
            }
        }

        private void btnSavePos_Click(object sender, EventArgs e)
        {
            saveFileDialog1.DefaultExt = "csv";
            saveFileDialog1.FileName = "";
            saveFileDialog1.Filter = "坐标数据文件 | *.csv";
            if (DialogResult.OK == saveFileDialog1.ShowDialog())
                CsvFile.SaveCSV(calData, saveFileDialog1.FileName);
        }

        private void 普通输入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            calData.Rows.Add(count++, 0, 0, 0, 0, 0);
        }

        private void 交叉点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HImage ho_ImageReduced;
            HRegion ho_Circle;
            HTuple hv_Row, hv_Column, hv_CoRR, hv_CoRC, hv_CoCC, hv_RowArea, hv_ColumnArea, hv_CoRRArea, hv_CoRCArea, hv_CoCCArea;
            double row, column, radius;
            processing = true;
            hWindowControl1.Focus();
            hWindowControl1.HalconWindow.DrawCircle(out row, out column, out radius);
            ho_Circle = new HRegion(row, column, radius);
            ho_ImageReduced = ho_Image.ReduceDomain(ho_Circle);
            //ho_ImageReduced.PointsHarris(0.7, 5.0, 0.08, 1000.0, out hv_Row, out hv_Column);
            ho_ImageReduced.PointsFoerstner(1.0, 3.0, 5.0, 200, 0.3, "gauss", "true",
                out hv_Row, out hv_Column, out hv_CoRR, out hv_CoRC, out hv_CoCC,
                out hv_RowArea, out hv_ColumnArea, out hv_CoRRArea, out hv_CoRCArea, out hv_CoCCArea);
            if (hv_Row.Length == 1)
                calData.Rows.Add(count++, hv_Row.D, hv_Column.D, 0, 0, 0);
            Thread.Sleep(1000);
            processing = false;
            ho_ImageReduced.Dispose();
            ho_Circle.Dispose();
        }

        private void 圆点中心ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HImage ho_ImageReduced;
            HRegion ho_Circle, ho_Region, ho_ConnectedRegions, ho_SelectedRegions1, ho_SelectedRegions2, ho_RegionUnion;
            double row, column, radius;
            processing = true;
            hWindowControl1.Focus();
            hWindowControl1.HalconWindow.DrawCircle(out row, out column, out radius);
            ho_Circle = new HRegion(row, column, radius);
            ho_ImageReduced = ho_Image.ReduceDomain(ho_Circle);
            ho_Region = ho_ImageReduced.Threshold(160.0, 255.0);
            ho_ConnectedRegions = ho_Region.Connection();
            ho_SelectedRegions1 = ho_ConnectedRegions.SelectShape("circularity", "and", 0.7, 1.0);
            ho_Region.Dispose();
            ho_Region = ho_ImageReduced.Threshold(0.0, 80.0);
            ho_ConnectedRegions.Dispose();
            ho_ConnectedRegions = ho_Region.Connection();
            ho_SelectedRegions2 = ho_ConnectedRegions.SelectShape("circularity", "and", 0.7, 1.0);
            ho_RegionUnion = ho_SelectedRegions1 | ho_SelectedRegions2;
            ho_ConnectedRegions.Dispose();
            ho_ConnectedRegions = ho_RegionUnion.Connection();
            if (ho_ConnectedRegions.CountObj() == 1)
                calData.Rows.Add(count++, ho_ConnectedRegions.Row.D, ho_ConnectedRegions.Column.D, 0, 0, 0);
            Thread.Sleep(1000);
            processing = false;
            ho_ImageReduced.Dispose();
            ho_Circle.Dispose();
            ho_Region.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions1.Dispose();
            ho_SelectedRegions2.Dispose();
            ho_RegionUnion.Dispose();
        }

        private void 机器人坐标ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                TsRemoteS robot = new TsRemoteS();
                robot.SetIPaddr(0, IpAddress, Port);
                robot.Connect(1);
                TsPointS p = robot.GetPsnFbkWorld();
                DataGridViewRow dr = dataGridView1.SelectedRows.OfType<DataGridViewRow>().Single();
                calData.Rows[dr.Index]["x"] = p.X;
                calData.Rows[dr.Index]["y"] = p.Y;
                calData.Rows[dr.Index]["z"] = p.Z;
                robot.Disconnect();
                robot.Dispose();
            }
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
                calData.Rows[dataGridView1.SelectedRows.OfType<DataGridViewRow>().Single().Index].Delete();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            calData.Clear();
            count = 1;
        }

        private void btnCalib_Click(object sender, EventArgs e)
        {
            try
            {
                hv_CameraParam = new HTuple(double.Parse(tbxFocus.Text), double.Parse(tbxKappa.Text), double.Parse(tbxSx.Text), double.Parse(tbxSy.Text),
                    double.Parse(tbxCx.Text), double.Parse(tbxCy.Text), int.Parse(tbxWidth.Text), int.Parse(tbxHeight.Text));
                hv_WorldX = new HTuple();
                hv_WorldY = new HTuple();
                hv_WorldZ = new HTuple();
                hv_ImageRow = new HTuple();
                hv_ImageColumn = new HTuple();
                foreach (DataRow dr in calData.Rows)
                {
                    hv_WorldX = hv_WorldX.TupleConcat((double)dr["x"]);
                    hv_WorldY = hv_WorldY.TupleConcat((double)dr["y"]);
                    hv_WorldZ = hv_WorldZ.TupleConcat((double)dr["z"]);
                    hv_ImageRow = hv_ImageRow.TupleConcat((double)dr["r"]);
                    hv_ImageColumn = hv_ImageColumn.TupleConcat((double)dr["c"]);
                }

                hv_Pose = HImage.VectorToPose(hv_WorldX, hv_WorldY, hv_WorldZ, hv_ImageRow, hv_ImageColumn, hv_CameraParam,
                    "planar_analytic", "error", out error);
                tbxTx.Text = hv_Pose[0].D.ToString();
                tbxTy.Text = hv_Pose[1].D.ToString();
                tbxTz.Text = hv_Pose[2].D.ToString();
                tbxAlpha.Text = hv_Pose[3].D.ToString();
                tbxBeta.Text = hv_Pose[4].D.ToString();
                tbxGamma.Text = hv_Pose[5].D.ToString();
                tbxType.Text = hv_Pose[6].I.ToString();
                tbxError.Text = Error.ToString();
            }
            catch { }
        }

        private void btnReadParam_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = "cal";
            openFileDialog1.FileName = "campar.cal";
            openFileDialog1.Filter = "内参数据文件 | *.*";
            if (DialogResult.OK == openFileDialog1.ShowDialog())
            {
                hv_CameraParam = HMisc.ReadCamPar(openFileDialog1.FileName);
                tbxFocus.Text = hv_CameraParam[0].D.ToString();
                tbxKappa.Text = hv_CameraParam[1].D.ToString();
                tbxSx.Text = hv_CameraParam[2].D.ToString();
                tbxSy.Text = hv_CameraParam[3].D.ToString();
                tbxCx.Text = hv_CameraParam[4].D.ToString();
                tbxCy.Text = hv_CameraParam[5].D.ToString();
                tbxWidth.Text = hv_CameraParam[6].I.ToString();
                tbxHeight.Text = hv_CameraParam[7].I.ToString();
            }
        }

        private void btnSavePose_Click(object sender, EventArgs e)
        {
            saveFileDialog1.DefaultExt = "dat";
            saveFileDialog1.FileName = "campose.dat";
            saveFileDialog1.Filter= "外参数据文件 | *.*";
            //HMisc.WriteCamPar(hv_CameraParam, "campar.cal");
            if (DialogResult.OK == saveFileDialog1.ShowDialog())
                hv_Pose.WritePose(saveFileDialog1.FileName);
        }
    }
}
