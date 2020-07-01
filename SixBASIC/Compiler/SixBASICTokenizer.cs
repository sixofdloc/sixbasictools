using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SixBASIC.LangSpec;

namespace SixBASIC
{
    /// <summary>
    /// Produces a tokenized binary with pointers instead of line numbers
    /// In IF THEN GOTO ON GOTO GOSUB
    /// </summary>
    public static class SixBASICTokenizer
    {

        public static List<OptimizedBASICLine> Lines;
        public static List<byte> Output;
        public static int NextLineAddress = 0x0000;
        public static int ThisLineAddress = 0x0000;
        public static OptimizedBASICLine CurrentLine;
        /// <summary>
        /// Tokenizes an entire BASIC program
        /// </summary>
        /// <returns>The tokenize.</returns>
        /// <param name="program">Program.</param>
        /// <param name="compilerSettings">Compiler settings.</param>
        public static byte[] Tokenize(BASICProgram program, ref CompilerSettings compilerSettings)
        {
            Output = new List<byte>();
            Lines = new List<OptimizedBASICLine>();
            NextLineAddress = compilerSettings.BaseAddress;
            ThisLineAddress = NextLineAddress;
            //Adding our load address, essentially
            Output.Add((byte)(NextLineAddress & 0x00ff));
            Output.Add((byte)((NextLineAddress & 0xff00) >> 8));

            //Process every line in the file to an optimizedBASIC Line.
            foreach (BASIC_Line line in program.Lines)
            {
                CurrentLine = new OptimizedBASICLine() { OriginalLine = line, BaseAddress = ThisLineAddress, LineNumber = line.Line_Number };
                //Get tokenized bytes for this line
                var tokenizedLineBytes = TokenizeLine(line);
                CurrentLine.Tokenized = tokenizedLineBytes;
                //Build the line header (pointer to the next line)
                NextLineAddress = NextLineAddress + tokenizedLineBytes.Count;

                Output.Add((byte)(NextLineAddress & 0x00ff));
                Output.Add((byte)((NextLineAddress & 0xff00) >> 8));

                //Now add our tokenized line
                Output.AddRange(tokenizedLineBytes);

                Lines.Add(CurrentLine);

                ThisLineAddress = NextLineAddress;
            }

            //Pad end with zeroes
            Output.Add(0x00);
            Output.Add(0x00);

            return Output.ToArray();
        }

        private static List<byte> TokenizeLine(BASIC_Line line)
        {
            //Needs to encode line number and trailing zero after tokenized basic
            List<byte> tokenizedLineBytes = new List<byte>();
            //int linenum = GetLineNumber(line); Already done by preproccesor
            bool first = true;

            //TODO: Get rid of line numbers entirely for our optimized BASIC

            //Add line number to the beginning of our output
            tokenizedLineBytes.Add((byte)(line.Line_Number & 0x00ff));
            tokenizedLineBytes.Add((byte)((line.Line_Number & 0xff00) >> 8)); 
            //line = StripLineNumber(line);

            //Splits the line on any : characters it finds
            List<String> lineSegments = Utils.SplitLine(line.Text);

            foreach (string lineSegment in lineSegments)
            {
                if (!first) tokenizedLineBytes.Add(PETSCII.CHAR_COLON);
                List<byte> tokenizedSegmentBytes = TokenizeBlock(lineSegment);
                tokenizedLineBytes.AddRange(tokenizedSegmentBytes);
                first = false;
            }

            tokenizedLineBytes.Add(0x00);
            return tokenizedLineBytes;
        }

        /// <summary>
        /// Tokenizes a block of text
        /// </summary>
        /// <returns>The block.</returns>
        /// <param name="s">S.</param>
        public static List<byte> TokenizeBlock(string s)
        {
            var bytes = new List<byte>();

            //Get rid of any zero chars
            var currentTextString = s.Replace("\0", "");

            //Strip off any leading spaces or colons
            currentTextString = currentTextString.TrimStart(new char[] { ' ', ':' });
            var done = false;
            var quotemode = false;

            while (!done)
            {
                var keywordFound = false;
                if (!quotemode)
                {
                    var token = MicrosoftBasic2.Tokens.FirstOrDefault(p => currentTextString.ToUpper().StartsWith(p.ASCII));
                    if (token != null)
                    {
                        bytes.Add(token.Token);
                        if (currentTextString.Length > token.ASCII.Length)
                        {
                            //Slice the keyword out of t
                            currentTextString = currentTextString.Substring(token.ASCII.Length, currentTextString.Length - token.ASCII.Length).Trim();
                        }
                        else
                        {
                            currentTextString = "";
                        }

                        //Is this a branching keyword
                        if (MicrosoftBasic2.BranchingKeywords.Contains(token.ASCII))
                        {
                            bytes.Add(token.Token);
                            var lineReference = new LineReference(CurrentLine.Tokenized.Count+bytes.Count, "");
                            //Turn line number reference into 2-byte token and tell the parser about it.
                            while (currentTextString.Length > 0 && Utils.IsNumber(currentTextString.Substring(0, 1)))
                            {
                                lineReference.LineNumberOrLabel += currentTextString.Substring(0, 1);
                                currentTextString = currentTextString.Substring(1, currentTextString.Length - 1);
                            }
                            CurrentLine.OtherLineRefs.Add(lineReference);
                            //Add temporary placeholder
                            bytes.Add(0xff);
                            bytes.Add(0xff);
                        }
                        else
                        {
                            //Add normal non-branching token
                            keywordFound = true;
                            break;
                        }
                    }
                }
                if (!keywordFound)
                {
                    if (currentTextString.Length > 0)
                    {
                        //Invert quote mode whenver we hit a " char
                        if (currentTextString[0] == PETSCII.CHAR_QUOTE) quotemode = !quotemode;

                        if (quotemode)
                        {
                            //If we're in quote mode, convert this to printable PETSCII
                           bytes.Add((byte)PETSCII.FixCase(currentTextString[0]));
                        }
                        else
                        {
                            //Otherwise add it as-is
                            bytes.Add((byte)(currentTextString[0]));
                        }

                        //Either way, shave this character off of the current string
                        currentTextString = currentTextString.Substring(1, currentTextString.Length - 1);
                    }
                }
                //If the string is exhausted, we're done processing
                if (currentTextString.Length == 0) done = true;
            }
            return bytes;

        }
    }
}
