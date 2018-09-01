using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SixBASIC.Enums;

namespace SixBASIC.Compiler.CompilerElements
{
 
	public abstract class CompilerElement
    {
        public abstract CompilerElementType ElementType { get; set; }
        public string Original { get; set; }
        public List<byte> Processed { get; set; }
        public CompilerElement()
        {
            Processed = new List<byte>();
        }
        public string ToXML()
        {
            return "<ElementType>" + ElementType.ToString()  + "</ElementType><Original>" + Original + "</Original><Processed>" + Processed + "</Processed>";
        }

		public new string ToString(){
			return "Compiler Element, "+ElementType.ToString()+", Original: " + Original + ", Processed: " + Processed;
		}

    }

 
    
    public class Keyword : CompilerElement
    {
		public override CompilerElementType ElementType
        {
			get { return CompilerElementType.Keyword; }
            set { }
        }
        public MicrosoftBasic2Token Token { get; set; }
        public new string ToString()
        {
            return base.ToString() + "<Token>" + Token.ASCII + "</Token>";
        }
    }

    public class Position_Label : CompilerElement
    {
		public override CompilerElementType ElementType
        {
			get { return CompilerElementType.PositionLabel; }
            set { }
        }
        public int Program_Line { get; set; }
        public int Line_Number { get; set; }
        public string Label { get; set; }
        public new string ToString()
        {
            return base.ToString() + "<Program_Line>" + Program_Line.ToString() + "</Program_Line>"
                + "<Line_Number>" + Line_Number.ToString() + "</Line_Number>"
                + "<Label>" + Label + "</Label>";
        }
    }

    public class Line_Reference : CompilerElement
    {
		public override CompilerElementType ElementType
        {
			get { return CompilerElementType.LineReference; }
            set { }
        }
        public int Number { get; set; }
        public string Label { get; set; }

        public new string ToString()
        {
            return base.ToString() 
                + "<Line_Reference>" + Number.ToString() + "</Line_Reference>"
                + "<Label>" + Label + "</Label>";
        }

    }

  //  public class Punctuation : CompilerElement
  //  {
		//public override CompilerElementType ElementType
  //      {
		//	get { return CompilerElementType.Punctuation; }
  //          set { }
  //      }
  //      public string Character { get; set; }
  //      public new string ToString()
  //      {
  //          return base.ToString() + "<Character>" + Character + "</Character>";
  //      }

  //  }

  //  public class Text_String : CompilerElement
  //  {
		//public override CompilerElementType ElementType
  //      {
		//	get { return CompilerElementType.TextString; }
  //          set { }
  //      }
  //      public string Text { get; set; }
  //      public new string ToString()
  //      {
  //          return base.ToString() + "<Text>" + Text + "</Text>";
  //      }

  //  }

  //  public class Variable : CompilerElement
  //  {
		//public override CompilerElementType ElementType
  //      {
		//	get { return CompilerElementType.Variable; }
  //          set { }
  //      }
  //      public string VariableName { get; set; }
  //      public int VariableType { get; set; }
  //      public new string ToString()
  //      {
  //          return base.ToString() + "<VariableName>" + VariableName + "</VariableName>"
  //              + "<VariableType>" + VariableType.ToString() + "</VariableType>";
  //      }
  //  }

  //  public class REM_Comment : CompilerElement
  //  {
		//public override CompilerElementType ElementType
  //      {
		//	get { return CompilerElementType.REMComment; }
  //          set { }
  //      }
  //  }

  //  public class Numeric_Constant : CompilerElement
  //  {
		//public override CompilerElementType ElementType
   //     {
			//get { return CompilerElementType.NumericConstant; }
    //        set { }
    //    }
    //}

    public class CompilerElementRow
    {
        public List<CompilerElement> Elements { get; set; }

        public CompilerElementRow()
        {
            Elements = new List<CompilerElement>();
        }
    }

}
