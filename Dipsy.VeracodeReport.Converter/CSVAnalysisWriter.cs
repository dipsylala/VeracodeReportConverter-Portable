using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dipsy.VeracodeReport.Converter.Schema;

namespace Dipsy.VeracodeReport.Converter
{
    public class CSVAnalysisWriter : ICSVAnalysisWriter
    {
        private readonly ICSVFormatter csvFormatter;

        public CSVAnalysisWriter(ICSVFormatter csvFormatter)
        {
            this.csvFormatter = csvFormatter;
        }

        public void Write(detailedreport detailedXml, string outputFilename)
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
                this.Write(outFile, detailedXml);
            }
        }

        public void Write(TextWriter outFile, detailedreport detailedXml)
        {
            // Separated so that we can mock the Writer (and caller might have their own TextWriter derivative)
            this.WriteHeader(outFile);
            this.WriteComponentsToFile(detailedXml.software_composition_analysis.vulnerable_components, outFile);
        }

        private static string FormatVulnerabilities(IEnumerable<Vulnerability> vulnerabilities, int severity)
        {
            const string VulnerabilitySeparator = "---------\n";
            var severityString = Convert.ToString(severity);

            return string.Join(
                VulnerabilitySeparator,
                vulnerabilities.Where(x => x.severity == severityString).Select(x => string.Format(
                    "CWE: {0}, CVE: {1}. Summary: {2}",
                    x.cwe_id == string.Empty ? "N/A" : x.cwe_id,
                    x.cve_id == string.Empty ? "N/A" : x.cve_id,
                    x.cve_summary == string.Empty ? "N/A" : x.cve_summary)));
        }

        private void WriteComponentsToFile(IEnumerable<Component> components, TextWriter outFile)
        {
            foreach (var component in components)
            {
                this.WriteComponentToFile(component, outFile);
            }
        }

        private void WriteHeader(TextWriter outFile)
        {
            var csvLine = this.csvFormatter.FormatLine(new List<string>
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
            var filePaths = this.FormatFilePaths(component.file_paths);
            var licenses = this.FormatLicenses(component.licenses);
            var violatedPolicyRules = this.FormatViolatedPolicyRules(component.violated_policy_rules);

            var csvLine = this.csvFormatter.FormatLine(
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
            const string ViolationSeparator = "\n";

            return string.Join(ViolationSeparator, componentViolatedPolicyRules.Select(x => x.desc));
        }

        private string FormatLicenses(IEnumerable<License> componentLicenses)
        {
            const string FilePathSeparator = "\n";

            return string.Join(FilePathSeparator, componentLicenses.Select(x => $"Name: {x.name}, URL: {x.license_url}"));
        }

        private string FormatFilePaths(IEnumerable<FilePath> componentFilePaths)
        {
            const string FilePathSeparator = "\n";

            return string.Join(FilePathSeparator, componentFilePaths.Select(x => x.value));
        }
    }
}
