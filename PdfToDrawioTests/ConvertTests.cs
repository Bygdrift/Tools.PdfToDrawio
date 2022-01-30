using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace PdfToDrawioTests
{
    [TestClass]
    public class ConvertTests
    {
        private static readonly string basePath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));

        [TestMethod]
        public void PdfToDrawIo()
        {
            var fileIn = Path.Combine(basePath, "Files", "In", "SomeContent.pdf");
            var fileOut = Path.Combine(basePath, "Files", "Out", "SomeContent from pdf.drawio");

            var drawIo = new Bygdrift.PdfToDrawio.Convert(fileIn);
            drawIo.ToDrawIo(fileOut);
            var stream = drawIo.ToDrawIo();
            Assert.IsTrue(stream.Length > 100000);
        }

        [TestMethod]
        public void PdfToXml()
        {
            var fileIn = Path.Combine(basePath, "Files", "In", "SomeContent.pdf");
            var fileOut = Path.Combine(basePath, "Files", "Out", "SomeContent from pdf.xml");

            var drawIo = new Bygdrift.PdfToDrawio.Convert(fileIn);
            drawIo.ToMxGraphModel(fileOut + ".xml");
            var stream = drawIo.ToMxGraphModel();
            Assert.IsTrue(stream.Length > 100000);
        }

        [TestMethod]
        public void SvgToDrawIo()
        {
            var fileIn = Path.Combine(basePath, "Files", "In", "SomeContent.svg");
            var fileOut = Path.Combine(basePath, "Files", "Out", "SomeContent from svg.drawio");

            var drawIo = new Bygdrift.PdfToDrawio.Convert(fileIn);
            drawIo.ToDrawIo(fileOut);
            var stream = drawIo.ToDrawIo();
            Assert.IsTrue(stream.Length > 100000);
        }

        [TestMethod]
        public void SvgToXml()
        {
            var fileIn = Path.Combine(basePath, "Files", "In", "SomeContent.svg");
            var fileOut = Path.Combine(basePath, "Files", "Out", "SomeContent from svg.xml");

            var drawIo = new Bygdrift.PdfToDrawio.Convert(fileIn);
            drawIo.ToMxGraphModel(fileOut + ".xml");
            var stream = drawIo.ToMxGraphModel();
            Assert.IsTrue(stream.Length > 100000);
        }
    }
}