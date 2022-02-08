using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization;
using System.Windows.Forms.DataVisualization.Charting;

namespace GraphVisual.WorkChart
{
    abstract class BaseRenderingCSV
    {
        abstract protected Series NewSeries(string name);
        abstract protected void LoadSeriesPoints(SeriesStorage series, string strY);
        abstract public void LoadFile(string nameFile);//iteration in series; series<=columns in csv
        abstract public void ResetFile();
    }

    abstract class BaseRenderingStorage
    {
        abstract public int getCount();
        abstract public double getMaxValue();
        abstract public double getMinValue();
    }
    
    class SeriesStorage: BaseRenderingStorage
    {
        Series series;

        double max, min;
        static int id=0;
        int oid=0;
        public SeriesStorage(Series _series)
        {
            series = _series;
            max = min = 0;
            oid=id++;
        }

        public override double getMaxValue() { return max; }
        public override double getMinValue() { return min; }
        static public bool operator ==(SeriesStorage obj1, SeriesStorage obj2)
        {
            return obj1.oid == obj2.getId();
        }

        static public bool operator!=(SeriesStorage obj1,SeriesStorage obj2)
        {
            return obj1.oid != obj2.getId();
        }

        public int getId() { return oid; }

        public void UpdMinMaxY(double y)
        {
            if (getCount() == 0)
            {
                if (min == 0 && max == 0)
                {
                    min = max = y;
                }
            }

            if (min > y) min = y;
            if (max < y) max = y;
        }

        public void AddPoint(double x, double y)
        {
            this.UpdMinMaxY(y);
            int index=this.series.Points.AddXY(x, y);

            //var obj=this.series.Points[0].;

            //int size=System.Runtime.InteropServices.Marshal.SizeOf(obj);
        }
        public override int getCount()
        {
            return this.series.Points.Count;
        }
        public override string ToString()
        {
            return series.Name;
        }
    }

    class RenderingCSV : BaseRenderingCSV
    {
        protected Chart chart1; int blockSize;
        protected List<SeriesStorage> listSeries;

        public RenderingCSV(Chart object_set, int _blockSize)
        {
            chart1 = object_set;
            blockSize = _blockSize;
            listSeries = new List<SeriesStorage>();
        }

        override public void LoadFile(string nameFile)
        {
            ResetFile();

            if (this.chart1 != null)
            {
                using (CSV file = new CSV(nameFile))
                {
                    string[] vals = file.getValues();

                    Series series = null;
                    int startLenght = vals.Length;
                    for (int i = 1; i < startLenght; i++)
                        series = this.NewSeries(vals[i]);
                    if (series != null)
                    {
                        for (string[] values = null; (values = file.getValues()) != null; file.Next())
                        {

                            if (startLenght == values.Length)
                            {
                                for (int i = 0; i < listSeries.Count; i++)
                                    this.LoadSeriesPoints(listSeries[i], values[i+1]);
                            }
                        }

                        MiscSettings(series);
                    }
                }
            }
        }

        virtual protected void MiscSettings(Series series)
        {
            var chartArea = this.chart1.ChartAreas[series.ChartArea];
            //this.chart1.Series[0].ToolTip = "Y = #VALY";
            //this.chart1.Series[1].ToolTip = "Y = #VALY";

            chartArea.AxisX.Minimum = 0;
            chartArea.AxisX.Maximum = listSeries[0].getCount();

            // enable autoscroll
            chartArea.CursorX.AutoScroll = true;

            // let's zoom to [0,blockSize] (e.g. [0,100])
            chartArea.AxisX.ScaleView.Zoomable = true;
            chartArea.AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            int position = 0;
            int size = blockSize;
            chartArea.AxisX.ScaleView.Zoom(position, size);

            // disable zoom-reset button (only scrollbar's arrows are available)
            chartArea.AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;

            // set scrollbar small change to blockSize (e.g. 100)
            chartArea.AxisX.ScaleView.SmallScrollSize = blockSize;
        }

        public override void ResetFile()
        {
            if (this.chart1 != null)
            {
                this.chart1.Titles.Clear();
                //this.chart1.Legends.Clear();
                this.chart1.Series.Clear();
            }
            if (listSeries!=null)
            {
                listSeries.Clear();
            }
        }

        protected override Series NewSeries(string name)
        {
            Series series = this.chart1.Series.Add(name);
            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            listSeries.Add(new SeriesStorage(series));
            return series;
        }

        protected override void LoadSeriesPoints(SeriesStorage series, string strY)
        {
            double buf = 0;
            if (doubleEn.TryParse(strY, out buf))
            {
                series.AddPoint(series.getCount(), buf);
            }
        }

        //private void LoadProfits()//where?
        //{

        //}

    }

    class AnalyzProfit: RenderingCSV
    {
        public AnalyzProfit(Chart object_set, int _blockSize=400)
            : base(object_set, _blockSize) {}

        public override void LoadFile(string nameFile)
        {
            base.LoadFile(nameFile);
            setDescriptions();
        }

        virtual protected void setDescriptions()
        {
            if (listSeries.Count > 0)
            {
                
                for (int i = 0; i < base.listSeries.Count-1; i+=2)
                {
                    var it1 = base.listSeries[i];
                    var it2 = base.listSeries[i + 1];

                    //foreach (var it2 in base.listSeries)
                    {
                        //if (it1 != it2)
                        {
                            this.chart1.Series.Add(it1 + "+" +
                                it2 + " " + ((it1.getMaxValue() + it2.getMaxValue()) * 100).ToString() + "%");
                        }
                    }
                }
            }
        }
    }

    class SmartRendering : AnalyzProfit
    {
        const long maxPoints = 20000;//~ * 1024 bytes

        public SmartRendering(Chart object_set, int block_size=400)
            :base(object_set,block_size) {}

        public override void LoadFile(string nameFile)
        {
            if (this.chart1 != null)
            {
                string firstDate="", lastDate="";
                long countLines = 0, countColumns = 0;
                using (CSV file = new CSV(nameFile))
                {
                    for (string[] values = null; (values = file.getValues()) != null; file.Next())
                    {
                        if (countLines==0)
                        {
                            countColumns = values.Length;
                        }
                        else if (countLines == 1)
                        {
                            firstDate = values[0];
                        }

                        if (countColumns == values.Length)
                        {
                            countLines++;
                        }
                    }
                }

                long smooth = (countLines*(countColumns-1) / maxPoints);
                if (smooth<2)
                {
                    base.LoadFile(nameFile);
                }
                else
                {
                    ResetFile();
                    using (CSV file = new CSV(nameFile))
                    {
                        string[] vals = file.getValues();
                        if (vals!=null)
                        {
                            Series series = null;
                            int startLenght = vals.Length;
                            for (int i = 1; i < startLenght; i++)
                                series = this.NewSeries(vals[i]);

                            double[] averages = new double[listSeries.Count]; int counter = 0;

                            if (series != null)
                            {
                                for (string[] values = null; (values = file.getValues()) != null; file.Next())
                                {
                                    if (startLenght == values.Length)
                                    {
                                        for (int i = 0; i < listSeries.Count; i++)
                                        {
                                            double value=0;
                                            if (doubleEn.TryParse(values[i + 1], out value))
                                            {
                                                listSeries[i].UpdMinMaxY(value);
                                                if (counter % smooth==0)
                                                {
                                                    averages[i] += value;
                                                    this.LoadSeriesPoints(listSeries[i], (double)averages[i]/smooth);
                                                    //нормально считать min и макс,как?
                                                    averages[i]=0;
                                                }
                                                else
                                                {
                                                    averages[i]+=value;
                                                }
                                            }
                                        }

                                        counter++;
                                        if (counter == countLines)
                                            lastDate = values[0];
                                    }
                                    
                                }
                                chart1.Titles.Add(firstDate+" - "+lastDate);
                                MiscSettings(series); 
                                base.setDescriptions();
                            }
                        }
                    }
                }
            }
        }

        virtual protected void LoadSeriesPoints(SeriesStorage series, double y)
        {
            series.AddPoint(series.getCount(), y);
        }

        //protected override Series NewSeries(string name)
        //{
        //    Series result=base.NewSeries(name);
        //    result.BorderWidth = 2;
        //    return result;
        //}

    }


}
