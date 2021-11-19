using Multimedia;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace StartbitKit
{
    public partial class MainWid : Form
    {
        MainWidConfig mainWidConfig = new MainWidConfig();
        Wid1 wid = new Wid1();
        SettingWid settingWid = new SettingWid();
        NodesManageWid nodesManageWid = new NodesManageWid();

        VirtualBus virtualBus = new VirtualBus();
        MTimer mTimer;
        MTimer mTimer1;

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

            ThemeBase theme = new VS2015LightTheme();
            this.visualStudioToolStripExtender1.SetStyle(toolStrip1, VisualStudioToolStripExtender.VsVersion.Vs2015, theme);
            this.visualStudioToolStripExtender1.SetStyle(menuStrip1, VisualStudioToolStripExtender.VsVersion.Vs2015, theme);
            this.visualStudioToolStripExtender1.SetStyle(statusStrip1, VisualStudioToolStripExtender.VsVersion.Vs2015, theme);
            dockPanel1.Theme = theme;
            wid.Show(dockPanel1, WeifenLuo.WinFormsUI.Docking.DockState.Document);
            nodesManageWid.Show(dockPanel1, WeifenLuo.WinFormsUI.Docking.DockState.DockRight);
            virtualBus.Start();

            mTimer = new MTimer();
            mTimer.Mode = Multimedia.TimerMode.Periodic;
            mTimer.Period = 1;//ms
            mTimer.Resolution = 1;
            mTimer.Tick += mTimer_Tick;
            //mTimer.Start();

            mTimer1 = new MTimer();
            mTimer1.Mode = Multimedia.TimerMode.Periodic;
            mTimer1.Period = 2;//ms
            mTimer1.Resolution = 1;
            mTimer1.Tick += mTimer1_Tick;
            // mTimer1.Start();

            virtualBus.nodes[0].receiveEvent = new Node.ReceiveEvent(ReceiveEvent);
            virtualBus.nodes[1].receiveEvent = new Node.ReceiveEvent(ReceiveEvent1);

        }

        private void ReceiveEvent(Message message)
        {
            //throw new NotImplementedException();
            //Console.WriteLine("n0 get:"+ message.id);
        }
        private void ReceiveEvent1(Message message)
        {
            //Console.WriteLine("n1 get:" + message.id);
        }

        private void mTimer_Tick(object sender, EventArgs e)
        {
            //Message message = new Message();
            //message.Data = new byte[8];
            //message.id = 0x345;
            //virtualBus.nodes[0].Send(message);
        }
        private void mTimer1_Tick(object sender, EventArgs e)
        {
            //Message message = new Message();
            //message.Data = new byte[8];
            //message.id = 0x123;
            //virtualBus.nodes[1].Send(message);
        }
        private void toolStripMenuItem_settings_Click(object sender, EventArgs e)
        {

            settingWid.ShowDialog();
        }

        private void MainWid_FormClosed(object sender, FormClosedEventArgs e)
        {
            mTimer.Stop();
            mTimer1.Stop();
            virtualBus.Stop();
            SaveConfig.SaveConfigFile();
        }
    }
    public class MainWidConfig
    {

    }
}
