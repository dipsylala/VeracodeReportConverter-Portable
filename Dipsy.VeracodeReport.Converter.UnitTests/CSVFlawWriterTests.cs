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
    public class CSVFlawWriterTests
    {
        [TestMethod]
        public void ShouldReturnFilenameIfSupplied()
        {
            var mockFormatter = new Mock<ICSVFormatter>();
            var options = new Options { OutputFileName = "input.csv"};
            var sut = new CSVFlawWriter(mockFormatter.Object);
            var detailedReport = new detailedreport();

            sut.GetOutputFilename(detailedReport, options).ShouldBe("input.csv");
        }

        [TestMethod]
        public void ShouldReturnFilenameFromXML()
        {
            var mockFormatter = new Mock<ICSVFormatter>();
            var options = new Options( );
            var sut = new CSVFlawWriter(mockFormatter.Object);
            var detailedReport = new detailedreport { app_name = "app name" };

            sut.GetOutputFilename(detailedReport, options).ShouldBe("app name.csv");
        }

        [TestMethod]
        public void ExceptionShouldBeThrownWhenNullWriter()
        {
            var mockFormatter = new Mock<ICSVFormatter>();
            var options = new Options { OutputFileName = "output.csv" };
            var sut = new CSVFlawWriter(mockFormatter.Object);
            var detailedReport = new detailedreport();

            Should.Throw<ArgumentNullException>(() => sut.Write(null, detailedReport, options));
        }

        [TestMethod]
        public void ExceptionShouldBeThrownWhenNullReport()
        {
            var mockFormatter = new Mock<ICSVFormatter>();
            var options = new Options { OutputFileName = "output.csv" };
            var sut = new CSVFlawWriter(mockFormatter.Object);

            using (var resultStream = new MemoryStream())
            using (var resultWriter = new StreamWriter(resultStream))
            {
                Should.Throw<ArgumentNullException>(() => sut.Write(resultWriter, null, options));
                resultWriter.Flush();
            }
        }

        [TestMethod]
        public void ExceptionShouldBeThrownWhenNullOptions()
        {
            var mockFormatter = new Mock<ICSVFormatter>();
            var detailedReport = new detailedreport();
            var sut = new CSVFlawWriter(mockFormatter.Object);

            using (var resultStream = new MemoryStream())
            using (var resultWriter = new StreamWriter(resultStream))
            {
                Should.Throw<ArgumentNullException>(() => sut.Write(resultWriter, detailedReport, null));
            }
        }

        [TestMethod, DeploymentItem("./xml/LoadValidStaticFileTest.xml")]
        public void ShouldLoadStaticResults()
        {
            var detailedReport = detailedreport.LoadFromFile("./LoadValidStaticFileTest.xml");
            var options = new Options
                              {
                                  IncludeFixedFlaws = false
                              };
            var mockFormatter = new Mock<ICSVFormatter>();

            using (var resultStream = new MemoryStream())
            using (var resultWriter = new StreamWriter(resultStream))
            {
                var sut = new CSVFlawWriter(mockFormatter.Object);
                sut.Write(resultWriter, detailedReport, options);
                resultWriter.Flush();
            }

            // 1 for header, 5 for static results
            mockFormatter.Verify(x => x.FormatLine(It.IsAny<IEnumerable<string>>()), Times.Exactly(6));
        }
    }
}
