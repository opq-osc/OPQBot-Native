﻿namespace Launcher.Forms
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBox_Auth = new System.Windows.Forms.ListBox();
            this.button_Unload = new System.Windows.Forms.Button();
            this.panel_Desp = new System.Windows.Forms.Panel();
            this.label_Description = new System.Windows.Forms.Label();
            this.button_Dev = new System.Windows.Forms.Button();
            this.button_Menu = new System.Windows.Forms.Button();
            this.button_Disable = new System.Windows.Forms.Button();
            this.label_Auth = new System.Windows.Forms.Label();
            this.label_Version = new System.Windows.Forms.Label();
            this.label_Author = new System.Windows.Forms.Label();
            this.label_DescriptionTitle = new System.Windows.Forms.Label();
            this.label_VersionTitle = new System.Windows.Forms.Label();
            this.label_AuthorTitle = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel_Desp.SuspendLayout();
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
            this.listView_PluginList.Location = new System.Drawing.Point(12, 12);
            this.listView_PluginList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listView_PluginList.MultiSelect = false;
            this.listView_PluginList.Name = "listView_PluginList";
            this.listView_PluginList.Size = new System.Drawing.Size(403, 449);
            this.listView_PluginList.TabIndex = 0;
            this.listView_PluginList.UseCompatibleStateImageBehavior = false;
            this.listView_PluginList.View = System.Windows.Forms.View.Details;
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
            this.panel1.Location = new System.Drawing.Point(0, 475);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(825, 68);
            this.panel1.TabIndex = 1;
            // 
            // label_MainVersion
            // 
            this.label_MainVersion.AutoSize = true;
            this.label_MainVersion.ForeColor = System.Drawing.Color.DarkGray;
            this.label_MainVersion.Location = new System.Drawing.Point(576, 26);
            this.label_MainVersion.Name = "label_MainVersion";
            this.label_MainVersion.Size = new System.Drawing.Size(114, 20);
            this.label_MainVersion.TabIndex = 5;
            this.label_MainVersion.Text = "1.0.1(开发模式)";
            // 
            // button_Close
            // 
            this.button_Close.Location = new System.Drawing.Point(703, 15);
            this.button_Close.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new System.Drawing.Size(103, 42);
            this.button_Close.TabIndex = 4;
            this.button_Close.Text = "关闭";
            this.button_Close.UseVisualStyleBackColor = true;
            this.button_Close.Click += new System.EventHandler(this.button_Close_Click);
            // 
            // linkLabel_MoreApps
            // 
            this.linkLabel_MoreApps.AutoSize = true;
            this.linkLabel_MoreApps.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabel_MoreApps.Location = new System.Drawing.Point(360, 26);
            this.linkLabel_MoreApps.Name = "linkLabel_MoreApps";
            this.linkLabel_MoreApps.Size = new System.Drawing.Size(107, 20);
            this.linkLabel_MoreApps.TabIndex = 3;
            this.linkLabel_MoreApps.TabStop = true;
            this.linkLabel_MoreApps.Text = "获取更多应用..";
            this.linkLabel_MoreApps.UseMnemonic = false;
            this.linkLabel_MoreApps.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // button_AppDir
            // 
            this.button_AppDir.Location = new System.Drawing.Point(241, 15);
            this.button_AppDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_AppDir.Name = "button_AppDir";
            this.button_AppDir.Size = new System.Drawing.Size(103, 42);
            this.button_AppDir.TabIndex = 2;
            this.button_AppDir.Text = "应用目录";
            this.button_AppDir.UseVisualStyleBackColor = true;
            // 
            // button_EventList
            // 
            this.button_EventList.Location = new System.Drawing.Point(128, 15);
            this.button_EventList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_EventList.Name = "button_EventList";
            this.button_EventList.Size = new System.Drawing.Size(103, 42);
            this.button_EventList.TabIndex = 1;
            this.button_EventList.Text = "事件列表";
            this.button_EventList.UseVisualStyleBackColor = true;
            // 
            // button_Reload
            // 
            this.button_Reload.Location = new System.Drawing.Point(15, 15);
            this.button_Reload.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Reload.Name = "button_Reload";
            this.button_Reload.Size = new System.Drawing.Size(103, 42);
            this.button_Reload.TabIndex = 0;
            this.button_Reload.Text = "重载插件";
            this.button_Reload.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBox_Auth);
            this.groupBox1.Controls.Add(this.button_Unload);
            this.groupBox1.Controls.Add(this.panel_Desp);
            this.groupBox1.Controls.Add(this.button_Dev);
            this.groupBox1.Controls.Add(this.button_Menu);
            this.groupBox1.Controls.Add(this.button_Disable);
            this.groupBox1.Controls.Add(this.label_Auth);
            this.groupBox1.Controls.Add(this.label_Version);
            this.groupBox1.Controls.Add(this.label_Author);
            this.groupBox1.Controls.Add(this.label_DescriptionTitle);
            this.groupBox1.Controls.Add(this.label_VersionTitle);
            this.groupBox1.Controls.Add(this.label_AuthorTitle);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(421, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(384, 458);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "应用信息";
            // 
            // listBox_Auth
            // 
            this.listBox_Auth.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBox_Auth.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBox_Auth.FormattingEnabled = true;
            this.listBox_Auth.ItemHeight = 20;
            this.listBox_Auth.Location = new System.Drawing.Point(5, 249);
            this.listBox_Auth.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.listBox_Auth.Name = "listBox_Auth";
            this.listBox_Auth.ScrollAlwaysVisible = true;
            this.listBox_Auth.Size = new System.Drawing.Size(373, 140);
            this.listBox_Auth.TabIndex = 6;
            // 
            // button_Unload
            // 
            this.button_Unload.Location = new System.Drawing.Point(292, 409);
            this.button_Unload.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Unload.Name = "button_Unload";
            this.button_Unload.Size = new System.Drawing.Size(77, 40);
            this.button_Unload.TabIndex = 10;
            this.button_Unload.Text = "卸载";
            this.button_Unload.UseVisualStyleBackColor = true;
            // 
            // panel_Desp
            // 
            this.panel_Desp.Controls.Add(this.label_Description);
            this.panel_Desp.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel_Desp.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel_Desp.Location = new System.Drawing.Point(0, 110);
            this.panel_Desp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel_Desp.Name = "panel_Desp";
            this.panel_Desp.Size = new System.Drawing.Size(379, 110);
            this.panel_Desp.TabIndex = 5;
            // 
            // label_Description
            // 
            this.label_Description.AutoSize = true;
            this.label_Description.Location = new System.Drawing.Point(5, 4);
            this.label_Description.Name = "label_Description";
            this.label_Description.Size = new System.Drawing.Size(61, 20);
            this.label_Description.TabIndex = 0;
            this.label_Description.Text = "说明1.0";
            // 
            // button_Dev
            // 
            this.button_Dev.Location = new System.Drawing.Point(196, 409);
            this.button_Dev.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Dev.Name = "button_Dev";
            this.button_Dev.Size = new System.Drawing.Size(77, 40);
            this.button_Dev.TabIndex = 9;
            this.button_Dev.Text = "开发";
            this.button_Dev.UseVisualStyleBackColor = true;
            // 
            // button_Menu
            // 
            this.button_Menu.Location = new System.Drawing.Point(103, 409);
            this.button_Menu.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Menu.Name = "button_Menu";
            this.button_Menu.Size = new System.Drawing.Size(77, 40);
            this.button_Menu.TabIndex = 8;
            this.button_Menu.Text = "菜单";
            this.button_Menu.UseVisualStyleBackColor = true;
            // 
            // button_Disable
            // 
            this.button_Disable.Location = new System.Drawing.Point(9, 409);
            this.button_Disable.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Disable.Name = "button_Disable";
            this.button_Disable.Size = new System.Drawing.Size(77, 40);
            this.button_Disable.TabIndex = 7;
            this.button_Disable.Text = "停用";
            this.button_Disable.UseVisualStyleBackColor = true;
            // 
            // label_Auth
            // 
            this.label_Auth.AutoSize = true;
            this.label_Auth.Location = new System.Drawing.Point(7, 222);
            this.label_Auth.Name = "label_Auth";
            this.label_Auth.Size = new System.Drawing.Size(140, 20);
            this.label_Auth.TabIndex = 5;
            this.label_Auth.Text = "需要以下权限(xx个)";
            // 
            // label_Version
            // 
            this.label_Version.AutoSize = true;
            this.label_Version.Location = new System.Drawing.Point(332, 51);
            this.label_Version.Name = "label_Version";
            this.label_Version.Size = new System.Drawing.Size(44, 20);
            this.label_Version.TabIndex = 4;
            this.label_Version.Text = "1.0.1";
            // 
            // label_Author
            // 
            this.label_Author.AutoSize = true;
            this.label_Author.Location = new System.Drawing.Point(320, 24);
            this.label_Author.Name = "label_Author";
            this.label_Author.Size = new System.Drawing.Size(54, 20);
            this.label_Author.TabIndex = 3;
            this.label_Author.Text = "落花茗";
            // 
            // label_DescriptionTitle
            // 
            this.label_DescriptionTitle.AutoSize = true;
            this.label_DescriptionTitle.Location = new System.Drawing.Point(5, 79);
            this.label_DescriptionTitle.Name = "label_DescriptionTitle";
            this.label_DescriptionTitle.Size = new System.Drawing.Size(39, 20);
            this.label_DescriptionTitle.TabIndex = 2;
            this.label_DescriptionTitle.Text = "说明";
            // 
            // label_VersionTitle
            // 
            this.label_VersionTitle.AutoSize = true;
            this.label_VersionTitle.Location = new System.Drawing.Point(5, 51);
            this.label_VersionTitle.Name = "label_VersionTitle";
            this.label_VersionTitle.Size = new System.Drawing.Size(39, 20);
            this.label_VersionTitle.TabIndex = 1;
            this.label_VersionTitle.Text = "版本";
            // 
            // label_AuthorTitle
            // 
            this.label_AuthorTitle.AutoSize = true;
            this.label_AuthorTitle.Location = new System.Drawing.Point(5, 24);
            this.label_AuthorTitle.Name = "label_AuthorTitle";
            this.label_AuthorTitle.Size = new System.Drawing.Size(39, 20);
            this.label_AuthorTitle.TabIndex = 0;
            this.label_AuthorTitle.Text = "作者";
            // 
            // PluginManageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(817, 545);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.listView_PluginList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "PluginManageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "应用";
            this.Load += new System.EventHandler(this.PluginManageForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel_Desp.ResumeLayout(false);
            this.panel_Desp.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox1;
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
        private System.Windows.Forms.Panel panel_Desp;
        private System.Windows.Forms.Label label_Description;
        private System.Windows.Forms.ListBox listBox_Auth;
    }
}