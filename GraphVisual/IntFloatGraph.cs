using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GraphVisual
{
    class IntFloatGraph: ControlGraph
    {
        const int offsetLeft=30;
        const int offsetRight = 30;
        const int offsetHeader = 25;
        const int offsetBottom = 25;

        int width, height;//const
        double moveW, moveH;//const
        int iterator;

        private Point ValueToPoint(double val)
        {
            int x = (int)Math.Round(iterator * moveW) + offsetLeft;
            int y;
            if (val >= 0)
            {
                y = height/2 + offsetBottom - (int)Math.Round(moveH * val)/2;
            }
            else
            {
                y = height/2 + offsetBottom - (int)Math.Round(moveH * val)/2;
            }
            return new Point(x,y);
        }

        public IntFloatGraph(PictureBox view, double minValue, double maxValue, int count)
            : base(view)
        {
            iterator = 0;
            base.setSizePoints(count);

            width = view.Width;
            height = view.Height;

            int otherW = (offsetLeft + offsetRight);
            if (width - otherW > 0) width -= otherW;
            int otherH = (offsetHeader + offsetBottom);
            if (height - otherH > 0) height -= otherH;

            int AvHeight = height / 2;
            base.Graph.DrawLine(new Pen(Color.Black, 3f), new Point(offsetLeft, offsetHeader + AvHeight), new Point(width + offsetLeft, offsetHeader + AvHeight));

            moveW = (double)width / (count-1);
            moveH = (double)height / (Math.Abs(maxValue-minValue));
        }
        ~IntFloatGraph() 
        {
           // int test = 0;
        }

        public void AddValue(double val, string desc="")//desc - contain time or other info
        {
            this.AddPoint(new SaveInfo(ValueToPoint(val),desc));
            iterator++;
        }
        public override void Reset()
        {
            iterator = 0;
            base.Reset();
        }
    }
}
