using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StartbitKit
{
    class CommonLib
    {
        //十六进制字符串转byte数组
        public static byte[]StringToAry(string str)
        {
            byte[] result =new byte[0];

            if(str != null)
            {
                string []ary =  str.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                result = new byte[ary.Length];
                for(int i=0;i< ary.Length;i++)
                {
                    result[i] = Convert.ToByte(ary[i], 16);
                }
            }

            return result;
        }
        //byte数组转字符串
        static public string ByteAryToString(byte[] ary)
        {
            string re = "";
            if (ary != null)
            {
                for (int i = 0; i < ary.Length; i++)
                {
                    re += ary[i].ToString("X2");
                    if (i != (ary.Length - 1))
                    {
                        re += " ";
                    }
                }
            }
            return re;
        }
    }
}
