using System;

using Dipsy.VeracodeReport.Converter.Interfaces;
using Dipsy.VeracodeReport.Converter.Schema;

namespace Dipsy.VeracodeReport.Converter
{
    using System.IO;

    public abstract class CSVWriterBase
    {
        protected readonly ICSVFormatter CSVFormatter;

        protected readonly string DefaultMultilineSeparator = "---------\n";

        protected CSVWriterBase(ICSVFormatter csvFormatter)
        {
            CSVFormatter = csvFormatter;
        }

        protected void ValidateWriteParameters(TextWriter outFile, detailedreport detailedXml, Options options)
        {
            ArgumentNullException.ThrowIfNull(outFile);

            ArgumentNullException.ThrowIfNull(detailedXml);

            ArgumentNullException.ThrowIfNull(options);
        }
    }
}
