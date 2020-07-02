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
                    //var basic = new BASIC();
                    var pp = new PreProcessor();
                    var basfile = File.ReadAllLines(args[0]).ToList();
					var compilerSettings = new CompilerSettings();
                    var p = pp.PreProcess(basfile,ref compilerSettings);
                    var bytes = (true) ? SixBASICTokenizer.Tokenize(p, ref compilerSettings) : Tokenizer.Tokenize(p, ref compilerSettings);
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
