using ProductoImagenes.Services;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace ProductoImagenes.IntegrationTests.Services;

public class TestBlobService : IBlobService
{
    public Task<string> UploadFileAsync(string fileName, Stream fileStream, string contentType)
    {
        return Task.FromResult($"https://fakestorage.blob.core.windows.net/test-container/{fileName}");
    }

    public Task<bool> DeleteFileAsync(string fileName)
    {
        return Task.FromResult(true);
    }

    public Task<Stream> GetFileAsync(string fileName)
    {
        return Task.FromResult<Stream>(new MemoryStream());
    }

    public Task<bool> FileExistsAsync(string fileName)
    {
        return Task.FromResult(true);
    }

    public BlobContainerClient GetBlobContainerClient()
    {
        return null; // Para pruebas, podemos devolver null
    }
}