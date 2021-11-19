using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StartbitKit
{
    public struct Message
    {
        public uint id;
        public float timeStamp;
        public bool isExtern;
        public byte[] Data;
    }
}
