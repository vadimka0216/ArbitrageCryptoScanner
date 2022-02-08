namespace AnalyzStrategy
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.strategy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pair1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pair2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isRun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxProfit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PathCSV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.LeaderProfit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(267, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(132, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Загружено прокси:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(405, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(247, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(152, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Загружено стратегий:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(209, 47);
            this.button1.TabIndex = 2;
            this.button1.Text = "Загрузить список";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(405, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "0";
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.strategy,
            this.pair1,
            this.pair2,
            this.isRun,
            this.MaxProfit,
            this.PathCSV,
            this.LeaderProfit});
            this.dataGridView1.Location = new System.Drawing.Point(27, 22);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(957, 142);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentDoubleClick);
            this.dataGridView1.Resize += new System.EventHandler(this.dataGridView1_Resize);
            // 
            // id
            // 
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Width = 48;
            // 
            // strategy
            // 
            this.strategy.HeaderText = "name strategy";
            this.strategy.Name = "strategy";
            this.strategy.ReadOnly = true;
            this.strategy.Width = 127;
            // 
            // pair1
            // 
            this.pair1.HeaderText = "pair1";
            this.pair1.Name = "pair1";
            this.pair1.ReadOnly = true;
            this.pair1.Width = 69;
            // 
            // pair2
            // 
            this.pair2.HeaderText = "pair2";
            this.pair2.Name = "pair2";
            this.pair2.ReadOnly = true;
            this.pair2.Width = 69;
            // 
            // isRun
            // 
            this.isRun.HeaderText = "isRun";
            this.isRun.Name = "isRun";
            this.isRun.ReadOnly = true;
            this.isRun.Width = 73;
            // 
            // MaxProfit
            // 
            this.MaxProfit.HeaderText = "MaxProfit";
            this.MaxProfit.Name = "MaxProfit";
            this.MaxProfit.ReadOnly = true;
            this.MaxProfit.Width = 95;
            // 
            // PathCSV
            // 
            this.PathCSV.HeaderText = "PathCSV";
            this.PathCSV.Name = "PathCSV";
            this.PathCSV.ReadOnly = true;
            this.PathCSV.Width = 93;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(27, 192);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(957, 149);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Управление";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(405, 126);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 17);
            this.label6.TabIndex = 13;
            this.label6.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(292, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 17);
            this.label5.TabIndex = 12;
            this.label5.Text = "Время работы:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(15, 96);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(209, 47);
            this.button2.TabIndex = 11;
            this.button2.Text = "Очистить таблицу";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(729, 86);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(211, 47);
            this.button6.TabIndex = 10;
            this.button6.Text = "Приостановить все стратегии";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(496, 86);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(211, 47);
            this.button5.TabIndex = 9;
            this.button5.Text = "Запустить все стратегии";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(729, 33);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(211, 47);
            this.button4.TabIndex = 8;
            this.button4.Text = "Приостановить выделенные стратегии";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(496, 33);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(211, 47);
            this.button3.TabIndex = 7;
            this.button3.Text = "Запустить выделенные стратегии";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(250, 96);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(171, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 14;
            // 
            // LeaderProfit
            // 
            this.LeaderProfit.HeaderText = "LeaderProfit";
            this.LeaderProfit.Name = "LeaderProfit";
            this.LeaderProfit.ReadOnly = true;
            this.LeaderProfit.Width = 115;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 347);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn strategy;
        private System.Windows.Forms.DataGridViewTextBoxColumn pair1;
        private System.Windows.Forms.DataGridViewTextBoxColumn pair2;
        private System.Windows.Forms.DataGridViewTextBoxColumn isRun;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxProfit;
        private System.Windows.Forms.DataGridViewTextBoxColumn PathCSV;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.DataGridViewTextBoxColumn LeaderProfit;

    }
}

