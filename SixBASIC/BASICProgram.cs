using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SixBASIC
{
    public class BASICProgram
    {
        public string LastError { get; set; }

        public int StartAddress { get; set; }
        public List<BASIC_Line> Lines { get; set; }

        public BASICProgram()
        {
            Lines = new List<BASIC_Line>();
            StartAddress = 0x0801;
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
        public bool AddLineByText(string line_text)
        {
            bool b = false;
            try
            {
                BASIC_Line bl = new BASIC_Line();
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
                    bl.Line_Number = int.Parse(n);
                    if (Lines.FirstOrDefault(p => p.Line_Number >= bl.Line_Number) == null)
                    {
                        bl.Text = stripped_line;
                        Lines.Add(bl);
                    }
                    else
                    {
                        //Report Collision
                        LastError = "Line Number collision or out of order: " + bl.Line_Number.ToString();
                        b = false;
                    }
                }
                else
                {
                    bl.Line_Number = Lines.Max(p => p.Line_Number) + 1;
                    bl.Text = line_text;
                    Lines.Add(bl);
                }
            }
            catch (Exception )
            {
                //Notify?
            }
            return b;
        }


    }
}
