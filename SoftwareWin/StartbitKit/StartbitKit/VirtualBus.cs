using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace StartbitKit
{
    class VirtualBus
    {
        public List<VirtualNode> nodes = new List<VirtualNode>();
        Stopwatch stopwatch = new Stopwatch();
        BusState busState = new BusState();
        VirtualMessage currentMessage = new VirtualMessage();
        Logging logging = new Logging();
        double lastTime = 0;

        public delegate void TickEvent();
        public TickEvent tickEvent;

        public void Start()
        {
            nodes.Add(new VirtualNode("N1"));
            nodes.Add(new VirtualNode("N2"));
            Thread thread = new Thread(new ThreadStart(run));
            thread.IsBackground = true;
            thread.Start();
            stopwatch.Start();
            logging.Start("./Test.asc");
        }

        public void Stop()
        {
            logging.Stop();
        }

        private void run()
        {
            while(true)
            {
                double time = (stopwatch.ElapsedTicks / (double)Stopwatch.Frequency);
                if ((time - lastTime) >= 0.001)
                {
                    if(tickEvent!=null)
                    {
                        tickEvent();
                    }
                    lastTime = time;
                }

                //deal with node tx
                int leastIdIndex = -1;
                uint idTemp = 0xFFFFFFFF;
                for(int i=0;i< nodes.Count;i++)
                {
                    if(nodes[i].txFifo.Count>0)
                    {
                        VirtualMessage msg = nodes[i].txFifo.ElementAt(0);
                        var id = DbcManager.dbcFiles1[msg.fileName].messages[msg.messageName].message.Id;
                        if (id < idTemp)
                        {
                            idTemp = id;
                            leastIdIndex = i;
                        }
                    }
                }
                if(leastIdIndex >=0)
                {
                    nodes[leastIdIndex].txFifo.TryDequeue(out currentMessage);
                    currentMessage.timeStamp = (float)time;
                    busState = BusState.BUSY;
                    logging.Append(currentMessage);
                }

                //deal with node rx
                //TODO consider message Time?
                if (busState == BusState.BUSY)
                {
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        //if(i!= leastIdIndex)
                        {
                            if (nodes[i].receiveEvent != null)
                            {
                                nodes[i].receiveEvent(currentMessage);
                            }
                        }
                    }
                    busState = BusState.IDLE;
                }
            }
        }
    }
    enum BusState
    {
        IDLE,
        BUSY
    }
}
