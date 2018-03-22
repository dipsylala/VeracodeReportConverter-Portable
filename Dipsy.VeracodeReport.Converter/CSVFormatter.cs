using System.Collections.Generic;

namespace Dipsy.VeracodeReport.Converter
{
    public sealed class CSVFormatter : ICSVFormatter
    {
        public string FormatLine(IEnumerable<string> values)
        {
            var valueLine = string.Empty;

            foreach (var val in values)
            {
                if (val != null)
                {
                    var valueString = ProcessValueForSpecialCharacters(val);
                    valueLine = string.Concat(valueLine, valueString, ",");
                }
                else
                {
                    valueLine += ",";
                }
            }

            return valueLine.TrimEnd(',');
        }

        private static string ProcessValueForSpecialCharacters(string value)
        {
            if (value.Contains(",") || 
                value.Contains("\r") || 
                value.Contains("\n") || 
                value.Contains("\""))
            {
                value = value.Replace("\"", "\"\"");
                value = string.Concat("\"", value, "\"");
            }

            return value;
        }
    }
}