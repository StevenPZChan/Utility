using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using HalconDotNet;

using TsRemoteLib;

using Utility.Files;

namespace Utility.Form
{
    /// <summary>
    /// 四轴SCARA机器人标定助手
    /// </summary>
    [ToolboxBitmap(typeof(ScaraCalibTool), "TSAssist.ico")]
    public partial class ScaraCalibTool : UserControl
    {
        #region Fields
        /// <summary>
        /// 标定数据
        /// </summary>
        public DataTable CalData;
        /// <summary>
        /// 相机内参
        /// </summary>
        public HTuple CameraParam;
        /// <summary>
        /// 相机外参
        /// </summary>
        public HPose Pose;
        private int count = 1;
        private double error;
        private HImage ho_Image = new HImage();
        private HTuple hv_WorldX, hv_WorldY, hv_WorldZ, hv_ImageRow, hv_ImageColumn;
        private bool processing;
        #endregion

        #region Constructors
        /// <summary>
        /// 构造函数
        /// </summary>
        public ScaraCalibTool()
        {
            InitializeComponent();
            this.CalData = new DataTable();
            this.CalData.Columns.Add("ID", typeof(int));
            this.CalData.Columns.Add("r", typeof(double));
            this.CalData.Columns.Add("c", typeof(double));
            this.CalData.Columns.Add("x", typeof(double));
            this.CalData.Columns.Add("y", typeof(double));
            this.CalData.Columns.Add("z", typeof(double));
            this.dataGridView1.DataSource = this.CalData;
            Disposed += (sender, e) => {
                if (this.ho_Image.IsInitialized())
                    this.ho_Image.Dispose();
            };
        }
        #endregion

        #region Properties
        /// <summary>
        /// 标定误差
        /// </summary>
        public double Error => this.error;
        /// <summary>
        /// Halcon窗口
        /// </summary>
        public HWindowControl HwControl { get; private set; }

        /// <summary>
        /// 显示的图像
        /// </summary>
        public HImage Image
        {
            set
            {
                if (this.processing)
                    return;

                int width, height;
                if (this.ho_Image.IsInitialized())
                    this.ho_Image.Dispose();
                this.ho_Image = value;
                this.ho_Image.GetImageSize(out width, out height);
                HSystem.SetSystem("width", width);
                HSystem.SetSystem("height", height);
                this.HwControl.HalconWindow.SetPart(0, 0, height - 1, width - 1);
                this.HwControl.HalconWindow.DispObj(this.ho_Image);
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
        #endregion

        #region Methods
        private void btnCalib_Click(object sender, EventArgs e)
        {
            try
            {
                this.CameraParam = new HTuple(double.Parse(this.tbxFocus.Text), double.Parse(this.tbxKappa.Text), double.Parse(this.tbxSx.Text),
                    double.Parse(this.tbxSy.Text), double.Parse(this.tbxCx.Text), double.Parse(this.tbxCy.Text), int.Parse(this.tbxWidth.Text),
                    int.Parse(this.tbxHeight.Text));
                this.hv_WorldX = new HTuple();
                this.hv_WorldY = new HTuple();
                this.hv_WorldZ = new HTuple();
                this.hv_ImageRow = new HTuple();
                this.hv_ImageColumn = new HTuple();
                foreach (DataRow dr in this.CalData.Rows)
                {
                    this.hv_WorldX = this.hv_WorldX.TupleConcat((double)dr["x"]);
                    this.hv_WorldY = this.hv_WorldY.TupleConcat((double)dr["y"]);
                    this.hv_WorldZ = this.hv_WorldZ.TupleConcat((double)dr["z"]);
                    this.hv_ImageRow = this.hv_ImageRow.TupleConcat((double)dr["r"]);
                    this.hv_ImageColumn = this.hv_ImageColumn.TupleConcat((double)dr["c"]);
                }

                this.Pose = HImage.VectorToPose(this.hv_WorldX, this.hv_WorldY, this.hv_WorldZ, this.hv_ImageRow, this.hv_ImageColumn, this.CameraParam,
                    "planar_analytic", "error", out this.error);
                this.tbxTx.Text = this.Pose[0].D.ToString(CultureInfo.InvariantCulture);
                this.tbxTy.Text = this.Pose[1].D.ToString(CultureInfo.InvariantCulture);
                this.tbxTz.Text = this.Pose[2].D.ToString(CultureInfo.InvariantCulture);
                this.tbxAlpha.Text = this.Pose[3].D.ToString(CultureInfo.InvariantCulture);
                this.tbxBeta.Text = this.Pose[4].D.ToString(CultureInfo.InvariantCulture);
                this.tbxGamma.Text = this.Pose[5].D.ToString(CultureInfo.InvariantCulture);
                this.tbxType.Text = this.Pose[6].I.ToString();
                this.tbxError.Text = this.Error.ToString(CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                MessageBox.Show("出现错误：" + ex.Message, "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReadParam_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.DefaultExt = "cal";
            this.openFileDialog1.FileName = "campar.cal";
            this.openFileDialog1.Filter = "内参数据文件 | *.*";
            if (DialogResult.OK != this.openFileDialog1.ShowDialog())
                return;

            this.CameraParam = HMisc.ReadCamPar(this.openFileDialog1.FileName);
            this.tbxFocus.Text = this.CameraParam[0].D.ToString(CultureInfo.InvariantCulture);
            this.tbxKappa.Text = this.CameraParam[1].D.ToString(CultureInfo.InvariantCulture);
            this.tbxSx.Text = this.CameraParam[2].D.ToString(CultureInfo.InvariantCulture);
            this.tbxSy.Text = this.CameraParam[3].D.ToString(CultureInfo.InvariantCulture);
            this.tbxCx.Text = this.CameraParam[4].D.ToString(CultureInfo.InvariantCulture);
            this.tbxCy.Text = this.CameraParam[5].D.ToString(CultureInfo.InvariantCulture);
            this.tbxWidth.Text = this.CameraParam[6].I.ToString();
            this.tbxHeight.Text = this.CameraParam[7].I.ToString();
        }

        private void btnReadPos_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.DefaultExt = "csv";
            this.openFileDialog1.FileName = "";
            this.openFileDialog1.Filter = "坐标数据文件 | *.csv";
            if (DialogResult.OK != this.openFileDialog1.ShowDialog())
                return;

            DataTable oldData = CsvFile.OpenCsv(this.openFileDialog1.FileName);
            this.CalData.Clear();
            foreach (DataRow dr in oldData.Rows)
            {
                DataRow newrow = this.CalData.NewRow();
                newrow["ID"] = int.Parse(dr["ID"].ToString());
                newrow["r"] = double.Parse(dr["r"].ToString());
                newrow["c"] = double.Parse(dr["c"].ToString());
                newrow["x"] = double.Parse(dr["x"].ToString());
                newrow["y"] = double.Parse(dr["y"].ToString());
                newrow["z"] = double.Parse(dr["z"].ToString());
                this.CalData.Rows.Add(newrow);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            this.CalData.Clear();
            this.count = 1;
        }

        private void btnSavePos_Click(object sender, EventArgs e)
        {
            this.saveFileDialog1.DefaultExt = "csv";
            this.saveFileDialog1.FileName = "";
            this.saveFileDialog1.Filter = "坐标数据文件 | *.csv";
            if (DialogResult.OK == this.saveFileDialog1.ShowDialog())
                CsvFile.SaveCsv(this.CalData, this.saveFileDialog1.FileName);
        }

        private void btnSavePose_Click(object sender, EventArgs e)
        {
            this.saveFileDialog1.DefaultExt = "dat";
            this.saveFileDialog1.FileName = "campose.dat";
            this.saveFileDialog1.Filter = "外参数据文件 | *.*";
            //HMisc.WriteCamPar(hv_CameraParam, "campar.cal");
            if (DialogResult.OK == this.saveFileDialog1.ShowDialog())
                this.Pose.WritePose(this.saveFileDialog1.FileName);
        }

        private void btnSub_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count == 1)
                this.CalData.Rows[this.dataGridView1.SelectedRows.OfType<DataGridViewRow>().Single().Index].Delete();
        }

        private void 机器人坐标ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows.Count == 1)
                Task.Run(() => {
                    var robot = new TsRemoteS();
                    robot.SetIPaddr(0, this.IpAddress, this.Port);
                    robot.Connect(1);
                    TsPointS p = robot.GetPsnFbkWorld();
                    DataGridViewRow dr = this.dataGridView1.SelectedRows.OfType<DataGridViewRow>().Single();
                    this.CalData.Rows[dr.Index]["x"] = p.X;
                    this.CalData.Rows[dr.Index]["y"] = p.Y;
                    this.CalData.Rows[dr.Index]["z"] = p.Z;
                    Task.Run(() => {
                        robot.Disconnect();
                        robot.Dispose();
                    });
                });
        }

        private async void 交叉点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HImage ho_ImageReduced;
            HRegion ho_Circle;
            HTuple hv_Row, hv_Column, hv_CoRR, hv_CoRC, hv_CoCC, hv_RowArea, hv_ColumnArea, hv_CoRRArea, hv_CoRCArea, hv_CoCCArea;
            double row, column, radius;
            this.processing = true;
            this.HwControl.Focus();
            this.HwControl.HalconWindow.DrawCircle(out row, out column, out radius);
            ho_Circle = new HRegion(row, column, radius);
            ho_ImageReduced = this.ho_Image.ReduceDomain(ho_Circle);
            //ho_ImageReduced.PointsHarris(0.7, 5.0, 0.08, 1000.0, out hv_Row, out hv_Column);
            ho_ImageReduced.PointsFoerstner(1.0, 3.0, 5.0, 200, 0.3, "gauss", "true", out hv_Row, out hv_Column, out hv_CoRR, out hv_CoRC, out hv_CoCC,
                out hv_RowArea, out hv_ColumnArea, out hv_CoRRArea, out hv_CoRCArea, out hv_CoCCArea);
            if (hv_Row.Length == 1)
                this.CalData.Rows.Add(this.count++, hv_Row.D, hv_Column.D, 0, 0, 0);
            await Task.Delay(1000);
            this.processing = false;
            ho_ImageReduced.Dispose();
            ho_Circle.Dispose();
        }

        private void 普通输入ToolStripMenuItem_Click(object sender, EventArgs e) => this.CalData.Rows.Add(this.count++, 0, 0, 0, 0, 0);

        private async void 圆点中心ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HImage ho_ImageReduced;
            HRegion ho_Circle, ho_Region, ho_ConnectedRegions, ho_SelectedRegions1, ho_SelectedRegions2, ho_RegionUnion;
            double row, column, radius;
            this.processing = true;
            this.HwControl.Focus();
            this.HwControl.HalconWindow.DrawCircle(out row, out column, out radius);
            ho_Circle = new HRegion(row, column, radius);
            ho_ImageReduced = this.ho_Image.ReduceDomain(ho_Circle);
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
                this.CalData.Rows.Add(this.count++, ho_ConnectedRegions.Row.D, ho_ConnectedRegions.Column.D, 0, 0, 0);
            await Task.Delay(1000);
            this.processing = false;
            ho_ImageReduced.Dispose();
            ho_Circle.Dispose();
            ho_Region.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions1.Dispose();
            ho_SelectedRegions2.Dispose();
            ho_RegionUnion.Dispose();
        }
        #endregion
    }
}
