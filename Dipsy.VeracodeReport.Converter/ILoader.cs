using Dipsy.VeracodeReport.Converter.Schema;

namespace Dipsy.VeracodeReport.Converter
{
    public interface ILoader
    {
        detailedreport Parse(string optionsInputFile);
    }
}