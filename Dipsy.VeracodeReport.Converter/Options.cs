using CommandLine;

namespace Dipsy.VeracodeReport.Converter
{
    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "Detailed XML file to be processed")]
        public string InputFileName { get; set; }

        [Option('o', "output", Required = false, HelpText = "Output filename")]
        public string OutputFileName { get; set; }

        [Option('f', "fixed", Required = false, HelpText = "Include fixed flaws in the output")]
        public bool IncludeFixedFlaws { get; set; }

        [Option('s', "sca", Required = false, HelpText = "Generate Software Composition Analysis report")]
        public bool GenerateSCA { get; set; }
    }
}
