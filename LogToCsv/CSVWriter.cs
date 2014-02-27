using System;
using System.IO;
using System.Linq;

namespace LogToCsv
{
    class CSVWriter : IDisposable
    {
        private readonly char _delimiter;
        private readonly TextWriter _writer;

        public CSVWriter(char delimiter, TextWriter writer)
        {
            _delimiter = delimiter;
            _writer = writer;
        }

        public void Append(string[] values)
        {
            string aggregate = values.Aggregate((s1, s2) => string.Format("{0}{1} {2}", s1, _delimiter, s2));

            _writer.WriteLine(aggregate);
        }

        public void Dispose()
        {
            _writer.Dispose();
        }
    }
}
