using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SixBASIC
{
    public static class Utils
    {
        public static bool IsNumber(string s)
        {
            return "0123456789".Contains(s);
        }

        public static string TrimLead(string s)
        {
            return s.TrimStart(new char[] { ' ', '\t' });
        }

        public static string GetNumberFromString(string s, int start)
        {
            string n = "";
            for (int i = start; i < s.Length; i++)
            {
                if (IsNumber(s.Substring(i, 1)))
                {
                    n = n + s.Substring(i, 1);
                }
                else
                {
                    if (n != "") break;
                }
            }
            return n;
        }

        public static List<string> GetNumberListFromString(string s, int start)
        {
            List<string> slist = new List<string>();
            string n = "";
            for (int i = start; i < s.Length; i++)
            {
                if (IsNumber(s.Substring(i, 1)))
                {
                    n = n + s.Substring(i, 1);
                }
                else
                {
                        if (s.Substring(i, 1) == ",") { }
                        if (n != "")
                        {
                            slist.Add(n);
                            n = "";
                        }

                }
            }
            if (n != "") slist.Add(n);
            return slist;
        }

        public static List<string> SplitLine(string s)
        {
            bool quotemode = false;
            List<string> slist = new List<string>();
            string t = "";
            foreach (char c in s)
            {
                if (quotemode)
                {
                    if ((byte)c == 0x22) quotemode = false;
                    t = t + c;
                }
                else
                {
                    if ((byte)c == 0x22) quotemode = true;
                    if (c == ':')
                    {
                        slist.Add(t);
                        t = "";
                    }
                    else
                    {
                        t = t + c;
                    }
                }
            }
            slist.Add(t);

            return slist;
        }

        public static string ExtractString(string s)
        {
            bool firstfound = false;
            string t = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] != 0x22)
                {
                    t = t + s[i];
                }
                else
                {
                    t = t + (char)0x22;
                    if (firstfound)
                    {
                        break;
                    }
                    else
                    {
                        firstfound = true;
                    }
                }
            }
            return t;
        }

        public static string ExtractMacro(string s)
        {
            bool foundend = false;
            string t = "";
            for (int i = 0; i < s.Length; i++)
            {
                if ((s[i] != '}')&& (s[i]!='{'))
                {
                    t = t + s[i];
                }
                else
                {
                    if (s[i] == '}')
                    {
                        foundend = true;
                        break;
                    }
                }
            }
            if (!foundend) t = "";
            return t;
        }
    }
}
