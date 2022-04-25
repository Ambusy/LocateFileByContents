using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FindFile
{
    public partial class SelectForm : Form
    {
        int NrFilesFound = 0;
        int NrFoldersError = 0;
        int NrFilesSearched = 0;
        List<String> DirsToBeProcessed = new List<String>(); // names of directories to be processed
        string[] filter;
        int currentFilelistIndex; // current pointer to file in progress
        int currentFoldelistDirIndex;
        String currentFolderlistDirname; // current folder in progress
        bool HasDisplayFormBeenOpened = false; // opened once, then changes visibility only.
        TermSearcher ts = new TermSearcher(); // for locating files

        public SelectForm()
        {
            InitializeComponent();
        }
        private void Brws_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Initial folder";
            folderBrowserDialog1.ShowNewFolderButton = false;
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;
            if (StartMap.Text != "")
                folderBrowserDialog1.SelectedPath = StartMap.Text;
            DialogResult rc = folderBrowserDialog1.ShowDialog();
            if (rc == DialogResult.OK)
            {
                StartMap.Text = folderBrowserDialog1.SelectedPath;
                FoldersList.Items.Clear();
                FoldersList.Items.Add(folderBrowserDialog1.SelectedPath);
                filter = SrcMask.Text.Split(';');
                M.StartingDirectory = folderBrowserDialog1.SelectedPath;
                InitLists(folderBrowserDialog1.SelectedPath);
            }
            if (TermsList.Items.Count > 0) 
                SearchBut.Visible = true;
        }
        private void InitLists(String dir)
        {
            DirsToBeProcessed.Clear();
            NrFilesFound = 0;
            NrFoldersError = 0;
            NrFilesSearched = 0;
            //dspMsg();
            DirsToBeProcessed.Add(dir);
            FillListboxes(dir);
        }
        private void dspMsg()
        {
            Msg1.Text = NrFilesSearched.ToString() + " searched, " + NrFilesFound.ToString() + " hits, " + (NrFoldersError + M.NrFilesError).ToString() + " no access";
        }
        private void FillListboxes(String dir)
        {
            int i;
            FoldersList.Items.Clear(); // listbox with maps
            FilesList.Items.Clear();   // listbox with files
            if (dir.Length == 2) 
                dir += @"\";
            string[] p = dir.Split('\\');
            currentFoldelistDirIndex = -1;
            for (i = 0; i < p.Length; i++) // add parts of foldername, with mapnames indented with a space
            {
                if (p[i] != "")
                {
                    FoldersList.Items.Add("                                    ".Substring(0, i) + p[i]);
                    currentFoldelistDirIndex += 1;
                }
            }
            FoldersList.SelectedIndex = currentFoldelistDirIndex; // start search with lowest level map
            currentFolderlistDirname = dir; // for composition of filenames
            currentFilelistIndex = -1; // start with first file in list
            try
            {
                String masks = SrcMask.Text.Trim();
                if (masks == "") masks = "*.*";
                string[] m = masks.Split(';');
                foreach (String mask in m)
                    foreach (string foundFile in Directory.GetFiles(dir, mask.Trim(), SearchOption.TopDirectoryOnly))
                    {
                        i = foundFile.LastIndexOf("\\");
                        String fName = foundFile.Substring(i + 1);
                        if (!FilesList.Items.Contains(fName))
                            FilesList.Items.Add(foundFile.Substring(i + 1));
                    }
            }
            catch (Exception e)
            {
                NrFoldersError += 1;
            }
            try // after having processed all files, try all directories at a lower level
            {
                int idr = 1;
                foreach (string foundDir in Directory.GetDirectories(dir))
                {
                    DirectoryInfo dr = new DirectoryInfo(foundDir);
                    bool Hidd = (dr.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
                    if (!Hidd | incHidden.Checked)
                    {
                        FoldersList.Items.Add("                                    ".Substring(0, currentFoldelistDirIndex + 1) + dr.Name);
                        // add the name to the list of maps still to be processed
                        if (DirsToBeProcessed.Count == 0)
                        {
                            DirsToBeProcessed.Add(foundDir);
                        }
                        else
                        {
                            DirsToBeProcessed.Insert(idr - 1, foundDir);
                        }
                        idr += 1;
                    }
                }
            }
            catch (Exception)
            {
                NrFoldersError += 1;
            }
        }
        private void SelectForm_Load(object sender, EventArgs e)
        {
            String CommandLine = M.CommandLine;
            if (CommandLine != "")
            { // insert data and simulate browse_click
                string[] args = CommandLine.Split(';'); // startfolder terms separated by and/or operator. Ex: C:\Users;windows & dos | unix
                StartMap.Text = args.Length > 0 ? args[0] : "";
                int i = 1;
                while (i < args.Length)
                {
                    Term.Text = args[i];
                    ConOr.Checked = true;
                    AddOneTerm();
                    i++;
                }
                M.StartingDirectory = StartMap.Text;
                folderBrowserDialog1.SelectedPath = StartMap.Text;
                InitLists(StartMap.Text);
                ManageButtons();
            }
            ManageButtons();
        }
        private void ManageButtons()
        {
            SearchBut.Visible = (TermsList.Items.Count > 0 & StartMap.Text != "");
            ClearBut.Visible = (TermsList.Items.Count > 0);
            AddTerm.Visible = (Term.Text.Trim() != "");
            Connectors.Visible = (TermsList.Items.Count > 0);
            //if (TermsList.Items.Count > 0)
            //    ConOr.Checked = true;
        }
        private void ClearBut_Click(object sender, EventArgs e)
        {
            TermsList.Items.Clear();
            ConOr.Checked = false;
            ManageButtons();
        }
        private void Term_TextChanged(object sender, EventArgs e)
        {
            ManageButtons();
        }
        private void Term_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                AddOneTerm();
        }
        private void AddOneTerm()
        {            
            if (TermsList.Items.Count == 0)
                TermsList.Items.Add("  " + Term.Text);
            else if (ConOr.Checked)
                TermsList.Items.Add("| " + Term.Text);
            else
                TermsList.Items.Add("& " + Term.Text);
            Term.Text = "";
            ManageButtons();
        }
        private void AddTerm_Click(object sender, EventArgs e)
        {
            AddOneTerm();
            if (TermsList.Items.Count > 0)
                ConOr.Checked = true;
        }
        private void SrcMask_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitLists(M.StartingDirectory);
        }
        private String GetName() // find name of next file to be processed from Directory-tree
        {
            currentFilelistIndex += 1;
            if (currentFilelistIndex < FilesList.Items.Count) // file available
            {
                FilesList.SelectedIndex = currentFilelistIndex;
                return (String)FilesList.Items[currentFilelistIndex];
            }
            else // no more files, get list of files in next folder (if there is one left)
            {
                if (M.StopSearching)
                    return "";
                if (DirsToBeProcessed.Count > 0)
                {
                    String s = DirsToBeProcessed[0];
                    DirsToBeProcessed.RemoveAt(0);
                    if (DirsToBeProcessed.Count > 0)
                    {
                        FillListboxes(s);
                        if (M.StopSearching)
                            return "";
                        dspMsg();
                        return GetName(); // return 1st file of next list
                    }
                }
            }
            return ""; // ready
        }
        private void SearchBut_Click(object sender, EventArgs e)
        {
            SearchBut.Visible = false;
            M.NrFilesError = 0;
            NrFilesFound = 0;
            NrFoldersError = 0;
            NrFilesSearched = 0;


            M.terms = new TermMgr(TermsList, IgnCase.Checked);
            // searching is as follows: 
            // say L is the number of chars in the longest searchterm
            // read L chars as (HEAD). 
            // if more text exists:
            //  read at most 20000 chars (BLOCK)
            //  consider string HEAD (all chars) and BLOCK (all chars up to 20000-L) as one string and flag found searchterms. 
            //  move last L chars from BLOCK to HEAD and repeat. This way terms in 2 blocks are found using the head.
            // flag found searchterms in HEAD
            M.StopSearching = false;
            String searchfile;
            while (!M.StopSearching && (searchfile = GetName()) != "")
            {
                ts.Open(currentFolderlistDirname + @"\" + searchfile, accDeny.Checked);
                if (ts.Reader != null && ts.Reader.BaseStream != null && !M.StopSearching)
                {
                    NrFilesSearched++;
                    ts.IgnoreCase = IgnCase.Checked;
                    if (ts.LocateTerms(0))
                    {
                        NrFilesFound++;
                        dspMsg();
                        DisplayFoundFile();
                    }
                    else 
                        ts.Close();  
                }
                Application.DoEvents();
            }
            dspMsg();
            InitLists(folderBrowserDialog1.SelectedPath);
            SearchBut.Visible = true;
        }
        private void DisplayFoundFile()
        {
            while (M.dspForm.Visible) // wait for user to close previous file, which is still being displayed
            {
                System.Threading.Thread.Sleep(1000);
                Application.DoEvents();
            }
            ts.Close();
            if (!M.StopSearching) // user did not click STOP
            {
                M.tsd.Open(ts.FileName, true);
                M.tsd.FileName = ts.FileName;
                M.tsd.IgnoreCase = IgnCase.Checked;
                M.tsd.FirstIndex = ts.FirstIndex;
                M.tsd.FoundAtPosition = ts.FirstIndex;
                M.tsd.DisplayedAtPosition = -1;
                if (HasDisplayFormBeenOpened)
                {
                    M.dspForm.Visible = true;
                }
                else
                    M.dspForm.Show();
                HasDisplayFormBeenOpened = true;
            }
        }

        private void FoldersList_MouseClick(object sender, MouseEventArgs e)
        {
            int l = (int)((double)e.Y / 12.93)+1; // find the line on which the user clicked. That directory will be considered "completed"
            String selectedDir = FoldersList.Items[l - 1].ToString();
            int ns = 0;
            for (int i = 0; ns == 0 && i < selectedDir.Length; i++)
                if (selectedDir[i] != ' ')
                    ns = i;
            selectedDir = selectedDir.TrimStart();
            bool stop = false;
            while (!stop && DirsToBeProcessed.Count > 0)
            {
                string[] args = DirsToBeProcessed[0].Split('\\');
                if (ns < args.Length)
                {
                    if (args[ns] == selectedDir)
                    {
                        DirsToBeProcessed.RemoveAt(0);
                    }
                    else                    
                        stop = true;                   
                }
                else
                    stop = true;
            }
            FillListboxes(DirsToBeProcessed[0]);
            currentFilelistIndex = -1; // start with first file in list
        }

        private void StopBut_Click(object sender, EventArgs e)
        {
            M.StopSearching = true;
            if (M.dspForm.Visible) 
                M.dspForm.Close();
            M.selForm.Close();
        }
    }
}
