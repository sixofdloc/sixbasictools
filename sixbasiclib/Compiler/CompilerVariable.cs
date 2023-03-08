using System;
namespace sixbasiclib.Compiler
{
    public class CompilerVariable

    {
		public string VariableName { get; set; } = string.Empty;
        public string VariableValue { get; set; } = string.Empty;
		public CompilerVariable()
        {
        }

		public CompilerVariable(string variableName, string variableValue) : this(){
			VariableName = variableName;
			VariableValue = variableValue;
		}


    }
}
