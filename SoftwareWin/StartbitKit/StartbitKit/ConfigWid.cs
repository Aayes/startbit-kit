using DBCLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace StartbitKit
{
    public partial class ConfigWid : DockContent
    {
        TreeNode hardwareNode;
        TreeNode databaseNode;
        TreeNode currentNode;
        public ConfigWidConfig configWidConfig = new ConfigWidConfig();

        Reader dbcReader = new Reader();


        public ConfigWid()
        {
            InitializeComponent();
        }
        
        private void ConfigWid_Load(object sender, EventArgs e)
        {
            this.Text = "配置";

            hardwareNode = treeView1.Nodes.Add("硬件配置");
            hardwareNode.Nodes.Add("CAN");

            databaseNode = treeView1.Nodes.Add("数据库(DBC)");
         

            treeView1.ShowPlusMinus = true;

            for(int i=0;i< configWidConfig.dbcFiles.Count;i++)
            {
                string fullName = configWidConfig.dbcFiles[i];
                string fileName = Path.GetFileName(fullName);

                dbcReader.AllowErrors = true;
                List<KeyValuePair<uint, string>> errors = new List<KeyValuePair<uint, string>>();
                List<KeyValuePair<uint, string>> warnings = new List<KeyValuePair<uint, string>>();

                DbcFile1 dbcFile1 = new DbcFile1();
                dbcFile1.fileName = fileName;
                dbcFile1.Init(dbcReader.Read(fullName, errors, warnings));
                DbcManager.dbcFiles1.Add(fileName, dbcFile1);
                
                if ((errors.Count != 0) || (warnings.Count != 0))
                {

                }
                databaseNode.Nodes.Add(fileName);
                databaseNode.ExpandAll();
            }
            
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                currentNode = e.Node;
                if (e.Node.Text == "数据库(DBC)")
                {
                    e.Node.ContextMenuStrip = contextMenuStrip_dbc_root;
                }
                else if((e.Node.Parent!=null) && (e.Node.Parent.Text == "数据库(DBC)"))
                {
                    e.Node.ContextMenuStrip = contextMenuStrip_dbc;
                }
            }
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.Multiselect = true;
            openFile.InitialDirectory = Application.ExecutablePath;
            openFile.Filter = "DBC文件(*.dbc)|*.dbc";
            //openFile.FilterIndex = 1;
            openFile.RestoreDirectory = true;

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                foreach(string fullName in openFile.FileNames)
                {
                    string fileName = Path.GetFileName(fullName);
                    if (!configWidConfig.dbcFiles.Contains(fullName))
                    {
                        configWidConfig.dbcFiles.Add(fullName);
                        dbcReader.AllowErrors = true;
                        List<KeyValuePair<uint, string>> errors = new List<KeyValuePair<uint, string>>();
                        List<KeyValuePair<uint, string>> warnings = new List<KeyValuePair<uint, string>>();
                        
                        DbcFile1 dbcFile1 = new DbcFile1();
                        dbcFile1.fileName = fileName;
                        dbcFile1.Init(dbcReader.Read(fullName, errors, warnings));
                        DbcManager.dbcFiles1.Add(fileName, dbcFile1);

                        if ((errors.Count!=0) || (warnings.Count != 0))
                        {
                           
                        }
                        databaseNode.Nodes.Add(fileName);
                        databaseNode.ExpandAll();
                    }
                    else
                    {
                        MessageBox.Show("dbc已经存在");
                    }
                }
            }
        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            configWidConfig.dbcFiles.Clear();
            databaseNode.Nodes.Clear();
            DbcManager.dbcFiles1.Clear();
        }

        private void 移除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            configWidConfig.dbcFiles.RemoveAt(currentNode.Index);
            databaseNode.Nodes.RemoveAt(currentNode.Index);
            DbcManager.dbcFiles1.Remove(currentNode.Text);

        }
    }
    public class ConfigWidConfig
    {
        public List<string> dbcFiles = new List<string>();
    }

}
