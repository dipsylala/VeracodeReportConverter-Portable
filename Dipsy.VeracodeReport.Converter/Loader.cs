using Dipsy.VeracodeReport.Converter.Schema;

namespace Dipsy.VeracodeReport.Converter
{
    public class Loader : ILoader
    {
        public detailedreport Parse(string optionsInputFile)
        {
            // One-liner, but here so that the loading is decoupled completely from the program class
            return detailedreport.LoadFromFile(optionsInputFile);
        }
    }
}
