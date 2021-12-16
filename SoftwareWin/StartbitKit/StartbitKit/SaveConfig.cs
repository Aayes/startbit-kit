using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StartbitKit
{
    /// <summary>
    /// 配置类，软件的配置定义在这个类中
    /// </summary>
    public class Config
    {
        public MainWidConfig mainWidConfig = new MainWidConfig();
        public ConfigWidConfig configWidConfig = new ConfigWidConfig();
        public TransmitWidConfig transmitWidConfig = new TransmitWidConfig();
        public SignalViewWidConfig signalViewWidConfig = new SignalViewWidConfig();
    }

    /// <summary>
    /// 保存配置类，加载和保存配置
    /// </summary>
    public static class SaveConfig
    {
        public static Config config=new Config();
        public static string error = "";

        private static string path = "";

        //加载配置文件
        public static int LoadConfigFile(string _path)
        {
            int re = 0;
            string configFileString = "";
            FileStream configFile = null;
            StreamReader configReader = null;
            path = _path;
            try
            {
                configFile = new FileStream(_path, FileMode.Open, FileAccess.Read);
                configReader = new StreamReader(configFile, Encoding.Default);
                configFileString = configReader.ReadToEnd();
                configReader.Close();
                configFile.Close();
            }
            catch (Exception en)
            {
                re = 1;
                error = "读取配置文件失败 " + en.Message;
            }

            if (re == 0)
            {
                try
                {
                    config = JsonConvert.DeserializeObject<Config>(configFileString, new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace});

                }
                catch (Exception en)
                {
                    re = 2;
                    error = "解析配置文件失败 " + en.Message;
                }
            }

            if (re != 0)
            {
                config = new Config();
            }

            return re;
        }

        //保存配置文件
        public static int SaveConfigFile()
        {
            int re = 0;
            JsonSerializer serializer = new JsonSerializer();
            FileStream configFile = null;
            StreamWriter configWriter = null;

            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;

            try
            {
                configFile = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);
                configWriter = new StreamWriter(configFile, Encoding.Default);
            }
            catch (Exception en)
            {
                re = 1;
                error = "保存配置文件失败 " + en.Message;
            }

            if (re == 0)
            {
                try
                {
                    JsonWriter writer = new JsonTextWriter(configWriter);
                    serializer.Formatting = Formatting.Indented;
                    serializer.Serialize(writer, config);

                    writer.Close();
                    configWriter.Close();
                    configFile.Close();
                }
                catch (Exception en)
                {
                    re = 2;
                    error = "保存配置文件失败1 " + en.Message;
                }
            }

            return re;
        }
    }
}
