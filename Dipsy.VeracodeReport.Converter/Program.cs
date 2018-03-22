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
            ICSVFlawWriter icsvWriter = new CSVFlawWriter(csvFormatter);

            try
            {
                var detailedXml = loader.Parse(options.InputFileName);

                var outputFileName = options.OutputFileName ?? detailedXml.app_name + ".csv";

                icsvWriter.Write(detailedXml, outputFileName, options.IncludeFixedFlaws);
            }
            catch (FileNotFoundException)
            {
                Console.Error.WriteLine($"{options.InputFileName} not found");
            }
        }
    }
}
