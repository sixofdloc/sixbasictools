using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SixBASIC
{
    public class PreProcessor
    {
		//TODO:Finish building out Renumbering system		
		//TODO:Finish building out Renumbering system       
        //foreach line
        //Do any lines refer to it?
        //If not, give it the next available line_number
        //If other lines refer to it (GOTO, GO TO, THEN, GOSUB), update them
		//        int Line_Number_Start = 1;
		//        int Line_Number_Increment = 1;
		//        bool renumber;

  
        
        public BASICProgram PreProcess(List<string> prglines)
        {
            BASICProgram pr = new BASICProgram();
            List<string> processlines = prglines;
            //Figure load address
            string s = processlines.FirstOrDefault(p => p.Contains("{loadaddr:"));
            if (s == null)
            {
                pr.StartAddress = 0x0801;
            }
            else
            {
                int i = s.IndexOf("{loadaddr:")+10;
                int j = s.IndexOf("}");
                pr.StartAddress = int.Parse( s.Substring(i,j-i)  );
                processlines.Remove(s);
            }
            //Renumber?
            s = processlines.FirstOrDefault(p => p.Contains("{renumber}"));
            if (s == null)
            {
				//TODO:Finish building out Renumbering system       
 //               renumber = false;
            }
            else
            {
				//TODO:Finish building out Renumbering system       
//                renumber = true;
                processlines.Remove(s);
            }

            //Parse the rest of our tags.
            foreach (string line in processlines)
            {
                if (Utils.TrimLead(line).StartsWith("'"))
                {
                    //It's a comment.
                }
                else
                {
                    string result = line;
                    //Get the obvious tags out of the way.
                    result = ParseTags(result);
                    pr.AddLineByText(result);
                }
            }

            return pr;
        }


        //Parses stock verbose PETSCII tags, and {$ff} style char insertion tags
        private string ParseTags(string s)
        {
            string t = s;
            for (int i = 0; i < PETSCII.Macros.Length; i++)
            {
                t = t.Replace(PETSCII.Macros[i].Text, PETSCII.Macros[i].Replacement);
            }
            string r = t;
            for (int i = 0; i < t.Length; i++)
            {
                if (t[i] == ('{'))
                {
                    if (i <= (t.Length - 5))
                    {
                        if ((t[i + 1] == '$') && (t[i + 4] == '}'))
                        {
                            try
                            {
                                int q = Convert.ToInt32(t.Substring(i + 1, 3).ToUpper().Replace("$", ""), 16);
                                String qs = new String((char)q, 1);
                                r = r.Replace(t.Substring(i, 5), qs);
                            }
                            catch (Exception )
                            {
								//TODO: Decide what to do with conversion errors       
                                //Log?
                            }
                        }
                    }
                    else
                    {
                        //Invalid token
                    }
                }
            }
            t = r;
            return t;
        }


        



    }
}
