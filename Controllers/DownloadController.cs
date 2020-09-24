namespace azBlobZip.Controllers
{
    using Azure;
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using Azure.Storage.Blobs.Specialized;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Net.Http.Headers;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="DownloadController" />.
    /// </summary>
    [ApiController]
    public class DownloadController : ControllerBase
    {
   

        /// <summary>
        /// Defines the AZURE_CONTAINER.
        /// </summary>
        public static string AZURE_CONTAINER = string.Empty;

        /// <summary>
        /// Defines the AZURE_CONNECTION_STRING.
        /// </summary>
        public static string AZURE_CONNECTION_STRING = string.Empty;

        public DownloadController(IConfiguration configuration)
        {
            
            AZURE_CONNECTION_STRING = Environment.GetEnvironmentVariable("AZURE_CONNECTION_STRING") ?? configuration.GetConnectionString("AzureConnectionString");
            AZURE_CONTAINER = Environment.GetEnvironmentVariable("AZURE_CONTAINER") ?? configuration.GetValue<string>("AzureContainer");

        }

        /// <summary>
        /// The Download.
        /// </summary>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [HttpGet]
        [Route("api/download")]
        public async Task<IActionResult> Download()
        {
            var zipFileName = "blobs.zip";
            Response.ContentType = "application/octet-stream";
            Response.Headers.Add("Content-Disposition", "attachment; filename=" + zipFileName);
            Response.StatusCode = Ok().StatusCode;
            //return Utils.AzureBlob(Response);
            List<string> blobFileNames = new List<string>();
            BlobServiceClient serviceClient = new BlobServiceClient(AZURE_CONNECTION_STRING);
            BlobContainerClient container = serviceClient.GetBlobContainerClient(AZURE_CONTAINER);
            string continuationToken = null;           
            

            try
            {
                // Call the listing operation and enumerate the result segment.
                // When the continuation token is empty, the last segment has been returned
                // and execution can exit the loop.
                do
                {
                    var resultSegment = container.GetBlobs().AsPages(continuationToken);

                    foreach (Azure.Page<BlobItem> blobPage in resultSegment)
                    {
                        foreach (BlobItem blobItem in blobPage.Values)
                        {
                            blobFileNames.Add(blobItem.Name);
                        }

                        // Get the continuation token and loop until it is empty.
                        continuationToken = blobPage.ContinuationToken;


                    }

                } while (continuationToken != "");

            }
            catch (RequestFailedException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
            
            var pipe = Response.BodyWriter;
            var outputStream = pipe.AsStream();
           
            using var zipArchive = new ZipArchive(outputStream, ZipArchiveMode.Create);
            foreach (var blobName in blobFileNames)
            {
                var blob = container.GetBlockBlobClient(blobName);
                var zipEntry = zipArchive.CreateEntry(blobName);
                using var zipStream = zipEntry.Open();
                using var stream = new MemoryStream();
                await blob.DownloadToAsync(stream);
                await zipStream.WriteAsync(stream.ToArray());


            }
            await outputStream.FlushAsync();
            //Response is already started any result would break the controller.
            return new EmptyResult();
        }
    }
}
