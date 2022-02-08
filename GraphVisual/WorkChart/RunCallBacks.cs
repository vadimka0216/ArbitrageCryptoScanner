using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;

namespace GraphVisual.WorkChart
{
    class RunCallBacks
    {
        Chart chart1; int zoom;
        public RunCallBacks(Chart obj, int zoomValue=400)
        {
            chart1 = obj;
            size_zoom = zoom = zoomValue;
        }

        public void Run()
        {
            if (chart1 != null)
            {
                if (chart1.Series.Count > 0)
                {
                    series = chart1.Series[0];
                    chart1.MouseWheel += char1_MouseWheel;
                    chart1.MouseMove += chart1_MouseMove;
                    chart1.MouseDown += chart1_MouseDown;
                    chart1.MouseClick += chart1_MouseClick_1;
                    chart1.AxisScrollBarClicked += (chart1_AxisScrollBarClicked);
                    chart1.MouseUp += chart1_MouseUp;
                    FixMouseWheel();
                }
            }
        }

        private void FixMouseWheel()//thanks:
        {               //https://stackoverflow.com/questions/13782763/mousewheel-event-not-firing
            this.chart1.MouseLeave += (s, e) =>
            {
                if (chart1.Focused)
                    chart1.Parent.Focus();
            };
            this.chart1.MouseEnter += (s, e) =>
            {
                if (!chart1.Focused)
                    chart1.Focus();
            };
        }

        Series series;
        private void char1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (series != null)
            {
                bool ScrolledUp = e.Delta > 0;

                var chartArea = this.chart1.ChartAreas[series.ChartArea];
                double pos = chartArea.AxisX.ScaleView.Position;
                int block_size = zoom;
                int curr = size_zoom;//(int)chartArea.AxisX.ScaleView.Size;
                if (!ScrolledUp)
                {
                    if (curr <= block_size * 16)
                    {
                        curr *= 2;
                    }

                    if (curr>=chartArea.AxisX.Maximum)
                    {
                        curr = (int)chartArea.AxisX.Maximum;
                    }
                }
                else
                {
                    if (curr > block_size / 4)
                    {
                        curr /= 2;
                    }
                    else
                    {
                        curr = block_size / 4;
                    }
                }
                size_zoom = curr;
                chartArea.AxisX.ScaleView.Zoom(pos - curr, pos + curr);
                //oldMouseMoveX = e.X;
            }
        }

        int savePos = 0; int size_zoom = 0;

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left) //Или Right
            {
                if (!isPressScrollBar)//если не жмем на скролл-бар!
                {
                    //if (chart1.AxisScrollBarClicked)

                    //int savePos = Form1.MousePosition.X;
                    if (series != null)
                    {
                        var chartArea = this.chart1.ChartAreas[series.ChartArea];
                        double pos = chartArea.AxisX.ScaleView.Position;
                        //int curr_block_size = (int)chartArea.AxisX.ScaleView.Size;
                        int block_size = zoom;

                        //chartArea.AxisX.ScaleView.Zoom(pos + (double)(savePos - e.X) * (double)size_zoom / block_size, size_zoom);
                        double newPos = pos + (double)(savePos - e.X) * (double)size_zoom / block_size;
                        if (newPos + size_zoom >= chartArea.AxisX.Maximum)
                        {
                            chartArea.AxisX.ScaleView.Position = chartArea.AxisX.Maximum - size_zoom;
                        }
                        else if (newPos <= chartArea.AxisX.Minimum)
                        {
                            chartArea.AxisX.ScaleView.Position = chartArea.AxisX.Minimum;
                        }
                        else
                        {
                            chartArea.AxisX.ScaleView.Position = newPos;
                        }
                        //oldMouseMoveX = e.X;
                        savePos = e.X;
                    }
                }
            }
        }

        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
            savePos = e.X;
        }
        DataPoint lastPont;

        private void chart1_MouseClick_1(object sender, MouseEventArgs e)
        {
            var result = chart1.HitTest(e.X, e.Y);
            //Cursor = result.ChartElementType == ChartElementType.DataPoint ? Cursors.Hand : Cursors.Default;
            if (result.ChartElementType == ChartElementType.DataPoint)
            {
                if (lastPont != null)
                {
                    lastPont.IsValueShownAsLabel = false;
                }
                lastPont = result.Series.Points[result.PointIndex];
                lastPont.IsValueShownAsLabel = true;
            }
        }

        bool isPressScrollBar = false;

        private void chart1_AxisScrollBarClicked(object sender, ScrollBarEventArgs e)
        {
            isPressScrollBar = true;
        }

        private void chart1_MouseUp(object sender, MouseEventArgs e)
        {
            isPressScrollBar = false;
        }

    }
}
