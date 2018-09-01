using System;
using SixBASIC.Compiler.CompilerElements;
using SixBASIC.Enums;

namespace SixBASIC
{
    public class Label : CompilerElement
    {
        //Label format
		// {:LabelName}

        public string LabelName { get; set; }
        public override CompilerElementType ElementType { get { return CompilerElementType.Label; } set { } }
        
        public Label()
        {

        }

        public Label(string text) : this()
        {
			LabelName = GetLabelName(text);
        }
        
        public new string ToString()
        {
            return base.ToString() + "<Label>" + LabelName + "</Label>";
        }

		public static string GetLabelName(string inputLine){
			return inputLine.Trim().Substring(2, inputLine.Trim().Length - 3);
		}

        public static bool IsLabel(string inputLine)
        {
			return (inputLine.Trim().StartsWith("{:") && inputLine.Trim().EndsWith("}"));
        }

		public static bool ContainsLabel(string inputLine){
			var checkText = inputLine.Trim();
			if (!checkText.Contains("{:") || !checkText.Contains("}")) return false;
			var potentialLabelStart = checkText.Trim().IndexOf("{:");
			if (!checkText.Substring(potentialLabelStart).Contains("}")) return false;
			return true;

		}

		public static string ReplaceLabelInText(string labelName, string inputLine, string replacementText){
			return inputLine.Trim().Replace("{:" + labelName + "}", replacementText);
		}

		public static string FirstLabelInText(string inputLine){
			var checkText = inputLine.Trim();
			var labelstart = checkText.IndexOf("{:");
			checkText = checkText.Substring(labelstart);
			var labelEnd = checkText.Trim().IndexOf("}");
			return checkText.Substring(0,labelEnd+1);
		}
    }
}
