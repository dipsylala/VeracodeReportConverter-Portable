# Veracode Report Converter
Takes a detailed xml report from the Veracode platform and generates a CSV file containing typical fields that are useful in day-to-day reporting.

The Veracode Help Center provides guidance on how to retrieve the detailed xml report here: https://help.veracode.com/reader/DGHxSJy3Gn3gtuSIN2jkRQ/wtJ0ZMLZcYuRd22PmK5vxg

If you don't want the standalone version, you need .NET Core Runtime 2 - available here for your OS: https://www.microsoft.com/net/download/Windows/run

The standalone release is as an exe but contains the whole Runtime as well, hence the size.

## Usage
``` dotnet Dipsy.VeracodeReport.Converter.dll -i <input filename> [-o <output filename>] [-f]```

* -i, --input     Required. Detailed XML file to be processed
* -o, --output    Output filename
* -f, --fixed     Include fixed flaws in the output
* --help          Display this help screen.
* --version       Display version information.

## Examples

``` dotnet Dipsy.VeracodeReport.Converter.dll -i LoadValidFileTest.xml```

```dotnet Dipsy.VeracodeReport.Converter.dll -i LoadValidFileTest.xml -o myoutput.csv```
