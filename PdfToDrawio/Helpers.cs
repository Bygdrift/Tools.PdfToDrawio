using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Bygdrift.PdfToDrawio
{
    public class Helpers
    {
        public static byte[] ZipStr(String str)
        {
            using var output = new MemoryStream();
            using (var gzip = new DeflateStream(output, CompressionMode.Compress))
            using (var writer = new StreamWriter(gzip, Encoding.UTF8))
            writer.Write(str);

            return output.ToArray();
        }

        public static string UnZipStr(byte[] input)
        {
            using var inputStream = new MemoryStream(input);
            using var gzip = new DeflateStream(inputStream, CompressionMode.Decompress);
            using var reader = new StreamReader(gzip, Encoding.UTF8);
            return reader.ReadToEnd();
        }

        internal static string CompileXml(XElement element, XmlWriterSettings settings)
        {
            var stringBuild = new StringBuilder();
            using (var xmlWriter = XmlWriter.Create(stringBuild, settings))
                element.WriteTo(xmlWriter);

            return stringBuild.ToString();
        }
    }
}
