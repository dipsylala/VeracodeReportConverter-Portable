# Veracode Report Converter
Takes a detailed xml report from the Veracode platform and generates a CSV file containing typical fields that are useful in day-to-day reporting:

                Flaw ID
                CWE ID
                Category Name
                Description
                Affects Policy Compliance
                Exploit (Manual)
                Severity (Manual)
                Remediation (Manual)
                Date First Occurrence
                Module
                Source File
                Source File Path
                Function Prototype
                Line
                Function Relative Location (%)
                Scope
                Severity
                Exploitability Adjustments
                Grace Period Expires
                Remediation Status
                Mitigation Status
                Mitigation Status Description
                Mitigation Text

It can also optionally output SCA details into another file:

                Library
                Version
                Vendor
                Description
                File Paths
                Licenses
                Max CVSS Score
                Affects Policy Compliance
                Violated Policy Rules
                Vulnerabilities (Very Low)
                Vulnerabilities (Low)
                Vulnerabilities (Medium)
                Vulnerabilities (High)
                Vulnerabilities (Very High)

The Veracode Help Center provides guidance on how to retrieve the detailed xml report here: https://help.veracode.com/reader/DGHxSJy3Gn3gtuSIN2jkRQ/wtJ0ZMLZcYuRd22PmK5vxg

If you don't want the standalone version, you need .NET Core Runtime 2 - available here for your OS: 
https://dotnet.microsoft.com/download/dotnet-core

The standalone release is as an exe but contains the whole Runtime as well, hence the size.

## Usage
``` dotnet Dipsy.VeracodeReport.Converter.dll -i <input filename> [-o <output filename>] [-f]```

* -i, --input     Required. Detailed XML file to be processed
* -o, --output    Output filename
* -f, --fixed     Include fixed flaws in the output
* -s, --sca       Generate Software Composition Analysis report
* --help          Display this help screen.
* --version       Display version information.

## Examples

``` dotnet Dipsy.VeracodeReport.Converter.dll -i LoadValidFileTest.xml```

``` dotnet Dipsy.VeracodeReport.Converter.dll -i LoadValidFileTest.xml -o myoutput.csv```
