using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Export;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Streaming;

namespace PdfProcessingAzureFunc
{
    public static class PdfProcessingFunction
    {
        [FunctionName("PdfProcessingFunction")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log,
            ExecutionContext executionContext)
        {
            log.LogInformation("START PROCESSING");

            // Check to see if there was a preferred page count, passed as a querystring parameter
            string pageCountParam = req.Query["pageCount"];

            // Parse the page count, or use a default count of 10,000 pages.
            var pageCount = int.TryParse(pageCountParam, out int count) ? count : 10000;

            log.LogInformation($"PageCount Defined: {pageCount}, starting document processing...");

            var finalFilePath = executionContext.FunctionAppDirectory + "\\FileResultFile.pdf";

            if (File.Exists(finalFilePath))
            {
                File.Delete(finalFilePath);
            }

            // Create a PdfStreamWriter
            using var fileWriter = new PdfStreamWriter(File.Create(finalFilePath));

            fileWriter.Settings.ImageQuality = ImageQuality.High;
            fileWriter.Settings.DocumentInfo.Author = "Progress Software";
            fileWriter.Settings.DocumentInfo.Title = "Azure Function Test";
            fileWriter.Settings.DocumentInfo.Description = "Generated in a C# Azure Function, this large document was generated with PdfStreamWriter class with minimal memory footprint and optimized result file size.";

            // Load the original file (NOTE: In this test, we're using a single test PDF download from public azure blob)
            byte[] sourcePdfBytes = null;

            using (var client = new HttpClient())
            {
                sourcePdfBytes = await client.GetByteArrayAsync("https://progressdevsupport.blob.core.windows.net/sampledocs/BarChart.pdf");
                log.LogInformation($"Source File Downloaded...");
            }

            if (sourcePdfBytes == null)
            {
                return new ExceptionResult(new Exception("Original file source could not be downloaded"), true);
            }

            // Because HttpClient result stream is not seekable, I switch to using the byte[] and a new MemoryStream for the Telerik PdfFileSource
            await using var sourcePdfStream = new MemoryStream(sourcePdfBytes);

            using var fileSource = new PdfFileSource(sourcePdfStream);

            log.LogInformation($"PdfFileSource loaded, beginning merge loop...");

            // IMPORTANT NOTE:
            // This is iterating over the test "page count" number and merging the same source page (fileSource.Pages[0]) for each loop
            // You would instead iterate over fileSource.Pages and merge them, then move on to the next document and repeat the process
            for (var i = 0; i < pageCount; i++)
            {
                fileWriter.WritePage(fileSource.Pages.FirstOrDefault());
            }

            // Now that we're done merging everything, prepare to return the file as a result of the completed function
            log.LogInformation($"END PROCESSING");

            return new PhysicalFileResult(finalFilePath, "application/pdf") { FileDownloadName = $"Merged_{pageCount}_Pages.pdf" };
        }
    }
}
