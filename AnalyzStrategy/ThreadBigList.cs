using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace AnalyzStrategy
{
    class ThreadBigList
    {
        int count, countThreads; 
        object locker;

        private int getCount()
        {
            int result = 0;
            lock (locker)
            {
                result=count;
            }
            return result;
        }
        private void IncCount()
        {
            lock (locker)
            {
                count++;
            }
        }
        private void DecrCount()
        {
            lock (locker)
            {
                count--;
            }
        }

        public ThreadBigList(int _countThreads)//requestThreads...
        {
            countThreads = count = _countThreads;
            locker = new object();
        }
        public ThreadBigList()//unknow count threads..
        {
            countThreads = count = 0;
            locker = new object();
        }

        public delegate void function();
        public void New(function func)
        {
            if (countThreads == 0)
            {
                IncCount();
            }

            ThreadPool.QueueUserWorkItem((object callback) =>
            {
                if (func!=null)
                {
                    func();
                }
                DecrCount();
            });
        }
        public void Wait()
        {
            while (getCount()!= 0)
            {
                Thread.Sleep(50);
            }
        }
    }
}
