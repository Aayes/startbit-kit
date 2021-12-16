using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StartbitKit
{
    class Logging
    {
        FileManager fileManager = new FileManager();

        public void Start(string path)
        {
            fileManager.CreateFile(path);
            fileManager.AppendFile("base hex timestamps absolute\r\n");
        }

        public void Append(string str)
        {
            fileManager.AppendFile(str);
        }

        public void Append(VirtualMessage message)
        {
            string str = string.Format("{0:F4}\t1 {1:x8}{2} {3} d\t{4:d}\t{5:X2} {6:X2} {7:X2} {8:X2} {9:X2} {10:X2} {11:X2} {12:X2}\r\n"
                            , message.timeStamp
                            , message.GetMsg().Id
                            , (((message.GetMsg().Id & 0x80000000) == 0) ? ("") : ("x"))
                            , ("Rx")
                            , 8
                            , message.Data[0]
                            , message.Data[1]
                            , message.Data[2]
                            , message.Data[3]
                            , message.Data[4]
                            , message.Data[5]
                            , message.Data[6]
                            , message.Data[7]);
           
            fileManager.AppendFile(str);
        }

        public void Stop()
        {
            fileManager.AppendFile("End Triggerblock\r\n");
            fileManager.SaveFile();
        }
    }
}
