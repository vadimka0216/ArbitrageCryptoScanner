using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalyzStrategy.Exchanges;
using System.Threading;
using System.Windows.Forms;

namespace AnalyzStrategy
{
    partial class WorkerExchanges
    {
        ManageProxy proxys = new ManageProxy_v2();
        bool isTest = false; MainForm form;
        List<BaseExchange> exchanges = new List<BaseExchange>();
        public WorkerExchanges(MainForm obj, bool RunTest=false)
        {
            form = obj;
            isTest = RunTest;
        }
        ~WorkerExchanges() { this.Dispose(); }
        public void Dispose() 
        {
            ClearStrategys();
            exchanges.Clear(); 
        }
        Task prevTask = null;
        public void Load()
        {
            if (prevTask == null&&form!=null)
            {
                prevTask = (new Task(() =>
                {
                    var threads = new ThreadBigList();
                    threads.New(() => //run proxy checker
                    { 
                        int state=0;
                        proxys.reloadList((int count) => { form.IncValueProgress(state++,count,50.0); });
                        //form.AddValueBar(25);
                        form.UpdateCountProxy(proxys.getCount());
                    });

                    if (isTest)
                    {
                        foreach (var exchange in exchanges)
                        {
                            TestExchange test = new TestExchange(exchange);
                            test.RunTests();
                        }
                    }

                    InitialStrategys();//run parse strategys....

                    threads.Wait();
                    prevTask = null;
                }));
                prevTask.Start();
            }
        }
        private void AddExchange(BaseExchange exchange)
        {
            exchanges.Add(exchange);
        }
        static public WorkerExchanges operator +(WorkerExchanges obj, BaseExchange exchange)
        {
            obj.AddExchange(exchange);
            return obj;
        }
        static private void LogError(BaseExchange exchange, string msg)
        {
            MessageBox.Show(exchange.getName()+": "+msg,"Error",
                MessageBoxButtons.OK,MessageBoxIcon.Error);
        }
    }
}
