using Bygdrift.PdfToDrawio.Drawio.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Bygdrift.PdfToDrawio
{
    public class Convert
    {
        internal readonly MxGraphModel GraphModel;

        public Convert(Stream stream, Format? format)
        {
            if (stream != null)
                stream.Position = 0;

            var svgs = new List<SvgIn>();
            if (format == Format.PDF)
            {
                using var doc = PdfToSvg.PdfDocument.Open(stream);
                foreach (var page in doc.Pages)
                    svgs.Add(new SvgIn(page.ToSvgString()));
            }
            else if (format == Format.SVG)
            {
                var doc = XDocument.Load(stream);
                svgs.Add(new SvgIn(doc.Root.ToString()));
            }
            GraphModel = new MxGraphModel(svgs);
        }

        /// <param name="inputFile">Either a path for a pdf or svg-file</param>
        public Convert(string inputFile) : this(FileInToStream(inputFile), GetFileFormat(inputFile)) { }

        private static Stream FileInToStream(string fileName)
        {
            return new FileStream(fileName, FileMode.Open);
        }

        private static Format? GetFileFormat(string fileName)
        {
            var extension = Path.GetExtension(fileName).TrimStart('.').ToUpper();
            if (!Enum.TryParse(extension, out Format res))
                throw new Exception($"The fil has extension {extension}, but it can only be PDF or SVG.");

            return res;
        }

        /// <param name="outputFile">A filepath to where the drawio-file can be saved</param>
        public void ToDrawIo(string outputFile)
        {
            FilePathCheck(outputFile, ".drawio");
            DrawioFileOut.ToFile(GraphModel, outputFile);
        }

        /// <param name="outputFile">A filepath to where the drawio-file can be saved</param>
        public Stream ToDrawIo()
        {
            return DrawioFileOut.ToStream(GraphModel);
        }

        /// <summary>
        /// Drawio works with mxGraphModel and here the content is presented before it gets packed into a drawio or svg-file
        /// </summary>
        /// <param name="outputFile">A filepath to where the xml-file can be saved</param>
        public void ToMxGraphModel(string outputFile)
        {
            FilePathCheck(outputFile, ".xml");
            GraphModel.ToFile(outputFile);
        }

        /// <summary>
        /// Drawio works with mxGraphModel and here the content is presented before it gets packed into a drawio or svg-file
        /// </summary>
        public Stream ToMxGraphModel()
        {
           return GraphModel.ToStream();
        }

        private static void FilePathCheck(string file, string extension)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var fileExtension = Path.GetExtension(file).ToLower();
            if (fileExtension != extension.ToLower())
                throw new Exception($"The extension on the file path should be {extension.ToLower()}, but it is {fileExtension}.");
        }
    }

    public enum Format
    {
        PDF,
        SVG
    }
}