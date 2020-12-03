namespace Launcher.Forms
{
    partial class PluginManageForm
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
            this.listView_PluginList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_MainVersion = new System.Windows.Forms.Label();
            this.button_Close = new System.Windows.Forms.Button();
            this.linkLabel_MoreApps = new System.Windows.Forms.LinkLabel();
            this.button_AppDir = new System.Windows.Forms.Button();
            this.button_EventList = new System.Windows.Forms.Button();
            this.button_Reload = new System.Windows.Forms.Button();
            this.groupBox_Desc = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label_VersionTitle = new System.Windows.Forms.Label();
            this.label_Version = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label_AuthorTitle = new System.Windows.Forms.Label();
            this.label_Author = new System.Windows.Forms.Label();
            this.listBox_Auth = new System.Windows.Forms.ListBox();
            this.button_Unload = new System.Windows.Forms.Button();
            this.button_Dev = new System.Windows.Forms.Button();
            this.button_Menu = new System.Windows.Forms.Button();
            this.button_Disable = new System.Windows.Forms.Button();
            this.label_Auth = new System.Windows.Forms.Label();
            this.label_DescriptionTitle = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label_Description = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox_Desc.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView_PluginList
            // 
            this.listView_PluginList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView_PluginList.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView_PluginList.FullRowSelect = true;
            this.listView_PluginList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView_PluginList.HideSelection = false;
            this.listView_PluginList.LabelWrap = false;
            this.listView_PluginList.Location = new System.Drawing.Point(9, 10);
            this.listView_PluginList.Margin = new System.Windows.Forms.Padding(2);
            this.listView_PluginList.MultiSelect = false;
            this.listView_PluginList.Name = "listView_PluginList";
            this.listView_PluginList.Size = new System.Drawing.Size(303, 360);
            this.listView_PluginList.TabIndex = 0;
            this.listView_PluginList.UseCompatibleStateImageBehavior = false;
            this.listView_PluginList.View = System.Windows.Forms.View.Details;
            this.listView_PluginList.SelectedIndexChanged += new System.EventHandler(this.listView_PluginList_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "插件名称";
            this.columnHeader1.Width = 168;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "版本";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "作者";
            this.columnHeader3.Width = 68;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.label_MainVersion);
            this.panel1.Controls.Add(this.button_Close);
            this.panel1.Controls.Add(this.linkLabel_MoreApps);
            this.panel1.Controls.Add(this.button_AppDir);
            this.panel1.Controls.Add(this.button_EventList);
            this.panel1.Controls.Add(this.button_Reload);
            this.panel1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(0, 380);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(619, 54);
            this.panel1.TabIndex = 1;
            // 
            // label_MainVersion
            // 
            this.label_MainVersion.AutoSize = true;
            this.label_MainVersion.ForeColor = System.Drawing.Color.DarkGray;
            this.label_MainVersion.Location = new System.Drawing.Point(408, 21);
            this.label_MainVersion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_MainVersion.Name = "label_MainVersion";
            this.label_MainVersion.Size = new System.Drawing.Size(117, 17);
            this.label_MainVersion.TabIndex = 5;
            this.label_MainVersion.Text = "1.0.1.0（开发模式）";
            // 
            // button_Close
            // 
            this.button_Close.Location = new System.Drawing.Point(527, 12);
            this.button_Close.Margin = new System.Windows.Forms.Padding(2);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new System.Drawing.Size(77, 34);
            this.button_Close.TabIndex = 4;
            this.button_Close.Text = "关闭";
            this.button_Close.UseVisualStyleBackColor = true;
            this.button_Close.Click += new System.EventHandler(this.button_Close_Click);
            // 
            // linkLabel_MoreApps
            // 
            this.linkLabel_MoreApps.AutoSize = true;
            this.linkLabel_MoreApps.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel_MoreApps.Location = new System.Drawing.Point(270, 21);
            this.linkLabel_MoreApps.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.linkLabel_MoreApps.Name = "linkLabel_MoreApps";
            this.linkLabel_MoreApps.Size = new System.Drawing.Size(86, 17);
            this.linkLabel_MoreApps.TabIndex = 3;
            this.linkLabel_MoreApps.TabStop = true;
            this.linkLabel_MoreApps.Text = "获取更多应用..";
            this.linkLabel_MoreApps.UseMnemonic = false;
            this.linkLabel_MoreApps.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // button_AppDir
            // 
            this.button_AppDir.Location = new System.Drawing.Point(181, 12);
            this.button_AppDir.Margin = new System.Windows.Forms.Padding(2);
            this.button_AppDir.Name = "button_AppDir";
            this.button_AppDir.Size = new System.Drawing.Size(77, 34);
            this.button_AppDir.TabIndex = 2;
            this.button_AppDir.Text = "应用目录";
            this.button_AppDir.UseVisualStyleBackColor = true;
            this.button_AppDir.Click += new System.EventHandler(this.button_AppDir_Click);
            // 
            // button_EventList
            // 
            this.button_EventList.Location = new System.Drawing.Point(96, 12);
            this.button_EventList.Margin = new System.Windows.Forms.Padding(2);
            this.button_EventList.Name = "button_EventList";
            this.button_EventList.Size = new System.Drawing.Size(77, 34);
            this.button_EventList.TabIndex = 1;
            this.button_EventList.Text = "事件列表";
            this.button_EventList.UseVisualStyleBackColor = true;
            // 
            // button_Reload
            // 
            this.button_Reload.Location = new System.Drawing.Point(11, 12);
            this.button_Reload.Margin = new System.Windows.Forms.Padding(2);
            this.button_Reload.Name = "button_Reload";
            this.button_Reload.Size = new System.Drawing.Size(77, 34);
            this.button_Reload.TabIndex = 0;
            this.button_Reload.Text = "重载插件";
            this.button_Reload.UseVisualStyleBackColor = true;
            this.button_Reload.Click += new System.EventHandler(this.button_Reload_Click);
            // 
            // groupBox_Desc
            // 
            this.groupBox_Desc.Controls.Add(this.panel3);
            this.groupBox_Desc.Controls.Add(this.panel2);
            this.groupBox_Desc.Controls.Add(this.listBox_Auth);
            this.groupBox_Desc.Controls.Add(this.button_Unload);
            this.groupBox_Desc.Controls.Add(this.button_Dev);
            this.groupBox_Desc.Controls.Add(this.button_Menu);
            this.groupBox_Desc.Controls.Add(this.button_Disable);
            this.groupBox_Desc.Controls.Add(this.label_Auth);
            this.groupBox_Desc.Controls.Add(this.label_DescriptionTitle);
            this.groupBox_Desc.Controls.Add(this.groupBox1);
            this.groupBox_Desc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox_Desc.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox_Desc.Location = new System.Drawing.Point(316, 4);
            this.groupBox_Desc.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox_Desc.Name = "groupBox_Desc";
            this.groupBox_Desc.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox_Desc.Size = new System.Drawing.Size(288, 366);
            this.groupBox_Desc.TabIndex = 2;
            this.groupBox_Desc.TabStop = false;
            this.groupBox_Desc.Text = "应用信息";
            this.groupBox_Desc.Visible = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label_VersionTitle);
            this.panel3.Controls.Add(this.label_Version);
            this.panel3.Location = new System.Drawing.Point(4, 40);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(277, 24);
            this.panel3.TabIndex = 4;
            // 
            // label_VersionTitle
            // 
            this.label_VersionTitle.AutoSize = true;
            this.label_VersionTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.label_VersionTitle.Location = new System.Drawing.Point(0, 0);
            this.label_VersionTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_VersionTitle.Name = "label_VersionTitle";
            this.label_VersionTitle.Size = new System.Drawing.Size(32, 17);
            this.label_VersionTitle.TabIndex = 1;
            this.label_VersionTitle.Text = "版本";
            // 
            // label_Version
            // 
            this.label_Version.AutoSize = true;
            this.label_Version.Dock = System.Windows.Forms.DockStyle.Right;
            this.label_Version.Location = new System.Drawing.Point(242, 0);
            this.label_Version.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Version.Name = "label_Version";
            this.label_Version.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label_Version.Size = new System.Drawing.Size(35, 17);
            this.label_Version.TabIndex = 4;
            this.label_Version.Text = "1.0.1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label_AuthorTitle);
            this.panel2.Controls.Add(this.label_Author);
            this.panel2.Location = new System.Drawing.Point(4, 19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(281, 19);
            this.panel2.TabIndex = 4;
            // 
            // label_AuthorTitle
            // 
            this.label_AuthorTitle.AutoSize = true;
            this.label_AuthorTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.label_AuthorTitle.Location = new System.Drawing.Point(0, 0);
            this.label_AuthorTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_AuthorTitle.Name = "label_AuthorTitle";
            this.label_AuthorTitle.Size = new System.Drawing.Size(32, 17);
            this.label_AuthorTitle.TabIndex = 0;
            this.label_AuthorTitle.Text = "作者";
            // 
            // label_Author
            // 
            this.label_Author.AutoSize = true;
            this.label_Author.Dock = System.Windows.Forms.DockStyle.Right;
            this.label_Author.Location = new System.Drawing.Point(237, 0);
            this.label_Author.Margin = new System.Windows.Forms.Padding(0);
            this.label_Author.Name = "label_Author";
            this.label_Author.Size = new System.Drawing.Size(44, 17);
            this.label_Author.TabIndex = 3;
            this.label_Author.Text = "落花茗";
            this.label_Author.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // listBox_Auth
            // 
            this.listBox_Auth.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBox_Auth.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.listBox_Auth.FormattingEnabled = true;
            this.listBox_Auth.ItemHeight = 19;
            this.listBox_Auth.Location = new System.Drawing.Point(4, 199);
            this.listBox_Auth.Margin = new System.Windows.Forms.Padding(2);
            this.listBox_Auth.Name = "listBox_Auth";
            this.listBox_Auth.ScrollAlwaysVisible = true;
            this.listBox_Auth.Size = new System.Drawing.Size(280, 95);
            this.listBox_Auth.TabIndex = 6;
            // 
            // button_Unload
            // 
            this.button_Unload.Location = new System.Drawing.Point(219, 327);
            this.button_Unload.Margin = new System.Windows.Forms.Padding(2);
            this.button_Unload.Name = "button_Unload";
            this.button_Unload.Size = new System.Drawing.Size(58, 32);
            this.button_Unload.TabIndex = 10;
            this.button_Unload.Text = "卸载";
            this.button_Unload.UseVisualStyleBackColor = true;
            // 
            // button_Dev
            // 
            this.button_Dev.Location = new System.Drawing.Point(147, 327);
            this.button_Dev.Margin = new System.Windows.Forms.Padding(2);
            this.button_Dev.Name = "button_Dev";
            this.button_Dev.Size = new System.Drawing.Size(58, 32);
            this.button_Dev.TabIndex = 9;
            this.button_Dev.Text = "开发";
            this.button_Dev.UseVisualStyleBackColor = true;
            // 
            // button_Menu
            // 
            this.button_Menu.Location = new System.Drawing.Point(77, 327);
            this.button_Menu.Margin = new System.Windows.Forms.Padding(2);
            this.button_Menu.Name = "button_Menu";
            this.button_Menu.Size = new System.Drawing.Size(58, 32);
            this.button_Menu.TabIndex = 8;
            this.button_Menu.Text = "菜单";
            this.button_Menu.UseVisualStyleBackColor = true;
            this.button_Menu.Click += new System.EventHandler(this.button_Menu_Click);
            // 
            // button_Disable
            // 
            this.button_Disable.Location = new System.Drawing.Point(7, 327);
            this.button_Disable.Margin = new System.Windows.Forms.Padding(2);
            this.button_Disable.Name = "button_Disable";
            this.button_Disable.Size = new System.Drawing.Size(58, 32);
            this.button_Disable.TabIndex = 7;
            this.button_Disable.Text = "停用";
            this.button_Disable.UseVisualStyleBackColor = true;
            this.button_Disable.Click += new System.EventHandler(this.button_Disable_Click);
            // 
            // label_Auth
            // 
            this.label_Auth.AutoSize = true;
            this.label_Auth.Location = new System.Drawing.Point(5, 178);
            this.label_Auth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Auth.Name = "label_Auth";
            this.label_Auth.Size = new System.Drawing.Size(128, 17);
            this.label_Auth.TabIndex = 5;
            this.label_Auth.Text = "需要以下权限（xx个）";
            // 
            // label_DescriptionTitle
            // 
            this.label_DescriptionTitle.AutoSize = true;
            this.label_DescriptionTitle.Location = new System.Drawing.Point(4, 62);
            this.label_DescriptionTitle.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_DescriptionTitle.Name = "label_DescriptionTitle";
            this.label_DescriptionTitle.Size = new System.Drawing.Size(32, 17);
            this.label_DescriptionTitle.TabIndex = 2;
            this.label_DescriptionTitle.Text = "说明";
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.label_Description);
            this.groupBox1.Location = new System.Drawing.Point(1, 74);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(287, 99);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            // 
            // label_Description
            // 
            this.label_Description.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_Description.Location = new System.Drawing.Point(3, 19);
            this.label_Description.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_Description.Name = "label_Description";
            this.label_Description.Size = new System.Drawing.Size(281, 77);
            this.label_Description.TabIndex = 0;
            this.label_Description.Text = "说明1.0";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(318, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(286, 366);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(91, 178);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "从左侧选择一个应用";
            // 
            // PluginManageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(613, 436);
            this.Controls.Add(this.groupBox_Desc);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.listView_PluginList);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "PluginManageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "应用";
            this.Load += new System.EventHandler(this.PluginManageForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox_Desc.ResumeLayout(false);
            this.groupBox_Desc.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView_PluginList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label_MainVersion;
        private System.Windows.Forms.Button button_Close;
        private System.Windows.Forms.LinkLabel linkLabel_MoreApps;
        private System.Windows.Forms.Button button_AppDir;
        private System.Windows.Forms.Button button_EventList;
        private System.Windows.Forms.Button button_Reload;
        private System.Windows.Forms.GroupBox groupBox_Desc;
        private System.Windows.Forms.Button button_Unload;
        private System.Windows.Forms.Button button_Dev;
        private System.Windows.Forms.Button button_Menu;
        private System.Windows.Forms.Button button_Disable;
        private System.Windows.Forms.Label label_Auth;
        private System.Windows.Forms.Label label_Version;
        private System.Windows.Forms.Label label_Author;
        private System.Windows.Forms.Label label_DescriptionTitle;
        private System.Windows.Forms.Label label_VersionTitle;
        private System.Windows.Forms.Label label_AuthorTitle;
        private System.Windows.Forms.Label label_Description;
        private System.Windows.Forms.ListBox listBox_Auth;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
    }
}