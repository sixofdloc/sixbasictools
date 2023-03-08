using System;
using System.Collections.Generic;

namespace sixbasiclib.Compiler
{
    public class TokenizedLine
    {
        public List<byte> Bytes { get; set; } = new List<byte>();
        public List<LineReference> LinePointers { get; set; } = new List<LineReference>();
    }
}
