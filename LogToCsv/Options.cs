using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CommandLine;
using CommandLine.Text;

namespace LogToCsv
{
    class Options
    {
        [Option('p', "pattern", HelpText = "Parsing RegEx pattern in case regeular expression based parsing is required", Required = true)]
        public string Pattern { get; set; }
        
        [Option('d', "delimiter", DefaultValue = ',', HelpText = "Parsing delimiter in case delimiter based parsing is required", Required = true)]
        public char Delimiter { get; set; }

        [Option('n', "indexes", DefaultValue = "", HelpText = "Indexes of required parts  in delimiter based parsing")]
        public string Indexes { get; set; }
        
        [Option('i', "input", HelpText = "Input log file.", Required = true)]
        public string Input { get; set; }
        
        [Option('o', "output", HelpText = "Output file, if missing default output stream is used.", Required = false)]
        public string Output { get; set; }

        [Option('t', "trim", HelpText = "Trim parsed part value before write to CSV")]
        public bool Trim { get; set; }

        [Option('h', "header", HelpText = "Add header to output")]
        public bool Header { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              current => HelpText.DefaultParsingErrorsHandler(this, current));
        }

        public StreamWriter GetWriter()
        {
            StreamWriter streamWriter;

            if (string.IsNullOrEmpty(Output))
            {
                streamWriter = new StreamWriter(Console.OpenStandardOutput());
            }
            else
            {
                if (File.Exists(Output))
                {
                    System.Diagnostics.Trace.WriteLine("Output file {0} already exists", Output);

                    return null;
                }

                streamWriter = new StreamWriter(Output);
            }

            return streamWriter;
        }

        public IParser GetParser()
        {
            IParser parser = GetParserInternal();

            if (parser != null && Trim)
            {
                return new Trimer(parser);
            }

            return parser;
        }

        private IParser GetParserInternal()
        {
            if (!string.IsNullOrEmpty(Pattern))
            {
                Regex pattern = new Regex(Pattern, RegexOptions.Compiled);

                return new RegExParser(pattern);
            }

            if (!string.IsNullOrEmpty(Indexes))
            {
                int[] indexes = Indexes.Split(',').Select(int.Parse).ToArray();

                return new DelimiterParser(Delimiter, indexes);
            }

            return null;
        }
    }
}
