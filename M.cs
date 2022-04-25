using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FindFile
{
    class M
    {
        public static String CommandLine;
        public static SelectForm selForm = new SelectForm();
        public static DisplayForm dspForm = new DisplayForm();
        public static string StartingDirectory = "";
        public static bool StopSearching = false;
        public static TermMgr terms;
        public static int NrFilesError;
        public static List<SearchTerm> Terms = new List<SearchTerm>();
        public static String WrkfileName; // to write HTML to
        public static TermSearcher tsd = new TermSearcher(); // for displaying



        [STAThread]
        static int Main(string[] args)
        {
            if (args.Length == 0)
                CommandLine = "";
            else
                CommandLine = string.Join(" ", args);
            WrkfileName = Path.GetTempFileName();
            selForm.Visible = false;
            selForm.WindowState = FormWindowState.Normal;
            selForm.ShowDialog();
            File.Delete(WrkfileName);
            return 0;
        }
        public static  void signal(String what, long start, long len) // for debugging purposes
        {
            Debug.WriteLine(what + " " + start.ToString() + " " + len.ToString() + " " + (start + len).ToString());
        }
    }
    class SearchTerm
    {
        public char operat; // & or |
        public String searchText; // as typed by user
        public byte[] searchBytesU; // in bytes as unicode search text
        public byte[] searchBytes8; // in bytes as utf8 search text
        public byte[] searchBytesA; // in bytes as ascii search text
        public Boolean textFound;
    }
    class TermMgr
    {
        public int MaxLenTerm = 0;
        public int MaxLenTermA = 0;
        public int MaxLenTermU = 0;
        public int MaxLenTerm8 = 0;
        public bool canBeAscii; // term contains only ASCII characters. If not we need to fallback on UTF8 for files without BOM


        public TermMgr(ListBox TermsList, bool IgnoreCase)
        {
            canBeAscii = true;
            M.Terms.Clear();
            foreach (String Tr in TermsList.Items)
            {
                SearchTerm T = new SearchTerm();
                T.operat = Tr[0];
                if (T.operat == ' ') T.operat = '&';
                T.searchText = Tr.Substring(2);
                if (IgnoreCase) T.searchText = T.searchText.ToLower();
                T.searchBytesA = Encoding.ASCII.GetBytes(T.searchText);
                if (T.searchBytesA.Length > MaxLenTermA) MaxLenTermA = T.searchBytesA.Length;

                String recodedString = Encoding.ASCII.GetString(T.searchBytesA);
               if (recodedString != T.searchText)
                    canBeAscii=false;
               
                T.searchBytesU = Encoding.Unicode.GetBytes(T.searchText);
                if (T.searchBytesU.Length > MaxLenTermU) MaxLenTermU = T.searchBytesU.Length;
                T.searchBytes8 = Encoding.UTF8.GetBytes(T.searchText);
                if (T.searchBytes8.Length > MaxLenTerm8) MaxLenTerm8 = T.searchBytes8.Length;
                T.textFound = false;
                M.Terms.Add(T);
            }
        }
    }
}
