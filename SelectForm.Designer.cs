
namespace FindFile
{
    partial class SelectForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectForm));
            this.SrcMask = new System.Windows.Forms.ComboBox();
            this.incHidden = new System.Windows.Forms.CheckBox();
            this.IgnCase = new System.Windows.Forms.CheckBox();
            this.ClearBut = new System.Windows.Forms.Button();
            this.AddTerm = new System.Windows.Forms.Button();
            this.Term = new System.Windows.Forms.TextBox();
            this.Connectors = new System.Windows.Forms.GroupBox();
            this.ConOr = new System.Windows.Forms.RadioButton();
            this.ConAnd = new System.Windows.Forms.RadioButton();
            this.FilesList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SearchBut = new System.Windows.Forms.Button();
            this.FoldersList = new System.Windows.Forms.ListBox();
            this.TermsList = new System.Windows.Forms.ListBox();
            this.Brws = new System.Windows.Forms.Button();
            this.labelMTSW = new System.Windows.Forms.Label();
            this.StartMap = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.Msg1 = new System.Windows.Forms.Label();
            this.StopBut = new System.Windows.Forms.Button();
            this.accDeny = new System.Windows.Forms.CheckBox();
            this.Connectors.SuspendLayout();
            this.SuspendLayout();
            // 
            // SrcMask
            // 
            this.SrcMask.FormattingEnabled = true;
            this.SrcMask.Items.AddRange(new object[] {
            "*.*",
            "*.txt; *.doc*; *.odt; *.rtf",
            "*.exe; *.dll"});
            this.SrcMask.Location = new System.Drawing.Point(56, 32);
            this.SrcMask.Name = "SrcMask";
            this.SrcMask.Size = new System.Drawing.Size(182, 21);
            this.SrcMask.TabIndex = 41;
            this.SrcMask.SelectedIndexChanged += new System.EventHandler(this.SrcMask_SelectedIndexChanged);
            // 
            // incHidden
            // 
            this.incHidden.AutoSize = true;
            this.incHidden.Location = new System.Drawing.Point(668, 4);
            this.incHidden.Name = "incHidden";
            this.incHidden.Size = new System.Drawing.Size(124, 17);
            this.incHidden.TabIndex = 40;
            this.incHidden.Text = "Include hidden maps";
            this.incHidden.UseVisualStyleBackColor = true;
            // 
            // IgnCase
            // 
            this.IgnCase.AutoSize = true;
            this.IgnCase.Location = new System.Drawing.Point(514, 92);
            this.IgnCase.Name = "IgnCase";
            this.IgnCase.Size = new System.Drawing.Size(82, 17);
            this.IgnCase.TabIndex = 39;
            this.IgnCase.Text = "Ignore case";
            this.IgnCase.UseVisualStyleBackColor = true;
            // 
            // ClearBut
            // 
            this.ClearBut.Location = new System.Drawing.Point(497, 35);
            this.ClearBut.Name = "ClearBut";
            this.ClearBut.Size = new System.Drawing.Size(87, 25);
            this.ClearBut.TabIndex = 38;
            this.ClearBut.Text = "Clear all";
            this.ClearBut.UseVisualStyleBackColor = true;
            this.ClearBut.Visible = false;
            this.ClearBut.Click += new System.EventHandler(this.ClearBut_Click);
            // 
            // AddTerm
            // 
            this.AddTerm.Location = new System.Drawing.Point(390, 35);
            this.AddTerm.Name = "AddTerm";
            this.AddTerm.Size = new System.Drawing.Size(87, 25);
            this.AddTerm.TabIndex = 37;
            this.AddTerm.Text = "Add term";
            this.AddTerm.UseVisualStyleBackColor = true;
            this.AddTerm.Visible = false;
            this.AddTerm.Click += new System.EventHandler(this.AddTerm_Click);
            // 
            // Term
            // 
            this.Term.Location = new System.Drawing.Point(282, 66);
            this.Term.Name = "Term";
            this.Term.Size = new System.Drawing.Size(314, 20);
            this.Term.TabIndex = 35;
            this.Term.TextChanged += new System.EventHandler(this.Term_TextChanged);
            this.Term.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Term_KeyDown);
            // 
            // Connectors
            // 
            this.Connectors.Controls.Add(this.ConOr);
            this.Connectors.Controls.Add(this.ConAnd);
            this.Connectors.ForeColor = System.Drawing.Color.Black;
            this.Connectors.Location = new System.Drawing.Point(286, 28);
            this.Connectors.Name = "Connectors";
            this.Connectors.Size = new System.Drawing.Size(73, 36);
            this.Connectors.TabIndex = 36;
            this.Connectors.TabStop = false;
            this.Connectors.Visible = false;
            // 
            // ConOr
            // 
            this.ConOr.AutoSize = true;
            this.ConOr.Checked = true;
            this.ConOr.Location = new System.Drawing.Point(43, 13);
            this.ConOr.Name = "ConOr";
            this.ConOr.Size = new System.Drawing.Size(27, 17);
            this.ConOr.TabIndex = 2;
            this.ConOr.TabStop = true;
            this.ConOr.Text = "|";
            this.ConOr.UseVisualStyleBackColor = true;
            // 
            // ConAnd
            // 
            this.ConAnd.AutoSize = true;
            this.ConAnd.Location = new System.Drawing.Point(6, 13);
            this.ConAnd.Name = "ConAnd";
            this.ConAnd.Size = new System.Drawing.Size(31, 17);
            this.ConAnd.TabIndex = 1;
            this.ConAnd.Text = "&&";
            this.ConAnd.UseVisualStyleBackColor = true;
            // 
            // FilesList
            // 
            this.FilesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilesList.FormattingEnabled = true;
            this.FilesList.Location = new System.Drawing.Point(447, 112);
            this.FilesList.Name = "FilesList";
            this.FilesList.Size = new System.Drawing.Size(425, 420);
            this.FilesList.TabIndex = 34;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 33;
            this.label1.Text = "Filters";
            // 
            // SearchBut
            // 
            this.SearchBut.BackColor = System.Drawing.Color.Aqua;
            this.SearchBut.Location = new System.Drawing.Point(19, 76);
            this.SearchBut.Name = "SearchBut";
            this.SearchBut.Size = new System.Drawing.Size(124, 29);
            this.SearchBut.TabIndex = 32;
            this.SearchBut.Text = "Search";
            this.SearchBut.UseVisualStyleBackColor = false;
            this.SearchBut.Visible = false;
            this.SearchBut.Click += new System.EventHandler(this.SearchBut_Click);
            // 
            // FoldersList
            // 
            this.FoldersList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FoldersList.FormattingEnabled = true;
            this.FoldersList.Location = new System.Drawing.Point(16, 112);
            this.FoldersList.Name = "FoldersList";
            this.FoldersList.Size = new System.Drawing.Size(425, 420);
            this.FoldersList.TabIndex = 31;
            this.FoldersList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FoldersList_MouseClick);
            // 
            // TermsList
            // 
            this.TermsList.FormattingEnabled = true;
            this.TermsList.Location = new System.Drawing.Point(610, 36);
            this.TermsList.Name = "TermsList";
            this.TermsList.Size = new System.Drawing.Size(189, 69);
            this.TermsList.TabIndex = 30;
            // 
            // Brws
            // 
            this.Brws.Location = new System.Drawing.Point(583, 2);
            this.Brws.Name = "Brws";
            this.Brws.Size = new System.Drawing.Size(79, 19);
            this.Brws.TabIndex = 29;
            this.Brws.Text = "browse...";
            this.Brws.UseVisualStyleBackColor = true;
            this.Brws.Click += new System.EventHandler(this.Brws_Click);
            // 
            // labelMTSW
            // 
            this.labelMTSW.AutoSize = true;
            this.labelMTSW.Location = new System.Drawing.Point(16, 9);
            this.labelMTSW.Name = "labelMTSW";
            this.labelMTSW.Size = new System.Drawing.Size(88, 13);
            this.labelMTSW.TabIndex = 28;
            this.labelMTSW.Text = "Map to start with:";
            // 
            // StartMap
            // 
            this.StartMap.Location = new System.Drawing.Point(110, 2);
            this.StartMap.Name = "StartMap";
            this.StartMap.Size = new System.Drawing.Size(467, 20);
            this.StartMap.TabIndex = 27;
            // 
            // Msg1
            // 
            this.Msg1.AutoSize = true;
            this.Msg1.Location = new System.Drawing.Point(15, 535);
            this.Msg1.Name = "Msg1";
            this.Msg1.Size = new System.Drawing.Size(91, 13);
            this.Msg1.TabIndex = 42;
            this.Msg1.Text = "Enter search data";
            // 
            // StopBut
            // 
            this.StopBut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.StopBut.Location = new System.Drawing.Point(805, 66);
            this.StopBut.Name = "StopBut";
            this.StopBut.Size = new System.Drawing.Size(66, 31);
            this.StopBut.TabIndex = 43;
            this.StopBut.Text = "Stop";
            this.StopBut.UseVisualStyleBackColor = false;
            this.StopBut.Click += new System.EventHandler(this.StopBut_Click);
            // 
            // accDeny
            // 
            this.accDeny.AutoSize = true;
            this.accDeny.Checked = true;
            this.accDeny.CheckState = System.Windows.Forms.CheckState.Checked;
            this.accDeny.Location = new System.Drawing.Point(286, 89);
            this.accDeny.Name = "accDeny";
            this.accDeny.Size = new System.Drawing.Size(128, 17);
            this.accDeny.TabIndex = 44;
            this.accDeny.Text = "Signal Access denied";
            this.accDeny.UseVisualStyleBackColor = true;
            // 
            // SelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(875, 557);
            this.Controls.Add(this.accDeny);
            this.Controls.Add(this.StopBut);
            this.Controls.Add(this.Msg1);
            this.Controls.Add(this.SrcMask);
            this.Controls.Add(this.incHidden);
            this.Controls.Add(this.IgnCase);
            this.Controls.Add(this.ClearBut);
            this.Controls.Add(this.AddTerm);
            this.Controls.Add(this.Term);
            this.Controls.Add(this.Connectors);
            this.Controls.Add(this.FilesList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SearchBut);
            this.Controls.Add(this.FoldersList);
            this.Controls.Add(this.TermsList);
            this.Controls.Add(this.Brws);
            this.Controls.Add(this.labelMTSW);
            this.Controls.Add(this.StartMap);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SelectForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.SelectForm_Load);
            this.Connectors.ResumeLayout(false);
            this.Connectors.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox SrcMask;
        internal System.Windows.Forms.CheckBox incHidden;
        internal System.Windows.Forms.CheckBox IgnCase;
        internal System.Windows.Forms.Button ClearBut;
        internal System.Windows.Forms.Button AddTerm;
        internal System.Windows.Forms.TextBox Term;
        internal System.Windows.Forms.GroupBox Connectors;
        internal System.Windows.Forms.RadioButton ConOr;
        internal System.Windows.Forms.RadioButton ConAnd;
        private System.Windows.Forms.ListBox FilesList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SearchBut;
        private System.Windows.Forms.ListBox FoldersList;
        private System.Windows.Forms.ListBox TermsList;
        private System.Windows.Forms.Button Brws;
        private System.Windows.Forms.Label labelMTSW;
        private System.Windows.Forms.TextBox StartMap;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label Msg1;
        private System.Windows.Forms.Button StopBut;
        internal System.Windows.Forms.CheckBox accDeny;
    }
}

