using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ProductoImagenes.IntegrationTests.Tests
{
    public class ProductosControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ProductosControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_ReturnsSuccessStatusCode()
        {
            // Act
            var response = await _client.GetAsync("/api/productos");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Fact]
        public async Task Post_CreatesProducto()
        {
            // Arrange
            var fileName = "test.txt";
            var content = new MultipartFormDataContent
    {
        { new ByteArrayContent(Encoding.UTF8.GetBytes("This is a test file")), "file", fileName }
    };

            // Act
            var response = await _client.PostAsync("/api/productos/upload", content);

            // Assert
            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response status: {response.StatusCode}");
            Console.WriteLine($"Response content: {responseString}");

            Assert.True(response.IsSuccessStatusCode, $"Response was not successful. Status code: {response.StatusCode}");

            var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseString);

            Assert.True(result.ContainsKey("message"), "Message property not found in response");
            Assert.True(result.ContainsKey("productId"), "ProductId property not found in response");
            Assert.True(result.ContainsKey("blobUrl"), "BlobUrl property not found in response");

            Assert.Equal("File uploaded successfully", result["message"].GetString());
            Assert.True(result["productId"].TryGetInt32(out int productId));
            Assert.True(Uri.TryCreate(result["blobUrl"].GetString(), UriKind.Absolute, out _));
        }
    }
}