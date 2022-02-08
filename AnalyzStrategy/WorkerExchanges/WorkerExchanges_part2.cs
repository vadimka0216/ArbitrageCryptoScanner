using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnalyzStrategy.Exchanges;
using System.Threading;
using System.IO;

namespace AnalyzStrategy
{
    partial class WorkerExchanges//This is class work with table and have methods start/stop analyz
    {
        public struct StorageStrategy//uniqs pairs
        {
            public StorageStrategy(BaseExchange.Pair _pair)
            {
                pair = _pair;
                StateThread = -1;//isRun
                idsExchanges = new List<int>();
            }
            public int StateThread;//bool isRun
            public List<int> idsExchanges;
            public BaseExchange.Pair pair;
        }
        List<StorageStrategy> strategys = new List<StorageStrategy>();
        //List<Thread> threads = new List<Thread>();
        private void ClearStrategys()
        {
            //uniqPairs.Clear();
            foreach (var it in strategys)
                it.idsExchanges.Clear();
            for (int i = 0; i < strategys.Count; i++)
                StopStrategy(i);
            foreach (var it in strategys)
                while (it.StateThread >= 0)
                    Thread.Sleep(50);
            strategys.Clear();
        }
        private void InitialStrategys()
        {
            ClearStrategys();
            //call table viewer in main form!
            List<BaseExchange.Pair[]> buffer = new List<BaseExchange.Pair[]>();
            List<Task> tasks = new List<Task>();
            int state = 0;
            foreach (var it in exchanges)
            {
                var exchange = it;
                int i = buffer.Count;
                buffer.Add(null);
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    do
                    {
                        if ((buffer[i] = it.getPairs()) == null)
                        {
                            LogError(exchange, "ошибка получения торговых пар");
                        }
                    }
                    while (buffer[i] == null);
                    form.IncValueProgress(state++, exchanges.Count, 35.0);
                }));
            }
            Task.WaitAll(tasks.ToArray());
            tasks.Clear();

            for (int i = 0; i < buffer.Count; i++)
            {
                for (int j = 0; j < buffer[i].Length; j++)
                {
                    bool isFind = false;
                    for (int k = 0; k < strategys.Count; k++)
                    {
                        if (buffer[i][j].name1 == strategys[k].pair.name1 &&
                            buffer[i][j].name2 == strategys[k].pair.name2)
                        {
                            isFind = true;
                            //break;
                            strategys[k].idsExchanges.Add(i);
                        }
                    }
                    if (!isFind)
                    {
                        strategys.Add(new StorageStrategy(buffer[i][j]));
                        strategys[strategys.Count - 1].idsExchanges.Add(i);
                    }
                }
                form.IncValueProgress(i+1, buffer.Count, 10.0);
            }

            buffer.Clear();

            for (int i = 0; i < strategys.Count; i++)
            {
                if (strategys[i].idsExchanges.Count > 1)
                {
                    string desc = "";
                    foreach (var id in strategys[i].idsExchanges)
                        desc += exchanges[id].getName() + " | ";
                    form.AddRowTable(i, desc, strategys[i].pair.name1, strategys[i].pair.name2, strategys[i].StateThread != -1);
                }
                form.IncValueProgress(i+1, strategys.Count, 5.0);
            }
        }

        public void StartStrategy(int id, string path = "", int wsleep = 2000)
        {
            if (strategys.Count > id)
            {
                if (strategys[id].StateThread == -1)
                {
                    //int ithread = threads.Count;
                    //threads.Add(null);

                    //threads[ithread].Start();
                    var temp = strategys[id]; temp.StateThread = -2;//wait start thread //ithread;
                    strategys[id] = temp;

                    ThreadPool.QueueUserWorkItem((object state) =>//ThreadPool
                    {
                        //threads[ithread] = Thread.CurrentThread;
                        temp.StateThread = 1; strategys[id] = temp;
                        form.UpdateRowTable(id, 4, true);

                        var strategy = strategys[id];
                        string nameFile = path;
                        foreach (var idExchange in strategy.idsExchanges)
                            nameFile += exchanges[idExchange].getName() + "_";
                        nameFile += strategy.pair.name1 + "_" + strategy.pair.name2 + ".csv";
                        form.UpdateRowTable(id, 6, nameFile);

                        double maxProfit = 0.0;
                        bool isFirst = true;

                        string HeaderString = "DATE:TIME,";
                        for (int i = 0; i < strategy.idsExchanges.Count-1; i++)
                        {
                            var exchange1 = exchanges[strategy.idsExchanges[i]];
                            var exchange2 = exchanges[strategy.idsExchanges[i+1]];
                            HeaderString += exchange1.getName()+"-" + exchange2.getName()+ ",";
                            HeaderString += exchange2.getName() + "-" + exchange1.getName();
                            if (strategy.idsExchanges.Count > i + 2)
                                HeaderString += ",";
                        }
                        var culture = doubleEn.getCulture();
                        List<Task<int>> tasks = new List<Task<int>>();
                        BaseExchange.InfoPair[] infos = new BaseExchange.InfoPair[strategy.idsExchanges.Count];
                        while (strategys[id].StateThread>0)
                        {
                            long currentTick = MiscFuncs.GetTickCount();
                            this.proxys.reloadRating();
                            for (int i = 0; i < strategy.idsExchanges.Count; i++)
                            {
                                var exchange = exchanges[strategy.idsExchanges[i]];
                                infos[i] = new BaseExchange.InfoPair(strategy.pair);
                                int index = i;
                                var task = (Task<int>.Factory.StartNew(() =>
                                {
                                    string proxy = this.proxys.getBestNext();
                                    exchange.setProxy(proxy);
                                    int res = 0;
                                    while ((res = exchange.getInfoPair(ref infos[index])) == 1)
                                    {
                                        this.proxys.setInvalid(proxy);
                                        proxy = this.proxys.getBestNext();
                                        exchange.setProxy(proxy);
                                    };
                                    return res;
                                }));
                                tasks.Add(task);
                                //task.Start();
                            }
                            for (int i = 0; i < tasks.Count; i++)
                            {
                                tasks[i].Wait();
                                if (tasks[i].Result == 2)
                                {
                                    LogError(exchanges[strategy.idsExchanges[i]], 
                                        "ошибка, торговая пара: "+strategy.pair.ToString()+" пропала");

                                    strategy.idsExchanges.RemoveAt(i);
                                    string desc = "";
                                    foreach (var idExch in strategys[i].idsExchanges)
                                        desc += exchanges[idExch].getName() + " | ";
                                    form.UpdateRowTable(id, 1, desc);
                                    if (strategy.idsExchanges.Count < 2)
                                    {                                        
                                        this.StopStrategy(id);
                                        //break?
                                    }

                                }
                            }
                            tasks.Clear();
                            this.proxys.reloadRating();
                            
                            if (!File.Exists(nameFile))
                            {
                                File.WriteAllText(nameFile, HeaderString+"\r\n");
                            }

                            if (isFirst)
                            {
                                maxProfit = infos[0].bprice / infos[1].sprice - infos[0].sprice / infos[1].bprice;
                                form.UpdateRowTable(id, 5, maxProfit);
                                isFirst = false;
                            }

                            string output = "";
                            for (int i = 0; i < infos.Length-1; i++)
                            {
                                double profit1 = -1; 
                                double profit2 = -1;
                                if (infos[i + 1].sprice > 0 && infos[i].bprice>0&&
                                    infos[i].sprice > 0 && infos[i + 1].bprice>0)
                                {
                                    profit1 = infos[i].bprice / infos[i + 1].sprice - 1;
                                    profit2 = 1 - infos[i].sprice / infos[i + 1].bprice;
                                    if (profit1 + profit2 > maxProfit)
                                    {
                                        maxProfit = profit1 + profit2;
                                        form.UpdateRowTable(id, 5, maxProfit);
                                        form.UpdateRowTable(id, 7, //leader
                                            exchanges[strategy.idsExchanges[i]].getName()+"-"+
                                            exchanges[strategy.idsExchanges[i+1]].getName());
                                    }
                                }
                                output += profit1.ToString("F6", culture) + "," + profit2.ToString("F6", culture);
                                if (infos.Length > i + 2) output += ",";
                            }

                            File.AppendAllText(nameFile, DateTime.Now.ToShortDateString() + ":"
                                + DateTime.Now.ToShortTimeString() + "," + output+"\r\n");

                            //File.WriteAppendText
                            long newsleep = wsleep - MiscFuncs.GetTickCount() + currentTick;
                            if (newsleep < 0) newsleep = 0;
                            Thread.Sleep((int)newsleep);
                        }

                        strategy.StateThread = -1; strategys[id] = strategy;
                    });
                }
            }
        }

        public delegate void func();

        public void StopStrategy(int id, func callback = null)
        {
            if (strategys.Count > id)
            {
                if (strategys[id].StateThread > 0)
                {
                    (new Task(() =>
                    {
                        var temp = strategys[id]; temp.StateThread = 0;
                        strategys[id] = temp;
                        while (strategys[id].StateThread >= 0)
                            Thread.Sleep(50);
                        //threads[ithread].Abort();
                        //threads[ithread].Join();
                        form.UpdateRowTable(id, 4, false);

                        if (callback != null) callback();
                    })).Start();
                }
            }
        }
    }
}