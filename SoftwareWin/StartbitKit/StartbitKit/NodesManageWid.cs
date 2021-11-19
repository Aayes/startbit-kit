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
    public partial class NodesManageWid : DockContent
    {
        public NodesManageWid()
        {
            InitializeComponent();
        }

        private void NodesManageWid_Load(object sender, EventArgs e)
        {
            this.Text = "节点配置";
        }
    }
}
