using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AnalyzStrategy.Exchanges;
using System.IO;

namespace AnalyzStrategy
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        WorkerExchanges worker;
        private void MainForm_Load(object sender, EventArgs e)
        {
            worker = new WorkerExchanges(this);//, true);
            worker += new Yobit();
            worker += new Binance();
            worker += new Exmo();
            worker += new Huobi();
            worker += new Hitbtc();
            worker += new CryptoCom();
        }

        bool isExit = false;
        delegate void funcion();
        private void CallInvoke(funcion lamda)
        {
            if (!isExit)
            {
                this.BeginInvoke(new MethodInvoker(lamda));
            }
        }

        public void UpdateCountProxy(int count)
        {
            CallInvoke(() => label1.Text = count.ToString());
        }
        private void StopAll()
        {
            for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
            {
                var row = dataGridView1.Rows[i];
                worker.StopStrategy((int)row.Cells[0].Value);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (worker!=null)
            {
                progressBar1.Value=0;
                StopAll();
                dataGridView1.Rows.Clear();
                //StopAll
                //progress bar
                worker.Load();
            }
        }
        public void IncValueProgress(int i, int maxIterations, double weight)
        {
            CallInvoke(() =>
            {
                if (i >= 0 && i<maxIterations)
                {
                    int valueNew = (int)(weight * (double)i / (maxIterations - 1));
                    int valuePrev = 0;
                    if (i > 0)
                    {
                        valuePrev = (int)(weight * (double)(i - 1) / (maxIterations - 1));
                    }
                    if (valueNew > 0)
                    {
                        if (progressBar1.Value + valueNew - valuePrev < progressBar1.Maximum)
                        {
                            progressBar1.Value += valueNew - valuePrev;
                        }
                    }
                }
            });
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            isExit = true;
            if (worker!=null)
            {
                //worker.Dispose();
            }
        }

        public void AddRowTable(int id, string strategy, string pair1, string pair2, bool isRun)
        {
            CallInvoke(() =>
                {
                    dataGridView1.Rows.Add(id, strategy, pair1, pair2, isRun);
                    label4.Text = dataGridView1.Rows.Count.ToString();
                });
        }
        public void UpdateRowTable(int id,int idCell, object value)
        {
            CallInvoke(() =>
                {
                    for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
                    {
                        var row = dataGridView1.Rows[i];
                        if ((int)row.Cells[0].Value == id)
                        {
                            row.Cells[idCell].Value = value;
                            break;
                        }
                    }
                });
        }
        int offset=0;
        private void dataGridView1_Resize(object sender, EventArgs e)
        {
            if (offset==0)
            {
                offset = Math.Abs(dataGridView1.Location.Y + dataGridView1.Height - groupBox1.Location.Y);
            }
            groupBox1.Location = new Point(groupBox1.Location.X, dataGridView1.Location.Y + dataGridView1.Height + offset);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count>1)
            {
                var buffer = new List<DataGridViewRow>();//List<object[]>();
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    var row = dataGridView1.Rows[i];
                    if ((bool)row.Cells[4].Value == true)
                    {
                        buffer.Add(row);
                    }
                }
                dataGridView1.Rows.Clear();
                foreach (var rows in buffer)
                {
                    dataGridView1.Rows.Add(rows);
                }
                buffer.Clear();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            StopAll();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var dir = Directory.CreateDirectory(DateTime.Now.ToShortDateString().Replace("/","_"));
            if (dir.Exists)
            {
                for (int i = 0; i < dataGridView1.Rows.Count-1; i++)
                {
                    var row = dataGridView1.Rows[i];
                    worker.StartStrategy((int)row.Cells[0].Value, dir.Name + "/");
                }
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string path=(string)dataGridView1.CurrentRow.Cells[6].Value;
            if (path != null && path != "")
            {
                if (File.Exists(path))
                {
                    var dlg = new GraphVisual.WorkChart.CreateForm(path);
                    dlg.ShowDialog();
                    dlg.Dispose();
                }
                else
                {
                    MessageBox.Show("Не удалось найти указанный файл!");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var dir = Directory.CreateDirectory(DateTime.Now.ToShortDateString().Replace("/","_"));
            if (dir.Exists)
            {
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    var row = dataGridView1.SelectedRows[i];
                    worker.StartStrategy((int)row.Cells[0].Value, dir.Name + "/");
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                var row = dataGridView1.SelectedRows[i];
                worker.StopStrategy((int)row.Cells[0].Value);
            }
        }
    }
}
