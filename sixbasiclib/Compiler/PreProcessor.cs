using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace sixbasiclib
{

    /// <summary>
    /// Pre processor Plan as follows
    /// 1. Parse and fix all replacement macros
    /// 2. Number entire program
    /// 3. Replace any labels with line numbers, and any refs to labels with line numbers
    /// 4. Crunch program if needed
    /// Program should now be ready for tokenizer
    ///
    /// </summary>
	public class PreProcessor
	{

		private List<ReplacementMacro> _replacementMacros;
		private CompilerSettings compilerSettings = new CompilerSettings();

		public PreProcessor()
		{
			_replacementMacros = new List<ReplacementMacro>();
		}


		public BASICProgram PreProcess(List<string> prgLines, ref CompilerSettings CompilerSettings)
		{
			//Stash our settings
			compilerSettings = CompilerSettings;

			//First pass, just put them in a list with sourcefileLne
			var pass0Lines = ProcessStringList(prgLines);

			//Do compiler macros, settings, tags	
			var pass1Lines = HandleCompilerTagsAndMacros(pass0Lines);

			//Strip out comments
			var pass2Lines = StripComments(pass1Lines);

			var basicProgram = HandleRemainingTags(pass2Lines);

			if (InjectLineNumbers(ref basicProgram))
			{
				//This needs to be revisited - if you remove a line with REM
				//And a goto refers to it, the goto needs to be updated.
				//Because of this, removeREM should probably happen AFTER line-numberization.
				if (compilerSettings.RemoveREM)
				{
					//Strip anything after REM in this line
					//     lineText = Utils.StripREM(lineText);
				}

				//Are we crunching?  Figure out which lines can be crunched
				if (compilerSettings.Crunch)
				{
					var crunchedProgram = Crunch(basicProgram);
					basicProgram = crunchedProgram;
				}
			}
			return basicProgram;
		}

		public bool InjectLineNumbers(ref BASICProgram basicProgram)
		{
			//Pass 3, replace labels in basic line text with line # refs
			var result = true;
			try
			{
				foreach (var line in basicProgram.Lines)
				{
					while (Label.ContainsLabel(line.Text))
					{
						//Get our label text
						var labelString = Label.FirstLabelInText(line.Text);

						//Find line this label points to
						var labelLine = basicProgram.Lines.FirstOrDefault(prgLine => prgLine.Labels.Count(q => q.LabelName == Label.GetLabelName(labelString)) > 0);
						if (labelLine != null)
						{
							//REplace label in line text with line # of target
							line.Text = line.Text.Replace(labelString, labelLine.Line_Number.ToString());
						}
						else
						{
							//UNDEFINED LABEL ERROR!
							Console.WriteLine("Undefined Label: "+labelString+ "in "+line.Text);
							result = false;
							break;
						}
					}
				}
			}
			catch (Exception ex)
			{
                Console.WriteLine(ex);
				result = false;
			}
            basicProgram.Dump("pass3.txt");
			return result;
		}

		public BASICProgram HandleRemainingTags(List<ProcessedLine> lines)
		{
			//Pass 2, parse comments and the rest of our tags.
            var Labels = new List<Label>();
            var basicProgram = new BASICProgram();
            foreach (ProcessedLine line in lines)
            {
                if (Utils.TrimLead(line.LineText).StartsWith("'"))
                {
                    //It's a comment.
                }
                else
                {
                    //Handle any replacement macros
                    string result = line.LineText;
                    _replacementMacros.ForEach(macro =>
                    {
                        result = macro.Replace(result);
                    });
                    //Get the PETSCII and byte tags out of the way.
                    result = ParseTags(result);
                    if (Label.IsLabel(result)) {
                        //Accumulate labels
                        //Has this label already been defined?
                        var labelName = Label.GetLabelName(result);
                        var labelLine = basicProgram.Lines.FirstOrDefault(prgLine => prgLine.Labels.Count(q => q.LabelName == labelName)>0);
                        if (labelLine != null)
                        {
                            //REDEFINED LABEL ERROR!
                        }
                        else
                        {
                            Labels.Add(new Label(result));
                        }
                    } else {
                        basicProgram.AddLineByText(result,Labels,line.SourceFileLine);
                        Labels = new List<Label>();
                    }
                }
            }
            if (compilerSettings.Debug) basicProgram.Dump("pass2.txt");
			return basicProgram;
		}


        //Crunch a basic program
		public BASICProgram Crunch(BASICProgram basicProgram){
			BASICProgram crunchedProgram = new BASICProgram();
            var buildLine = BASIC_Line.Copy(basicProgram.Lines[0]);
            var currentLine = new BASIC_Line();
            foreach (var line in basicProgram.Lines.Skip(1))
            {
                currentLine = BASIC_Line.Copy(line);
                var canConcatenate = true;
				if (compilerSettings.Debug)
				{
					Console.WriteLine("buildLine: " + buildLine.Text);
					Console.WriteLine("currentLine: " + currentLine.Text);
				}
                canConcatenate &= buildLine.CanBeAppendedTo();
                canConcatenate &= !basicProgram.DoAnyLinesReferTo(line);
                canConcatenate &= currentLine.Labels.Count <= 0;
                canConcatenate &= currentLine.Text.Length + buildLine.Text.Length <= 250;
                if (canConcatenate)
                {
                    buildLine.Text += ":" + currentLine.Text.Trim().Replace("\t", "");
                }
                else
                {
                    //Can't tack anything else onto this, so push our old one on the stack and start on the new one.
                    crunchedProgram.Lines.Add(buildLine);//(buildLine.Text, buildLine.Labels, 0);
                    buildLine = currentLine;
                }
            }
            crunchedProgram.Lines.Add(buildLine);//(currentLine.Text, currentLine.Labels, 0);
            crunchedProgram.Lines.Add(currentLine);//(currentLine.Text, currentLine.Labels, 0);
            if (compilerSettings.Debug)crunchedProgram.Dump("crunch.txt");
            return crunchedProgram;

		}

        //Takes a list of strings and returns a processedLine List
		public List<ProcessedLine> ProcessStringList(List<string> prgLines){
			var pass0Lines = new List<ProcessedLine>();
            for (int i = 0; i < prgLines.Count; i++)
            {
                pass0Lines.Add(new ProcessedLine(i, prgLines[i]));
            }
			return pass0Lines;
		}

        //Passes through a ProcessedLine list and sets any compiler flags/variables
		public List<ProcessedLine> HandleCompilerTagsAndMacros(List<ProcessedLine> prgLines){
			var passLines = new List<ProcessedLine>();

            //Handle all compiler directives and replacement macro defines
            prgLines.ForEach(line => {
                if (line.LineText.Trim() != "")
                {
                    if (ParseCompilerDirective(line.LineText)) passLines.Add(line);
                }
            });

            Dump("Debug-AfterMacrosAndTags.txt", passLines);
			return passLines;
		}

		List<ProcessedLine> StripComments(List<ProcessedLine> prgLines){
			var passLines = new List<ProcessedLine>();
			//Strip out comments
			prgLines.ForEach(line => {
				var lineText = line.LineText;
                if (lineText.Trim().Length > 0 && lineText.Contains("'")) lineText = Utils.RemoveSingleQuoteComments(lineText.Trim());
                if (lineText.Trim().Length > 0) passLines.Add(new ProcessedLine(line.SourceFileLine, lineText.Trim()));
			});
			Dump("Debug-AfterStripComments.txt", passLines);
			return passLines;
		}

		private bool ParseCompilerDirective(string line){
			bool IncludeLineInPass2 = true;
            //LoadAddr directive
			if (line.StartsWith("{loadaddr:")){
				int i = line.IndexOf("{loadaddr:") + 10;
                int j = line.IndexOf("}");
                compilerSettings.BaseAddress = int.Parse(line.Substring(i, j - i));
				IncludeLineInPass2 = false;
			}
            //Renumber directive
			if (line == "{renumber}"){
				compilerSettings.Renumber = true;
				IncludeLineInPass2 = false;
			}
            //Crunch directive
			if (line == "{crunch}")
            {
                compilerSettings.Crunch = true;
				IncludeLineInPass2 = false;
            }
			//Turbo directive
            if (line == "{turbo}")
            {
                compilerSettings.TurboBASIC = true;
                IncludeLineInPass2 = false;
            }
			//Debug directive
            if (line == "{debug}")
            {
                compilerSettings.Debug = true;
                IncludeLineInPass2 = false;
            }

            //Remove REM directive
			if (line == "{removerem}")
            {
                compilerSettings.RemoveREM = true;
				IncludeLineInPass2 = false;
            }

            //Replacement Macro definitions
            if (ReplacementMacro.DetectDefinition(line))
            {
                _replacementMacros.Add(new ReplacementMacro(line));
				IncludeLineInPass2 = false;
            }
			return IncludeLineInPass2;
		}

        //Parses stock verbose PETSCII tags, and {$ff} style char insertion tags
        private string ParseTags(string s)
        {
            string t = s;

            
			PETSCII.Macros.ForEach(macro =>
			{
				//Handle single-unit replacement
				t = t.Replace(macro.Text, macro.Replacement);
				//Now try for repeated replacements
				var preambleMacro = macro.Text.Substring(0, macro.Text.Length - 1)+":";
				while (t.Contains(preambleMacro)){
					var preambleEnds = t.IndexOf(preambleMacro) + preambleMacro.Length;
					var macroEnds = t.Substring(preambleEnds).IndexOf("}");
					var macroRepeatString = t.Substring(preambleEnds,macroEnds);
					var macroRepeats = int.Parse(macroRepeatString);
					var macroReplaceString = preambleMacro + macroRepeatString + "}";
					var replaceString = "";
					for (int i = 0; i < macroRepeats;i++){
						replaceString = replaceString + macro.Replacement;
					}
					t=t.Replace(macroReplaceString, replaceString);
				}
			});

            return t;
        }


		public void Dump(string fileName,List<ProcessedLine> lines)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
				foreach (var line in lines)
				{
					writer.WriteLine(line.SourceFileLine.ToString()+" : "+line.LineText);
				}
            }
        }




    }
}
