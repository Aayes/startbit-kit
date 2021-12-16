namespace StartbitKit
{
    partial class MessageWid
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
            this.listView_message = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // listView_message
            // 
            this.listView_message.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_message.HideSelection = false;
            this.listView_message.Location = new System.Drawing.Point(0, 0);
            this.listView_message.Name = "listView_message";
            this.listView_message.Size = new System.Drawing.Size(460, 632);
            this.listView_message.TabIndex = 0;
            this.listView_message.UseCompatibleStateImageBehavior = false;
            // 
            // MessageWid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 632);
            this.Controls.Add(this.listView_message);
            this.Name = "MessageWid";
            this.Text = "MessageWid";
            this.Load += new System.EventHandler(this.MessageWid_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView_message;
    }
}