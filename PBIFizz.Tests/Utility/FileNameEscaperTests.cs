using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FizzCode.PBIFizz.Tests
{
    [TestClass]
    public class FileNameEscaperTests
    {
        [TestMethod]
        public void EscapeTest()
        {
            Assert.AreEqual("_95_", FileNameEscaper.Escape("_"));
            Assert.AreEqual("_47_", FileNameEscaper.Escape("/"));
            Assert.AreEqual("_92_", FileNameEscaper.Escape("\\"));
        }

        [TestMethod]
        public void UnescapeTest()
        {

            FileNameEscaper.Escape("Test_Underscore");

            Assert.AreEqual("Test_Underscore/Slash",
                FileNameEscaper.Unescape(
                    FileNameEscaper.Escape("Test_Underscore/Slash")
                )
            );
        }
    }
}