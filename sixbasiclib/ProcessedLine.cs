using System;
namespace sixbasiclib
{
    public class ProcessedLine
    {
		public int SourceFileLine { get; set; }
		public string LineText { get; set; }

		public ProcessedLine(int sourceFileLine, string lineText){
			SourceFileLine = sourceFileLine;
			LineText = lineText;
		}
    }
}
