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
    public partial class SelectSignalWid : Form
    {
        public List<SelectedSignal> selectedSignal = new List<SelectedSignal>();
        public SelectSignalWid()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
        }

        private void SelectSignalWid_Load(object sender, EventArgs e)
        {
            selectedSignal.Clear();

            treeView1.ShowPlusMinus = true;
            treeView1.CheckBoxes = true;
            treeView1.Nodes.Clear();

            foreach (var file in DbcManager.dbcFiles1)
            {
                TreeNode dbcNode = treeView1.Nodes.Add(file.Key);
                foreach (var msg in file.Value.messages)
                {
                    TreeNode messageNode = dbcNode.Nodes.Add(msg.Key);
                    foreach(var sig in msg.Value.signals)
                    {
                        messageNode.Nodes.Add(sig.Key);
                    }
                }
            }
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            for (int i=0;i< treeView1.Nodes.Count;i++)
            {
                for(int j=0;j< treeView1.Nodes[i].Nodes.Count;j++)
                {
                    for (int k = 0; k < treeView1.Nodes[i].Nodes[j].Nodes.Count; k++)
                    {
                        if (treeView1.Nodes[i].Nodes[j].Nodes[k].Checked)
                        {
                            selectedSignal.Add(new SelectedSignal(
                                treeView1.Nodes[i].Text, 
                                treeView1.Nodes[i].Nodes[j].Text, 
                                treeView1.Nodes[i].Nodes[j].Nodes[k].Text));
                        }
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
    public class SelectedSignal
    {
        public string fileName;
        public string messageName;
        public string signalName;

        public SelectedSignal(string _fileName, string _messageName,string _signalName)
        {
            fileName = _fileName;
            messageName = _messageName;
            signalName = _signalName;
        }
    }
}
