//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SixBASIC.Compiler
//{
//    public class AnalyzerMessage
//    {
//        public int TextLine { get; set; }
//        public int BASICLine { get; set; }
//        public string Message { get; set; }
//    }
//    public class Analyzer
//    {
//        public List<AnalyzerMessage> strings = new List<AnalyzerMessage>();
//        public List<AnalyzerMessage> tokens = new List<AnalyzerMessage>();
//        public List<AnalyzerMessage> line_refs = new List<AnalyzerMessage>();
//        public List<AnalyzerMessage> variables = new List<AnalyzerMessage>();
//        public List<AnalyzerMessage> remarks = new List<AnalyzerMessage>();


//        int textline = 0;
//        int basicline = 0;
        
//        public void Analyze(BASICProgram program)
//        {
//            strings = new List<AnalyzerMessage>();
//            line_refs = new List<AnalyzerMessage>();
//            variables = new List<AnalyzerMessage>();
//            remarks = new List<AnalyzerMessage>();
//            tokens = new List<AnalyzerMessage>();
//            textline = 0;
//            basicline = 0;
//            foreach (BASIC_Line line in program.Lines)
//            {
//                AnalyzeLine(line);
//                textline++;
//            }
//        }

//        public void AnalyzeLine(BASIC_Line line)
//        {
//            basicline = line.Line_Number;
//            List<String> segments = Utils.SplitLine(line.Text);
//            foreach (string segment in segments)
//            {
//                AnalyzeSegment(segment);
//            }
//        }

//        public void AnalyzeSegment(string segment)
//        {
//            string t = segment.Replace("\0", "");
//            t = t.TrimStart(new char[] { ' ', ':' });
//            bool done = false;
//            bool quotemode = false;
//            bool onflag = false;
//            bool remmode = false;    
//            string v = "";
//            while (!done)
//            {
//                MicrosoftBasic2Token toke = null;
//                bool found = false;
//                if (!quotemode)
//                {
//                    foreach (MicrosoftBasic2Token token in MicrosoftBasic2.Tokens)
//                    {
//                        if (t.ToUpper().StartsWith(token.ASCII.ToUpper())){
                        
//                            tokens.Add(new AnalyzerMessage() { BASICLine = basicline, TextLine = textline, Message = token.ASCII });
//                            if (v.Length>0) {
//                                variables.Add(new AnalyzerMessage() { BASICLine = basicline, TextLine = textline, Message = v });
//                                v = "";
//                            }
//                            if (t.Length > token.ASCII.Length)
//                            {
//                                t = t.Substring(token.ASCII.Length, t.Length - token.ASCII.Length);
//                            }
//                            else
//                            {
//                                t = "";
//                            }
//                            toke = token;
//                            found = true;
//                            break;
//                        }
//                    }
//                }
//                if (found)
//                {
//                    if (toke.ASCII == "ON")
//                    {
//                        onflag = true;
//                    }
//                    else
//                    {
//                        if ((toke.ASCII == "GOTO"))
//                        {
//                            if (onflag)
//                            {
//                                List<string> strings = Utils.GetNumberListFromString(t, 0);
//                                foreach (string st in strings) line_refs.Add(new AnalyzerMessage() { BASICLine = basicline, TextLine = textline, Message = st });
//                                onflag = false;
//                            }
//                            else
//                            {
//                                if (Utils.IsNumber(t.Trim().Substring(0, 1)))
//                                {
//                                    string sr = Utils.GetNumberFromString(t.Trim(), 0);
//                                    line_refs.Add(new AnalyzerMessage() { BASICLine = basicline, TextLine = textline, Message = sr });
//                                    t = t.Substring(sr.Length - 1, t.Length - sr.Length);
//                                }
//                            }
//                        }
//                        else
//                        {
//                            if ((toke.ASCII == "GOSUB") || (toke.ASCII == "THEN"))
//                            {
//                                if (Utils.IsNumber(t.Trim().Substring(0, 1)))
//                                {
//                                    string sr = Utils.GetNumberFromString(t.Trim(), 0);
//                                    line_refs.Add(new AnalyzerMessage() { BASICLine = basicline, TextLine = textline, Message = sr });
//                                    t = t.Substring(sr.Length - 1, t.Length - sr.Length);
//                                }
//                            }
//                            else
//                            {
//                                if (toke.ASCII == "REM") remmode = true;
//                            }
//                        }
//                    }
//                }
//                else
//                {
//                    if (remmode)
//                    {
//                        remarks.Add(new AnalyzerMessage() { BASICLine = basicline, TextLine = textline, Message = t });
//                        t = "";

//                    }
//                    else
//                    {
//                        if (t.Length > 0)
//                        {
//                            if (t[0] == 0x22) quotemode = !quotemode;
//                            if (quotemode)
//                            {
//                                string str = Utils.ExtractFirstString(t);
//                                strings.Add(new AnalyzerMessage() { BASICLine = basicline, TextLine = textline, Message = str });
//                                if (textline == 219)
//                                {
//                                    Console.WriteLine("Freeze");
//                                }
//                                t = t.Substring(str.Length, t.Length - str.Length);
//                                quotemode = false;
//                            }
//                            else
//                            {
//                                if (Utils.IsNumber(t.Substring(0, 1)))
//                                {
//                                    if (v != "")
//                                    {
//                                        v = v + t.Substring(0, 1);
//                                        if (t.Length > 1)
//                                        {
//                                            if ("$%".Contains(t.Substring(1, 1)))
//                                            {
//                                                v = v + t.Substring(1, 1);
//                                                t = t.Substring(1, t.Length - 1);
//                                            }
//                                        } 
//                                        variables.Add(new AnalyzerMessage() { BASICLine = basicline, TextLine = textline, Message = v });
//                                        v = "";
//                                    }
//                                    else
//                                    {
//                                        v = "";
//                                    }
//                                }
//                                else
//                                {
//                                    //                        bytes.Add((byte)(t[0]));
//                                    if ("ABCDEFGHIJKLMNOPQRSTUVWXYZ".Contains(t.Substring(0, 1)))
//                                    {
//                                        if (v != "")
//                                        {
//                                            v = v + t.Substring(0, 1);
//                                            if (t.Length > 1)
//                                            {
//                                                if ("$%".Contains(t.Substring(1, 1)))
//                                                {
//                                                    v = v + t.Substring(1,1);
//                                                    t = t.Substring(1, t.Length - 1);
//                                                }
//                                            } 
//                                            variables.Add(new AnalyzerMessage() { BASICLine = basicline, TextLine = textline, Message = v });
//                                            v = "";
//                                        }
//                                        else
//                                        {
//                                            v = t.Substring(0, 1);
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                        if (t.Length > 0) t = t.Substring(1, t.Length - 1);
//                    }

//                    if (t.Length == 0) done = true;
//                }
//            }

//        }

//    }
//}
