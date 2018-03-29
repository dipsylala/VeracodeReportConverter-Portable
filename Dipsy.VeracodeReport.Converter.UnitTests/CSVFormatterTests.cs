using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Dipsy.VeracodeReport.Converter.UnitTests
{
    [TestClass]
    public class CSVFormatterTests
    {
        [TestMethod]
        public void NormalValuesShouldBeCommaDelimited()
        {
            var sut = new CSVFormatter();
            var result = sut.FormatLine(new List<string>
            {
                "value a",
                "value b",
                "value c",
                "value d"
            });

            result.ShouldBe("value a,value b,value c,value d");
        }

        [TestMethod]
        public void CommaValuesShouldBeSurroundedWithQuotes()
        {
            var sut = new CSVFormatter();
            var result = sut.FormatLine(new List<string>
            {
                "value a",
                "value,b",
                "value c",
                "value d"
            });

            result.ShouldBe("value a,\"value,b\",value c,value d");
        }

        [TestMethod]
        public void QuotedValuesShouldBeSurroundedWithQuotes()
        {
            var sut = new CSVFormatter();
            var result = sut.FormatLine(new List<string>
            {
                "value a",
                "\"value b\"",
                "value\"c",
                "value d"
            });

            result.ShouldBe("value a,\"\"\"value b\"\"\",\"value\"\"c\",value d");
        }

        [TestMethod]
        public void CarriageReturnValuesShouldBeSurroundedWithQuotes()
        {
            var sut = new CSVFormatter();
            var result = sut.FormatLine(new List<string>
            {
                "value a",
                "value b",
                "value c",
                "value d\nvalue e\nvalue f"
            });

            result.ShouldBe("value a,value b,value c,\"value d\nvalue e\nvalue f\"");
        }

        [TestMethod]
        public void LineFeedValuesShouldBeSurroundedWithQuotes()
        {
            var sut = new CSVFormatter();
            var result = sut.FormatLine(new List<string>
            {
                "value a",
                "value b",
                "value c",
                "value d\rvalue e\rvalue f"
            });

            result.ShouldBe("value a,value b,value c,\"value d\rvalue e\rvalue f\"");
        }

        [TestMethod]
        public void NullValuesShouldBeEmptyStrings()
        {
            var sut = new CSVFormatter();
            var result = sut.FormatLine(new List<string>
                                            {
                                                "value a",
                                                null,
                                                "value c"
                                            });

            result.ShouldBe("value a,,value c");
        }
    }
}
