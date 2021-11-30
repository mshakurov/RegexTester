using System;

namespace regexTester
{
    partial class Form1
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
                Dock = System.Windows.Forms.DockStyle.Fill
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
                ScrollBars = System.Windows.Forms.ScrollBars.Vertical
            };
            edTemplate = new System.Windows.Forms.ComboBox()
            {
                DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                Width = 200,
                Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left
            };
            buttonTest = new System.Windows.Forms.Button()
            {
                Text = "Test"
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
            cbSingleLine = new System.Windows.Forms.CheckBox()
            {
                Text = "Single line"
            };
            buttonOpenFile = new System.Windows.Forms.Button()
            {
                Text = "File..."
            };
            openFileDialog = new System.Windows.Forms.OpenFileDialog()
            {
                CheckFileExists = true,
                RestoreDirectory = true
            };
            buttonMatchesLogSync = new System.Windows.Forms.Button()
            {
                Text = "sync",
                Dock = System.Windows.Forms.DockStyle.Top,
                MaximumSize = new System.Drawing.Size(new System.Windows.Forms.Button().Height * 2, new System.Windows.Forms.Button().Height)
            };

            this.Controls.Add(panelText);
            this.Controls.Add(panelTop);
            this.Controls.Add(panelMatchesLog);
            panelTop.BringToFront();
            panelMatchesLog.BringToFront();
            panelText.BringToFront();
            panelTop.Height /= 2;
            panelMatchesLog.Width = (int)(panelMatchesLog.Width * 1.5);

            panelText.Controls.Add(edText);
            panelText.Controls.Add(edTextState);
            edTextState.BringToFront();
            edText.BringToFront();

            panelMatchesLog.Controls.Add(edMatchesLog);
            panelMatchesLog.Controls.Add(this.buttonMatchesLogSync);
            buttonMatchesLogSync.BringToFront();
            buttonMatchesLogSync.Click += buttonMatchesLogSync_Click;
            edMatchesLog.BringToFront();

            this.edText.SelectionChanged += this.edText_SelectionChanged;

            edTemplate.Location = new System.Drawing.Point(4, (panelTop.ClientSize.Height - buttonTest.Height) / 2);
            edTemplate.Size = new System.Drawing.Size(300, panelTop.ClientSize.Height - 8);
            panelTop.Controls.Add(edTemplate);

            cbSingleLine.Location = new System.Drawing.Point(edTemplate.Right + 4, (panelTop.ClientSize.Height - buttonTest.Height) / 2);
            panelTop.Controls.Add(cbSingleLine);

            buttonTest.Location = new System.Drawing.Point(cbSingleLine.Right + 4, (panelTop.ClientSize.Height - buttonTest.Height) / 2);
            panelTop.Controls.Add(buttonTest);
            buttonTest.Click += buttonTest_Click;

            btnMatchPrev.Location = new System.Drawing.Point(buttonTest.Right + 12, (panelTop.ClientSize.Height - btnMatchPrev.Height) / 2);
            btnMatchPrev.Width /= 2;
            panelTop.Controls.Add(btnMatchPrev);
            btnMatchPrev.Click += btnMatchPrev_Click;

            btnMatchNext.Location = new System.Drawing.Point(btnMatchPrev.Right + 4, (panelTop.ClientSize.Height - btnMatchNext.Height) / 2);
            btnMatchNext.Width /= 2;
            panelTop.Controls.Add(btnMatchNext);
            btnMatchNext.Click += btnMatchNext_Click;

            btnTextClearFormat.Location = new System.Drawing.Point(btnMatchNext.Right + 4, (panelTop.ClientSize.Height - btnTextClearFormat.Height) / 2);
            btnTextClearFormat.Width = (int)(btnTextClearFormat.Width * 1.3);
            panelTop.Controls.Add(btnTextClearFormat);
            btnTextClearFormat.Click += btnTextClearFormat_Click;

            buttonOpenFile.Location = new System.Drawing.Point(btnTextClearFormat.Right + 4, (panelTop.ClientSize.Height - btnTextClearFormat.Height) / 2);
            buttonOpenFile.Width = (int)(buttonOpenFile.Width * 0.7);
            panelTop.Controls.Add(buttonOpenFile);
            buttonOpenFile.Click += buttonOpenFile_Click;

            this.components.Add(openFileDialog);

            this.Load += Form1_Load;

        }


        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelText;
        private System.Windows.Forms.RichTextBox edText;
        private System.Windows.Forms.Panel panelMatchesLog;
        private System.Windows.Forms.TextBox edMatchesLog;
        private System.Windows.Forms.Button buttonMatchesLogSync;
        private System.Windows.Forms.ComboBox edTemplate;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.Button btnMatchPrev;
        private System.Windows.Forms.Button btnMatchNext;
        private System.Windows.Forms.Button btnTextClearFormat;
        private System.Windows.Forms.TextBox edTextState;
        private System.Windows.Forms.CheckBox cbSingleLine;
        private System.Windows.Forms.Button buttonOpenFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog;

        #endregion
    }
}

