using Newtonsoft.Json;
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
    public partial class SignalViewWid : DockContent
    {
        public BindingList<SignalViewDataSource> signalViewDataSource = new BindingList<SignalViewDataSource>();
        public SignalViewWidConfig signalViewWidConfig = new SignalViewWidConfig();

        SelectSignalWid selectSignalWid = new SelectSignalWid();

        public SignalViewWid()
        {
            InitializeComponent();
        }

        private void SignalViewWid_Load(object sender, EventArgs e)
        {
            this.Text = "信号显示";
            int colIdx = 0;

            signalViewDataSource = signalViewWidConfig.signalViewDataSource;
            
            dataGridView1.ContextMenuStrip = contextMenuStrip1;
            dataGridView1.RowTemplate.Height = 22;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            colIdx = dataGridView1.Columns.Add("name", "消息名");
            dataGridView1.Columns[colIdx].DataPropertyName = "Name";
            dataGridView1.Columns[colIdx].Width = 100;
            dataGridView1.Columns[colIdx].SortMode = DataGridViewColumnSortMode.NotSortable;

            colIdx = dataGridView1.Columns.Add("val", "数值");
            dataGridView1.Columns[colIdx].DataPropertyName = "Val";
            //dataGridView1.Columns[colIdx].Width = 100;
            dataGridView1.Columns[colIdx].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[colIdx].SortMode = DataGridViewColumnSortMode.NotSortable;

            dataGridView1.DataSource = signalViewDataSource;
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectSignalWid.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < selectSignalWid.selectedSignal.Count; i++)
                {
                    SignalViewDataSource svds = new SignalViewDataSource();
                    svds.Name = selectSignalWid.selectedSignal[i].signalName;
                    svds.signalName = selectSignalWid.selectedSignal[i].signalName;
                    svds.fileName = selectSignalWid.selectedSignal[i].fileName;
                    svds.messageName = selectSignalWid.selectedSignal[i].messageName;
                    svds.Val = "0";
                    signalViewDataSource.Add(svds);
                }
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedNum = dataGridView1.SelectedRows.Count;
            if (selectedNum > 0)
            {
                int[] selectIndex = new int[selectedNum];
                for (int i = 0; i < selectedNum; i++)
                {
                    selectIndex[i] = dataGridView1.SelectedRows[i].Index;
                }
                for (int i = 0; i < selectedNum; i++)
                {
                    signalViewDataSource.RemoveAt(selectIndex[i]);
                }
            }
        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            signalViewDataSource.Clear();
        }

        public void Updata()
        {
            for(int i=0;i< signalViewDataSource.Count;i++)
            {
                signalViewDataSource[i].Val = DbcManager.dbcFiles1[signalViewDataSource[i].fileName].messages[signalViewDataSource[i].messageName].signals[signalViewDataSource[i].signalName].value.ToString();
            }
        }
    }

    public class SignalViewDataSource : INotifyPropertyChanged
    {
        public string fileName;
        public string messageName;
        public string signalName;

        private string name = "sig";
        public string Name
        {
            get { return name; }
            set { name = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Name")); }
        }
        private string val = "0";
        public string Val
        {
            get { return val; }
            set { val = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Value")); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class SignalViewWidConfig
    {
        public BindingList<SignalViewDataSource> signalViewDataSource = new BindingList<SignalViewDataSource>();

    }
}
