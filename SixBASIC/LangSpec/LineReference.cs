using System;
namespace SixBASIC
{
    public class LineReference
    {
		public int PositionInText { get; set; }
		public string LineNumberOrLabel { get; set; }
       public bool Fulfilled { get; set; }

        public LineReference()
        {
        }

		public LineReference(int positionInText, string lineNumberOrLabel) : this()
		{
            Fulfilled = false;
			PositionInText = positionInText;
			LineNumberOrLabel = lineNumberOrLabel;
		}
    }
}
