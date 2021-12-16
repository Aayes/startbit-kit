using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StartbitKit
{
    public struct VirtualMessage
    {
        public float timeStamp;
        public byte[] Data;
        public string fileName;
        public string messageName;

        public DBCLib.Message GetMsg()
        {
            return DbcManager.dbcFiles1[fileName].messages[messageName].message;
        }
    }

}
