using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace GraphVisual
{
    //class ModernArray<T> : ICollection<T>
    //{
    //    T[] array; uint counter = 0;
    //    public ModernArray(int size)
    //    {
    //        array = new T[size];
    //    }
    //    ~ModernArray() { Clear(); }
    //    public void Clear() { counter = 0; }
    //    public void Add(T obj)
    //    {
    //        if (counter < array.Length)
    //        {
    //            array[counter++] = obj;
    //        }
    //    }

    //    public T this[int index] { get; set; }
    //}

    class StoragePoints
    {
        public struct SaveInfo
        {
            public SaveInfo(Point XY, string Info)
            {
                xy = XY;
                info = Info;
            }
            public Point xy;
            public string info;
        }
        int iterator = 0;
        List<SaveInfo> points = new List<SaveInfo>();

        protected void AddStorage(SaveInfo storage)
        {
            if (iterator<points.Count)
            {
                points[iterator++] = storage;
            }
            else
            {
                iterator++;
                points.Add(storage);
            }
        }

        public void setSizePoints(int size)
        {
            this.Reset();
            points = new List<SaveInfo>(size);
        }

        public virtual void Reset()
        {
            if (points != null)
            {
                iterator = 0;
                points.Clear();
            }
        }
    }

    class ControlGraph : StoragePoints
    {
        protected Graphics Graph;
        PictureBox Model;

        public ControlGraph(PictureBox model)
        {
            Model = (PictureBox)model;
            Model.Image = new Bitmap(Model.Width, Model.Height);
            Graph=Graphics.FromImage(Model.Image);
        }
        ~ControlGraph()
        {
            this.Dispose();
        }

        public override void Reset()
        {
            if (Graph != null)
            {
                Graph.Clear(Color.White);
                Model.Invalidate();
            }

            base.Reset();
        }
        public void Dispose()
        { 
            if (Model!=null)
            {
                if (Graph != null && !Model.IsDisposed)
                {
                    this.Reset();
                    Graph.Dispose();
                    Graph = null;
                }

                Model.Image.Dispose();
                Model.Dispose();
                Model = null;
            }
        }

        public void AddPoint(SaveInfo info)
        {
            base.AddStorage(info);
            Graph.FillEllipse(Brushes.Red, info.xy.X, info.xy.Y, 5, 5);
            //Model.Invalidate();
        }
    }
}
