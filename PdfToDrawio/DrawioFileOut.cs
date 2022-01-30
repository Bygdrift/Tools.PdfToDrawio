using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Bygdrift.PdfToDrawio
{
    internal class DrawioFileOut
    {
        internal static void ToFile(MxGraphModel graphModel, string outputFile)
        {
            var drawioFile = DrawioFile(graphModel);
            File.WriteAllText(outputFile, drawioFile);
        }

        internal static Stream ToStream(MxGraphModel graphModel)
        {
            var drawioFile = DrawioFile(graphModel);
            return new MemoryStream(Encoding.UTF8.GetBytes(drawioFile ?? ""));
        }

        private static string DrawioFile(MxGraphModel graphModel)
        {
            var diagramContent = graphModel.Deflate(false, true, "\n");
            var doc = new XDocument(
                new XElement("mxfile",
                    new XAttribute("host", "app.diagrams.net"),
                    new XAttribute("modified", DateTime.UtcNow),
                    new XAttribute("type", "device"),
                    new XAttribute("version", "13.7.3"),
                    new XElement("diagram",
                        new XAttribute("id", "1"),
                        new XAttribute("name", "Page-1"),
                        diagramContent
                )));

            return doc.ToString();
        }
    }
}
