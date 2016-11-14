using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SixBASIC
{
    public class Tokenizer
    {
        public byte[] Tokenize(BASICProgram program)
        {
            int NextLineAddress = program.StartAddress;
            List<byte> output = new List<byte>();
            output.Add((byte)(NextLineAddress & 0x00ff));
            output.Add((byte)((NextLineAddress & 0xff00) >> 8));
            foreach (BASIC_Line line in program.Lines)
            {
                List<byte> bytes = TokenizeLine(line);
                NextLineAddress = NextLineAddress + bytes.Count;
                output.Add((byte)(NextLineAddress & 0x00ff));
                output.Add((byte)((NextLineAddress & 0xff00) >> 8));
                foreach (byte b in bytes) output.Add(b);
            }
            return output.ToArray();
        }

        private List<byte> TokenizeLine(BASIC_Line line)
        {
            //Needs to encode line number and trailing zero after tokenized basic
            List<byte> output = new List<byte>();
            //int linenum = GetLineNumber(line); Already done by preproccesor
            bool first = true;
            output.Add((byte)(line.Line_Number & 0x00ff));
            output.Add((byte)((line.Line_Number & 0xff00)>>8)); //placeholder bytes for next line number
            //line = StripLineNumber(line);

            List<String> segments = Utils.SplitLine(line.Text);
            foreach (string segment in segments)
            {
                if (!first) output.Add(0x3A);
                List<byte> bytes = TokenizeBlock(segment);
                foreach (byte b in bytes)
                {
                    output.Add(b);
                }
                first = false;
            }

            output.Add(0x00);
            return output;
        }


        private List<byte> TokenizeBlock(string s)
        {
            List<byte> bytes = new List<byte>();
            string t = s.Replace("\0", "");
            t = t.TrimStart(new char[]{' ',':'});
            bool done = false;
            bool quotemode = false;
            while (!done)
            {
                bool found = false;
                if (!quotemode)
                {
                    foreach (MicrosoftBasic2Token token in MicrosoftBasic2.Tokens)
                    {
                        if (t.ToUpper().StartsWith(token.ASCII.ToUpper()))
                        {
                            bytes.Add(token.Token);
                            if (t.Length > token.ASCII.Length)
                            {
                                t = t.Substring(token.ASCII.Length, t.Length - token.ASCII.Length);
                            }
                            else
                            {
                                t = "";
                            }
                            found = true;
                            break;
                        }
                    }
                }
                if (!found)
                {
                    if (t.Length > 0)
                    {
                        if (t[0] == 0x22) quotemode = !quotemode;
                        if (quotemode)
                        {
                            //Fix case
                            if (t[0] >= 'a' && t[0] <= 'z')
                            {
                                bytes.Add((byte)(t.Substring(0, 1).ToUpper()[0]));
                            }
                            else
                            {
                                if (t[0] >= 'A' && t[0] <= 'Z')
                                {
                                    bytes.Add((byte)(t.Substring(0, 1).ToLower()[0]));
                                }
                                else
                                {
                                    bytes.Add((byte)(t[0]));
                                }

                            }
                        }
                        else
                        {
                            bytes.Add((byte)(t[0]));
                        }
                        t = t.Substring(1, t.Length - 1);
                    }
                }
                if (t.Length == 0) done = true;
            }

            return bytes;

        }
    }
}
