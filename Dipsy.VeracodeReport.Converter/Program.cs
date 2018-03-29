using System;
using System.IO;
using CommandLine;

namespace Dipsy.VeracodeReport.Converter
{
    using System.Text;

    using Dipsy.VeracodeReport.Converter.Interfaces;
    using Dipsy.VeracodeReport.Converter.Schema;

    public class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(HandleParsed);
        }

        private static void HandleParsed(Options options)
        {
            ILoader loader = new Loader();
            ICSVFormatter csvFormatter = new CSVFormatter();
            ICSVWriter csvFlawWriter = new CSVFlawWriter(csvFormatter);
            ICSVWriter csvAnalysisWriter = new CSVAnalysisWriter(csvFormatter);
            detailedreport detailedXml;

            try
            {
                detailedXml = loader.Parse(options.InputFileName);
            }
            catch (FileNotFoundException)
            {
                Console.Error.WriteLine($"Could not find {options.InputFileName}");
                return;
            }
            catch (InvalidOperationException ex)
            {
                Console.Error.WriteLine($"Error parsing {options.InputFileName}: {ex.Message}");
                return;
            }

            var flawOutputFilename = csvFlawWriter.GetOutputFilename(detailedXml, options);
            try
            {
                using (var outFile = new StreamWriter(flawOutputFilename, false, Encoding.UTF8))
                {
                    csvFlawWriter.Write(outFile, detailedXml, options);
                }
            }
            catch (IOException)
            {
                Console.Error.WriteLine($"Error writing to {flawOutputFilename}");
                return;
            }

            if (options.GenerateAnalysis == false)
            {
                return;
            }

            var scaOutputFilename = csvAnalysisWriter.GetOutputFilename(detailedXml, options);
            try
            {
                using (var outFile = new StreamWriter(scaOutputFilename, false, Encoding.UTF8))
                {
                    csvAnalysisWriter.Write(outFile, detailedXml, options);
                }
            }
            catch (IOException)
            {
                Console.Error.WriteLine($"Error writing to {scaOutputFilename}");
            }
        }
    }
}
