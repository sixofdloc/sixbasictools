using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SixBASIC
{
    public class MicrosoftBasic2
    {
        public static List<MicrosoftBasic2Token> Tokens = new List<MicrosoftBasic2Token>(){
            new MicrosoftBasic2Token(){Token=0xAC, ASCII=      "*",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xAA, ASCII=      "+",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xAB, ASCII=      "-",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xAD, ASCII=      "/",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xB3, ASCII=      "<",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xB2, ASCII=      "=",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xB1, ASCII=      ">",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xAE, ASCII=      "^",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xB6, ASCII=    "ABS",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xAF, ASCII=    "AND",LeadingSpace=true,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0xC6, ASCII=    "ASC",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xC1, ASCII=    "ATN",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xC7, ASCII=   "CHR$",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xA0, ASCII=  "CLOSE",LeadingSpace=false,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0x9C, ASCII=    "CLR",LeadingSpace=false,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0x9D, ASCII=    "CMD",LeadingSpace=false,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0x9A, ASCII=   "CONT",LeadingSpace=false,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0xBE, ASCII=    "COS",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0x83, ASCII=   "DATA",LeadingSpace=false,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0x96, ASCII=    "DEF",LeadingSpace=false,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0x86, ASCII=    "DIM",LeadingSpace=false,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0x80, ASCII=    "END",LeadingSpace=false,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0xBD, ASCII=    "EXP",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xA5, ASCII=     "FN",LeadingSpace=false,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0x81, ASCII=    "FOR",LeadingSpace=false,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0xB8, ASCII=    "FRE",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xA1, ASCII=    "GET",LeadingSpace=false,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0x8D, ASCII=  "GOSUB",LeadingSpace=false,TrailingSpace=true}, /* must appear before "go" */
            new MicrosoftBasic2Token(){Token=0x89, ASCII=   "GOTO",LeadingSpace=true,TrailingSpace=true}, /* must appear before "go" */
            new MicrosoftBasic2Token(){Token=0xCB, ASCII=     "GO",LeadingSpace=false,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0x8B, ASCII=     "IF",LeadingSpace=false,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0x84, ASCII= "INPUT#",LeadingSpace=false,TrailingSpace=false}, /* must appear before "input" */
            new MicrosoftBasic2Token(){Token=0x85, ASCII=  "INPUT",LeadingSpace=false,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0xB5, ASCII=    "INT",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xC8, ASCII=  "LEFT$",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xC3, ASCII=    "LEN",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0x88, ASCII=    "LET",LeadingSpace=false,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0x9B, ASCII=   "LIST",LeadingSpace=false,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0x93, ASCII=   "LOAD",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xBC, ASCII=    "LOG",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xCA, ASCII=   "MID$",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xA2, ASCII=    "NEW",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0x82, ASCII=   "NEXT",LeadingSpace=false,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0xA8, ASCII=    "NOT",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0x91, ASCII=     "ON",LeadingSpace=false,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0x9F, ASCII=   "OPEN",LeadingSpace=false,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0xB0, ASCII=     "OR",LeadingSpace=true,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0xC2, ASCII=   "PEEK",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xFF, ASCII=   "{PI}",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0x97, ASCII=   "POKE",LeadingSpace=false,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0xB9, ASCII=    "POS",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0x98, ASCII= "PRINT#",LeadingSpace=false,TrailingSpace=false}, 
            new MicrosoftBasic2Token(){Token=0x99, ASCII=  "PRINT",LeadingSpace=false,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0x87, ASCII=   "READ",LeadingSpace=false,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0x8F, ASCII=    "REM",LeadingSpace=false,TrailingSpace=true}, 
            new MicrosoftBasic2Token(){Token=0x8C, ASCII="RESTORE",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0x8E, ASCII= "RETURN",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xC9, ASCII= "RIGHT$",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xBB, ASCII=    "RND",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0x8A, ASCII=    "RUN",LeadingSpace=false,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0x94, ASCII=   "SAVE",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xB4, ASCII=    "SGN",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xBF, ASCII=    "SIN",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xA6, ASCII=    "SPC",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xBA, ASCII=    "SQR",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xA9, ASCII=   "STEP",LeadingSpace=true,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0x90, ASCII=   "STOP",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xC4, ASCII=   "STR$",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0x9E, ASCII=    "SYS",LeadingSpace=false,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0xA3, ASCII=   "TAB(",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xC0, ASCII=    "TAN",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xA7, ASCII=   "THEN",LeadingSpace=true,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0xA4, ASCII=     "TO",LeadingSpace=true,TrailingSpace=true},
            new MicrosoftBasic2Token(){Token=0xB7, ASCII=    "USR",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0xC5, ASCII=    "VAL",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0x95, ASCII= "VERIFY",LeadingSpace=false,TrailingSpace=false},
            new MicrosoftBasic2Token(){Token=0x92, ASCII=   "WAIT",LeadingSpace=false,TrailingSpace=true}
        };

        public static List<String> BranchingKeywords = new List<String>() { "GOSUB", "GOTO", "GO TO", "THEN" };
        

    }
}
