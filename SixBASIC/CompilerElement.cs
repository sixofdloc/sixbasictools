using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SixBASIC
{
    public class CompilerElementTypes
    {
        public static int Whitespace = 0;
        public static int Keyword = 1;
        public static int Position_Label = 2;
        public static int Punctuation = 3;
        public static int Text_String = 4;
        public static int Variable = 5;
        public static int Line_Reference = 6;
        public static int REM_Comment = 7;
        public static int Numeric_Constant = 8;

        public static string[] TypeNames = {"Whitespace","Keyword","Position_Label","Punctuation", "Text_String", "Variable", "Line_Reference", "REM_Comment", "Numeric_Constant" };
    }


    public class VariableTypes
    {
        public static int Integer = 0;
        public static int String = 1;
        public static int Float = 2;
    }

    //public class ReplacementMacro
    //{
    //    public string Macro { get; set; }
    //    public string Replacement { get; set; }
    //}

    public abstract class CompilerElement
    {
        public abstract int ElementType { get; set; }
        public string Original { get; set; }
        public List<byte> Processed { get; set; }
        public CompilerElement()
        {
            Processed = new List<byte>();
        }
        public string ToString()
        {
            return "<ElementType>" + CompilerElementTypes.TypeNames[ElementType] + "</ElementType><Original>" + Original + "</Original><Processed>" + Processed + "</Processed>";
        }
    }

    public class Whitespace : CompilerElement
    {
        public override int ElementType
        {
            get { return CompilerElementTypes.Whitespace; }
            set { }
        }
        public int Number_Of_Spaces { get; set; }
        public string ToString()
        {
            return base.ToString() + "<Number_Of_Spaces>" + Number_Of_Spaces.ToString() + "</Number_Of_Spaces>";
        }
    }
    
    public class Keyword : CompilerElement
    {
        public override int ElementType
        {
            get { return CompilerElementTypes.Keyword; }
            set { }
        }
        public MicrosoftBasic2Token Token { get; set; }
        public string ToString()
        {
            return base.ToString() + "<Token>" + Token.ASCII + "</Token>";
        }
    }

    public class Position_Label : CompilerElement
    {
        public override int ElementType
        {
            get { return CompilerElementTypes.Position_Label; }
            set { }
        }
        public int Program_Line { get; set; }
        public int Line_Number { get; set; }
        public string Label { get; set; }
        public string ToString()
        {
            return base.ToString() + "<Program_Line>" + Program_Line.ToString() + "</Program_Line>"
                + "<Line_Number>" + Line_Number.ToString() + "</Line_Number>"
                + "<Label>" + Label + "</Label>";
        }
    }

    public class Line_Reference : CompilerElement
    {
        public override int ElementType
        {
            get { return CompilerElementTypes.Line_Reference; }
            set { }
        }
        public int Number { get; set; }
        public string Label { get; set; }

        public string ToString()
        {
            return base.ToString() 
                + "<Line_Number>" + Number.ToString() + "</Line_Number>"
                + "<Label>" + Label + "</Label>";
        }

    }

    public class Punctuation : CompilerElement
    {
        public override int ElementType
        {
            get { return CompilerElementTypes.Punctuation; }
            set { }
        }
        public string Character { get; set; }
        public string ToString()
        {
            return base.ToString() + "<Character>" + Character + "</Character>";
        }

    }

    public class Text_String : CompilerElement
    {
        public override int ElementType
        {
            get { return CompilerElementTypes.Text_String; }
            set { }
        }
        public string Text { get; set; }
        public string ToString()
        {
            return base.ToString() + "<Text>" + Text + "</Text>";
        }

    }

    public class Variable : CompilerElement
    {
        public override int ElementType
        {
            get { return CompilerElementTypes.Variable; }
            set { }
        }
        public string VariableName { get; set; }
        public int VariableType { get; set; }
        public string ToString()
        {
            return base.ToString() + "<VariableName>" + VariableName + "</VariableName>"
                + "<VariableType>" + VariableType.ToString() + "</VariableType>";
        }
    }

    public class REM_Comment : CompilerElement
    {
        public override int ElementType
        {
            get { return CompilerElementTypes.REM_Comment; }
            set { }
        }
    }

    public class Numeric_Constant : CompilerElement
    {
        public override int ElementType
        {
            get { return CompilerElementTypes.Numeric_Constant; }
            set { }
        }
    }

    public class CompilerElementRow
    {
        public List<CompilerElement> Elements { get; set; }

        public CompilerElementRow()
        {
            Elements = new List<CompilerElement>();
        }
    }

}
