using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using sixbasiclib.LangSpec;

namespace sixbasiclib
{
    /// <summary>
    /// Produces a tokenized binary with pointers instead of line numbers
    /// In IF THEN GOTO ON GOTO GOSUB
    /// </summary>
    public static class PointerBASICTokenizer
    {

        public static List<OptimizedBASICLine> Lines  = new List<OptimizedBASICLine>();
        public static List<byte> Output = new List<byte>();
        public static int NextLineAddress = 0x0000;
        public static int ThisLineAddress = 0x0000;
        public static OptimizedBASICLine CurrentLine = new OptimizedBASICLine();
        /// <summary>
        /// Tokenizes an entire BASIC program
        /// </summary>
        /// <returns>The tokenize.</returns>
        /// <param name="program">Program.</param>
        /// <param name="compilerSettings">Compiler settings.</param>
        public static byte[] Tokenize(BASICProgram program, ref CompilerSettings compilerSettings)
        {
            Lines = new List<OptimizedBASICLine>();
            NextLineAddress = compilerSettings.BaseAddress;
            ThisLineAddress = NextLineAddress;


            //Process every line in the file to an optimizedBASIC Line.
            foreach (var line in program.Lines)
            {
                CurrentLine = new OptimizedBASICLine()
                {
                    OriginalLine = line,
                    BaseAddress = ThisLineAddress,
                    LineNumber = line.Line_Number
                };
                //Get tokenized bytes for this line
                var tokenizedLineBytes = TokenizeLine(line);
                CurrentLine.Tokenized = tokenizedLineBytes;
            
                //Build the line header (pointer to the next line)
                NextLineAddress += tokenizedLineBytes.Count+2;  //pointer to next line (2 bytes) and line body
                CurrentLine.NextLineAddress = NextLineAddress;
                Lines.Add(CurrentLine);

                ThisLineAddress = NextLineAddress;
            }
            //Audit line positions
            var lineAuditRunningAddress = compilerSettings.BaseAddress;

            foreach (var line in Lines)
            {
                //This line's address should be our running base address
                if (line.BaseAddress != lineAuditRunningAddress)
                {
                    Console.WriteLine($"Invalid base address! {line.BaseAddress} at line {line.LineNumber}");
                }
                //The next line's address should be our running base address + our tokenized bytes length
                if (line.NextLineAddress != lineAuditRunningAddress + line.Tokenized.Count+2)
                {
                    Console.WriteLine($"Invalid next line address! {line.NextLineAddress} at line {line.LineNumber}");
                }
                lineAuditRunningAddress += line.Tokenized.Count+2;
            }

            //Now build the full tokenized prg

            Output = new List<byte>();
            //Adding our load address
            Output.Add((byte)(compilerSettings.BaseAddress & 0x00ff));
            Output.Add((byte)((compilerSettings.BaseAddress & 0xff00) >> 8));
            foreach (var line in Lines)
            {
                Output.Add((byte)(line.NextLineAddress & 0x00ff));
                Output.Add((byte)((line.NextLineAddress & 0xff00) >> 8));
                foreach (var lineref in line.OtherLineRefs)
                {
                    int? refAddress = null;
                    try
                    {
                        refAddress = Lines.FirstOrDefault(p => p.OriginalLine.Line_Number == int.Parse(lineref.LineNumberOrLabel))?.BaseAddress;
                    } 
                    catch (Exception)
                    {
                        Console.WriteLine($"Invalid Line Refrence: {lineref.LineNumberOrLabel}");
                    }
                    if (refAddress != null)
                    {
                        refAddress--;  //pointer needs to be to lineAddr -1
                        lineref.Fulfilled = true;
                        line.Tokenized[lineref.PositionInTokenizedData] = 0x20;
                        line.Tokenized[lineref.PositionInTokenizedData + 1] = (byte)(refAddress & 0x00ff);
                        line.Tokenized[lineref.PositionInTokenizedData + 2] = (byte)((refAddress & 0xff00) >> 8);
                    }
                }
                //Now add our tokenized line
                Output.AddRange(line.Tokenized);
            }
            var serializedLines = JsonConvert.SerializeObject(Lines);
            File.WriteAllText("test.json", serializedLines);
            //Pad end with zeroes
            Output.Add(0x00);
            Output.Add(0x00);

            return Output.ToArray();
        }

        private static List<byte> TokenizeLine(BASIC_Line line)
        {
            //Needs to encode line number and trailing zero after tokenized basic
            var tokenizedLineBytes = new List<byte>();
            //int linenum = GetLineNumber(line); Already done by preproccesor
            var first = true;

            //TODO: Get rid of line numbers entirely for our optimized BASIC

            //Add line number to the beginning of our output
            tokenizedLineBytes.Add((byte)(line.Line_Number & 0x00ff));
            tokenizedLineBytes.Add((byte)((line.Line_Number & 0xff00) >> 8)); 
            //line = StripLineNumber(line);

            //Splits the line on any : characters it finds
            var lineSegments = Utils.SplitLine(line.Text);
            var segBaseinTokenizedBytes = 2;
            var segBaseInOriginalText = 0;
            foreach (var lineSegment in lineSegments)
            {
                if (!first) tokenizedLineBytes.Add(PETSCII.CHAR_COLON);
                var tokenizedSegmentBytes = TokenizeBlock(lineSegment,segBaseinTokenizedBytes,segBaseInOriginalText,first);
                tokenizedLineBytes.AddRange(tokenizedSegmentBytes);
                first = false;
                segBaseinTokenizedBytes = tokenizedLineBytes.Count;
                segBaseInOriginalText += lineSegment.Length;
            }

            tokenizedLineBytes.Add(0x00);
            return tokenizedLineBytes;
        }

        /// <summary>
        /// Tokenizes a block of text
        /// </summary>
        /// <returns>The block.</returns>
        /// <param name="s">S.</param>
        public static List<byte> TokenizeBlock(string s, int segmentBaseInTokenizedBytes, int segmentBaseInOriginalText, bool first)
        {
            var bytes = new List<byte>();
            //Get rid of any zero chars
            var cleanedS = s.Replace("\0", "");
            var currentTextString = cleanedS.TrimStart(new char[] { ' ', ':' });
            var done = false;
            var quotemode = false;

            while (!done)
            {
                var keywordFound = false;
                if (!quotemode)
                {
                    //Is this a token?
                    var token = MicrosoftBasic2.Tokens.FirstOrDefault(p => currentTextString.ToUpper().StartsWith(p.ASCII, StringComparison.CurrentCulture));
                    if (token != null)
                    {
                        bytes.Add(token.Token);
                        keywordFound = true;
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
                            var processLineRef = true;
                            if (token.ASCII == "THEN")
                            {
                                var cleanForNextToken = currentTextString.ToUpper().TrimStart();
                                processLineRef = (Utils.IsNumber(cleanForNextToken.Substring(0, 1)));
                            }
                            if (processLineRef)
                            {
                                //                            bytes.Add(token.Token);
                                var lineReference = new LineReference()
                                {
                                    PositionInText = segmentBaseInOriginalText + (cleanedS.Length - currentTextString.Length),
                                    //If we are farther into our line than the first statement, add one to our base for the colon
                                    PositionInTokenizedData = (!first?segmentBaseInTokenizedBytes+1 : segmentBaseInTokenizedBytes) + bytes.Count,
                                    Fulfilled = false,
                                    LineNumberOrLabel = ""
                                };

                                //Turn line number reference into 2-byte token and tell the parser about it.
                                while (currentTextString.Length > 0 && Utils.IsNumber(currentTextString.Substring(0, 1)))
                                {
                                    lineReference.LineNumberOrLabel += currentTextString.Substring(0, 1);
                                    currentTextString = currentTextString.Substring(1, currentTextString.Length - 1);
                                }
                                CurrentLine.OtherLineRefs.Add(lineReference);
                                //Add a space and a temporary placeholder
                                bytes.Add(0x69); 
                                bytes.Add(0x69);
                                bytes.Add(0x69);
                            }
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
