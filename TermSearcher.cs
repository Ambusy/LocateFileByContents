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
    class TermSearcher
    {
        RdrData BinReaderData = new RdrData();
        private bool ignoreCase = false;
        private long startPositionBeforeRead=0;
        public long FoundAtPosition = 0;
        public long DisplayedAtPosition = -1;
        private String WhichRdr = ""; 
        public bool Open(String fileName, bool signalAD)
        {
            if (this == M.tsd) // for debugging purposes
                WhichRdr = "dsp ";
            else
                WhichRdr = "sel ";
            Debug.WriteLine("open " + WhichRdr + fileName);
            bool Ok = false;
            try
            {
                BinReaderData.reader = new BinaryReader(File.Open(fileName, FileMode.Open));
                BinReaderData.fileName = fileName;
                Ok = true;
            }
            catch (UnauthorizedAccessException ex)
            {
                M.NrFilesError += 1;
                if (signalAD) MessageBox.Show(ex.Message);
            }
            catch (System.IO.IOException ex)
            {
                M.NrFilesError += 1;
                if (signalAD) MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                M.NrFilesError += 1;
                MessageBox.Show(ex.Message);
            }
            if (Ok)
            {
                byte[] rdBuf;
                long chLeft = BinReaderData.reader.BaseStream.Length;
                int ncHead = (int)Math.Min(chLeft, 3);
                rdBuf = BinReaderData.reader.ReadBytes((int)ncHead);
                BinReaderData.reader.BaseStream.Position = 0;
                BinReaderData.firstIndex = 0;
                if (M.terms.canBeAscii)
                    BinReaderData.typeFile = 'A';
                else
                    BinReaderData.typeFile = '8';
                if (ncHead > 1) if (rdBuf[0] == 255) if (rdBuf[1] == 254) BinReaderData.typeFile = 'U';
                if (ncHead > 1) if (rdBuf[0] == 239) if (rdBuf[1] == 187) if (rdBuf[2] == 191) BinReaderData.typeFile = '8';
            }
            return Ok;
        }
        public void Close()
        {
            Debug.WriteLine("close " + WhichRdr + BinReaderData.fileName);
            BinReaderData.reader.Close();
            BinReaderData.reader.Dispose();
        }
        private String ByteToString(byte[] inp)
        {
            String outps = System.Text.Encoding.ASCII.GetString(inp);
            return outps;
        }

        private int even(int i)
        {
            if (i % 2 == 0)
                return i;
            else
                return i + 1;
        }
        public bool LocateTerms(long start)
        {
            byte[] rdBufi;
            byte[] rdBufn = null;
            int blLength = 20000;
            long prevChars = start;

            int ncHead = 0;
            long chLeft = 0;
            switch (TypeFile)
            {
                case 'A':
                    Position = start + 0;
                    chLeft = Length - Position;
                    int m = Math.Max(Math.Max(M.terms.MaxLenTermA, M.terms.MaxLenTerm8), M.terms.MaxLenTermU);
                    ncHead = (int)Math.Min(chLeft, even(m));
                    M.terms.MaxLenTerm = m;
                    break;
                case 'U':
                    Position = start + 2;
                    chLeft = Length - Position;
                    ncHead = (int)Math.Min(chLeft, even(M.terms.MaxLenTermU));
                    M.terms.MaxLenTerm = M.terms.MaxLenTermU;
                    break;
                case '8':
                    Position = start + 3;
                    chLeft = Length - Position;
                    ncHead = (int)Math.Min(chLeft, even(M.terms.MaxLenTerm8));
                    M.terms.MaxLenTerm = M.terms.MaxLenTerm8;
                    break;
                default:
                    break;
            }
            BinReaderData.firstIndex = 0;
            M.signal("ReadHead " + WhichRdr, Position, ncHead);
            rdBufi = ReadBytes((int)ncHead);
            chLeft -= rdBufi.Length;
            prevChars += rdBufi.Length;
            bool Found = false;
            while (chLeft > 0)
            {
                Application.DoEvents();
                if (M.StopSearching) return (false);
                int ncBlock = (int)Math.Min(chLeft, blLength);
                M.signal("ReadBytes " + WhichRdr, Position, ncBlock  );
                rdBufn = ReadBytes(ncBlock);
                chLeft -= rdBufn.Length;
                int ncToVerify = ncBlock + ncHead;
                int ncToDiscard = blLength - M.terms.MaxLenTerm; // tail will be new head
                if (chLeft == 0)
                    ncToDiscard = rdBufn.Length; // no more tail
                SearchPartial(rdBufi, rdBufn, ncToVerify, prevChars);
                Found = ResultReady();
                if (Found)
                {
                    chLeft = 0; // decision had been made already
                }
                else if (ncToDiscard != rdBufn.Length)
                {
                    Buffer.BlockCopy(rdBufn, blLength - M.terms.MaxLenTerm, rdBufi, 0, M.terms.MaxLenTerm); // last part of BLOCK to HEAD
                }
                prevChars += rdBufn.Length;
            }
            if (!Found)
                SearchPartial(rdBufi, rdBufn, rdBufi.Length,prevChars); // search only last head 
            return Found;
        }
        private void SearchPartial(byte[] rdBufi, byte[] rdBufn, int nPos, long prevChars)
        {
            foreach (SearchTerm sT in M.Terms)
            {
                byte[] sTerm = sT.searchBytesA;
                if (TypeFile == '8') sTerm = sT.searchBytes8;
                if (TypeFile == 'U') sTerm = sT.searchBytesU;
                bool Found = TryOneTerm(sT, rdBufi, rdBufn, nPos, TypeFile, sTerm, prevChars);
                if (!Found)
                    if (TypeFile == 'A')
                    {
                        Found = TryOneTerm(sT, rdBufi, rdBufn, nPos, '8', sT.searchBytes8, prevChars); // if Ascii not found, try again with utf8 for files without BOM but utf8
                        if (!Found)
                            TryOneTerm(sT, rdBufi, rdBufn, nPos - 1, 'U', sT.searchBytesU, prevChars); // if still not found, try again with Unicode for files with mixed Ascii and Unicode (example binaries)
                    }
            }
        }
        private bool TryOneTerm(SearchTerm sT, byte[] rdBufi, byte[] rdBufn, int nPos, Char typeFile, byte[] sTerm, long prevChars)
        {

            int iPos = 1; // position number of char to be verified, in combined (HEAD + BLOCK) buffer
            int rPos = 0; // index in current buffer, HEAD or BLOCK
            bool srcBufi = true;
            int iChar = 0; // which search char in searchterm is to be used for testing
            int nCharOk = 0; // how many chars are matching, so far?
            bool fndTerm = false;
            while (!fndTerm && iPos <= nPos - sTerm.Length)
            {
                if (iPos == rdBufi.Length + 1) // switch to second bytes buffer
                {
                    rPos = 0;
                    srcBufi = false;
                }
                byte TestChar;
                if (srcBufi) // chars in HEAD                   
                    TestChar = rdBufi[rPos];
                else
                    TestChar = rdBufn[rPos];
                if (ignoreCase)
                {
                    byte nxtChar = 0;
                    if (typeFile == 'U')
                    {
                        if (iPos % 2 == 1)
                        {
                            if (iPos == rdBufi.Length)
                                nxtChar = rdBufn[0];
                            else if (iPos < rdBufi.Length)
                                nxtChar = rdBufi[rPos + 1];
                            else
                                nxtChar = rdBufn[rPos + 1];
                        }
                    }
                    if (nxtChar == 0)
                    {
                        if (TestChar >= 65 && TestChar <= 90) TestChar += 32;
                    }
                }
                if (TestChar == sTerm[iChar])
                {
                    if (iChar == 0)
                        nCharOk = 1;
                    else
                    {
                        nCharOk++;
                   }
                   iChar++;
                    if (nCharOk == sTerm.Length)
                    {
                        fndTerm = true;
                        M.signal("Found at ", startPositionBeforeRead, iPos - sTerm.Length);
                        BinReaderData.firstIndex = startPositionBeforeRead + iPos - sTerm.Length; // prevChars - rdBufi.Length + iPos; // remember first hit.
                        M.signal("startpos ", BinReaderData.firstIndex,0);
                    }
                }
                else if (iChar > 0) // iChar: number of matching characters
                {
                    // restart after partial match. Point to first matching char, will continue with next char
                    iPos -= iChar;
                    rPos -= iChar;
                    if (rPos < -1) // go back to head?
                    {
                        srcBufi = true;
                        rPos = rdBufi.Length + rPos;
                    }
                    iChar = 0;
                }
                iPos++;
                rPos++;
            }
            sT.textFound = fndTerm;
            return fndTerm;
        }
        private bool ResultReady()
        {
            bool Found = true;
            foreach (SearchTerm sT in M.Terms)
            {
                if (sT.operat == '|')
                {
                    if (Found) // previous block resulted ok: all is ok
                        return true;
                    else
                        Found = true;
                }
                if (!sT.textFound) Found = false;
            }
            return Found;
        }

        public BinaryReader Reader
        {
            get
            {
                return BinReaderData.reader;
            }
        }
        public char TypeFile
        {
            get
            {
                return BinReaderData.typeFile;
            }
            set
            {
                BinReaderData.typeFile = value;
            }
        }
        public String FileName
        {
            get
            {
                return BinReaderData.fileName;
            }
            set
            {
                BinReaderData.fileName = value;
            }
        }
        public long Position
        {
            get
            {
                return BinReaderData.reader.BaseStream.Position;
            }
            set
            { 
                if (value <= BinReaderData.reader.BaseStream.Length)
                    BinReaderData.reader.BaseStream.Position = value;
            }
        }
        public bool IgnoreCase
        {
            get
            {
                return ignoreCase;
            }
            set
            {
                ignoreCase = value;
            }
        }
        public long Length
        {
            get
            {
                return BinReaderData.reader.BaseStream.Length;
            }
        }
        public long FirstIndex
        {
            get
            {
                return BinReaderData.firstIndex;
            }
            set
            {
                BinReaderData.firstIndex = value;
            }

        }
        public byte[] ReadBytes(int nChar)
        {
            startPositionBeforeRead = Position;
            byte[] block = BinReaderData.reader.ReadBytes(nChar);
            return block;
        }
    }
    class RdrData
    {
        public String fileName; // name of BinaryReader file
        public BinaryReader reader; // handle to BinaryReader
        public char typeFile; // A, 8 or U
        public long firstIndex;
    }
}
