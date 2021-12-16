namespace StartbitKit
{
    partial class TransmitWid
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView_transmit = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.propertyGrid_signal = new System.Windows.Forms.PropertyGrid();
            this.contextMenuStrip_message = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清空ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_transmit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip_message.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView_transmit
            // 
            this.dataGridView_transmit.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_transmit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_transmit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_transmit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_transmit.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_transmit.Name = "dataGridView_transmit";
            this.dataGridView_transmit.RowTemplate.Height = 27;
            this.dataGridView_transmit.Size = new System.Drawing.Size(462, 268);
            this.dataGridView_transmit.TabIndex = 0;
            this.dataGridView_transmit.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_transmit_CellContentClick);
            this.dataGridView_transmit.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_transmit_CellValueChanged);
            this.dataGridView_transmit.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridView_transmit_CurrentCellDirtyStateChanged);
            this.dataGridView_transmit.SelectionChanged += new System.EventHandler(this.dataGridView_transmit_SelectionChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView_transmit);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertyGrid_signal);
            this.splitContainer1.Size = new System.Drawing.Size(464, 540);
            this.splitContainer1.SplitterDistance = 270;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 1;
            // 
            // propertyGrid_signal
            // 
            this.propertyGrid_signal.Dock = System.Windows.Forms.DockStyle.Left;
            this.propertyGrid_signal.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid_signal.Name = "propertyGrid_signal";
            this.propertyGrid_signal.Size = new System.Drawing.Size(324, 262);
            this.propertyGrid_signal.TabIndex = 0;
            this.propertyGrid_signal.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_signal_PropertyValueChanged);
            // 
            // contextMenuStrip_message
            // 
            this.contextMenuStrip_message.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip_message.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加ToolStripMenuItem,
            this.删除ToolStripMenuItem,
            this.清空ToolStripMenuItem});
            this.contextMenuStrip_message.Name = "contextMenuStrip_message";
            this.contextMenuStrip_message.Size = new System.Drawing.Size(111, 76);
            // 
            // 添加ToolStripMenuItem
            // 
            this.添加ToolStripMenuItem.Name = "添加ToolStripMenuItem";
            this.添加ToolStripMenuItem.Size = new System.Drawing.Size(110, 24);
            this.添加ToolStripMenuItem.Text = "添加";
            this.添加ToolStripMenuItem.Click += new System.EventHandler(this.添加ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(110, 24);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // 清空ToolStripMenuItem
            // 
            this.清空ToolStripMenuItem.Name = "清空ToolStripMenuItem";
            this.清空ToolStripMenuItem.Size = new System.Drawing.Size(110, 24);
            this.清空ToolStripMenuItem.Text = "清空";
            this.清空ToolStripMenuItem.Click += new System.EventHandler(this.清空ToolStripMenuItem_Click);
            // 
            // TransmitWid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 540);
            this.Controls.Add(this.splitContainer1);
            this.Name = "TransmitWid";
            this.Text = "TransmitWid";
            this.Load += new System.EventHandler(this.TransmitWid_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_transmit)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip_message.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_transmit;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PropertyGrid propertyGrid_signal;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_message;
        private System.Windows.Forms.ToolStripMenuItem 添加ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 清空ToolStripMenuItem;
    }
}