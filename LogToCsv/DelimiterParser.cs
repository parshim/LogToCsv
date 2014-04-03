using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace LogToCsv
{
    public class DelimiterParser : IParser
    {
        private readonly char _delimiter;
        private readonly int[] _indexes;

        public DelimiterParser(char delimiter, int[] indexes)
        {
            _delimiter = delimiter;
            _indexes = indexes;
        }

        public string[] GetHeader()
        {
            string[] header = new string[_indexes.Length];

            for (int i = 0; i < _indexes.Length; i++)
            {
                header[i] = _indexes[i].ToString(CultureInfo.InvariantCulture);
            }

            return header;
        }

        public IEnumerable<string[]> Read(TextReader reader)
        {
            for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
            {
                string[] parts = line.Split(_delimiter);

                if (parts.Length < _indexes.Max()) continue;

                string[] values = new string[_indexes.Length];

                for (int i = 0; i < _indexes.Length; i++)
                {
                    values[i] = parts[_indexes[i]];
                }

                yield return values;
            }
        }
    }
}