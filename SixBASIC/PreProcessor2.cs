//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SixBASIC
//{
//    public class PreProcessor2
//    {
//        int Line_Number_Start = 1;
//        int Line_Number_Increment = 1;

//        //Configurables
//        bool renumber = false;
//        int StartAddress = 0x0801;
//        bool remmode = false;
//        bool removewhitespace = false;


//        int Current_Program_Line = 0;
//        List<CompilerElementRow> output;
//        List<ReplacementMacro> Variables;
//        List<ReplacementMacro> Constants;
  
        
//        public List<CompilerElementRow> PreProcess(List<string> prglines)
//        {
//            output = new List<CompilerElementRow>();
//            Variables = new List<ReplacementMacro>();
//            Constants = new List<ReplacementMacro>();
//            List<string> processlines = prglines;
//            //Figure load address
//            string s = processlines.FirstOrDefault(p => p.Contains("{loadaddr:"));
//            if (s == null)
//            {
//                StartAddress = 0x0801;
//            }
//            else
//            {
//                int i = s.IndexOf("{loadaddr:")+10;
//                int j = s.IndexOf("}");
//                StartAddress = int.Parse( s.Substring(i,j-i)  );
//                processlines.Remove(s);
//            }
//            //Renumber?
//            s = processlines.FirstOrDefault(p => p.Contains("{renumber}"));
//            if (s == null)
//            {
//                renumber = false;
//            }
//            else
//            {
//                renumber = true;
//                processlines.Remove(s);
//            }
//            //Remove Whitespace?
//            s = processlines.FirstOrDefault(p => p.Contains("{remove whitespace}"));
//            if (s == null)
//            {
//                removewhitespace = false;
//            }
//            else
//            {
//                removewhitespace = true;
//                processlines.Remove(s);
//            }

//            //Parse the rest of our tags.
//            foreach (string line in processlines)
//            {
//                if (line.Trim() != "")
//                {
//                    CompilerElementRow cer = PreProcessLine(line);
//                    if (cer.Elements.Count > 0)
//                    {
//                        output.Add(cer);
//                    }
//                }
//                Current_Program_Line++;

//            }
//            return output;
//        }

//        public CompilerElementRow PreProcessLine(string line)
//        {
//            CompilerElementRow result = new CompilerElementRow();
//            string ln = line.Trim();
//            if (ln != "")
//            {
//                if (!ln.StartsWith("'"))
//                {
//                    if (Utils.IsNumber(ln.Substring(0, 1)))
//                    {
//                        string lnstr = Utils.GetNumberFromString(ln, 0);
//                        result.Elements.Add(new Position_Label() { Program_Line = Current_Program_Line, Line_Number = int.Parse(lnstr), Original = lnstr });
//                        if (ln.Length > lnstr.Length)
//                        {
//                            ln = ln.Substring(lnstr.Length, ln.Length - lnstr.Length);
//                        }
//                    }
//                    else
//                    {
//                        //Is there a label here?
//                        if (ln.StartsWith("{"))
//                        {
//                            string mac = Utils.ExtractMacro(ln);
//                            if (mac != "")
//                            {
//                                switch (mac.Substring(0, 1))
//                                {
//                                    case ":":
//                                        result.Elements.Add(new Position_Label() { Program_Line = Current_Program_Line,  Label = mac, Line_Number = -1, Original = mac });
//                                        ln = ln.Substring(mac.Length + 2, ln.Length - (mac.Length + 2));
//                                        break;
//                                    case "%":
//                                        if (mac.Contains("="))
//                                        {
//                                            string[] smac = mac.Split('=');
//                                            if (Variables.FirstOrDefault(p => p.Macro.ToUpper().Equals(smac[0].ToUpper())) == null)
//                                            {
//                                                Variables.Add(new ReplacementMacro() { Macro = smac[0], Replacement = smac[1] });
//                                                ln = ln.Substring(mac.Length + 2, ln.Length - (mac.Length + 2));
//                                            }
//                                            else
//                                            {
//                                                //throw redefined variable error
//                                            }
//                                        }
//                                        break;
//                                    case "_":
//                                        if (mac.Contains("="))
//                                        {
//                                            string[] smac = mac.Split('=');
//                                            if (Constants.FirstOrDefault(p => p.Macro.ToUpper().Equals(smac[0].ToUpper())) == null)
//                                            {
//                                                Constants.Add(new ReplacementMacro() { Macro = smac[0], Replacement = smac[1] });
//                                                ln = ln.Substring(mac.Length + 2, ln.Length - (mac.Length + 2));
//                                            }
//                                            else
//                                            {
//                                                //throw redefined constant error
//                                            }
//                                        }
//                                        break;
//                                }
//                            }
//                        }
//                        else
//                        {
//                            //Add dummy position label
//                            //Why do this?  Fuck this shit.
//                            //result.Elements.Add(new Position_Label() { Program_Line = Current_Program_Line, Line_Number = -1 });
//                        }
//                    }
//                    if (ln.Length > 0)
//                    {
//                        List<String> segments = Utils.SplitLine(ln);
//                        remmode = false;
//                        for (int i = 0; i < segments.Count; i++)
//                        {
//                            if (remmode)
//                            {
//                                ((REM_Comment)(result.Elements.Last(p => p.ElementType.Equals(CompilerElementTypes.REM_Comment)))).Original += segments[i];
//                            }
//                            else
//                            {
//                                result.Elements.AddRange(PreProcessSegment(segments[i]));
//                            }
//                            if (remmode)
//                            {
//                                ((REM_Comment)(result.Elements.Last(p => p.ElementType.Equals(CompilerElementTypes.REM_Comment)))).Original += ":";
//                            }
//                            else
//                            {
//                                if (i < segments.Count - 1) result.Elements.Add(new Punctuation() { Character = ":" });
//                            }
//                        }
//                    }
//                }
//            }
//            return result;
//        }

//        string Segment;
//        List<CompilerElement> ProcessedSegment;
//        bool OnFlag = false;
//        MicrosoftBasic2Token Toke = null;
//        bool FoundToken = false;

//        private bool CheckTokens()
//        {
//            FoundToken = false;

//            foreach (MicrosoftBasic2Token token in MicrosoftBasic2.Tokens)
//            {
//                if (Segment.ToUpper().StartsWith(token.ASCII.ToUpper()))
//                {
//                    ProcessedSegment.Add(new Keyword() { Token = token, Original = token.ASCII });
//                    //tokens.Add(new AnalyzerMessage() { BASICLine = basicline, TextLine = textline, Message = token.ASCII });
//                    if (Segment.Length > token.ASCII.Length)
//                    {
//                        Segment = Segment.Substring(token.ASCII.Length, Segment.Length - token.ASCII.Length);
//                    }
//                    else
//                    {
//                        Segment = "";
//                    }
//                    Toke = token;
//                    FoundToken = true;
//                    break;
//                }
//            }
//            if (FoundToken)
//            {
//                if (Toke.ASCII == "ON")
//                {
//                    OnFlag = true;
//                }
//                else
//                {
//                    if ((Toke.ASCII == "GOTO"))
//                    {
//                        if (OnFlag)
//                        {
//                            List<string> strings = Utils.GetNumberListFromString(Segment, 0);
//                            for (int i = 0; i < strings.Count; i++)
//                            {
//                                ProcessedSegment.Add(new Line_Reference() { Number = int.Parse(strings[i]), Original = strings[i], Label = "" });
//                                if (i < strings.Count - 1) ProcessedSegment.Add(new Punctuation() { Character = "," });
//                                //line_refs.Add(new AnalyzerMessage() { BASICLine = basicline, TextLine = textline, Message = st });
//                            }
//                            OnFlag = false;
//                        }
//                        else
//                        {
//                            if (Utils.IsNumber(Segment.Trim().Substring(0, 1)))
//                            {
//                                string sr = Utils.GetNumberFromString(Segment.Trim(), 0);
//                                ProcessedSegment.Add(new Line_Reference() { Number = int.Parse(sr), Original = sr, Label = "" });
//                                //                                    line_refs.Add(new AnalyzerMessage() { BASICLine = basicline, TextLine = textline, Message = sr });
//                                Segment = Segment.Substring(sr.Length - 1, Segment.Length - sr.Length);
//                            }
//                        }
//                    }
//                    else
//                    {
//                        if ((Toke.ASCII == "GOSUB") || (Toke.ASCII == "THEN"))
//                        {
//                            if (Utils.IsNumber(Segment.Trim().Substring(0, 1)))
//                            {
//                                string sr = Utils.GetNumberFromString(Segment.Trim(), 0);
//                                ProcessedSegment.Add(new Line_Reference() { Number = int.Parse(sr), Original = sr , Label = ""});
//                                //line_refs.Add(new AnalyzerMessage() { BASICLine = basicline, TextLine = textline, Message = sr });
//                                Segment = Segment.Substring(sr.Length - 1, Segment.Length - sr.Length);
//                            }
//                        }
//                        else
//                        {
//                            if (Toke.ASCII == "REM")
//                            {
//                                remmode = true;
//                                ProcessedSegment.Add(new REM_Comment() { Original = Segment });
//                                Segment = "";
//                            }
//                        }
//                    }
//                }
//            }

//            return FoundToken;
//        }

//        private bool CheckSpace()
//        {
//            if (Segment.Length > 0)
//            {
//                if (Segment.Substring(0, 1) == " ")
//                {
//                    if (!removewhitespace) ProcessedSegment.Add(new Whitespace() { Number_Of_Spaces = 1, Original = " " });
//                    Segment = Segment.Substring(1, Segment.Length - 1);
//                    return true;
//                }
//            }
//            return false;
//        }

//        private bool CheckString()
//        {
//            if (Segment.Length > 0)
//            {
//                if (Segment.Substring(0, 1) == "\"")
//                {
//                    string str = Utils.ExtractString(Segment);
//                    ProcessedSegment.Add(new Text_String() { Original = str });
//                    Segment = Segment.Substring(str.Length, Segment.Length - str.Length);
//                    return true;
//                }
//            }
//            return false;
//        }

//        private bool CheckVariable()
//        {
//            int nc = 0;
//            int vt = VariableTypes.Integer;
//            if (Segment.Length > 0)
//            {
//                if ("ABCDEFGHIJKLMNOPQRSTUVWXYZ".Contains(Segment.Substring(0, 1).ToUpper()))
//                {
//                    if (Segment.Length > 1)
//                    {
//                        if ("ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890$%".Contains(Segment.Substring(1, 1).ToUpper()))
//                        {
//                            //Double-char variable, may have $ or %?
//                            if ("$%".Contains(Segment.Substring(1, 1).ToUpper()))
//                            {
//                                //Single-char variable with $ or %
//                                nc = 2;
//                                vt = (Segment.Substring(1, 1).Equals("$")) ? VariableTypes.String : VariableTypes.Float;
//                            }
//                            else
//                            {
//                                if (Segment.Length > 2)
//                                {
//                                    if ("$%".Contains(Segment.Substring(2, 1).ToUpper()))
//                                    {
//                                        //Double-char variable with $ or % at the end
//                                        nc = 3;
//                                        vt = (Segment.Substring(1, 1).Equals("$")) ? VariableTypes.String : VariableTypes.Float;
//                                    }
//                                    else
//                                    {
//                                        //Double-char variable, no $ or %
//                                        nc = 2;
//                                    }
//                                }
//                                else
//                                {
//                                    //Double-char variable, no $ or %
//                                    nc = 2;
//                                }
//                            }
//                        }
//                        else
//                        {
//                            //Single-char variable no % or $
//                            nc = 1;
//                        }
//                    }
//                    else
//                    {
//                        //Single-Char variable, no % or $
//                        nc = 1;
//                    }
//                }
//                else
//                {
//                    //Not a variable
//                }
//                if (nc > 0)
//                {
//                    ProcessedSegment.Add(new Variable() { VariableName = Segment.Substring(0, nc), VariableType = vt, Original = Segment.Substring(0, nc) });
//                    Segment = Segment.Substring(nc, Segment.Length - nc);
//                    return true;
//                }
//            }
//            return false;
//        }

//        private bool CheckNumber()
//        {
//            if (Segment.Length > 0)
//            {
//                if (Utils.IsNumber(Segment.Substring(0, 1)))
//                {
//                    String nstr = Utils.GetNumberFromString(Segment, 0);
//                    ProcessedSegment.Add(new Numeric_Constant() { Original = nstr });
//                    Segment = Segment.Substring(nstr.Length, Segment.Length - nstr.Length);
//                    return true;
//                }
//            }
//            return false;
//        }

//        private bool CheckPunctuation()
//        {
//            if (Segment.Length > 0)
//            {
//                if ("#();,".Contains(Segment.Substring(0, 1)))
//                {
//                    ProcessedSegment.Add(new Punctuation() { Character = Segment.Substring(0, 1), Original = Segment.Substring(0, 1) });
//                    Segment = Segment.Substring(1, Segment.Length - 1);
//                    return true;
//                }
//            }
//            return false;
//        }

//        private bool CheckMacro()
//        {
//            if (Segment.Length > 0)
//            {
//                if (Segment.StartsWith("{"))
//                {
//                    string mac = Utils.ExtractMacro(Segment);
//                    if (mac.Length > 0)
//                    {
//                        switch (mac.Substring(0, 1))
//                        {
//                            case ":":
//                                //Line Reference
//                                ProcessedSegment.Add(new Line_Reference() { Original = mac, Label = mac });
//                                Segment = Segment.Substring(mac.Length+2, Segment.Length - (mac.Length+2));
//                                return true;
//                                break;
//                            case "%":
//                                //Variable reference
//                                ReplacementMacro rm = Variables.FirstOrDefault(p => p.Macro.ToUpper().Equals(mac.ToUpper()));
//                                if (rm != null)
//                                {
//                                    Segment = Segment.Substring(mac.Length+2, Segment.Length - (mac.Length+2));
//                                    Segment = rm.Replacement + Segment;
//                                    return true;
//                                }
//                                else
//                                {
//                                    //throw variable not defined error
//                                }
//                                break;
//                            case "_":
//                                //String/Int constant definition or reference
//                                rm = Constants.FirstOrDefault(p => p.Macro.ToUpper().Equals(mac.ToUpper()));
//                                if (rm != null)
//                                {
//                                    Segment = Segment.Substring(mac.Length+2, Segment.Length - (mac.Length+2));
//                                    Segment = rm.Replacement + Segment;
//                                    return true;
//                                }
//                                else
//                                {
//                                    //throw variable not defined error
//                                }
//                                break;
//                        }
//                    }
//                }
//            }
//            return false;
//        }

//        public List<CompilerElement> PreProcessSegment(string segment)
//        {
//            Segment = segment.Replace("\0", "");
//            ProcessedSegment = new List<CompilerElement>();
//            bool done = false;
//            OnFlag = false;
//            Toke = null;
//            FoundToken = false;
//            while (!done)
//            {
//                if (Segment.Length == 0)
//                {
//                    done = true;
//                }
//                else
//                {
//                    //Process what's left
//                    if (!CheckMacro())
//                    {
//                        if (!CheckSpace())
//                        {
//                            if (!CheckTokens())
//                            {
//                                if (!CheckString())
//                                {
//                                    if (!CheckVariable())
//                                    {
//                                        if (!CheckNumber())
//                                        {
//                                            if (!CheckPunctuation())
//                                            {
//                                                Console.WriteLine("Unparseable: '" + Segment.Substring(0, 1) + "' in BASIC Line ");
//                                                Segment = Segment.Substring(1, Segment.Length - 1);
//                                            }
//                                        }
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            return ProcessedSegment;
//        }





//    }
//}
