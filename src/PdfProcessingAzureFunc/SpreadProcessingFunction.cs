using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace PdfProcessingAzureFunc
{
    public static class SpreadProcessingFunction
    {
        [FunctionName("SpreadProcessingFunction")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            // Step 1. Mocking an uploaded XLSX file
            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets.Add();
            CellSelection selection = worksheet.Cells[1, 1];
            selection.SetValue("I was created by RadSpreadProcessing and exported to PDF in an Azure Function!");

            // Step 2. Export to PDF
            using (var memoryStream = new MemoryStream())
            {
                // Export the Spreadsheet document to PDF file (in a MemoryStream instead of a file)
                var pdfFormatProvider = new Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.PdfFormatProvider();
                pdfFormatProvider.Export(workbook, memoryStream);


                // Step 3. do whatever steps you need to to return the PDF
                // In this example, we're creating an HttpResponseMessage with the PDF content

                if (memoryStream.Position != 0) memoryStream.Position = 0;

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    //Set the PDF document content response
                    Content = new ByteArrayContent(memoryStream.ToArray())
                };

                //Set the contentDisposition as attachment
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "Output.pdf"
                };

                //Set the content type as PDF format mime type
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

                //Return the response with output PDF stream
                return response;
            }
        }
    }
}
