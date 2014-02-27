using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using CommandLine;

namespace LogToCsv
{
    public  static class Program
    {
        public static int Main(string[] args)
        {
            Options options = new Options();

            using (Parser cmdParser = new Parser())
            {
                cmdParser.ParseArguments(args, options);
            }

            if (!File.Exists(options.Input))
            {
                Console.Error.WriteLine("File {0} not found", options.Input);

                return -1;
            }

            StreamWriter streamWriter = GetWriter(options);

            if (streamWriter == null)
            {
                return -1;
            }

            Regex pattern = new Regex(options.Pattern, RegexOptions.Compiled);

            LogParser parser = new LogParser(pattern);

            using (CSVWriter csvWriter = new CSVWriter(options.Delimiter, streamWriter))
            {
                using (StreamReader reader = new StreamReader(options.Input))
                {
                    string[] header = parser.GetHeader();

                    csvWriter.Append(header);

                    IEnumerable<string[]> rows = parser.Read(reader);

                    foreach (string[] row in rows)
                    {
                        csvWriter.Append(row);
                    }
                }
            }

            return 0;
        }

        private static StreamWriter GetWriter(Options options)
        {
            StreamWriter streamWriter;

            if (string.IsNullOrEmpty(options.Output))
            {
                streamWriter = new StreamWriter(Console.OpenStandardOutput());
            }
            else
            {
                if (File.Exists(options.Output))
                {
                    Console.Error.WriteLine("Output file {0} already exists", options.Output);

                    return null;
                }

                streamWriter = new StreamWriter(options.Output);
            }
            return streamWriter;
        }
    }
}
