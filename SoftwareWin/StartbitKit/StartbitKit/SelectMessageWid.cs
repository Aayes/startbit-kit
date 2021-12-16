using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StartbitKit
{
    public partial class SelectMessageWid : Form
    {
        public List<SelectedMessage> selectedMessages = new List<SelectedMessage>();
        public SelectMessageWid()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
        }

        private void SelectSignalWid_Load(object sender, EventArgs e)
        {
            selectedMessages.Clear();

            treeView1.ShowPlusMinus = true;
            treeView1.CheckBoxes = true;
            treeView1.Nodes.Clear();
            
            foreach(var file in DbcManager.dbcFiles1)
            {
                TreeNode dbcNode = treeView1.Nodes.Add(file.Key);
                foreach(var msg in file.Value.messages)
                {
                    TreeNode messageNode = dbcNode.Nodes.Add(msg.Key);
                }
            }
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            for (int i=0;i< treeView1.Nodes.Count;i++)
            {
                for(int j=0;j< treeView1.Nodes[i].Nodes.Count;j++)
                {
                    if(treeView1.Nodes[i].Nodes[j].Checked)
                    {
                        selectedMessages.Add(new SelectedMessage(treeView1.Nodes[i].Text, treeView1.Nodes[i].Nodes[j].Text));
                    }
                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            SetSubNodeCheckState(e.Node, e.Node.Checked);
        }
        private void SetSubNodeCheckState(TreeNode node, bool state)
        {
            if (node.Nodes.Count != 0)
            {
                for (int i = 0; i < node.Nodes.Count; i++)
                {
                    node.Nodes[i].Checked = state;
                    SetSubNodeCheckState(node.Nodes[i], state);
                }
            }
        }
    }

    public class SelectedMessage
    {
        public string fileName;
        public string messageName;

        public SelectedMessage(string _fileName, string _messageName)
        {
            fileName = _fileName;
            messageName = _messageName;
        }

        public DBCLib.Message GetMsg()
        {
            return DbcManager.dbcFiles1[fileName].messages[messageName].message;
        }
    }
}
