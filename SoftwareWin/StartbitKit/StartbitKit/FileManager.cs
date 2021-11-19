using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StartbitKit
{
    class FileManager
    {
        FileStream fileStream;
        StreamWriter streamWriter;
        StringBuilder buffer;

        public void CreateFile(string path)
        {
            fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
            streamWriter = new StreamWriter(fileStream);
            buffer = new StringBuilder();
        }

        public void AppendFile(string str)
        {
            buffer.Append(str);
        }

        public void SaveFile()
        {
            streamWriter.Write(buffer);
            streamWriter.Close();
        }
    }

    public struct FilePath
    {
        public string path;
        public string prefix;
        public string name;
        public string suffix;
        public string fullPath;

        public string GetFullPath()
        {
            fullPath = path + prefix + name + suffix;
            return fullPath;
        }
    }
}
