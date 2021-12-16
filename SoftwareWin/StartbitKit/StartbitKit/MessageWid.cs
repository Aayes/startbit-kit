using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace StartbitKit
{
    public partial class MessageWid : DockContent
    {
        //用虚拟模式加载比较好
        bool useVirtualMode = true;
        List<ListViewItem> CurrentCacheItemsSource=new List<ListViewItem>();

        public MessageWid()
        {
            InitializeComponent();
        }

        private void MessageWid_Load(object sender, EventArgs e)
        {
            this.Text = "报文接收";

            Type dgvType = this.listView_message.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(this.listView_message, true, null);

            listView_message.View = View.Details;
            listView_message.AllowColumnReorder = false;
            listView_message.FullRowSelect = true;
            if (useVirtualMode)
            {
                listView_message.VirtualMode = true;
            }
            else
            {
                listView_message.VirtualMode = false;
            }

            listView_message.Columns.Add("时间", 84, HorizontalAlignment.Left);
            listView_message.Columns.Add("ID", 78, HorizontalAlignment.Left);
            listView_message.Columns.Add("数据", 200, HorizontalAlignment.Left);
        }

        public void AddMessage(List<VirtualMessage> message)
        {
            if (message.Count>0)
            {
                ListViewItem[] items = new ListViewItem[message.Count];
                for (int i=0;i< message.Count;i++)
                {
                    items[i] = new ListViewItem();
                    items[i].Text = message[i].timeStamp.ToString();
                    if ((message[i].GetMsg().Id & 0x80000000) == 0)
                    {
                        items[i].SubItems.Add(message[i].GetMsg().Id.ToString("X"));
                    }
                    else
                    {
                        items[i].SubItems.Add((message[i].GetMsg().Id & 0x7FFFFFFF).ToString("X") + "x");
                    }
                    items[i].SubItems.Add(CommonLib.ByteAryToString(message[i].Data));
                }
                CurrentCacheItemsSource.AddRange(items);
                if(useVirtualMode)
                {
                    listView_message.VirtualListSize = CurrentCacheItemsSource.Count;
                    listView_message.RetrieveVirtualItem += ListView_message_RetrieveVirtualItem;
                }
                else
                {
                    listView_message.Items.AddRange(items);
                }
                listView_message.EnsureVisible(CurrentCacheItemsSource.Count - 1);
            }
        }

        private void ListView_message_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            if (CurrentCacheItemsSource == null || CurrentCacheItemsSource.Count == 0)
            {
                return;
            }
            e.Item = CurrentCacheItemsSource[e.ItemIndex];
        }
    }
}
