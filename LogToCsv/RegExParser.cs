using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace LogToCsv
{
    public class RegExParser : IParser
    {
        private readonly Regex _pattern;

        public RegExParser(Regex pattern)
        {
            _pattern = pattern;
        }

        public string[] GetHeader()
        {
            return _pattern.GetGroupNames().Skip(1).ToArray();
        }

        public IEnumerable<string[]> Read(TextReader reader)
        {
            for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
            {
                Match match = _pattern.Match(line);

                if (match.Success)
                {
                    int count = match.Groups.Count;

                    string[] values = new string[count - 1];

                    for (int i = 1; i < count; i++)
                    {
                        Group g = match.Groups[i];

                        values[i - 1] = g.Success ? g.Value : "";
                    }

                    yield return values;
                }
            }
        }
    }
}
