using System.Collections.Generic;

namespace Dipsy.VeracodeReport.Converter.Interfaces
{
    public interface ICSVFormatter
    {
        string FormatLine(IEnumerable<string> values);
    }
}
