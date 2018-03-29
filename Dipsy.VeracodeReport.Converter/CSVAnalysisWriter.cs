using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Dipsy.VeracodeReport.Converter.Interfaces;
using Dipsy.VeracodeReport.Converter.Schema;

namespace Dipsy.VeracodeReport.Converter
{
    public class CSVAnalysisWriter : CSVWriterBase, ICSVWriter
    {
        public CSVAnalysisWriter(ICSVFormatter csvFormatter) : base(csvFormatter)
        {
        }

        public void Write(TextWriter outFile, detailedreport detailedXml, Options options)
        {
            ValidateWriteParameters(outFile, detailedXml, options);
            WriteHeader(outFile);
            WriteComponents(detailedXml.software_composition_analysis.vulnerable_components, outFile);
        }

        public string GetOutputFilename(detailedreport detailedXml, Options options)
        {
            var baseFilename = options.OutputFileName ?? detailedXml.app_name + ".csv";

            // If we're generating flaws and SCA, add _sca
            var newFilename = Path.GetFileNameWithoutExtension(baseFilename) + "_sca"
                                                                             + Path.GetExtension(
                                                                                 baseFilename);
            return Path.Combine(Path.GetDirectoryName(baseFilename), newFilename);
        }

        private string FormatVulnerabilities(IEnumerable<Vulnerability> vulnerabilities, int severity)
        {
            var severityString = Convert.ToString(severity);

            return string.Join(
                DefaultMultilineSeparator,
                vulnerabilities.Where(x => x.severity == severityString).Select(x => string.Format(
                    "CWE: {0}, CVE: {1}. Summary: {2}",
                    x.cwe_id == string.Empty ? "N/A" : x.cwe_id,
                    x.cve_id == string.Empty ? "N/A" : x.cve_id,
                    x.cve_summary == string.Empty ? "N/A" : x.cve_summary)));
        }

        private void WriteComponents(IEnumerable<Component> components, TextWriter outFile)
        {
            foreach (var component in components)
            {
                WriteComponentToFile(component, outFile);
            }
        }

        private void WriteHeader(TextWriter outFile)
        {
            var csvLine = CSVFormatter.FormatLine(new List<string>
            {
                "Library",
                "Version",
                "Vendor",
                "Description",
                "File Paths",
                "Licenses",
                "Max CVSS Score",
                "Affects Policy Compliance",
                "Violated Policy Rules",
                "Vulnerabilities (Very Low)",
                "Vulnerabilities (Low)",
                "Vulnerabilities (Medium)",
                "Vulnerabilities (High)",
                "Vulnerabilities (Very High)"
            });

            outFile.WriteLine(csvLine);
        }

        private void WriteComponentToFile(Component component, TextWriter outFile)
        {
            var filePaths = FormatFilePaths(component.file_paths);
            var licenses = FormatLicenses(component.licenses);
            var violatedPolicyRules = FormatViolatedPolicyRules(component.violated_policy_rules);

            var csvLine = CSVFormatter.FormatLine(
                new List<string>
                    {
                        component.library,
                        component.version,
                        component.vendor,
                        component.description,
                        filePaths,
                        licenses,
                        component.max_cvss_score,
                        component.component_affects_policy_compliance,
                        violatedPolicyRules,
                        FormatVulnerabilities(component.vulnerabilities, 1),
                        FormatVulnerabilities(component.vulnerabilities, 2),
                        FormatVulnerabilities(component.vulnerabilities, 3),
                        FormatVulnerabilities(component.vulnerabilities, 4),
                        FormatVulnerabilities(component.vulnerabilities, 5)
                    });

            outFile.WriteLine(csvLine);
        }

        private string FormatViolatedPolicyRules(IEnumerable<PolicyRule> componentViolatedPolicyRules)
        {
            return string.Join("\n", componentViolatedPolicyRules.Select(x => x.desc));
        }

        private string FormatLicenses(IEnumerable<License> componentLicenses)
        {
            return string.Join("\n", componentLicenses.Select(x => $"Name: {x.name}, URL: {x.license_url}"));
        }

        private string FormatFilePaths(IEnumerable<FilePath> componentFilePaths)
        {
            return string.Join("\n", componentFilePaths.Select(x => x.value));
        }
    }
}
