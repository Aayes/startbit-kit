using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StartbitKit
{
    class DbcManager
    {
        public static Dictionary<string, DbcFile1> dbcFiles1 = new Dictionary<string, DbcFile1>();

        public static List<object> UnpackSignal(DBCLib.Message message, string data)
        {
            return UnpackSignal(message, CommonLib.StringToAry(data));
        }
        public static List<object> UnpackSignal(DBCLib.Message message,byte[]data)
        {
            List<object> objs = new List<object>();
            UInt64 signalValue = 0;
            DBCLib.Message.Signal.ByteOrderEnum msgByteOrder;

            msgByteOrder = message.Signals.ElementAt(0).ByteOrder;
            for (int i = 0; i < 8; i++)
            {
                if (msgByteOrder == DBCLib.Message.Signal.ByteOrderEnum.BigEndian)//motorola
                {
                    signalValue |= (UInt64)data[i] << (8 * (7 - i));
                }
                else//intel
                {
                    signalValue |= (UInt64)data[i] << (8 * i);
                }
            }
            foreach (var signal in message.Signals)
            {
                UInt64 temp = 0;
                int startBit = 0;
                if (signal.ByteOrder == DBCLib.Message.Signal.ByteOrderEnum.BigEndian) //motorola
                {
                    byte[] bitMap = new byte[64] {
                        7, 6, 5, 4, 3, 2, 1, 0,
                        15,14,13,12,11,10,9, 8,
                        23,22,21,20,19,18,17,16,
                        31,30,29,28,27,26,25,24,
                        39,38,37,36,35,34,33,32,
                        47,46,45,44,43,42,41,40,
                        55,54,53,52,51,50,49,48,
                        63,62,61,60,59,58,57,56
                    };
                    for (int i = 0; i < 64; i++)
                    {
                        if (signal.StartBit == bitMap[i])
                        {
                            startBit = i;
                            break;
                        }
                    }
                    startBit = (int)(64 - signal.BitSize - startBit);
                }
                else//intel
                {
                    startBit = (int)signal.StartBit;
                }
                temp = signalValue >> startBit;
                temp &= (~(0xFFFFFFFFFFFFFFFF << (int)signal.BitSize));
                objs.Add(temp * signal.ScaleFactor + signal.Offset);
            }
            return objs;
        }
        
        public static string PackSignalToString(DBCLib.Message message, List<object> signalValues)
        {
            return CommonLib.ByteAryToString(PackSignalToByte(message, signalValues));
        }
        public static byte[] PackSignalToByte(DBCLib.Message message, List<object> signalValues)
        {
            byte[] result = new byte[8];
            UInt64 signalValue = 0;
            int signalIndex = 0;
            DBCLib.Message.Signal.ByteOrderEnum msgByteOrder;

            msgByteOrder = message.Signals.ElementAt(0).ByteOrder;

            foreach (var signal in message.Signals)
            {
                UInt64 temp = 0;
                int startBit = 0;

                if (signal.ByteOrder == DBCLib.Message.Signal.ByteOrderEnum.BigEndian) //motorola
                {
                    byte[] bitMap = new byte[64] {
                        7, 6, 5, 4, 3, 2, 1, 0,
                        15,14,13,12,11,10,9, 8,
                        23,22,21,20,19,18,17,16,
                        31,30,29,28,27,26,25,24,
                        39,38,37,36,35,34,33,32,
                        47,46,45,44,43,42,41,40,
                        55,54,53,52,51,50,49,48,
                        63,62,61,60,59,58,57,56
                    };
                    for (int i = 0; i < 64; i++)
                    {
                        if (signal.StartBit == bitMap[i])
                        {
                            startBit = i;
                            break;
                        }
                    }
                    startBit = (int)(64 - signal.BitSize - startBit);
                }
                else//intel
                {
                    startBit = (int)signal.StartBit;
                }
                temp = (UInt64)((Convert.ToDouble(signalValues[signalIndex]) - signal.Offset) / signal.ScaleFactor);
                temp &= (~(0xFFFFFFFFFFFFFFFF << (int)signal.BitSize));
                signalValue &= ~((~(0xFFFFFFFFFFFFFFFF << (int)signal.BitSize)) << startBit);
                signalValue |= temp << startBit;

                signalIndex++;
            }
            for (int i = 0; i < 8; i++)
            {
                if (msgByteOrder == DBCLib.Message.Signal.ByteOrderEnum.BigEndian) //motorola
                {
                    result[i] = (byte)(signalValue >> (8 * (7 - i)));
                }
                else//intel
                {
                    result[i] = (byte)(signalValue >> (8 * i));
                }
            }
            return result;
        }
    }

    class DbcFile1
    {
        public string fileName;
        public Dictionary<string, Message1> messages = new Dictionary<string, Message1>();
        public Dictionary<string, DBCLib.Value> valueTables = new Dictionary<string, DBCLib.Value>();
        public void Init(List<object> dbcObj)
        {
            for (int i = 0; i < dbcObj.Count; i++)
            {
                if (dbcObj[i].GetType() == typeof(DBCLib.Message))
                {
                    DBCLib.Message message = (DBCLib.Message)dbcObj[i];
                    Message1 msg = new Message1();
                    msg.message = message;
                    foreach (var sig in message.Signals)
                    {
                        Signal1 signal1 = new Signal1();
                        signal1.Signal = sig;
                        msg.signals.Add(sig.Name,signal1);
                    }
                    messages.Add(message.Name, msg);
                }
                else if(dbcObj[i].GetType() == typeof(DBCLib.Value))
                {
                    valueTables.Add(((DBCLib.Value)dbcObj[i]).ContextSignalName,(DBCLib.Value)dbcObj[i]);
                }
            }
        }
    }
    class Message1
    {
        public DBCLib.Message message = new DBCLib.Message();
        public Dictionary<string, Signal1> signals = new Dictionary<string, Signal1>();
    }
    class Signal1
    {
        public DBCLib.Message.Signal Signal = new DBCLib.Message.Signal();
        public DBCLib.Value valueTable = new DBCLib.Value();
        public object value = 0;
    }

}
