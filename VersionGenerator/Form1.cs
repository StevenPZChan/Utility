using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace VersionGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Hide();
            this.folderBrowserDialog1.ShowDialog();
            try
            {
                XmlDocument updateConfig = new XmlDocument();
                updateConfig.AppendChild(updateConfig.CreateXmlDeclaration("1.0", "utf-8", null));
                updateConfig.AppendChild(updateConfig.CreateComment("更新信息文档"));
                XmlElement root = updateConfig.CreateElement("UpdateInformation");
                updateConfig.AppendChild(root);
                string path = this.folderBrowserDialog1.SelectedPath;
                var dirs = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                    .Where(s => s.EndsWith(".exe") || s.EndsWith(".dll") || s.EndsWith(".config") || s.EndsWith(".xml") || s.EndsWith(".chm"));
                foreach (string file in dirs)
                {
                    XmlElement f, name, version, source, dest;
                    FileVersionInfo info = FileVersionInfo.GetVersionInfo(file);
                    f = updateConfig.CreateElement("File");
                    if (info.FileDescription == null)
                    {
                        name = updateConfig.CreateElement("FileName");
                        name.AppendChild(updateConfig.CreateTextNode(info.FileName.Replace(path, "").Replace("\\", "")));
                        version = updateConfig.CreateElement("Version");
                        version.AppendChild(updateConfig.CreateTextNode(new FileInfo(file).LastWriteTimeUtc.ToString("yyyy-MM-dd hh:mm:ss")));
                    }
                    else
                    {
                        name = updateConfig.CreateElement("FileName");
                        name.AppendChild(updateConfig.CreateTextNode(info.FileDescription));
                        version = updateConfig.CreateElement("Version");
                        version.AppendChild(updateConfig.CreateTextNode(
                            info.ProductMajorPart + "." + info.ProductMinorPart + "." + info.ProductBuildPart + "." + info.ProductPrivatePart));
                    }
                    source = updateConfig.CreateElement("SourceDir");
                    source.AppendChild(updateConfig.CreateTextNode(info.FileName.Replace(path, "").Replace(@"/", @"/")));
                    dest = updateConfig.CreateElement("DestDir");
                    dest.AppendChild(updateConfig.CreateTextNode(info.FileName.Replace(path, "").Replace(@"/", @"/")));
                    f.AppendChild(name);
                    f.AppendChild(version);
                    f.AppendChild(source);
                    f.AppendChild(dest);
                    root.AppendChild(f);
                }
                updateConfig.Save(path + @"/UpdateConfig.xml");
                MessageBox.Show("生成成功！", "成功信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("生成时出错！", "错误信息", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Application.Exit();
        }
    }
}
