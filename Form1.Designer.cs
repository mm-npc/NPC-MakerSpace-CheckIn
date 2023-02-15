namespace NPC_MakerSpace_CheckIn
{
  partial class Form1
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
      this.lblID = new System.Windows.Forms.Label();
      this.tbID = new System.Windows.Forms.TextBox();
      this.lblReason = new System.Windows.Forms.Label();
      this.cbReason = new System.Windows.Forms.ComboBox();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.cbInOut = new System.Windows.Forms.CheckBox();
      this.lblEstHours = new System.Windows.Forms.Label();
      this.tbEstHours = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.gbSearch = new System.Windows.Forms.GroupBox();
      this.lbClassList = new System.Windows.Forms.ListBox();
      this.lblSearch = new System.Windows.Forms.Label();
      this.tbSearch = new System.Windows.Forms.TextBox();
      this.btnSearch = new System.Windows.Forms.Button();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.copyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.onScreenKeyboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.statusBar = new System.Windows.Forms.StatusBar();
      this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
      this.btnComplete = new System.Windows.Forms.Button();
      this.flpCheckOutList = new System.Windows.Forms.FlowLayoutPanel();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.gbSearch.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
      this.SuspendLayout();
      // 
      // lblID
      // 
      this.lblID.Location = new System.Drawing.Point(73, 35);
      this.lblID.Name = "lblID";
      this.lblID.Size = new System.Drawing.Size(144, 15);
      this.lblID.TabIndex = 5;
      this.lblID.Text = "ID:";
      // 
      // tbID
      // 
      this.tbID.Location = new System.Drawing.Point(73, 53);
      this.tbID.Name = "tbID";
      this.tbID.Size = new System.Drawing.Size(144, 20);
      this.tbID.TabIndex = 0;
      // 
      // lblReason
      // 
      this.lblReason.Location = new System.Drawing.Point(223, 34);
      this.lblReason.Name = "lblReason";
      this.lblReason.Size = new System.Drawing.Size(188, 15);
      this.lblReason.TabIndex = 6;
      this.lblReason.Text = "Reason:";
      // 
      // cbReason
      // 
      this.cbReason.FormattingEnabled = true;
      this.cbReason.Location = new System.Drawing.Point(223, 52);
      this.cbReason.Name = "cbReason";
      this.cbReason.Size = new System.Drawing.Size(188, 21);
      this.cbReason.TabIndex = 1;
      this.cbReason.SelectedIndexChanged += new System.EventHandler(this.cbReason_SelectedIndexChanged);
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
      this.pictureBox1.Location = new System.Drawing.Point(12, 38);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(50, 50);
      this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
      this.pictureBox1.TabIndex = 10;
      this.pictureBox1.TabStop = false;
      // 
      // cbInOut
      // 
      this.cbInOut.Appearance = System.Windows.Forms.Appearance.Button;
      this.cbInOut.Location = new System.Drawing.Point(481, 51);
      this.cbInOut.Name = "cbInOut";
      this.cbInOut.Size = new System.Drawing.Size(49, 21);
      this.cbInOut.TabIndex = 3;
      this.cbInOut.Text = "In/Out";
      this.cbInOut.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.cbInOut.UseVisualStyleBackColor = true;
      this.cbInOut.CheckedChanged += new System.EventHandler(this.cbInOut_CheckedChanged);
      // 
      // lblEstHours
      // 
      this.lblEstHours.Location = new System.Drawing.Point(417, 34);
      this.lblEstHours.Name = "lblEstHours";
      this.lblEstHours.Size = new System.Drawing.Size(58, 15);
      this.lblEstHours.TabIndex = 13;
      this.lblEstHours.Text = "Est Hours:";
      // 
      // tbEstHours
      // 
      this.tbEstHours.Location = new System.Drawing.Point(417, 52);
      this.tbEstHours.Name = "tbEstHours";
      this.tbEstHours.Size = new System.Drawing.Size(58, 20);
      this.tbEstHours.TabIndex = 2;
      // 
      // label1
      // 
      this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.label1.Location = new System.Drawing.Point(12, 89);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(518, 2);
      this.label1.TabIndex = 14;
      // 
      // gbSearch
      // 
      this.gbSearch.Controls.Add(this.lbClassList);
      this.gbSearch.Controls.Add(this.lblSearch);
      this.gbSearch.Controls.Add(this.tbSearch);
      this.gbSearch.Controls.Add(this.btnSearch);
      this.gbSearch.Enabled = false;
      this.gbSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.gbSearch.Location = new System.Drawing.Point(12, 104);
      this.gbSearch.Name = "gbSearch";
      this.gbSearch.Padding = new System.Windows.Forms.Padding(0);
      this.gbSearch.Size = new System.Drawing.Size(518, 185);
      this.gbSearch.TabIndex = 15;
      this.gbSearch.TabStop = false;
      // 
      // lbClassList
      // 
      this.lbClassList.Font = new System.Drawing.Font("InputMono", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lbClassList.FormattingEnabled = true;
      this.lbClassList.Location = new System.Drawing.Point(30, 70);
      this.lbClassList.Name = "lbClassList";
      this.lbClassList.Size = new System.Drawing.Size(457, 95);
      this.lbClassList.TabIndex = 6;
      // 
      // lblSearch
      // 
      this.lblSearch.Location = new System.Drawing.Point(30, 26);
      this.lblSearch.Name = "lblSearch";
      this.lblSearch.Size = new System.Drawing.Size(144, 15);
      this.lblSearch.TabIndex = 6;
      this.lblSearch.Text = "Search:";
      // 
      // tbSearch
      // 
      this.tbSearch.Location = new System.Drawing.Point(30, 44);
      this.tbSearch.Name = "tbSearch";
      this.tbSearch.Size = new System.Drawing.Size(379, 20);
      this.tbSearch.TabIndex = 4;
      this.tbSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyUp);
      // 
      // btnSearch
      // 
      this.btnSearch.Location = new System.Drawing.Point(415, 44);
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new System.Drawing.Size(72, 20);
      this.btnSearch.TabIndex = 5;
      this.btnSearch.Text = "Search";
      this.btnSearch.UseVisualStyleBackColor = true;
      this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.fileToolStripMenuItem, this.editToolStripMenuItem, this.helpToolStripMenuItem });
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(927, 24);
      this.menuStrip1.TabIndex = 16;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this.exitToolStripMenuItem });
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
      this.fileToolStripMenuItem.Text = "File";
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(93, 22);
      this.exitToolStripMenuItem.Text = "Exit";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // editToolStripMenuItem
      // 
      this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this.copyToolStripMenuItem, this.copyToolStripMenuItem1, this.pasteToolStripMenuItem, this.toolStripSeparator1, this.settingsToolStripMenuItem, this.onScreenKeyboardToolStripMenuItem });
      this.editToolStripMenuItem.Name = "editToolStripMenuItem";
      this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
      this.editToolStripMenuItem.Text = "Edit";
      // 
      // copyToolStripMenuItem
      // 
      this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
      this.copyToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
      this.copyToolStripMenuItem.Text = "Cut";
      // 
      // copyToolStripMenuItem1
      // 
      this.copyToolStripMenuItem1.Name = "copyToolStripMenuItem1";
      this.copyToolStripMenuItem1.Size = new System.Drawing.Size(183, 22);
      this.copyToolStripMenuItem1.Text = "Copy";
      // 
      // pasteToolStripMenuItem
      // 
      this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
      this.pasteToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
      this.pasteToolStripMenuItem.Text = "Paste";
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(180, 6);
      // 
      // settingsToolStripMenuItem
      // 
      this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
      this.settingsToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
      this.settingsToolStripMenuItem.Text = "Settings";
      // 
      // onScreenKeyboardToolStripMenuItem
      // 
      this.onScreenKeyboardToolStripMenuItem.Name = "onScreenKeyboardToolStripMenuItem";
      this.onScreenKeyboardToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
      this.onScreenKeyboardToolStripMenuItem.Text = "On-Screen Keyboard";
      this.onScreenKeyboardToolStripMenuItem.Click += new System.EventHandler(this.onScreenKeyboardToolStripMenuItem_Click);
      // 
      // helpToolStripMenuItem
      // 
      this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { this.aboutToolStripMenuItem });
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
      this.helpToolStripMenuItem.Text = "Help";
      // 
      // aboutToolStripMenuItem
      // 
      this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
      this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
      this.aboutToolStripMenuItem.Text = "About";
      // 
      // statusBar
      // 
      this.statusBar.Location = new System.Drawing.Point(0, 349);
      this.statusBar.Name = "statusBar";
      this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] { this.statusBarPanel1 });
      this.statusBar.Size = new System.Drawing.Size(927, 22);
      this.statusBar.TabIndex = 17;
      // 
      // statusBarPanel1
      // 
      this.statusBarPanel1.Name = "statusBarPanel1";
      this.statusBarPanel1.Text = "statusBarPanel1";
      // 
      // btnComplete
      // 
      this.btnComplete.Location = new System.Drawing.Point(12, 295);
      this.btnComplete.Name = "btnComplete";
      this.btnComplete.Size = new System.Drawing.Size(518, 37);
      this.btnComplete.TabIndex = 18;
      this.btnComplete.Text = "Complete";
      this.btnComplete.UseVisualStyleBackColor = true;
      this.btnComplete.Click += new System.EventHandler(this.btnComplete_Click);
      // 
      // flpCheckOutList
      // 
      this.flpCheckOutList.AutoScroll = true;
      this.flpCheckOutList.Location = new System.Drawing.Point(546, 34);
      this.flpCheckOutList.Name = "flpCheckOutList";
      this.flpCheckOutList.Size = new System.Drawing.Size(369, 298);
      this.flpCheckOutList.TabIndex = 19;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(927, 371);
      this.Controls.Add(this.flpCheckOutList);
      this.Controls.Add(this.btnComplete);
      this.Controls.Add(this.statusBar);
      this.Controls.Add(this.gbSearch);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.lblEstHours);
      this.Controls.Add(this.tbEstHours);
      this.Controls.Add(this.cbInOut);
      this.Controls.Add(this.pictureBox1);
      this.Controls.Add(this.cbReason);
      this.Controls.Add(this.lblReason);
      this.Controls.Add(this.lblID);
      this.Controls.Add(this.tbID);
      this.Controls.Add(this.menuStrip1);
      this.Name = "Form1";
      this.Text = "NPC MakerSpace Check-In";
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.gbSearch.ResumeLayout(false);
      this.gbSearch.PerformLayout();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private System.Windows.Forms.FlowLayoutPanel flpCheckOutList;

    private System.Windows.Forms.ToolStripMenuItem onScreenKeyboardToolStripMenuItem;

    private System.Windows.Forms.StatusBarPanel statusBarPanel1;

    private System.Windows.Forms.Button btnComplete;

    private System.Windows.Forms.StatusBar statusBar;

    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;

    private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;

    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;

    private System.Windows.Forms.GroupBox gbSearch;

    private System.Windows.Forms.Label label1;

    private System.Windows.Forms.Label lblEstHours;
    private System.Windows.Forms.TextBox tbEstHours;

    private System.Windows.Forms.CheckBox cbInOut;

    private System.Windows.Forms.PictureBox pictureBox1;

    private System.Windows.Forms.ComboBox cbReason;

    private System.Windows.Forms.Label lblID;
    private System.Windows.Forms.TextBox tbID;
    private System.Windows.Forms.Label lblReason;

    private System.Windows.Forms.TextBox tbSearch;
    private System.Windows.Forms.Label lblSearch;
    private System.Windows.Forms.ListBox lbClassList;

    private System.Windows.Forms.Button btnSearch;

    #endregion
  }
}

