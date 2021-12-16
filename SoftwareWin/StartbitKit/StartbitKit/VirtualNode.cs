using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StartbitKit
{
    public class VirtualNode
    {
        public string name;
        public ConcurrentQueue<VirtualMessage> txFifo = new ConcurrentQueue<VirtualMessage>();
        
        public VirtualNode(string _name)
        {
            name = _name;
        }
        
        //tx
        public void Send(VirtualMessage msg)
        {
            txFifo.Enqueue(msg);
        }
        //rx
        public delegate void ReceiveEvent(VirtualMessage message);
        public ReceiveEvent receiveEvent;
    }
}
