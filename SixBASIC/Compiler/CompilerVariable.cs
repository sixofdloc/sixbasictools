using System;
namespace SixBASIC.Compiler
{
    public class CompilerVariable

    {
		public string VariableName { get; set; }
        public string VariableValue { get; set; }
		public CompilerVariable()
        {
        }

		public CompilerVariable(string variableName, string variableValue) : this(){
			VariableName = variableName;
			VariableValue = variableValue;
		}


    }
}
