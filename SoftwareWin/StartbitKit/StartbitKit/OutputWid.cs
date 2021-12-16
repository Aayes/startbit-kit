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
    public partial class OutputWid : DockContent
    {
        static List<ListViewItem> Items = new List<ListViewItem>();

        public OutputWid()
        {
            InitializeComponent();
            this.Text = "输出";
        }

        private void OutputWid_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listView1.AllowColumnReorder = false;
            listView1.FullRowSelect = true;
            
            listView1.Columns.Add("时间", 84, HorizontalAlignment.Left);
            listView1.Columns.Add("类型", 78, HorizontalAlignment.Center);
            listView1.Columns.Add("内容", 500, HorizontalAlignment.Left);

            timer1.Start();
        }
        
        public static void Output(OutputType type, string content)
        {
            ListViewItem item = new ListViewItem();

            item.Text = DateTime.Now.ToLongTimeString();
            switch(type)
            {
                case OutputType.INFO:
                    {
                        item.SubItems.Add("info");
                        break;
                    }
                case OutputType.WARNING:
                    {
                        item.SubItems.Add("warning");
                        break;
                    }
                case OutputType.ERROR:
                    {
                        item.SubItems.Add("error");
                        break;
                    }
            }
            item.SubItems.Add(content);
            Items.Add(item);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(Items.Count>0)
            {
                listView1.Items.AddRange(Items.ToArray());
                listView1.EnsureVisible(listView1.Items.Count - 1);
                Items.Clear();
            }
        }
    }
    public enum OutputType
    {
        INFO,
        WARNING,
        ERROR
    }
}
