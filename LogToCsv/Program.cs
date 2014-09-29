using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

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
            
            List<string> files = Directory.EnumerateFiles(Environment.CurrentDirectory, options.Input, SearchOption.TopDirectoryOnly).ToList();

            if (files.Count == 0)
            {
                Console.Error.WriteLine("File {0} not found", options.Input);

                return -1;
            }

            StreamWriter streamWriter;
            IParser parser;
            
            int result = AssertConditions(options, out streamWriter, out parser);
            
            if (result != 0)
            {
                string usage = options.GetUsage();

                Console.WriteLine(usage);

                return result;
            }

            using (CSVWriter csvWriter = new CSVWriter(',', streamWriter))
            {
                foreach(string file in files)
                {
                    using (StreamReader reader = new StreamReader(file))
                    {
                        if (options.Header)
                        {
                            string[] header = parser.GetHeader();

                            csvWriter.Append(header);
                        }

                        IEnumerable<string[]> rows = parser.Read(reader);

                        foreach (string[] row in rows)
                        {
                            csvWriter.Append(row);
                        }
                    }
                }
            }

            return 0;
        }

        private static int AssertConditions(Options options, out StreamWriter streamWriter, out IParser parser)
        {
            streamWriter = null;
            parser = null;

            streamWriter = options.GetWriter();

            if (streamWriter == null)
            {
                return -1;
            }

            parser = options.GetParser();

            if (parser == null)
            {
                return -1;
            }

            return 0;
        }
    }
}
