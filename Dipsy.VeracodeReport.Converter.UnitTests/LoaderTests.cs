using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Dipsy.VeracodeReport.Converter.UnitTests
{
    [TestClass]
    public class LoaderTests
    {
        [TestMethod, DeploymentItem("./xml/LoadValidFileTest.xml")]
        public void LoadValidFileTest()
        {
            var sut = new Loader();
            var results = sut.Parse("./LoadValidFileTest.xml");

            // Typically we should aim for one assertion per test, but 
            // it's all part of one big XML read and I'll cut it short.
            results.app_id.ShouldBe(405364);
            results.app_name.ShouldBe("Encoding Test");
            results.staticanalysis.modules.Count.ShouldBe(1);
            results.severity.Count.ShouldBe(6);
        }

        [TestMethod, DeploymentItem("./xml/LoadInvalidFileTest.xml")]
        public void LoadInvalidFileTest()
        {
            var sut = new Loader();
            Should.Throw<InvalidOperationException>(() => sut.Parse("./LoadInvalidFileTest.xml"));
        }

        [TestMethod]
        public void LoadNonExistentFileTest()
        {
            var sut = new Loader();
            Should.Throw<FileNotFoundException>(() => sut.Parse(".xml"));
        }
    }
}
