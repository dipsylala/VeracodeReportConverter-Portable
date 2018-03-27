using Dipsy.VeracodeReport.Converter.Schema;

namespace Dipsy.VeracodeReport.Converter
{
    public interface ICSVAnalysisWriter
    {
        void Write(detailedreport detailedXml, string outputFilename);
    }
}