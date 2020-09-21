namespace Launcher.Forms
{
    partial class LogForm
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
            this.listView_LogMain = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.comboBox_LogLevel = new System.Windows.Forms.ComboBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.checkBox_Update = new System.Windows.Forms.CheckBox();
            this.checkBox_AboveAll = new System.Windows.Forms.CheckBox();
            this.button_Close = new System.Windows.Forms.Button();
            this.label_Desc = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listView_LogMain
            // 
            this.listView_LogMain.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.listView_LogMain.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView_LogMain.FullRowSelect = true;
            this.listView_LogMain.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView_LogMain.HideSelection = false;
            this.listView_LogMain.LabelWrap = false;
            this.listView_LogMain.Location = new System.Drawing.Point(12, 12);
            this.listView_LogMain.MultiSelect = false;
            this.listView_LogMain.Name = "listView_LogMain";
            this.listView_LogMain.Size = new System.Drawing.Size(776, 382);
            this.listView_LogMain.TabIndex = 0;
            this.listView_LogMain.UseCompatibleStateImageBehavior = false;
            this.listView_LogMain.View = System.Windows.Forms.View.Details;
            this.listView_LogMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.listView_LogMain_MouseUp);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "时间";
            this.columnHeader1.Width = 97;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "来源";
            this.columnHeader2.Width = 91;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "类型";
            this.columnHeader3.Width = 89;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "内容";
            this.columnHeader4.Width = 417;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "状态/耗时";
            this.columnHeader5.Width = 73;
            // 
            // comboBox_LogLevel
            // 
            this.comboBox_LogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_LogLevel.Items.AddRange(new object[] {
            "调试 Debug",
            "信息 Info",
            "警告 Warn",
            "错误 Error",
            "致命 Fatal"});
            this.comboBox_LogLevel.Location = new System.Drawing.Point(12, 409);
            this.comboBox_LogLevel.Name = "comboBox_LogLevel";
            this.comboBox_LogLevel.Size = new System.Drawing.Size(99, 20);
            this.comboBox_LogLevel.TabIndex = 1;
            this.comboBox_LogLevel.SelectedIndexChanged += new System.EventHandler(this.comboBox_LogLevel_SelectedIndexChanged);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Cursor = System.Windows.Forms.Cursors.Default;
            this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel1.Location = new System.Drawing.Point(486, 412);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(53, 12);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "常见问题";
            this.linkLabel1.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // checkBox_Update
            // 
            this.checkBox_Update.AutoSize = true;
            this.checkBox_Update.Location = new System.Drawing.Point(551, 411);
            this.checkBox_Update.Name = "checkBox_Update";
            this.checkBox_Update.Size = new System.Drawing.Size(72, 16);
            this.checkBox_Update.TabIndex = 3;
            this.checkBox_Update.Text = "实时模式";
            this.checkBox_Update.UseVisualStyleBackColor = true;
            this.checkBox_Update.CheckedChanged += new System.EventHandler(this.checkBox_Update_CheckedChanged);
            // 
            // checkBox_AboveAll
            // 
            this.checkBox_AboveAll.AutoSize = true;
            this.checkBox_AboveAll.Location = new System.Drawing.Point(629, 411);
            this.checkBox_AboveAll.Name = "checkBox_AboveAll";
            this.checkBox_AboveAll.Size = new System.Drawing.Size(72, 16);
            this.checkBox_AboveAll.TabIndex = 4;
            this.checkBox_AboveAll.Text = "窗口置顶";
            this.checkBox_AboveAll.UseVisualStyleBackColor = true;
            this.checkBox_AboveAll.CheckedChanged += new System.EventHandler(this.checkBox_AboveAll_CheckedChanged);
            // 
            // button_Close
            // 
            this.button_Close.Location = new System.Drawing.Point(713, 407);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new System.Drawing.Size(75, 23);
            this.button_Close.TabIndex = 5;
            this.button_Close.Text = "关闭";
            this.button_Close.UseVisualStyleBackColor = true;
            this.button_Close.Click += new System.EventHandler(this.button_Close_Click);
            // 
            // label_Desc
            // 
            this.label_Desc.AutoSize = true;
            this.label_Desc.Location = new System.Drawing.Point(117, 415);
            this.label_Desc.Name = "label_Desc";
            this.label_Desc.Size = new System.Drawing.Size(113, 12);
            this.label_Desc.TabIndex = 6;
            this.label_Desc.Text = "日志列表将实时滚动";
            this.label_Desc.Visible = false;
            // 
            // LogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 442);
            this.Controls.Add(this.label_Desc);
            this.Controls.Add(this.button_Close);
            this.Controls.Add(this.checkBox_AboveAll);
            this.Controls.Add(this.checkBox_Update);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.comboBox_LogLevel);
            this.Controls.Add(this.listView_LogMain);
            this.MaximizeBox = false;
            this.Name = "LogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "运行日志 - ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LogForm_FormClosing);
            this.Load += new System.EventHandler(this.LogForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listView_LogMain;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ComboBox comboBox_LogLevel;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.CheckBox checkBox_Update;
        private System.Windows.Forms.CheckBox checkBox_AboveAll;
        private System.Windows.Forms.Button button_Close;
        private System.Windows.Forms.Label label_Desc;
    }
}