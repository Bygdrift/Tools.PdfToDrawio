using System;
using System.Xml.Linq;

namespace Bygdrift.Tools.PdfToDrawio
{
    /// <summary>Import of the svg</summary>
    public class SvgIn
    {
        /// <summary>Height of the paper</summary>
        public double HeightAsPoints;

        /// <summary>Width of the paper</summary>
        public double WidthAsPoints;

        /// <summary>Import of the svg</summary>
        public SvgIn(string svg)
        {
            SvgAsString = svg;
            Document = XDocument.Parse(SvgAsString);
            CalculateWidthAndHeight();
        }

        /// <summary>The svg</summary>
        public string SvgAsString { get; }

        /// <summary>The document</summary>
        public XDocument Document { get; }

        /// <summary>Height of the paper</summary>
        public double HeightAsDecimatedInches { get { return Math.Round(HeightAsPoints * 1.38888889, 3); } }  //Howto convert: https://www.unitconverters.net/typography-converter.html

        /// <summary>Width of the paper</summary>
        public double WidthAsDecimatedInches { get { return Math.Round(WidthAsPoints * 1.38888889, 3); } }

        private void CalculateWidthAndHeight()
        {
            if (!double.TryParse(Document.Root.Attribute("width")?.Value, out double WidthAsPoints) | !double.TryParse(Document.Root.Attribute("height")?.Value, out double HeightAsPoints))
            {
                var viewbox = Document.Root.Attribute("viewBox")?.Value;
                if (!string.IsNullOrEmpty(viewbox))
                {
                    var viewboxSplit = viewbox.Split(' ');
                    if (viewboxSplit.Length == 4)
                    {
                        double.TryParse(viewboxSplit[2], out WidthAsPoints);
                        double.TryParse(viewboxSplit[3], out HeightAsPoints);
                    }
                }
            }

            if (WidthAsPoints == 0 || HeightAsPoints == 0)
            {
                WidthAsPoints = 595.27559055;  //A4 format
                HeightAsPoints = 841.88976378;  //A4 fomrat
            }

            this.WidthAsPoints = WidthAsPoints;
            this.HeightAsPoints = HeightAsPoints;
        }
    }
}
