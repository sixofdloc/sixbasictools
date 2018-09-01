using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SixBASIC
{
    public class BASICProgram
    {
        public string LastError { get; set; }

        public List<BASIC_Line> Lines { get; set; }

        public BASICProgram()
        {
            Lines = new List<BASIC_Line>();
        }

        public bool AddLine(int line_number, string line_text)
        {
            bool b = false;
            if (Lines.FirstOrDefault(p => p.Line_Number >= line_number) == null)
            {
                Lines.Add(new BASIC_Line() { Line_Number = line_number, Text = line_text });
                b = true;
            } else {
                //Report LIne number collision
                LastError = "Line Number collision or out of order: " + line_number.ToString();
                b = false;
            }
            return b;
        }

        public bool AddLine(int line_number, byte[] line_data)
        {
            bool b = false;
            if (Lines.FirstOrDefault(p => p.Line_Number>=line_number) == null)
            {
                Lines.Add(new BASIC_Line() { Line_Number = line_number, Line_Data = line_data });
                b = true;
            } else {
                //Report Line number collision
                LastError = "Line Number collision or out of order: " + line_number.ToString();
                b = false;
            }
            return b;
        }

        public bool AddLineByData(byte[] bytes)
        {
            if (bytes.Length < 2) return false;
            if (bytes[bytes.Length - 1] != 0) return false;
            BASIC_Line bl = new BASIC_Line();
            bl.Line_Number = bytes[0] + (bytes[1] * 256);
            bl.Line_Data = new byte[bytes.Length - 3];
            for (int i = 2; i < bytes.Length - 3; i++)
            {
                bl.Line_Data[i - 2] = bytes[i];
            }
            if (Lines.FirstOrDefault(p => p.Line_Number >= bl.Line_Number) == null)
            {

                Lines.Add(bl);
            }
            else
            {
                //Report collision
                LastError = "Line Number collision or out of order: " + bl.Line_Number.ToString();
                return false;
            }
            return true;

        }

        //Add a line by text, either with or without a line number.
        //If the line does not begin with a line number, it will use the next available.
		public bool AddLineByText(string line_text, List<Label>labels, int sourceFileLine)
        {
            bool b = false;
            try
            {
				BASIC_Line bl = new BASIC_Line(line_text,labels,sourceFileLine);
				if (bl.Line_Number == -1)
				{
					try
					{
						bl.Line_Number = Lines.Max(p => p.Line_Number) + 1;
					} catch (Exception){
						bl.Line_Number = 1;
					}
					Lines.Add(bl);
					b = true;
				}
				else
				{
					var line = Lines.FirstOrDefault(p => p.Line_Number >= bl.Line_Number);
					if ( line == null)
					{
						Lines.Add(bl);
						b = true;
					}
					else
					{
						//Report Collision
						LastError = "Line Number collision or out of order: " + bl.Line_Number.ToString();
						b = false;
					}
				}
            }
            catch (Exception )
            {
                //Notify?
            }
            return b;
        }

		public bool DoAnyLinesReferTo(BASIC_Line line)
		{
			var referenceFound = false;
			if (line.Line_Number > -1)
			{
				referenceFound = (Lines.Count(l => l.RefersToLine(line.Line_Number)) > 0);
			}
			if (line.Labels.Count > 0)
			{
				foreach (var label in line.Labels)
				{
					if (Lines.Count(ln => ln.RefersToLabel(label.LabelName)) > 0)
					{
						referenceFound = true;
						break;
					}
				}
			}
			return referenceFound;
		}


		public void Dump(string fileName){
			using (StreamWriter writer = new StreamWriter(fileName))
			{
				foreach (var line in Lines)
				{
					writer.WriteLine(line.SourceFileLine + " |"+line.Line_Number.ToString()+"| : "+line.Text);
				}

			}            
		}


    }
}
