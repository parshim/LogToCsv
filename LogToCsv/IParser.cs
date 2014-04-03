using System.Collections.Generic;
using System.IO;

namespace LogToCsv
{
    public interface IParser
    {
        string[] GetHeader();

        IEnumerable<string[]> Read(TextReader reader);
    }
}