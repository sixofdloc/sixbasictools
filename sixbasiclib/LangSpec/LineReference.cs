using System;
namespace sixbasiclib
{
    public class LineReference
    {
		public int PositionInText { get; set; }
		public string LineNumberOrLabel { get; set; } = string.Empty;
        public bool Fulfilled { get; set; }
        public int PositionInTokenizedData { get; set; }

        public LineReference()
        {
        }

		public LineReference(int positionInText, int positionInTokenizedData,string lineNumberOrLabel) : this()
		{
            Fulfilled = false;
			PositionInText = positionInText;
            PositionInTokenizedData = positionInTokenizedData;
			LineNumberOrLabel = lineNumberOrLabel;
		}
    }
}
