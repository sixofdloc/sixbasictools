using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SixBASIC
{
    public class Detokenizer
    {


        public string Detokenize(BASIC_Line input)
        {
            string s = input.Line_Number.ToString()+" ";
            bool quotemode = false;
            for (int i = 0; i < input.Line_Data.Length; i++)
            {
                if (quotemode)
                {
                    //if (input.Line_Data[i] >= 0xc0 && input.Line_Data[i] <= 0xdf) s=s+ (char)(input.Line_Data[i] - 0x60);
                    s = s + PETSCII.Table[input.Line_Data[i]];
                    //s = s + Encoding.ASCII.GetString(new byte[]{input.Line_Data[i]});
                    if (input.Line_Data[i] == 0x22) quotemode = false;
                }
                else
                {
                    if (input.Line_Data[i] == 0x22)
                    {
                        quotemode = true;
                        s = s + "\"";
                    }
                    else
                    {
                        MicrosoftBasic2Token token = MicrosoftBasic2.Tokens.FirstOrDefault(p=>p.Token.Equals(input.Line_Data[i]));
                        if (token != null)
                        {
                            s = s + ((token.LeadingSpace && (s.Substring(s.Length-1,1)!=" "))?" ":"")+ token.ASCII + ((token.TrailingSpace && (i<input.Line_Data.Length && (input.Line_Data[i+1]!=0x20)))?" ":"");
                        }
                        else
                        {
                            s = s + Encoding.ASCII.GetString(new byte[] { input.Line_Data[i] });
                        }
                    }
                }
            }

            return s;
        }
    }
}
