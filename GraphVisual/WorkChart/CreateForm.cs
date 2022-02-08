using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphVisual.WorkChart
{
    public partial class CreateForm : Form
    {
        RenderingCSV obj;
        //RunCallBacks settings;

        public CreateForm()
        {
            InitializeComponent();
        }
        string nameFile = null;
        public CreateForm(string file)
        {
            InitializeComponent();
            nameFile = file;
        }

        private void CreateForm_Load(object sender, EventArgs e)
        {
            obj = new AnalyzProfit(chart1, (int)numericUpDown1.Value);
            (new RunCallBacks(chart1, (int)numericUpDown1.Value)).Run();
            if (nameFile!=null)
            {
                obj.LoadFile(nameFile);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.ShowDialog();
            if (dlg.CheckPathExists)
            {
                obj.LoadFile(dlg.FileName);
            }
        }
    }
}
