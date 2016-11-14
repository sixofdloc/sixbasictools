using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SixBASIC;

namespace list
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 1)
                {
                    Console.WriteLine("list");
                    Console.WriteLine("A commandline tool to list CBM Basic prg files.");
                    Console.WriteLine("By Six/Style 2016");
                    Console.WriteLine("usage: list filename.prg");
                    Console.WriteLine();
                }
                else
                {
                    BASIC basic = new BASIC();
                    basic.Load(args[1]);
                    List<string> slist = basic.List();
                    foreach (string s in slist)
                    {
                        Console.WriteLine(s);
                    }
                    Console.WriteLine("Press Any Key To Continue");
                    Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}
