using System;
using System.Collections.Generic;
using System.IO;

using Dipsy.VeracodeReport.Converter.Interfaces;
using Dipsy.VeracodeReport.Converter.Schema;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Shouldly;

namespace Dipsy.VeracodeReport.Converter.UnitTests
{
    [TestClass]
    public class CSVAnalysisWriterTests
    {
        [TestMethod]
        public void ShouldReturnFilenameIfSupplied()
        {
            var mockFormatter = new Mock<ICSVFormatter>();
            var options = new Options { OutputFileName = "bob.csv", GenerateAnalysis = true };
            var sut = new CSVAnalysisWriter(mockFormatter.Object);
            var detailedReport = new detailedreport();

            sut.GetOutputFilename(detailedReport, options).ShouldBe("bob_sca.csv");
        }

        [TestMethod]
        public void ShouldReturnFilenameFromXML()
        {
            var mockFormatter = new Mock<ICSVFormatter>();
            var options = new Options { GenerateAnalysis = true };
            var sut = new CSVAnalysisWriter(mockFormatter.Object);
            var detailedReport = new detailedreport { app_name = "sca app name" };

            sut.GetOutputFilename(detailedReport, options).ShouldBe("sca app name_sca.csv");
        }

        [TestMethod]
        public void ExceptionShouldBeThrownWhenNullWriter()
        {
            var mockFormatter = new Mock<ICSVFormatter>();
            var options = new Options { OutputFileName = "output.csv" };
            var detailedReport = new detailedreport();

            var sut = new CSVAnalysisWriter(mockFormatter.Object);

            Should.Throw<ArgumentNullException>(() => sut.Write(null, detailedReport, options));
        }

        [TestMethod]
        public void ExceptionShouldBeThrownWhenNullReport()
        {
            var mockFormatter = new Mock<ICSVFormatter>();
            var options = new Options { OutputFileName = "output.csv" };
            var sut = new CSVAnalysisWriter(mockFormatter.Object);

            using var resultStream = new MemoryStream();
            using var resultWriter = new StreamWriter(resultStream);
            Should.Throw<ArgumentNullException>(() => sut.Write(resultWriter, null, options));
        }

        [TestMethod]
        public void ExceptionShouldBeThrownWhenNullOptions()
        {
            var mockFormatter = new Mock<ICSVFormatter>();
            var detailedReport = new detailedreport();
            var sut = new CSVAnalysisWriter(mockFormatter.Object);

            using var resultStream = new MemoryStream();
            using var resultWriter = new StreamWriter(resultStream);
            Should.Throw<ArgumentNullException>(() => sut.Write(resultWriter, detailedReport, null));
        }

        [TestMethod, DeploymentItem("./xml/LoadValidStaticFileWithSCATest.xml")]
        public void ShouldLoadStaticResultsWithSCA()
        {
            var detailedReport = detailedreport.LoadFromFile("./xml/LoadValidStaticFileWithSCATest.xml");
            var mockFormatter = new Mock<ICSVFormatter>();
            var options = new Options { GenerateAnalysis = true };

            using (var resultStream = new MemoryStream())
            using (var resultWriter = new StreamWriter(resultStream))
            {
                var sut = new CSVAnalysisWriter(mockFormatter.Object);
                sut.Write(resultWriter, detailedReport, options);
                resultWriter.Flush();
            }

            // 1 for header, 2 for sca results
            mockFormatter.Verify(x => x.FormatLine(It.IsAny<List<string>>()), Times.Exactly(3));
        }
    }
}
