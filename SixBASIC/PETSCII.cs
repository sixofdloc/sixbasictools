using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SixBASIC
{
    public static class PETSCII
    {
        public class Macro
        {
            public string Text { get; set; }
            public string Replacement { get; set; }
        }

        public static Macro[] Macros = {
                                           new Macro(){Text="{white}",Replacement="\x05"},
                                           new Macro(){Text="{disable_shift}",Replacement="\x08"},
                                           new Macro(){Text="{enable_shift}",Replacement="\x09"},
                                           new Macro(){Text="{lowercase}",Replacement="\x0e"},
                                           new Macro(){Text="{down}",Replacement="\x11"},
                                           new Macro(){Text="{reverse on}",Replacement="\x12"},
                                           new Macro(){Text="{home}",Replacement="\x13"},
                                           new Macro(){Text="{del}",Replacement="\x14"},
                                           new Macro(){Text="{red}",Replacement="\x1c"},
                                           new Macro(){Text="{right}",Replacement="\x1d"},
                                           new Macro(){Text="{green}",Replacement="\x1e"},
                                           new Macro(){Text="{blue}",Replacement="\x1f"},
                                           new Macro(){Text="{orange}",Replacement="\x81"},
                                           new Macro(){Text="{f1}",Replacement="\x85"},
                                           new Macro(){Text="{f3}",Replacement="\x86"},
                                           new Macro(){Text="{f5}",Replacement="\x87"},
                                           new Macro(){Text="{f7}",Replacement="\x88"},
                                           new Macro(){Text="{f2}",Replacement="\x89"},
                                           new Macro(){Text="{f4}",Replacement="\x8a"},
                                           new Macro(){Text="{f6}",Replacement="\x8b"},
                                           new Macro(){Text="{f8}",Replacement="\x8c"},
                                           new Macro(){Text="{uppercase}",Replacement="\x8e"},
                                           new Macro(){Text="{black}",Replacement="\x90"},
                                           new Macro(){Text="{up}",Replacement="\x91"},
                                           new Macro(){Text="{reverse off}",Replacement="\x92"},
                                           new Macro(){Text="{clear}",Replacement="\x93"},
                                           new Macro(){Text="{insert}",Replacement="\x94"},
                                           new Macro(){Text="{brown}",Replacement="\x95"},
                                           new Macro(){Text="{pink}",Replacement="\x96"},
                                           new Macro(){Text="{gray 1}",Replacement="\x97"},
                                           new Macro(){Text="{gray 2}",Replacement="\x98"},
                                           new Macro(){Text="{light green}",Replacement="\x99"},
                                           new Macro(){Text="{light blue}",Replacement="\x9a"},
                                           new Macro(){Text="{gray 3}",Replacement="\x9b"},
                                           new Macro(){Text="{purple}",Replacement="\x9c"},
                                           new Macro(){Text="{left}",Replacement="\x9d"},
                                           new Macro(){Text="{yellow}",Replacement="\x9e"},
                                           new Macro(){Text="{cyan}",Replacement="\x9f"}
                                       };

        public static string[] Table = {"{$00}","{$01}","{$02}","{$03}","{$04}","{white}","{$06}","{$07}",
                                    "{disable_shift}","{enable_shift}","{$0a}","{$0b}","{$0c}","{$0d}","{lowercase}","{$0f}",
                                    "{$10}","{down}","{reverse on}","{home}","{del}","{$15}","{$16}","{$17}",
                                    "{$18}","{$19}","{$1a}","{$1b}","{red}","{right}","{green}","{blue}",
                                    " ","!","\"","#","$","%","&","'",
                                    "(",")","*","+",",","-",".","/",
                                    "0","1","2","3","4","5","6","7", //0x30-0x37
                                    "8","9",":",";","<","=",">","?", //0x38-0x3f
                                    "@","a","b","c","d","e","f","g", //0x40-0x47
                                    "h","i","j","k","l","m","n","o", //0x48-0x4f
                                    "p","q","r","s","t","u","v","w", //0x50-0x57
                                    "x","y","z","[","\\","]","^","_", //0x58-0x5f
                                    "`","A","B","C","D","E","F","G", //0x60-0x67
                                    "H","I","J","K","L","M","N","O", //0x68-0x6f
                                    "P","Q","R","S","T","U","V","W", //x070-0x77
                                    "X","Y","Z","{","|","}","~","{$7f}", //0x78-0x7f
                          "{$80}","{orange}","{$82}","{$83}","{$84}","{f1}","{f3}","{f5}",
                          "{f7}","{f2}","{f4}","{f6}","{f8}","{$8d}","{uppercase}","{$8f}",       //0x80-0x8F
                          "{black}","{up}","{reverse off}","{clear}","{insert}","{brown}","{pink}","{gray 1}",
                          "{gray 2}","{light green}","{light blue}","{gray 3}","{purple}","{left}","{yellow}","{cyan}",       //0x90-0x9F
                          "{$a0}","{$a1}","{$a2}","{$a3}","{$a4}","{$a5}","{$a6}","{$a7}",
                          "{$a8}","{$a9}","{$aa}","{$ab}","{$ac}","{$ad}","{$ae}","{$af}",       //0xA0-0xAF
                          "{$b0}","{$b1}","{$b2}","{$b3}","{$b4}","{$b5}","{$b6}","{$b7}",
                          "{$b8}","{$b9}","{$ba}","{$bb}","{$bc}","{$bd}","{$be}","{$bf}",       //0xB0-0xBF
                                    "`","A","B","C","D","E","F","G", //0xc0-0xc7
                                    "H","I","J","K","L","M","N","O", //0xc8-0xcf
                                    "P","Q","R","S","T","U","V","W", //x0d0-0xd7
                                    "X","Y","Z","{","|","}","~","{$7f}", //0xd8-0xdf
                          "{$e0}","{$e1}","{$e2}","{$e3}","{$e4}","{$e5}","{$e6}","{$e7}",
                          "{$e8}","{$e9}","{$ea}","{$eb}","{$ec}","{$ed}","{$ee}","{$ef}",       //0xE0-0xEF
                          "{$f0}","{$f1}","{$f2}","{$f3}","{$f4}","{$f5}","{$f6}","{$f7}",
                          "{$f8}","{$f9}","{$fa}","{$fb}","{$fc}","{$fd}","{$fe}","{$ff}" };//0xF0-0xFF

    }
}
