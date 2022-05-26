using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace FindFile
{
    public partial class DisplayForm : Form
    {
        readonly int  CharsBeforeTerm = 500; // display approx. 3000 bytes in total on one screen, found term at potition 501
        readonly int CharsAfterTerm = 2500;
        long StartDisplayed;
        readonly ASCIIEncoding ascii = new ASCIIEncoding();
        readonly UTF8Encoding utf8 = new UTF8Encoding();
        readonly UTF32Encoding utf32 = new UTF32Encoding();

        public DisplayForm()
        {
            InitializeComponent();
        }
        private void DisplayForm_Activated(object sender, EventArgs e)
        {
            
            if (M.tsd.DisplayedAtPosition != M.tsd.FoundAtPosition)
                BuildHtml();
        }
        private void BuildHtml()
        {
            this.Text = M.tsd.FileName + " at pos. " + M.tsd.FoundAtPosition.ToString();
            long startRead = Math.Max(0, M.tsd.FoundAtPosition - CharsBeforeTerm);
            M.tsd.DisplayedAtPosition = M.tsd.FoundAtPosition;
            StartDisplayed = startRead;
            byte[] rdBufn;
            int nchRead = CharsBeforeTerm + CharsAfterTerm;
            rdBufn = ReadBlock( startRead, nchRead);
            button2.Visible = (rdBufn.Length == nchRead);
            if (M.tsd.IgnoreCase)
            {
                if (M.tsd.TypeFile == 'U')
                {
                    for (int iPos = 1; iPos < rdBufn.Length; iPos += 2)
                    {
                        if (rdBufn[iPos - 1] == 0 && rdBufn[iPos] >= 65 && rdBufn[iPos] <= 90) rdBufn[iPos] += 32;
                    }
                }
                else
                {
                    for (int iPos = 0; iPos < rdBufn.Length; iPos++)
                    {
                        if (rdBufn[iPos] >= 65 && rdBufn[iPos] <= 90) rdBufn[iPos] += 32;
                    }
                }
            }
            string html;    
            if (M.tsd.TypeFile == 'A') html = ascii.GetString(rdBufn);
            else if (M.tsd.TypeFile == '8') html = utf8.GetString(rdBufn);
            else html = utf32.GetString(rdBufn);

            int procd = -1;
            String htmlEnc = "";
            while (procd+1 < html.Length)
            {
                int minPos = int.MaxValue;
                String minTxt = "";
                foreach (SearchTerm sT in M.Terms)
                {
                    int i = html.IndexOf(sT.searchText, procd + 1);
                    if (i > -1 && i < minPos)
                    {
                        minPos = i;
                        minTxt = sT.searchText;
                    }
                }
                if (procd == -1 && minPos == int.MaxValue) // for some reason, I cannot find the found text again (unicode in ascii, for example)
                {
                    minPos = CharsBeforeTerm-5; // here is somehow where the first one should be
                    minTxt = "          "; // lets assume 10 chars long

                }
                if (minPos == int.MaxValue)
                {
                    htmlEnc += HttpUtility.HtmlEncode(html.Substring(procd + 1));
                }
                else
                {
                    htmlEnc += HttpUtility.HtmlEncode(html.Substring(procd + 1, minPos - procd - 1));
                    htmlEnc += "<span style=\"background-color:yellow;color:red;\">" + HttpUtility.HtmlEncode(html.Substring(minPos, minTxt.Length)) + "</span>";
                }
                procd = minPos + minTxt.Length-1;
            }

            using (StreamWriter writer = new StreamWriter(M.WrkfileName, false, new UTF8Encoding(M.tsd.TypeFile == '8')))
            {
                writer.WriteLine("<htmlEnc><body>");
                writer.WriteLine(htmlEnc);
                writer.WriteLine("</body></htmlEnc>");
            }
            wBr.Url = new Uri(M.WrkfileName);
            wBr.Refresh();
            LocateNextPage() ;
        }
        private void LocateNextPage()
        {
            button2.Visible = false;
            // continue with last "term length" char on screen to locate next occurrence
            long startRead = Math.Max(0, StartDisplayed + CharsBeforeTerm + CharsAfterTerm - M.terms.MaxLenTerm);
            if (startRead < M.tsd.Length)
            {
                if (M.tsd.LocateTerms(startRead))
                {
                    button2.Visible = true;
                    StartDisplayed = M.tsd.FirstIndex; // on next display!
                }               
            }
        }
        private byte[] ReadBlock( long start, int nChars)
        {
            M.tsd.Position = start;
            byte[] rdBufn = M.tsd.ReadBytes(nChars);
            return rdBufn;
        }

        private void CloseDisplay() // close resources
        {
             M.tsd.Close();
            this.Visible = false;
       }

        private void Button1_Click(object sender, EventArgs e) // stop all
        {
            M.StopSearching = true;
            CloseDisplay();
        }

        private void Button3_Click(object sender, EventArgs e) // next file
        {
            CloseDisplay();
        }

        private void Button2_Click(object sender, EventArgs e) // next occ
        {
            if (button2.Visible)
            {
                M.tsd.FoundAtPosition = M.tsd.FirstIndex; // Display new block of text
                BuildHtml();
            }         
        }

        private void DisplayForm_FormClosing(object sender, FormClosingEventArgs e) // next file
        {
            Button3_Click(sender, e);
            e.Cancel = true;
        }

        private void Button4_Click(object sender, EventArgs e) // open
        {
            String FileName = M.tsd.FileName;
            CloseDisplay();
            Process myProcess = Process.Start(FileName, "");
            myProcess.WaitForExit();
        }
    }
}
