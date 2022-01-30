using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Bygdrift.PdfToDrawio.Drawio.Models
{
    public class SvgIn
    {
        public string SvgAsString { get; }
        public XDocument Document { get; }

        public double HeightAsPoints;
        public double WidthAsPoints;
        public double HeightAsDecimatedInches { get { return Math.Round(HeightAsPoints * 1.38888889, 3); } }  //Howto convert: https://www.unitconverters.net/typography-converter.html
        public double WidthAsDecimatedInches { get { return Math.Round(WidthAsPoints * 1.38888889, 3); } }

        public SvgIn(string svg)
        {
            SvgAsString = svg;
            Document = XDocument.Parse(SvgAsString);
            CalculateWidthAndHeight();
        }

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
