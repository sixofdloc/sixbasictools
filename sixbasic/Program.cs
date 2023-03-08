using Microsoft.Extensions.CommandLineUtils;
using sixbasiclib;
using System.IO;

var app = new CommandLineApplication();

app.HelpOption("-?|-h|--help");
app.VersionOption("-v|--version", "1.0.0");
app.Description = "A command-line tool for listing, tokenizing, and detokenizing BASIC files.";
app.Name = "sixbasic";
app.ShortVersionGetter = () => "1.0.0";

app.Command("list", cmd =>
{
    cmd.HelpOption("-h|--help");
    cmd.Description = "List files in the target directory.";
    var inputFileOption = cmd.Option("-i|--input <INPUT_FILE>", "The input BASIC PRG file to list.", CommandOptionType.SingleValue);
    cmd.OnExecute(async () =>
    {
        if (!inputFileOption.HasValue())
        {
            Console.WriteLine("No input file specified.");
            return 1;
        } else {
            var inputFile = inputFileOption.Value();
            BASIC basic = new BASIC();
            var loadResult = await basic.Load(inputFile);
            if (loadResult){
                List<string> slist = basic.List();
                foreach (string s in slist)
                {
                    Console.WriteLine(s);
                }
            } else {
                Console.WriteLine("Load error: " + basic.LastError);
                return 1;
            }
        }
        return 0;
    });
});

app.Command("bas2prg", cmd =>
{
    cmd.HelpOption("-h|--help");
    cmd.Description = "Tokenize a BASIC file to a PRG file.";
    var inputFileOption = cmd.Option("-i|--input <INPUT_FILE>", "The input BASIC file to tokenize.", CommandOptionType.SingleValue);
    var outputFileOption = cmd.Option("-o|--output <OUTPUT_FILE>", "The output PRG file to create.", CommandOptionType.SingleValue);
    var pointerBasicOption = cmd.Option("-p|--pointer", "Tokenize using POINTER BASIC.", CommandOptionType.NoValue);
    cmd.OnExecute(async () =>
    {
        if (!inputFileOption.HasValue())
        {
            Console.WriteLine("No input file specified.");
            return 1;
        } else {
            if (!outputFileOption.HasValue())
            {
                Console.WriteLine("No output file specified.");
                return 1;
            } else {
                if (!File.Exists(inputFileOption.Value()))
                {
                    Console.WriteLine("Input file not found: " + inputFileOption.Value());
                    return 1;
                } else {
                    var inputFile = inputFileOption.Value();
                    var outputFile = outputFileOption.Value();
                    var pointerBasic = pointerBasicOption.HasValue();
                    //var basic = new BASIC();
                    var pp = new PreProcessor();
                    var basfile = new List<string>();
                    try {
                        basfile = File.ReadAllLines(inputFile).ToList();
                    } catch (FileNotFoundException) {
                        Console.WriteLine("Input file not found: " + inputFile);
                        return 1;
                    }
                    var compilerSettings = new CompilerSettings();
                    var p = pp.PreProcess(basfile,ref compilerSettings);
                    var bytes = (pointerBasic) ? PointerBASICTokenizer.Tokenize(p, ref compilerSettings) : Tokenizer.Tokenize(p, ref compilerSettings);
                    try {
                        await File.WriteAllBytesAsync(outputFile, bytes);
                    } catch (IOException) {
                        Console.WriteLine("Unable to write to output file: " + outputFile);
                        return 1;
                    }
                    Console.WriteLine($"Tokenized {inputFile} to {outputFile}");
                    return 0;
                }
            }
        }
    });
});

app.Command("prg2bas", cmd =>
{
    cmd.HelpOption("-h|--help");
    cmd.Description = "Detokenize a PRG file to a BASIC file.";
    var inputFileOption = cmd.Option("-i|--input <INPUT_FILE>", "The input PRG file to detokenize.", CommandOptionType.SingleValue);
    var outputFileOption = cmd.Option("-o|--output <OUTPUT_FILE>", "The output BASIC file to create.", CommandOptionType.SingleValue);
    cmd.OnExecute(async () =>
    {
        if (!inputFileOption.HasValue())
        {
            Console.WriteLine("No input file specified.");
            return 1;
        } else {
            if (!outputFileOption.HasValue())
            {
                Console.WriteLine("No output file specified.");
                return 1;
            } else {
                if (!File.Exists(inputFileOption.Value()))
                {
                    Console.WriteLine("Input file not found: " + inputFileOption.Value());
                    return 1;
                } else {
                    var inputFile = inputFileOption.Value();
                    var outputFile = outputFileOption.Value();
                    BASIC basic = new BASIC();
                    var loadResult = await basic.Load(inputFile);
                    if (loadResult){
                        List<string> slist = basic.List();
                        try {
                            await File.WriteAllLinesAsync(outputFile, slist);
                            Console.WriteLine($"Detokenized {inputFile} to {outputFile}");
                        } catch (IOException) {
                            Console.WriteLine("Unable to write to output file: " + outputFile);
                            return 1;
                        }
                    } else {
                        Console.WriteLine("Load error: " + basic.LastError);
                        return 1;
                    }
                    return 0;
                }
            }
        }
    });
});
app.OnExecute(()=>{
    app.ShowHelp(); 
    return 0;
});
// execute the application
app.Execute(args);
