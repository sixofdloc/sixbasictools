using System;
using System.Collections.Generic;

namespace SixBASIC.LangSpec
{
    public class OptimizedBASICLine
    {
        public int LineNumber { get; set; }
        public int BaseAddress { get; set; }
        public BASIC_Line OriginalLine { get; set; }
        public List<byte> Tokenized { get; set; }
        public List<LineReference> OtherLineRefs { get; set; }

        public OptimizedBASICLine()
        {
            Tokenized = new List<byte>();
            OtherLineRefs = new List<LineReference>();
        }
    }
}
