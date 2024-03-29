﻿using System;
using sixbasiclib.Compiler.CompilerElements;
using sixbasiclib.Enums;

namespace sixbasiclib
{
	public class ReplacementMacro : CompilerElement
    {
		//ReplacementMacro format
        // Set Value
		// {#MACROTEXT|REPLACEMENTTEXT}
        //
        // Inline Macro
		// {#MACROTEXT}
        
		public string MacroText { get; set; } = string.Empty;
        public string ReplacementText { get; set; }	 = string.Empty;
		public override CompilerElementType ElementType { get { return CompilerElementType.ReplacementMacro; } set {} }

		public ReplacementMacro()
        {
			
        }
        
		public ReplacementMacro(string text):this()
		{
			var pass1 = text.Substring(2, text.Length - 3).Split('|');
			MacroText = pass1[0];
			ReplacementText = pass1[1];
		}

		public string Replace(string inputLine)
		{
			return inputLine.Replace("{#" + MacroText + "}", ReplacementText);	
		}

		public new string ToString()
		{
			return base.ToString() + "<ReplacementMacro>" + MacroText +"="+ReplacementText + "</ReplacementMacro>";
        }

		public static bool DetectDefinition(string inputLine){
			return (inputLine.StartsWith("{#") && inputLine.Contains("|") && inputLine.EndsWith("}"));
		}
    }
}
