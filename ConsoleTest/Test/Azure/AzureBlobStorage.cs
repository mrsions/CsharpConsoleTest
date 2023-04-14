using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.Test
{
    public class AzureBlobStorage
    {
        public async Task Run()
        {
            var container = await GetBlobContainer("bbc");

            BlobClient blob = container.GetBlobClient("Screen_Recording_20211119-152616.mp4");
            bool blobExists = true;// await blob.ExistsAsync();

            using (var stream = File.OpenRead("d:/Screen_Recording_20211119-152616.mp4"))
            {
                await blob.UploadAsync(stream, overwrite: blobExists);
            }

            using (var stream = File.OpenWrite("d:/Screen_Recording_20211119-152616-blob.mp4"))
            using (await blob.DownloadToAsync(stream))
            {
                
            }
            
        }

        static string serviceUri = ""; //"DefaultEndpointsProtocol=https;AccountName=rwayoonsblobstorage;AccountKey=7mvfW3Cxmf/hx65qisQOB7Kn1Xmw/AffJA3G1qH8+VdeuiTQm4X+0MVfquAGABW0p0B+kp+RulG5gd4PG/YSiA==;EndpointSuffix=core.windows.net";
        static string containerUri = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;";

        private async Task<BlobContainerClient> GetBlobContainer(string name)
        {
            BlobContainerClient container;
            if (!string.IsNullOrWhiteSpace(serviceUri))
            { 
                BlobServiceClient service = new BlobServiceClient(serviceUri);

                container = service.GetBlobContainerClient(name);
                await container.CreateIfNotExistsAsync();
                //if(!await container.ExistsAsync())
                //{
                //    container = await service.CreateBlobContainerAsync("abc");
                //}
            }
            else
            {
                //container = new BlobContainerClient(containerUri, name);
                container = new BlobContainerClient(new Uri("https://127.0.0.1:10000/devstoreaccount1/" + name),
                    new StorageSharedKeyCredential("devstoreaccount1", "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw=="));
                await container.CreateIfNotExistsAsync();
            }

            return container;
        }

        private static async Task PrintBlobs(BlobContainerClient container)
        {
            Console.WriteLine("------------------------------");
            await foreach (BlobItem item in container.GetBlobsAsync())
            {
                Console.WriteLine("--- " + item.Name);
            }
        }
    }
}
