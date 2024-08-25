using System;

namespace regexTester
{
  partial class FormMatchesViewer
  {
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Text = "Regex Tester";

      panelCustom = new System.Windows.Forms.Panel()
      {
        Dock = System.Windows.Forms.DockStyle.Top,
        BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
      };
      panelTop = new System.Windows.Forms.Panel()
      {
        Dock = System.Windows.Forms.DockStyle.Top,
        BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
      };
      panelText = new System.Windows.Forms.Panel()
      {
        Dock = System.Windows.Forms.DockStyle.Fill,
        BorderStyle = System.Windows.Forms.BorderStyle.None
      };
      edText = new System.Windows.Forms.RichTextBox()
      {
        Dock = System.Windows.Forms.DockStyle.Fill,
        HideSelection = false
      };
      panelMatchesLog = new System.Windows.Forms.Panel()
      {
        Dock = System.Windows.Forms.DockStyle.Right,
        BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
      };
      edMatchesLog = new System.Windows.Forms.TextBox()
      {
        Multiline = true,
        Dock = System.Windows.Forms.DockStyle.Fill,
        BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
        ScrollBars = System.Windows.Forms.ScrollBars.Vertical,
        HideSelection = false
      };
      edTemplate = new System.Windows.Forms.ComboBox()
      {
        DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown,
        FlatStyle = System.Windows.Forms.FlatStyle.Flat,
        Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Left
      };
      buttonTest = new System.Windows.Forms.Button()
      {
        Text = "Test",
        Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right
      };
      btnMatchPrev = new System.Windows.Forms.Button()
      {
        Text = "<"
      };
      btnMatchNext = new System.Windows.Forms.Button()
      {
        Text = ">"
      };
      btnTextClearFormat = new System.Windows.Forms.Button()
      {
        Text = "Clear colors"
      };
      edTextState = new System.Windows.Forms.TextBox()
      {
        ReadOnly = true,
        Dock = System.Windows.Forms.DockStyle.Bottom
      };
      var btnCopyToClipBrd = new System.Windows.Forms.Button()
      {
        Text = "Copy Templ",
        Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right
      };
      cbSingleLine = new System.Windows.Forms.CheckBox()
      {
        Text = "Single line",
        Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right
      };
      cbMultiLine = new System.Windows.Forms.CheckBox()
      {
        Text = "Multi line",
        Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right
      };
      buttonOpenFile = new System.Windows.Forms.Button()
      {
        Text = "File...",
        Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right
      };
      openFileDialog = new System.Windows.Forms.OpenFileDialog()
      {
        CheckFileExists = true,
        RestoreDirectory = true
      };
      btnTextToMatchLogSync = new System.Windows.Forms.Button()
      {
        Text = "sync ->",
        Location = new System.Drawing.Point(2, 2),
      };
      var btnMatchLogToTextSync = new System.Windows.Forms.Button()
      {
        Text = "<- sync",
        Location = new System.Drawing.Point(btnTextToMatchLogSync.Right + 2, 2),
      };
      var panSync = new System.Windows.Forms.Panel()
      {
        Text = "",
        Dock = System.Windows.Forms.DockStyle.Top,
        Height = btnMatchLogToTextSync.Height + 4
      };
      tabControlMacthes = new System.Windows.Forms.TabControl() 
      {
        Dock = System.Windows.Forms.DockStyle.Fill
      };
      pgMatchesText = new System.Windows.Forms.TabPage()
      {
        Text = "Text"
      };
      pgMatchesTable = new System.Windows.Forms.TabPage()
      {
        Text = "Table"
      };
      bsMatchGroups = new System.Windows.Forms.BindingSource()
      {
        AllowNew = false,
      };
      gridMatches = new System.Windows.Forms.DataGridView()
      {
        DataSource = bsMatchGroups,
        ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText,
        Dock = System.Windows.Forms.DockStyle.Fill,
        AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None,
        ReadOnly = true,
      };
      gridMatches.CellDoubleClick += gridMatchesCellDoubleClick; 
      navigatorMatchGroups = new System.Windows.Forms.BindingNavigator()
      {
        BindingSource = bsMatchGroups,
        CanOverflow = true,
        Dock = System.Windows.Forms.DockStyle.Bottom,
      };
      navigatorMatchGroups.AddStandardItems();
      navigatorMatchGroups.Items.Remove(navigatorMatchGroups.DeleteItem);
      navigatorMatchGroups.Items.Remove(navigatorMatchGroups.AddNewItem);
      menuStripMatchGroups = new System.Windows.Forms.ContextMenuStrip()
      {
      };
      gridMatches.ContextMenuStrip = this.menuStripMatchGroups;
      this.CopyWOnewlines = new System.Windows.Forms.ToolStripMenuItem();
      this.CopyWOnewlines.Size = new System.Drawing.Size(220, 22);
      this.CopyWOnewlines.Text = @"Copy without \r\n";
      this.CopyWOnewlines.Click += new System.EventHandler(this.CopyWOnewlines_Click);
      this.menuStripMatchGroups.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CopyWOnewlines});

      this.Controls.Add(panelCustom);
      this.Controls.Add(panelTop);
      //   this.Controls.Add(panelText);
      //   this.Controls.Add(panelMatchesLog);

      var panClient = new System.Windows.Forms.SplitContainer
      {
        Dock = System.Windows.Forms.DockStyle.Fill,
        SplitterWidth = 8,
      };
      this.Controls.Add(panClient);
      panClient.SplitterDistance = (int)(panClient.ClientSize.Width * 0.7);
      panelText.Dock = System.Windows.Forms.DockStyle.Fill;
      panClient.Panel1.Controls.Add(panelText);
      panelMatchesLog.Dock = System.Windows.Forms.DockStyle.Fill;
      panClient.Panel2.Controls.Add(panelMatchesLog);

      panelCustom.BringToFront();
      panelTop.BringToFront();
      panClient.BringToFront();
      panelMatchesLog.BringToFront();
      panelText.BringToFront();

      panelCustom.Height = edTemplate.Height + 4;
      panelTop.Height = btnMatchPrev.Height + 4;
      panelMatchesLog.Width = (int)(panelMatchesLog.Width * 1.5);

      panelText.Controls.Add(edText);
      panelText.Controls.Add(edTextState);
      edTextState.BringToFront();
      edText.BringToFront();

      panelMatchesLog.Controls.Add(panSync);
      panelMatchesLog.Controls.Add(tabControlMacthes);
      panSync.BringToFront();
      tabControlMacthes.BringToFront();

      tabControlMacthes.TabPages.Add(pgMatchesText);
      tabControlMacthes.TabPages.Add(pgMatchesTable);

      pgMatchesText.Controls.Add(edMatchesLog);

      panSync.Controls.Add(btnMatchLogToTextSync);
      panSync.Controls.Add(this.btnTextToMatchLogSync);
      btnMatchLogToTextSync.BringToFront();
      btnTextToMatchLogSync.BringToFront();
      btnTextToMatchLogSync.Click += btnTextToMatchLogSync_Click;
      btnMatchLogToTextSync.Click += btnMatchLogToTextSync_Click;

      pgMatchesTable.Controls.Add(navigatorMatchGroups);
      pgMatchesTable.Controls.Add(gridMatches);
      navigatorMatchGroups.BringToFront();
      gridMatches.BringToFront();

      this.edText.SelectionChanged += this.edText_SelectionChanged;

      buttonOpenFile.Width = (int)(buttonOpenFile.Width * 0.7);
      buttonOpenFile.Location = new System.Drawing.Point(panelCustom.ClientSize.Width - buttonOpenFile.Width - 2, 2);
      panelCustom.Controls.Add(buttonOpenFile);
      buttonOpenFile.Click += buttonOpenFile_Click;

      buttonTest.Location = new System.Drawing.Point(buttonOpenFile.Left - buttonTest.Width - 2, 2);
      buttonTest.Font = new System.Drawing.Font(buttonTest.Font, System.Drawing.FontStyle.Bold);
      panelCustom.Controls.Add(buttonTest);
      buttonTest.Click += buttonTest_Click;

      cbMultiLine.Width = (int)(cbMultiLine.Width * 0.8);
      cbMultiLine.Location = new System.Drawing.Point(buttonTest.Left - cbMultiLine.Width - 2, 2);
      panelCustom.Controls.Add(cbMultiLine);

      cbSingleLine.Width = (int)(cbSingleLine.Width * 0.8);
      cbSingleLine.Location = new System.Drawing.Point(cbMultiLine.Left - cbSingleLine.Width - 2, 2);
      panelCustom.Controls.Add(cbSingleLine);

      btnCopyToClipBrd.Width = (int)(btnCopyToClipBrd.Width * 1.3);
      btnCopyToClipBrd.Location = new System.Drawing.Point(cbSingleLine.Left - btnCopyToClipBrd.Width - 2, 2);
      panelCustom.Controls.Add(btnCopyToClipBrd);
      btnCopyToClipBrd.Click += btnCopyToClipBrdClick;

      edTemplate.Location = new System.Drawing.Point(2, 2);
      edTemplate.Width = btnCopyToClipBrd.Left - edTemplate.Left;
      panelCustom.Controls.Add(edTemplate);

      btnMatchPrev.Location = new System.Drawing.Point(4, (panelTop.ClientSize.Height - btnMatchPrev.Height) / 2);
      panelTop.Controls.Add(btnMatchPrev);
      btnMatchPrev.Click += btnMatchPrev_Click;

      btnMatchNext.Location = new System.Drawing.Point(btnMatchPrev.Right + 4, (panelTop.ClientSize.Height - btnMatchNext.Height) / 2);
      panelTop.Controls.Add(btnMatchNext);
      btnMatchNext.Click += btnMatchNext_Click;

      btnTextClearFormat.Location = new System.Drawing.Point(btnMatchNext.Right + 4, (panelTop.ClientSize.Height - btnTextClearFormat.Height) / 2);
      btnTextClearFormat.Width = (int)(btnTextClearFormat.Width * 1.3);
      panelTop.Controls.Add(btnTextClearFormat);
      btnTextClearFormat.Click += btnTextClearFormat_Click;

      this.components.Add(openFileDialog);

      this.Load += Form1_Load;

    }

    public System.Windows.Forms.Panel panelCustom;
    private System.Windows.Forms.Panel panelTop;
    private System.Windows.Forms.Panel panelText;
    private System.Windows.Forms.RichTextBox edText;
    private System.Windows.Forms.Panel panelMatchesLog;
    private System.Windows.Forms.TextBox edMatchesLog;
    private System.Windows.Forms.Button btnTextToMatchLogSync;
    private System.Windows.Forms.ComboBox edTemplate;
    private System.Windows.Forms.Button buttonTest;
    private System.Windows.Forms.Button btnMatchPrev;
    private System.Windows.Forms.Button btnMatchNext;
    private System.Windows.Forms.Button btnTextClearFormat;
    private System.Windows.Forms.TextBox edTextState;
    private System.Windows.Forms.CheckBox cbSingleLine;
    private System.Windows.Forms.CheckBox cbMultiLine;
    private System.Windows.Forms.Button buttonOpenFile;
    private System.Windows.Forms.OpenFileDialog openFileDialog;
    private System.Windows.Forms.TabControl tabControlMacthes;
    private System.Windows.Forms.DataGridView gridMatches;
    private System.Windows.Forms.TabPage pgMatchesText;
    private System.Windows.Forms.TabPage pgMatchesTable;
    private System.Windows.Forms.BindingNavigator navigatorMatchGroups;
    private System.Windows.Forms.BindingSource bsMatchGroups;
    private System.Windows.Forms.ContextMenuStrip menuStripMatchGroups;
     private System.Windows.Forms.ToolStripMenuItem CopyWOnewlines;


    #endregion
  }
}

