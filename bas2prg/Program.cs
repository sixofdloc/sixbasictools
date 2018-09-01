using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SixBASIC;
using System.IO;

namespace bas2prg
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("bas2prg");
                    Console.WriteLine("A commandline CBM Basic compiler.");
                    Console.WriteLine("By Six/Style 2016-2018");
                    Console.WriteLine("usage: bas2prg basfile.bas prgfile.prg");
                    Console.WriteLine();
                }
                else
                {
                    BASIC basic = new BASIC();
                    PreProcessor pp = new PreProcessor();
                    List<string> basfile = File.ReadAllLines(args[0]).ToList();
					CompilerSettings compilerSettings = new CompilerSettings();
                    BASICProgram p = pp.PreProcess(basfile,ref compilerSettings);
                    byte[] bytes = Tokenizer.Tokenize(p, ref compilerSettings);
                    File.WriteAllBytes(args[1], bytes);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}
