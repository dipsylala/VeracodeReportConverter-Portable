using System;
using System.IO;
using CommandLine;

namespace Dipsy.VeracodeReport.Converter
{
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

            try
            {
                var detailedXml = loader.Parse(options.InputFileName);

                var outputFileName = options.OutputFileName
                                     ?? detailedXml.app_name + (options.GenerateSCA ? "_sca" : string.Empty) + ".csv";

                if (options.GenerateSCA)
                {
                    ICSVAnalysisWriter icsvWriter = new CSVAnalysisWriter(csvFormatter);
                    icsvWriter.Write(detailedXml, outputFileName);
                }
                else
                {
                    ICSVFlawWriter icsvWriter = new CSVFlawWriter(csvFormatter);
                    icsvWriter.Write(detailedXml, outputFileName, options.IncludeFixedFlaws);
                }
            }
            catch (FileNotFoundException)
            {
                Console.Error.WriteLine($"{options.InputFileName} not found");
            }
        }
    }
}
