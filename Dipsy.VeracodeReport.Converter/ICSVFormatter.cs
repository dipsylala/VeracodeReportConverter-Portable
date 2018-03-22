using System.Collections.Generic;

namespace Dipsy.VeracodeReport.Converter
{
    public interface ICSVFormatter
    {
        string FormatLine(IEnumerable<string> values);
    }
}
