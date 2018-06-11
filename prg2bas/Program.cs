using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SixBASIC;
using System.IO;

namespace prg2bas
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("prg2bas");
                    Console.WriteLine("A commandline CBM Basic decompiler.");
                    Console.WriteLine("By Six/Style 2016");
					Console.WriteLine("usage: prg2bas prgfile.prg basfile.bas");
                    Console.WriteLine();

                }
                else
                {
                    BASIC basic = new BASIC();

                    if (basic.Load(args[0]))
                    {
                        List<string> slist = basic.List();
                        File.WriteAllLines(args[1], slist);
                    }
                    else
                    {
                        Console.WriteLine("Load error: " + basic.LastError);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}
