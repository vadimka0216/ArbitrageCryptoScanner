using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using GraphVisual.WorkChart;

namespace GraphVisual
{
    public partial class MainMenu : Form
    {
        RenderingCSV obj;
        List<string> pathes = new List<string>();
        RunCallBacks setCallBacks;
        public MainMenu()
        {
            InitializeComponent();
            obj = new SmartRendering(chart1);
            (setCallBacks=new RunCallBacks(chart1)).Run();

            this.comboBox1.MouseWheel += (s, e) =>
            {
                HandledMouseEventArgs ev = e as HandledMouseEventArgs;
                if (ev != null)
                {
                    ev.Handled = true;
                }
            };
            comboBox1.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Up
                || e.KeyCode == Keys.Down
                || e.KeyCode == Keys.Left
                || e.KeyCode == Keys.Right)
                {
                    e.Handled = true;
                }
            };
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //(new WorkChart.CreateForm()).ShowDialog();
            
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = Directory.GetCurrentDirectory();
            dlg.ShowDialog();
            DirectoryInfo info = new DirectoryInfo(dlg.SelectedPath);
            if (info.Exists)
            {
                comboBox1.Items.Clear();
                pathes.Clear();
                RecursiveSearch(info);
            }
             
        }

        private void RecursiveSearch(DirectoryInfo dir)
        {
            foreach (DirectoryInfo curr in dir.GetDirectories())
            {
                RecursiveSearch(curr);
            }

            foreach (FileInfo file in dir.GetFiles())
            {
                if (file.Name.IndexOf(".txt") >= 0||
                    file.Name.IndexOf(".csv") >= 0)
                {
                    pathes.Add(file.FullName);
                    comboBox1.Items.Add(file.Name + " (" + file.Length / (1024) + " kbytes)");
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < pathes.Count)
            {
                string path = pathes[comboBox1.SelectedIndex];//comboBox1.GetItemText(comboBox1.Items[comboBox1.SelectedIndex]);
                obj.LoadFile(path);
            }
        }
    }
}
