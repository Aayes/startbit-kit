using Multimedia;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace StartbitKit
{
    public partial class MainWid : Form
    {
        MainWidConfig mainWidConfig = new MainWidConfig();
        SettingWid settingWid = new SettingWid();
        ConfigWid configWid = new ConfigWid();
        TransmitWid transmitWid = new TransmitWid();
        MessageWid messageWid = new MessageWid();
        OutputWid outputWid = new OutputWid();
        SignalViewWid signalViewWid = new SignalViewWid();

        VirtualBus virtualBus = new VirtualBus();
        MTimer mTimer;
        ConcurrentQueue<VirtualMessage> rxBuffer = new ConcurrentQueue<VirtualMessage>();
        
        string dockFilePath = "dock.xml";
        DeserializeDockContent deserializeDockContent;

        public MainWid()
        {
            InitializeComponent();
        }
        
        private void MainWid_Load(object sender, EventArgs e)
        {
            this.Text = "StartbitKit";

            //加载配置文件
            if (SaveConfig.LoadConfigFile("./Config.json") != 0)
            {
                SaveConfig.SaveConfigFile();
            }
            mainWidConfig = SaveConfig.config.mainWidConfig;
            configWid.configWidConfig = SaveConfig.config.configWidConfig;
            transmitWid.transmitWidConfig = SaveConfig.config.transmitWidConfig;
            signalViewWid.signalViewWidConfig = SaveConfig.config.signalViewWidConfig;

            ThemeBase theme = new VS2015LightTheme();
            dockPanel1.Theme = theme;
            this.visualStudioToolStripExtender1.SetStyle(toolStrip1, VisualStudioToolStripExtender.VsVersion.Vs2015, theme);
            this.visualStudioToolStripExtender1.SetStyle(menuStrip1, VisualStudioToolStripExtender.VsVersion.Vs2015, theme);
            this.visualStudioToolStripExtender1.SetStyle(statusStrip1, VisualStudioToolStripExtender.VsVersion.Vs2015, theme);

            deserializeDockContent = new DeserializeDockContent(GetString);
            //if(false)
            if (File.Exists(dockFilePath))
            {
                dockPanel1.LoadFromXml(dockFilePath, deserializeDockContent);
            }
            else
            {
                configWid.Show(dockPanel1, WeifenLuo.WinFormsUI.Docking.DockState.DockLeft);
                transmitWid.Show(dockPanel1, DockState.Document);
                messageWid.Show(transmitWid.Pane, DockAlignment.Left, 0.4);
                signalViewWid.Show(messageWid.Pane, DockAlignment.Top,0.5);
                //messageWid.DockAreas = DockAreas.Document | DockAreas.DockBottom;
                outputWid.Show(dockPanel1,DockState.DockBottomAutoHide);
                
            }
            dockPanel1.ActiveAutoHideContent = outputWid;
            outputWid.Activate();

            virtualBus.Start();

            mTimer = new MTimer();
            mTimer.Mode = Multimedia.TimerMode.Periodic;
            mTimer.Period = 1;//ms
            mTimer.Resolution = 1;
            mTimer.Tick += mTimer_Tick;
            //mTimer.Start();

            virtualBus.tickEvent = new VirtualBus.TickEvent(TickEvent);
            virtualBus.nodes[0].receiveEvent = new VirtualNode.ReceiveEvent(ReceiveEvent);
            virtualBus.nodes[1].receiveEvent = new VirtualNode.ReceiveEvent(ReceiveEvent1);

            OutputWid.Output(OutputType.INFO, "启动...");

            timer_ui.Start();
        }

        private void TickEvent()
        {
            transmitWid.TickEvent(virtualBus.nodes[0]);
        }

        private IDockContent GetString(string persistString)
        {
            if (persistString == typeof(ConfigWid).ToString())
            {
                return configWid;
            }
            else if (persistString == typeof(TransmitWid).ToString())
            {
                return transmitWid;
            }
            else if (persistString == typeof(MessageWid).ToString())
            {
                return messageWid;
            }
            else if (persistString == typeof(OutputWid).ToString())
            {
                return outputWid;
            }
            else if (persistString == typeof(SignalViewWid).ToString())
            {
                return signalViewWid;
            }
            else
            {
                return null;
            }
        }

        private void ReceiveEvent(VirtualMessage message)
        {
            List<object> objs = DbcManager.UnpackSignal(message.GetMsg(), message.Data);
            var sigs = DbcManager.dbcFiles1[message.fileName].messages[message.messageName].signals;

            for (int i=0;i< sigs.Count; i++)
            {
                DBCLib.Value val = new DBCLib.Value();
                if(DbcManager.dbcFiles1[message.fileName].valueTables.TryGetValue(sigs.ElementAt(i).Value.Signal.Name,out val))
                {
                    foreach (var k in val.Mapping)
                    {
                        if (k.Key == Convert.ToInt32(objs[i]))
                        {
                            sigs.ElementAt(i).Value.value = k.Value;
                        }
                    }
                    //sigs.ElementAt(i).Value.value = val.Mapping.ElementAt(Convert.ToInt32(objs[i])).Value;
                }
                else
                {
                    sigs.ElementAt(i).Value.value = objs[i];
                }
            }
            rxBuffer.Enqueue(message);
        }
        private void ReceiveEvent1(VirtualMessage message)
        {

        }

        private void mTimer_Tick(object sender, EventArgs e)
        {

        }
 
        private void toolStripMenuItem_settings_Click(object sender, EventArgs e)
        {
            dockPanel1.ActiveAutoHideContent = outputWid;
        }

        private void MainWid_FormClosed(object sender, FormClosedEventArgs e)
        {
            dockPanel1.SaveAsXml("dock.xml");
            mTimer.Stop();
            virtualBus.Stop();
            SaveConfig.SaveConfigFile();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            OutputWid.Output(OutputType.INFO, "test output");
            dockPanel1.ActiveAutoHideContent = outputWid;
            outputWid.Activate();
        }

        private void timer_ui_Tick(object sender, EventArgs e)
        {
            int len = rxBuffer.Count;
            List<VirtualMessage> msgs = new List<VirtualMessage>();
            for (int i = 0; i < len; i++)
            {
                VirtualMessage m;
                if (rxBuffer.TryDequeue(out m))
                {
                    msgs.Add(m);
                }
            }
            messageWid.AddMessage(msgs);

            //signal view
            signalViewWid.Updata();
        }
    }
    public class MainWidConfig
    {

    }
}
