# PdfToDrawio

With this project, you can convert a pdf or svg-file to a drawio-file. Drawio-files can be read with [Diagrams.net](https://www.diagrams.net), which is an excellent, free diagramming and vector graphics application, that can be used [online](https://app.diagrams.net), as a [desktop-program](https://github.com/jgraph/drawio-desktop/releases/tag/v14.9.6) and as a [plugin to visual studio code](https://marketplace.visualstudio.com/items?itemName=hediet.vscode-drawio).

This project uses a great project called [pdftosvg](https://github.com/dmester/pdftosvg.net) to convert pdf to svg and then merges the svg into drawio in a layer called Background and locks the layer. It also creates a new layer so the file is ready to draw upon. Here is an example on a [drawio file](https://github.com/Bygdrift/PdfToDrawio/tree/master/PdfToDrawioTests/Files/Out).

The reason for this project, was the need of making an extension to an FM system (facility management), that can be integrated on a homepage, so users can download drawing files of buildings, to a free drawing program, and easily make there own:
- Evacuation map [like this](https://raw.githubusercontent.com/Bygdrift/PdfToDrawio/master/Documentation/Images/Fire-Plan.jpg)
- "You are here" map [like this](https://raw.githubusercontent.com/Bygdrift/PdfToDrawio/master/Documentation/Images/Building_map.png)
- Description of correction to be done [like this](https://raw.githubusercontent.com/Bygdrift/PdfToDrawio/master/Documentation/Images/Revision_cloud.png)
- interior design plan


# How to use it

## Nuget
[![NuGet version (ScriptCs.Hosting)](https://img.shields.io/nuget/v/Bygdrift.Tools.PdfToDrawio)](https://www.nuget.org/packages/Bygdrift.Tools.PdfToDrawio)
[![NuGet version (ScriptCs.Hosting)](https://img.shields.io/nuget/dt/Bygdrift.Tools.PdfToDrawio)](https://www.nuget.org/packages/Bygdrift.Tools.PdfToDrawio)

## Code

How to parse a pdf or svg and get it returned as drawio:
```c#
var drawIo = new Bygdrift.PdfToDrawio.Convert("SomeContent.pdf"); //or svg
//var drawIo = new Bygdrift.PdfToDrawio.Convert(stream, Bygdrift.PdfToDrawio.Format.PDF);  //It can also be loaded as a stream
drawIo.ToDrawIo("someContent.drawio");
var stream = drawIo.ToDrawIo();
```
How to parse a pdf or svg and get it returned as xml:

```c#
var drawIo = new Bygdrift.PdfToDrawio.Convert("SomeContent.pdf");  //or svg
//var drawIo = new Bygdrift.PdfToDrawio.Convert(stream, Bygdrift.PdfToDrawio.Format.PDF);  //It can also be loaded as a stream
drawIo.ToMxGraphModel("someContent.xml");
var stream = drawIo.ToMxGraphModel();
```


The imported pdf can consist of multiple pages.

This project can be tested by downloading it and run the unit test `PdfToDrawioTests.ConvertTests.ConvertPdf()`.

# Inspiration on how to use the project

Personally, I will build a function app to Azure, as an http-trigger, that consist of a simple front end html, that can be integrated on an organisations intranet, so people can search and find a pdf from a FM-system. Then they can choose, either to download the pdf or a drawio-file.

# Whats next

It should be possible to add headers and footers to each page, and I will be working on a better help to users that doesn't know drawio. So if they are making an evacuation map, they can read in a "Howto get started"-shape on the fist page and some help to find a shape library for evacuation maps from their own organization.


[The license](License.md)
