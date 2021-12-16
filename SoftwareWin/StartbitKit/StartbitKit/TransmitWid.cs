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
    public partial class TransmitWid : DockContent
    {
        public BindingList<TransmitTableSource> transmitTableSource = new BindingList<TransmitTableSource>();
        public TransmitWidConfig transmitWidConfig = new TransmitWidConfig();

        SelectMessageWid selectMessageWid = new SelectMessageWid();
        List<BasicPropertyBag> messagePropList = new List<BasicPropertyBag>();

        int selectMsgIndex = 0;
        int tick = 0;

        public TransmitWid()
        {
            InitializeComponent();
        }

        private void TransmitWid_Load(object sender, EventArgs e)
        {
            this.Text = "报文发送";
            int colIdx = 0;

            transmitTableSource = transmitWidConfig.transmitTableSource;
 
            for(int i=0;i< transmitTableSource.Count;i++)
            {
                BasicPropertyBag bag = new BasicPropertyBag();
                foreach (var sig in transmitTableSource[i].GetMsg().Signals)
                {
                    MetaProp mp;
                    Type t = typeof(double);
                    Dictionary<string, long> valueTable = new Dictionary<string, long>();
                    bool isEnum = false;
                    foreach (var val in DbcManager.dbcFiles1[transmitTableSource[i].fileName].valueTables)
                    {
                        if ((val.Value.ContextMessageId == transmitTableSource[i].GetMsg().Id) && (val.Value.ContextSignalName == sig.Name))
                        {
                            t = val.Value.Mapping.GetType();
                            EnumConverter.table = new string[val.Value.Mapping.Count()];
                            int index = 0;
                            foreach (var v in val.Value.Mapping)
                            {
                                EnumConverter.table[index] = string.Format("(0x{0:X}){1}", v.Key, v.Value);
                                valueTable.Add(EnumConverter.table[index], v.Key);
                                index++;
                            }
                            isEnum = true;
                            break;
                        }
                    }
                    if (isEnum)
                    {
                        mp = new MetaProp(
                            sig.Name,
                            typeof(string),
                            new Attribute[] { new DescriptionAttribute(sig.Unit), new CategoryAttribute(transmitTableSource[i].Name), new TypeConverterAttribute(typeof(EnumConverter)) }
                            );
                        mp.valueTable = valueTable;
                        bag[sig.Name] = mp.valueTable.Keys.ToArray()[0];
                    }
                    else
                    {
                        mp = new MetaProp(
                            sig.Name,
                            t,
                            new Attribute[] { new DescriptionAttribute(sig.Unit), new CategoryAttribute(transmitTableSource[i].Name) }
                            );
                        bag[sig.Name] = 0.0;
                    }
                    bag.Properties.Add(mp);
                }
                messagePropList.Add(bag);
            }
            
            dataGridView_transmit.ContextMenuStrip = contextMenuStrip_message;
            dataGridView_transmit.RowTemplate.Height = 22;
            dataGridView_transmit.AllowUserToResizeRows = false;
            dataGridView_transmit.AllowUserToAddRows = false;
            dataGridView_transmit.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView_transmit.AutoGenerateColumns = false;
            dataGridView_transmit.RowHeadersVisible = false;
            dataGridView_transmit.BorderStyle = BorderStyle.None;
            dataGridView_transmit.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            colIdx = dataGridView_transmit.Columns.Add("name", "消息名");
            dataGridView_transmit.Columns[colIdx].DataPropertyName = "Name";
            dataGridView_transmit.Columns[colIdx].Width = 100;
            dataGridView_transmit.Columns[colIdx].SortMode = DataGridViewColumnSortMode.NotSortable;

            colIdx = dataGridView_transmit.Columns.Add("id", "ID(hex)");
            dataGridView_transmit.Columns[colIdx].DataPropertyName = "Id";
            dataGridView_transmit.Columns[colIdx].Width = 78;
            dataGridView_transmit.Columns[colIdx].SortMode = DataGridViewColumnSortMode.NotSortable;

            colIdx = dataGridView_transmit.Columns.Add("data", "数据(hex)");
            dataGridView_transmit.Columns[colIdx].ToolTipText = "hex,空格隔开";
            dataGridView_transmit.Columns[colIdx].DataPropertyName = "Data";
            dataGridView_transmit.Columns[colIdx].Width = 200;
            dataGridView_transmit.Columns[colIdx].SortMode = DataGridViewColumnSortMode.NotSortable;

            colIdx = dataGridView_transmit.Columns.Add("period", "周期");
            dataGridView_transmit.Columns[colIdx].DataPropertyName = "Period";
            dataGridView_transmit.Columns[colIdx].Width = 45;
            dataGridView_transmit.Columns[colIdx].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView_transmit.Columns[colIdx].SortMode = DataGridViewColumnSortMode.NotSortable;

            DataGridViewCheckBoxColumn chkCol = new DataGridViewCheckBoxColumn();
            chkCol.Name = "IsCycleSend";
            chkCol.DataPropertyName = "IsCycleSend";
            chkCol.TrueValue = true;
            chkCol.FalseValue = false;
            chkCol.HeaderText = "周期发送使能";
            chkCol.Width = 25;
            chkCol.ToolTipText = "周期发送使能";
            dataGridView_transmit.Columns.Add(chkCol);

            DataGridViewButtonColumn btnCol = new DataGridViewButtonColumn();
            btnCol.Name = "Send";
            btnCol.HeaderText = "";
            btnCol.DataPropertyName = "Button";
            btnCol.Text = "发送";
            btnCol.Width = 60;
            btnCol.SortMode = DataGridViewColumnSortMode.NotSortable;
            dataGridView_transmit.Columns.Add(btnCol);

            dataGridView_transmit.DataSource = transmitTableSource;
        }
  
        public void TickEvent(VirtualNode node)
        {
            tick++;
            for (int i = 0; i < transmitTableSource.Count; i++)
            {
                if (transmitTableSource[i].IsCycleSend)
                {
                    uint period = transmitTableSource[i].Period;
                    if (transmitTableSource[i].Period == 0)
                    {
                        period = 1;
                    }
                    if (transmitTableSource[i].isSending)
                    {
                        if ((tick % period) == 0)
                        {
                            VirtualMessage message = new VirtualMessage();
                            byte[] data = new byte[8];

                            message.fileName = transmitTableSource[i].fileName;
                            message.messageName = transmitTableSource[i].messageName;
                            message.Data = CommonLib.StringToAry(transmitTableSource[i].Data);
                            node.Send(message);
                        }
                    }
                }
                else
                {
                    if (transmitTableSource[i].isSending)
                    {
                        VirtualMessage message = new VirtualMessage();
                        byte[] data = new byte[8];

                        message.fileName = transmitTableSource[i].fileName;
                        message.messageName = transmitTableSource[i].messageName;
                        message.Data = CommonLib.StringToAry(transmitTableSource[i].Data);
                        node.Send(message);

                        transmitTableSource[i].isSending = false;
                    }
                }
            }
        }
        
        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(selectMessageWid.ShowDialog() == DialogResult.OK)
            {
                for(int i=0;i< selectMessageWid.selectedMessages.Count;i++)
                {
                    TransmitTableSource tts = new TransmitTableSource();
                    tts.messageName = selectMessageWid.selectedMessages[i].messageName;
                    tts.fileName = selectMessageWid.selectedMessages[i].fileName;
                    tts.Name = tts.messageName;
                    if ((tts.GetMsg().Id & 0x80000000) == 0)
                    {
                        tts.Id = tts.GetMsg().Id.ToString("X");
                    }
                    else
                    {
                        tts.Id = (tts.GetMsg().Id & 0x7FFFFFFF).ToString("X") + "x";
                    }
                    transmitTableSource.Add(tts);

                    BasicPropertyBag bag = new BasicPropertyBag();
                    foreach(var sig in tts.GetMsg().Signals)
                    {
                        MetaProp mp;
                        Type t = typeof(double);
                        Dictionary<string, long> valueTable = new Dictionary<string, long>();
                        bool isEnum = false;
                        foreach (var val in DbcManager.dbcFiles1[selectMessageWid.selectedMessages[i].fileName].valueTables)
                        {
                            if((val.Value.ContextMessageId == tts.GetMsg().Id) && (val.Value.ContextSignalName == sig.Name))
                            {
                                t = val.Value.Mapping.GetType();
                                EnumConverter.table = new string[val.Value.Mapping.Count()];
                                int index = 0;
                                foreach (var v in val.Value.Mapping)
                                {
                                    EnumConverter.table[index] = string.Format("(0x{0:X}){1}", v.Key, v.Value);
                                    valueTable.Add(EnumConverter.table[index], v.Key);
                                    index++;
                                }
                                isEnum = true;
                                break;
                            }
                        }
                        if(isEnum)
                        {
                            mp = new MetaProp(
                                sig.Name,
                                typeof(string),
                                new Attribute[] { new DescriptionAttribute(sig.Unit), new CategoryAttribute(transmitTableSource[i].Name),new TypeConverterAttribute(typeof(EnumConverter)) }
                                );
                            mp.valueTable = valueTable;
                            bag[sig.Name] = mp.valueTable.Keys.ToArray()[0];
                        }
                        else
                        {
                            mp = new MetaProp(
                                sig.Name,
                                t,
                                new Attribute[] { new DescriptionAttribute(sig.Unit), new CategoryAttribute(transmitTableSource[i].Name) }
                                );
                            bag[sig.Name] = 0.0;
                        }
                        bag.Properties.Add(mp);
                    }
                    messagePropList.Add(bag);
                    dataGridView_transmit.ClearSelection();
                    dataGridView_transmit.Rows[transmitTableSource.Count - 1].Selected = true;
                }
            }
        }
        
        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedNum = dataGridView_transmit.SelectedRows.Count;
            if(selectedNum > 0)
            {
                int[] selectIndex = new int[selectedNum];
                for(int i=0;i< selectedNum;i++)
                {
                    selectIndex[i] = dataGridView_transmit.SelectedRows[i].Index;
                }
                for(int i=0;i< selectedNum;i++)
                {
                    transmitTableSource.RemoveAt(selectIndex[i]);
                    messagePropList.RemoveAt(selectIndex[i]);
                }
                if(messagePropList.Count == 0)
                {
                    propertyGrid_signal.SelectedObject = null;
                }
            }
        }

        private void 清空ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            transmitTableSource.Clear();
            messagePropList.Clear();
            propertyGrid_signal.SelectedObject = null;
        }

        private void dataGridView_transmit_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView_transmit.SelectedRows.Count > 0)
            {
                selectMsgIndex = dataGridView_transmit.SelectedRows[0].Index;

                if (messagePropList.Count > 0)
                {
                    List<object> values = DbcManager.UnpackSignal(transmitTableSource[selectMsgIndex].GetMsg(), transmitTableSource[selectMsgIndex].Data);
                    var sigs = transmitTableSource[selectMsgIndex].GetMsg().Signals;
                    for (int i = 0; i < sigs.Count(); i++)
                    {
                        if(messagePropList[selectMsgIndex].Properties[i].Type == typeof(string))
                        {
                            foreach(var k in messagePropList[selectMsgIndex].Properties[i].valueTable)
                            {
                                if(k.Value == Convert.ToInt64(values[i]))
                                {
                                    messagePropList[selectMsgIndex][sigs.ElementAt(i).Name] = k.Key;
                                }
                            }
                        }
                        else
                        {
                            messagePropList[selectMsgIndex][sigs.ElementAt(i).Name] = values[i];
                        }
                    }
                    propertyGrid_signal.SelectedObject = messagePropList[selectMsgIndex];
                    propertyGrid_signal.Refresh();
                }
            }
        }

        private void dataGridView_transmit_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex >=0) && (e.ColumnIndex == dataGridView_transmit.Columns["Send"].DisplayIndex))
            {
                if (transmitTableSource[e.RowIndex].IsCycleSend)
                {
                    if (transmitTableSource[e.RowIndex].isSending)
                    {
                        transmitTableSource[e.RowIndex].Button = "发送";
                        transmitTableSource[e.RowIndex].isSending = false;
                    }
                    else
                    {
                        transmitTableSource[e.RowIndex].Button = "停止";
                        transmitTableSource[e.RowIndex].isSending = true;
                    }
                }
                else
                {
                    transmitTableSource[e.RowIndex].isSending = true;
                    transmitTableSource[e.RowIndex].Button = "发送";
                }
            }
        }

        private void dataGridView_transmit_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView_transmit.IsCurrentCellDirty)
            {
                if(dataGridView_transmit.CurrentCell.ColumnIndex == dataGridView_transmit.Columns["IsCycleSend"].DisplayIndex)
                {
                    dataGridView_transmit.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
        }

        private void dataGridView_transmit_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex >= 0) && (e.ColumnIndex == dataGridView_transmit.Columns["IsCycleSend"].DisplayIndex))
            {
                //get changed value
                //bool value = (bool)dataGridView_transmit[e.ColumnIndex, e.RowIndex].Value;
                transmitTableSource[e.RowIndex].Button = "发送";
                transmitTableSource[e.RowIndex].isSending = false;
            }

            if ((e.RowIndex >= 0) && (e.ColumnIndex == dataGridView_transmit.Columns["data"].DisplayIndex))
            {
                int selectMsgIndex = e.RowIndex;
                if (messagePropList.Count > 0)
                {
                    List<object> values = DbcManager.UnpackSignal(transmitTableSource[selectMsgIndex].GetMsg(), transmitTableSource[selectMsgIndex].Data);
                    var sigs = transmitTableSource[selectMsgIndex].GetMsg().Signals;
                    for (int i = 0; i < sigs.Count(); i++)
                    {
                        if (messagePropList[selectMsgIndex].Properties[i].Type == typeof(string))
                        {
                            foreach (var k in messagePropList[selectMsgIndex].Properties[i].valueTable)
                            {
                                if (k.Value == Convert.ToInt64(values[i]))
                                {
                                    messagePropList[selectMsgIndex][sigs.ElementAt(i).Name] = k.Key;
                                }
                            }
                        }
                        else
                        {
                            messagePropList[selectMsgIndex][sigs.ElementAt(i).Name] = values[i];
                        }
                    }
                    propertyGrid_signal.SelectedObject = messagePropList[selectMsgIndex];
                    propertyGrid_signal.Refresh();
                }
            }
        }

        private void propertyGrid_signal_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if(selectMsgIndex >= 0)
            {
                List<object> values = new List<object>();
                for(int i=0;i< messagePropList[selectMsgIndex].Properties.Count;i++)
                {
                    if(messagePropList[selectMsgIndex].Properties[i].Type == typeof(string))
                    {
                        long val = 0;
                        messagePropList[selectMsgIndex].Properties[i].valueTable.TryGetValue((string)(messagePropList[selectMsgIndex][messagePropList[selectMsgIndex].Properties[i].Name]), out val);
                        Console.WriteLine(val);
                        values.Add((uint)val);
                    }
                    else
                    {
                        Console.WriteLine(messagePropList[selectMsgIndex][messagePropList[selectMsgIndex].Properties[i].Name]);
                        values.Add(messagePropList[selectMsgIndex][messagePropList[selectMsgIndex].Properties[i].Name]);
                    }
                }
                transmitTableSource[selectMsgIndex].Data = DbcManager.PackSignalToString(transmitTableSource[selectMsgIndex].GetMsg(), values);
            }
        }
    }

    public class TransmitTableSource : INotifyPropertyChanged
    {
        public string fileName = "";
        public string messageName = "";
        public DBCLib.Message GetMsg()
        {
            return DbcManager.dbcFiles1[fileName].messages[messageName].message;
        }

        private string name = "msg";
        public string Name
        {
            get{return name;}
            set{name = value; if(PropertyChanged!=null)PropertyChanged(this, new PropertyChangedEventArgs("Name")); }
        }
        [JsonIgnore]
        private string id = "0";
        public string Id
        {
            get{return id;}
            set{id = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Id"));}
        }
        private string data = "00 00 00 00 00 00 00 00";
        public string Data
        {
            get { return data; }
            set { data = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Data")); }
        }
        private bool isCycleSend = false;
        public bool IsCycleSend
        {
            get { return isCycleSend; }
            set { isCycleSend = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("IsCycleSend")); }
        }
        private UInt32 period = 100;
        public UInt32 Period
        {
            get { return period; }
            set { period = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Period")); }
        }
       
        private string button = "发送";
        [JsonIgnore]
        public string Button
        {
            get{return button;}
            set{button = value; if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Button")); }
        }
        [JsonIgnore]
        public bool isButtonClick = false;
        [JsonIgnore]
        public bool isSending = false;
        
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class TransmitWidConfig
    {
        public BindingList<TransmitTableSource> transmitTableSource = new BindingList<TransmitTableSource>();

    }
}
