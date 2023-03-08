using System;
using System.Collections.Generic;

namespace sixbasiclib.LangSpec
{
    public class OptimizedBASICLine
    {
        public int LineNumber { get; set; }
        public int BaseAddress { get; set; }
        public int NextLineAddress { get; set; }
        public BASIC_Line OriginalLine { get; set; } = new BASIC_Line();
        public List<byte> Tokenized { get; set; } = new List<byte>();
        public List<LineReference> OtherLineRefs { get; set; } = new List<LineReference>();

        public OptimizedBASICLine()
        {
            Tokenized = new List<byte>();
            OtherLineRefs = new List<LineReference>();
        }
    }
}
