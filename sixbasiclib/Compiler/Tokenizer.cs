using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sixbasiclib
{
    public static class Tokenizer
    {
        public static byte[] Tokenize(BASICProgram program, ref CompilerSettings compilerSettings)
        {
            var NextLineAddress = compilerSettings.BaseAddress;
            var output = new List<byte>
            {
                (byte)(NextLineAddress & 0x00ff),
                (byte)((NextLineAddress & 0xff00) >> 8)
            };
            foreach (var line in program.Lines)
            {
                var  bytes = TokenizeLine(line);
                NextLineAddress = NextLineAddress + bytes.Count;
                output.Add((byte)(NextLineAddress & 0x00ff));
                output.Add((byte)((NextLineAddress & 0xff00) >> 8));
                foreach (var b in bytes) output.Add(b);
            }
			//Pad end
            output.Add(0x00);
            output.Add(0x00);

            return output.ToArray();
        }

        private static List<byte> TokenizeLine(BASIC_Line line)
        {
            //Needs to encode line number and trailing zero after tokenized basic
            var output = new List<byte>();
            //int linenum = GetLineNumber(line); Already done by preproccesor
            var first = true;
            output.Add((byte)(line.Line_Number & 0x00ff));
            output.Add((byte)((line.Line_Number & 0xff00)>>8)); //placeholder bytes for next line number
            //line = StripLineNumber(line);

            var segments = Utils.SplitLine(line.Text);
            foreach (var segment in segments)
            {
                if (!first) output.Add(0x3A);
                var bytes = TokenizeBlock(segment);
                foreach (var b in bytes)
                {
                    output.Add(b);
                }
                first = false;
            }

            output.Add(0x00);
            return output;
        }


        public static List<byte> TokenizeBlock(string s)
        {
            var bytes = new List<byte>();
            var t = s.Replace("\0", "");
            t = t.TrimStart(new char[]{' ',':'});
            var done = false;
            var quotemode = false;
            while (!done)
            {
                var found = false;
                if (!quotemode)
                {
                    foreach (var token in MicrosoftBasic2.Tokens)
                    {
                        if (t.ToUpper().StartsWith(token.ASCII.ToUpper(), StringComparison.CurrentCulture))
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
                done |= t.Length == 0;
            }
            return bytes;

        }
    }
}
