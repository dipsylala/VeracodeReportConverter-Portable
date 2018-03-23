using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dipsy.VeracodeReport.Converter.Schema;

namespace Dipsy.VeracodeReport.Converter
{
    public class CSVFlawWriter : ICSVFlawWriter
    {
        private readonly ICSVFormatter csvFormatter;

        private Dictionary<int, string> severityCache;

        public CSVFlawWriter(ICSVFormatter csvFormatter)
        {
            this.csvFormatter = csvFormatter;
            this.PopulateSeverityCache();
        }

        public void Write(detailedreport detailedXml, string outputFilename, bool includeFixed)
        {
            if (detailedXml == null)
            {
                throw new ArgumentNullException(nameof(detailedXml));
            }

            if (outputFilename == null)
            {
                throw new ArgumentNullException(nameof(outputFilename));
            }

            using (var outFile = new StreamWriter(outputFilename, false, Encoding.UTF8))
            {
                this.WriteHeader(outFile);
                foreach (var severity in detailedXml.severity)
                {
                    foreach (var category in severity.category)
                    {
                        foreach (var cwe in category.cwe)
                        {
                            this.WriteFlawsToFile(cwe.staticflaws, outFile, includeFixed);
                            this.WriteFlawsToFile(cwe.dynamicflaws, outFile, includeFixed);
                            this.WriteFlawsToFile(cwe.manualflaws, outFile, includeFixed);
                        }
                    }
                }
            }
        }

        private static string FormatExploitabilityAdjustments(IEnumerable<ExploitAdjustmentType> flawExploitabilityAdjustments)
        {
            const string AdjustmentSeparator = "---------\n";

            return string.Join(AdjustmentSeparator, flawExploitabilityAdjustments.Select(x => $"Adjustment: {x.score_adjustment}, Note: {x.note}"));
        }

        private static string FormatMitigations(IEnumerable<MitigationType> mitigations)
        {
            const string MitigationSeparator = "---------\n";

            return string.Join(MitigationSeparator, mitigations.Select(x => $"{x.date} ({x.user}) - {x.action}\n{x.description.TrimEnd('\n')}\n"));
        }

        private void WriteFlawsToFile(IEnumerable<FlawType> flaws, StreamWriter outFile, bool includeFixed)
        {
            foreach (var flaw in flaws.Where(x => includeFixed || x.remediation_status != "Fixed"))
            {
                this.WriteFlawToFile(flaw, outFile);
            }
        }

        private void WriteHeader(TextWriter outFile)
        {
            var csvLine = this.csvFormatter.FormatLine(new List<string>
            {
                "Flaw ID",
                "CWE ID",
                "Category Name",
                "Description",
                "Affects Policy Compliance",
                "Exploit (Manual)",
                "Severity (Manual)",
                "Remediation (Manual)",
                "Date First Occurrence",
                "Module",
                "Source File",
                "Source File Path",
                "Function Prototype",
                "Line",
                "Function Relative Location (%)",
                "Scope",
                "Severity",
                "Exploitability Adjustments",
                "Grace Period Expires",
                "Remediation Status",
                "Mitigation Status",
                "Mitigation Status Description",
                "Mitigation Text"
            });

            outFile.WriteLine(csvLine);
        }

        private void WriteFlawToFile(FlawType flaw, TextWriter outFile)
        {
            var mitigations = FormatMitigations(flaw.mitigations);
            var exploitabilityAdjustments = FormatExploitabilityAdjustments(flaw.exploitability_adjustments);
            var severity = this.SeverityStringFromNum(flaw.severity);

            var csvLine = this.csvFormatter.FormatLine(
                new List<string>
                    {
                        flaw.issueid,
                        flaw.cweid,
                        flaw.categoryname,
                        flaw.description,
                        flaw.affects_policy_compliance ? "True" : "False",
                        flaw.exploit_desc,
                        flaw.severity_desc,
                        flaw.remediation_desc,
                        flaw.date_first_occurrence,
                        flaw.module,
                        flaw.sourcefile,
                        flaw.sourcefilepath,
                        flaw.functionprototype,
                        flaw.line,
                        flaw.functionrelativelocation == "-1"
                            ? string.Empty
                            : flaw.functionrelativelocation,
                        flaw.scope,
                        severity,
                        exploitabilityAdjustments,
                        flaw.grace_period_expires,
                        flaw.remediation_status,
                        flaw.mitigation_status,
                        flaw.mitigation_status_desc,
                        mitigations
                    });

            outFile.WriteLine(csvLine);
        }

        private string SeverityStringFromNum(string severity)
        {
            if (int.TryParse(severity, out var severityId) == false)
            {
                throw new ArgumentOutOfRangeException(nameof(severity));
            }

            return this.severityCache[severityId];
        }

        private void PopulateSeverityCache()
        {
            this.severityCache = new Dictionary<int, string>
            {
                { 0, "0 - Informational" },
                { 1, "1 - Very Low" },
                { 2, "2 - Low" },
                { 3, "3 - Medium" },
                { 4, "4 - High" },
                { 5, "5 - Very High" }
            };
        }
    }
}
