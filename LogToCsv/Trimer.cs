using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LogToCsv
{
    public class Trimer : IParser
    {
        private readonly IParser _parser;

        public Trimer(IParser parser)
        {
            _parser = parser;
        }

        public string[] GetHeader()
        {
            return _parser.GetHeader();
        }

        public IEnumerable<string[]> Read(TextReader reader)
        {
            foreach (string[] values in _parser.Read(reader))
            {
                yield return values.Select(s => s.Trim()).ToArray();
            }
        }
    }
}