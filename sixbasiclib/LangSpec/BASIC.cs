using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace sixbasiclib
{
    public class BASIC
    {
        public Detokenizer DeTokenizer { get; set; }
        public List<BASIC_Line> Program { get; set; } = new List<BASIC_Line>();
        public int LoadAddress { get; set; }
        private int Linebase = 0x00;
        private int NextLine = 0x00;
        public string LastError = "";

        public BASIC()
        {
            DeTokenizer = new Detokenizer();
        }

        public async Task<bool> Load(string filename)
        {
            bool b = false;
            try
            {
                Program = new List<BASIC_Line>();
                bool doneflag = false;
                byte[] filecontents = await File.ReadAllBytesAsync(filename);
                if (filecontents.Length > 2)
                {
                    // Get Load Address
                    LoadAddress = filecontents[0] + (256 * filecontents[1]);
                    Linebase = 2;
                    if (filecontents.Length > 4)
                    {
                        while (!doneflag)
                        {
                            NextLine = ((filecontents[Linebase] + (filecontents[Linebase + 1] * 256)) - LoadAddress) + 2;
                            if (NextLine <= 0)
                            {
                                doneflag = true;
                            }
                            else
                            {
                                BASIC_Line bl = new BASIC_Line();
                                bl.Line_Number = filecontents[Linebase + 2] + (filecontents[Linebase + 3] * 256);
                                int ThisLineLen = NextLine - (Linebase + 4);
                                bl.Line_Data = new byte[ThisLineLen];
                                for (int i = 0; i < ThisLineLen - 1; i++)
                                {
                                    bl.Line_Data[i] = filecontents[Linebase + 4 + i];
                                }
                                Program.Add(bl);
                                Linebase = NextLine;
                            }
                        }
                        b = true;
                    }
                }
            }
            catch (Exception e)
            {
                b = false;
                LastError = e.Message;
            }
            return b;
        }

        public List<string> List()
        {
            List<string> result = new List<string>();
            foreach (BASIC_Line line in Program)
            {
                result.Add(DeTokenizer.Detokenize(line));
            }
            return result;
        }

    }
}
