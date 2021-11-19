using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace StartbitKit
{
    class VirtualBus
    {
        public List<Node> nodes = new List<Node>();
        Stopwatch stopwatch = new Stopwatch();
        BusState busState = new BusState();
        Message currentMessage = new Message();
        Logging logging = new Logging();
        public void Start()
        {
            nodes.Add(new Node("N1"));
            nodes.Add(new Node("N2"));
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
        double lastTime = 0;
        private void run()
        {
            while(true)
            {
                double time = (stopwatch.ElapsedTicks / (double)Stopwatch.Frequency);

                //simulate send
                if((time - lastTime) >= 0.01)
                {
                    Message message = new Message();
                    message.Data = new byte[8];
                    message.id = 0x345;
                    nodes[0].Send(message);

                    message.id = 0x123;
                    nodes[1].Send(message);

                    lastTime = time;
                }
                
               
                //deal with node tx
                int leastIdIndex = -1;
                uint idTemp = 0xFFFFFFFF;
                for(int i=0;i< nodes.Count;i++)
                {
                    if(nodes[i].txFifo.Count>0)
                    {
                        if (nodes[i].txFifo.ElementAt(0).id < idTemp)
                        {
                            idTemp = nodes[i].txFifo.ElementAt(0).id;
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
                        if(i!= leastIdIndex)
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
