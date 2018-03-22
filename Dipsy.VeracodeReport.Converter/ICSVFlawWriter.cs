using Dipsy.VeracodeReport.Converter.Schema;

namespace Dipsy.VeracodeReport.Converter
{
    public interface ICSVFlawWriter
    {
        void Write(detailedreport detailedXml, string outputFilename, bool includeFixedFlaws);
    }
}