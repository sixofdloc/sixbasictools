using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SixBASIC.Enums;
using SixBASIC.Compiler.CompilerElements;
using SixBASIC.Compiler;

namespace SixBASIC
{
    public class Renumber
    {

        private List<CompilerElementRow> Program;

        public List<CompilerElementRow> Process(List<CompilerElementRow> program, int start, int increment)
        {
            int labelnum =start;
            Program = program;
            foreach (CompilerElementRow row in Program)
            {
                
                if (row.Elements[0].ElementType != CompilerElementType.PositionLabel)
                {
                    row.Elements.Insert(0, new Position_Label() { Line_Number = labelnum });
                }
                else
                {
                    string labl = ((Position_Label)(row.Elements[0])).Label;
                    int lnum = ((Position_Label)(row.Elements[0])).Line_Number;
                    if (labl == "")
                    {
                        ((Position_Label)(row.Elements[0])).Line_Number = labelnum;
                        //Find all references to this label and assign them to the number instead.
                        FixLabelRefs(labl, labelnum);
                    }
                    else
                    {
                        //For a hard renumber, we'd change this line's number and 
                        //Find all references to this line number and change them to the new line number
                        FixLineNumRefs(lnum, labelnum);
                    }
                }

                if (row.Elements.Count > 1) labelnum += increment; //don't increment for label-only rows.
            }
            return Program;
        }

        private void FixLabelRefs(string label, int linenum)
        {
            foreach (CompilerElementRow row in Program)
            {
                foreach (CompilerElement element in row.Elements)
                {
                    if (element.ElementType == CompilerElementType.LineReference)
                    {
                        if (((Line_Reference)(element)).Label == label)
                        {
                            //Handle Numbered Line Reference
                            //CompilerElement ce = program.SelectMany(p=>(Position_Label)(p.Elements[0]).
                            ((Line_Reference)(element)).Number = linenum;
                        }
                    }
                }
            }
        }

        private void FixLineNumRefs(int oldlinenum, int linenum)
        {
            foreach (CompilerElementRow row in Program)
            {
                foreach (CompilerElement element in row.Elements)
                {
                    if (element.ElementType == CompilerElementType.LineReference)
                    {
                        if (((Line_Reference)(element)).Number == oldlinenum)
                        {
                            //Handle Numbered Line Reference
                            //CompilerElement ce = program.SelectMany(p=>(Position_Label)(p.Elements[0]).
                            ((Line_Reference)(element)).Number = linenum;
                        }
                    }
                }
            }
        }


    }

}
