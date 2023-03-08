using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sixbasiclib
{
	public class BASIC_Line
	{
		//The Basics
		public bool Initialized = false;
		public int Line_Number { get; set; }
		public string Text { get; set; } = string.Empty;
        //Labels that refer to this line
		public List<Label> Labels { get; set; } = new List<Label>();

        //Line from source file
		public int SourceFileLine { get; set; }

        //Analyzed data
		public byte[] Line_Data { get; set; } = new byte[0];
		public List<Element> Elements {get; set;} = new List<Element>();
		public List<LineReference> LineReferences { get; set; } = new List<LineReference>();

		public BASIC_Line(){
		    Elements = new List<Element>();
			LineReferences = new List<LineReference>();
			Labels = new List<Label>();
			Initialized = false;
		}

		public static BASIC_Line Copy(BASIC_Line line){
			var returnLine = new BASIC_Line()
			{
				Initialized = line.Initialized,
				Line_Number = line.Line_Number,
				Text = line.Text,
				SourceFileLine = line.SourceFileLine,
				Labels = new List<Label>()
			};
			foreach (var label in line.Labels){
				returnLine.Labels.Add(new Label() { ElementType = label.ElementType, LabelName = label.LabelName, Original = label.Original, Processed = label.Processed });
			}
			return returnLine;
		}

		public BASIC_Line(string line_text, List<Label> labels, int sourceFileLine)
		{
			LineReferences = new List<LineReference>();
			Elements = new List<Element>();
			Labels = labels;
			Line_Data = new byte[0];
			SourceFileLine = sourceFileLine;
			string stripped_line = "";
            string work_line = Utils.TrimLead(line_text);
            if (Utils.IsNumber(work_line.Substring(0, 1)))
            {
                string n = "";
                for (int i = 0; i < work_line.Length; i++)
                {
                    if (Utils.IsNumber(work_line.Substring(i, 1)))
                    {
                        n = n + work_line.Substring(i, 1);
                    }
                    else
                    {
                        stripped_line = work_line.Substring(i, work_line.Length - i);
                        break;
                    }
                }
                Line_Number = int.Parse(n);
				Text = stripped_line;
                Initialized = true;
            }
            else
            {
                Line_Number = -1;
                Text = line_text;
				Initialized = true;
            }
	
		}



		//public  List<byte> Tokenize()
        //{
        //    //Needs to encode line number and trailing zero after tokenized basic
        //    List<byte> output = new List<byte>();
        //    //int linenum = GetLineNumber(line); Already done by preproccesor
        //    bool first = true;
        //    output.Add((byte)(Line_Number & 0x00ff));
        //    output.Add((byte)((Line_Number & 0xff00) >> 8)); //placeholder bytes for next line number
        //    //line = StripLineNumber(line);

        //    List<String> segments = Utils.SplitLine(Text);
        //    foreach (string segment in segments)
        //    {
        //        if (!first) output.Add(0x3A);
        //        List<byte> bytes = Tokenizer.TokenizeBlock(segment);
        //        foreach (byte b in bytes)
        //        {
        //            output.Add(b);
        //        }
        //        first = false;
        //    }

        //    output.Add(0x00);
        //    return output;
        //}

		public bool ContainsBranch(){
			//returns true if this line contains any branching logic
			//GOTO, IF THEN, RETURN
			var branchFound = false;
			var searchText = Utils.StripdownToBasics(Text).ToUpper();
			branchFound = searchText.Contains("GOTO");
			if (!branchFound) branchFound = searchText.Contains("THEN");
			if (!branchFound) branchFound = searchText.Contains("RETURN");
			if (!branchFound) branchFound = searchText.Contains("STOP");
			if (!branchFound) branchFound = searchText.Contains("END");
			return branchFound;
		}

		public bool ContainsREM(){
			//returns true if this line contains a REM statement
			return Utils.StripdownToBasics(Text).ToUpper().Contains("REM");
		}

		public bool CanBeAppendedTo(){
			return !(ContainsBranch() || ContainsREM());
		}

		public bool FindReference(int lineNumber, string statement){
			bool foundLine = false;
			var textUpper = Utils.StripdownToBasics(Text).ToUpper().Trim();
            //Strip Quoted strings

            var lineText = lineNumber.ToString();
            while (textUpper.Contains(statement.ToUpper()) && !foundLine)
            {
                textUpper = textUpper.Substring(textUpper.IndexOf(statement.ToUpper()) + statement.Length).Trim();
                if (textUpper == lineText) foundLine = true;
                if (!foundLine)
                {
					var numberFound = Utils.GetNumberFromString(textUpper, 0);
					foundLine = (numberFound == lineText);
                }
            }
			return foundLine;
		}

		public bool RefersToLine(int lineNumber){
			//Returns true if this line refers to the specified line #
			// GOTO ###, GOSUB ###, THEN ###
			var foundline = false;
			foundline = FindReference(lineNumber, "GOTO");
			if (!foundline) foundline = FindReference(lineNumber, "GOSUB");
			if (!foundline) foundline = FindReference(lineNumber, "THEN");
			return foundline;
		}

		public bool RefersToLabel(string label){
			//Returns true if this line refers to the specified label
			return (Utils.RemoveQuotedStrings(Text).Contains("{:" + label + "}"));
		}
    }
}
