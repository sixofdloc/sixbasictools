using System;
using System.Collections.Generic;

namespace SixBASIC.Compiler.SixBasic
{
    public class TokenizedLine
    {
        public List<byte> Bytes { get; set; }
        public List<LinePointer> LinePointers { get; set; }
    }
}
