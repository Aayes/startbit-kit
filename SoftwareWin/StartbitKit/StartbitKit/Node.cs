using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StartbitKit
{
    class Node
    {
        public string name;
        public ConcurrentQueue<Message> txFifo = new ConcurrentQueue<Message>();


        public Node(string _name)
        {
            name = _name;
        }


        //for user
        public void Send(Message msg)
        {
            txFifo.Enqueue(msg);
        }

        public delegate void ReceiveEvent(Message message);
        public ReceiveEvent receiveEvent;
    }
}
