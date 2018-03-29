using System.IO;

using Dipsy.VeracodeReport.Converter.Schema;

namespace Dipsy.VeracodeReport.Converter.Interfaces
{
    public interface ICSVWriter
    {
        void Write(TextWriter textWriter, detailedreport detailedXml, Options options);

        string GetOutputFilename(detailedreport detailedXml, Options options);
    }
}