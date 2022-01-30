using Bygdrift.PdfToDrawio.Drawio.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Bygdrift.PdfToDrawio
{
    internal class MxGraphModel
    {
        internal XDocument Document { get; private set; }
        internal readonly List<SvgIn> Svgs;
        internal readonly double LargestWidth;
        internal readonly double LargestHeight;

        public MxGraphModel(List<SvgIn> svgs)
        {
            Svgs = svgs;
            LargestWidth = Math.Round(Svgs.Max(o => o.WidthAsDecimatedInches), 3);
            LargestHeight = Math.Round(Svgs.Max(o => o.HeightAsDecimatedInches), 3);
            CreateDocument();
            AddSvgToDocument();
        }

        private void CreateDocument()
        {
            //grid="1" gridSize="10" guides="1" tooltips="1" connect="1" arrows="1" fold="1" math="0" shadow="0"
            Document = new XDocument(
                new XElement("mxGraphModel",
                    new XAttribute("dx", LargestWidth),
                    new XAttribute("dy", LargestHeight),
                    new XAttribute("page", "1"),
                    new XAttribute("pageScale", "1"),
                    new XAttribute("pageWidth", LargestWidth),
                    new XAttribute("pageHeight", LargestHeight),

                    new XAttribute("grid", "1"),
                    new XAttribute("gridSize", "10"),
                    new XAttribute("guides", "1"),
                    new XAttribute("tooltips", "1"),
                    new XAttribute("connect", "1"),
                    new XAttribute("arrows", "1"),
                    new XAttribute("fold", "1"),
                    new XAttribute("math", "0"),
                    new XAttribute("shadow", "0"),

                    new XElement("root",
                    new XElement("mxCell",
                        new XAttribute("id", 0)
                    ),
                    new XElement("mxCell",
                        new XAttribute("id", 1),
                        new XAttribute("parent", "0"),
                        new XAttribute("value", "Background"),
                        new XAttribute("style", "locked=1;")
                    ),
                    new XElement("mxCell",
                        new XAttribute("id", 2),
                        new XAttribute("parent", "0"),
                        new XAttribute("value", "Layer 1")
                    )
                )));
        }

        private void AddSvgToDocument()
        {
            int nodeId = 3;
            var settings = new XmlWriterSettings { Indent = false, OmitXmlDeclaration = true, IndentChars = "", NewLineChars = "", NewLineHandling = NewLineHandling.None };
            for (int i = 0; i < Svgs.Count; i++)
            {
                var svg = Helpers.CompileXml(Svgs[i].Document.Root, settings);
                var svgAsBase64 = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(svg));
                var style = $"shape=image;verticalLabelPosition=bottom;labelBackgroundColor=#ffffff;verticalAlign=top;aspect=fixed;imageAspect=0;image=data:image/svg+xml,{ svgAsBase64 };";

                Document.Root.Element("root").Add(new XElement("mxCell",
                    new XAttribute("id", nodeId++.ToString()),
                    new XAttribute("parent", "1"),
                    new XAttribute("vertex", "1"),
                    new XAttribute("style", style),
                    new XElement("mxGeometry",
                        new XAttribute("as", "geometry"),
                        new XAttribute("x", LargestWidth * i),
                        new XAttribute("y", 0),
                        new XAttribute("width", Svgs[i].WidthAsDecimatedInches),
                        new XAttribute("height", Svgs[i].HeightAsDecimatedInches)
                )));
            }
        }

        internal void ToFile(string file)
        {
            File.WriteAllText(file, Document.ToString());
        }

        internal Stream ToStream()
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(Document.ToString() ?? ""));
        }


        /// <summary>
        /// Exports content of diagram: <mxfile><diagram>content</diagram></mxfile>
        /// </summary>
        internal string Deflate(bool urlEncode, bool indent, string newLineChars)
        {
            var settings = new XmlWriterSettings { Indent = indent, OmitXmlDeclaration = true, NewLineChars = newLineChars, NewLineHandling = NewLineHandling.Replace, Encoding = Encoding.UTF8 };
            var xml = Helpers.CompileXml(Document.Root, settings);
            if (urlEncode)
                xml = HttpUtility.UrlEncode(xml);

            var xmlZipped = Helpers.ZipStr(xml);
            return System.Convert.ToBase64String(xmlZipped);
        }
    }
}
