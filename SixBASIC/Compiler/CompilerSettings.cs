using System;
namespace SixBASIC
{
	public class CompilerSettings
	{
		public  bool Renumber { get; set; }
		public  int RenumberIncrement { get; set; }
		public  int BaseAddress { get; set; }
		public  bool Crunch { get; set; }
		public  bool RemoveREM { get; set; }
		public  bool TurboBASIC { get; set; }
		public bool Debug { get; set; }

		public CompilerSettings(){
			Renumber = false;
			Debug = false;
			RenumberIncrement = 1;
			BaseAddress = 0x0801;
			Crunch = false;
			RemoveREM = false;
			TurboBASIC = false;
		}
	}
}
