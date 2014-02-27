using CommandLine;
using CommandLine.Text;

namespace LogToCsv
{
    class Options
    {
        [Option('p', "pattern", HelpText = "Parsing RegEx pattern.", Required = true)]
        public string Pattern { get; set; }
        
        [Option('d', "delimiter", DefaultValue = ',', HelpText = "CSV delimiter.", Required = true)]
        public char Delimiter { get; set; }
        
        [Option('i', "input", HelpText = "Input log file.", Required = true)]
        public string Input { get; set; }
        
        [Option('o', "output", HelpText = "Output file, if missing default output stream is used.", Required = false)]
        public string Output { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
