using System.Net;
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
            Console.WriteLine($"Ejecutando prueba: {nameof(Get_ReturnsSuccessStatusCode)}");

            // Act
            var response = await _client.GetAsync("/api/productos");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }

        [Fact]
        public async Task Post_CreatesProducto()
        {
            Console.WriteLine($"Ejecutando prueba: {nameof(Post_CreatesProducto)}");

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

        [Fact]
        public async Task Get_ReturnsListOfProductos()
        {
            Console.WriteLine($"Ejecutando prueba: {nameof(Get_ReturnsListOfProductos)}");

            // Act
            var response = await _client.GetAsync("/api/productos");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var productos = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(responseString);

            Assert.NotNull(productos);
            Assert.IsType<List<Dictionary<string, JsonElement>>>(productos);
        }


        [Fact]
        public async Task Put_UpdatesExistingProducto()
        {
            Console.WriteLine($"Ejecutando prueba: {nameof(Put_UpdatesExistingProducto)}");

            // Arrange
            var uploadResponse = await UploadTestFile();
            var uploadResult = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(await uploadResponse.Content.ReadAsStringAsync());
            var productId = uploadResult["productId"].GetInt32();

            var updatedFileName = "updated_test.txt";
            var updatedContent = new MultipartFormDataContent
            {
                { new ByteArrayContent(Encoding.UTF8.GetBytes("This is an updated test file")), "file", updatedFileName }
            };

            // Act
            var response = await _client.PutAsync($"/api/productos/{productId}", updatedContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseString);

            Assert.True(result.ContainsKey("message"));
            Assert.True(result.ContainsKey("productoId"));
            Assert.True(result.ContainsKey("nuevaBlobUrl"));
            Assert.Equal("Archivo actualizado con éxito", result["message"].GetString());
        }

        [Fact]
        public async Task Delete_RemovesProducto()
        {
            Console.WriteLine($"Ejecutando prueba: {nameof(Delete_RemovesProducto)}");
            // Arrange
            var uploadResponse = await UploadTestFile();
            var uploadResult = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(await uploadResponse.Content.ReadAsStringAsync());
            var productId = uploadResult["productId"].GetInt32();

            // Act
            var response = await _client.DeleteAsync($"/api/productos/{productId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(responseString);

            Assert.True(result.ContainsKey("message"));
            Assert.Equal("Producto y archivo eliminados con éxito", result["message"].GetString());

            // Verificar que el producto ya no existe
            var getResponse = await _client.GetAsync($"/api/productos/{productId}");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        private async Task<HttpResponseMessage> UploadTestFile()
        {
            var fileName = "test.txt";
            var content = new MultipartFormDataContent
            {
                { new ByteArrayContent(Encoding.UTF8.GetBytes("This is a test file")), "file", fileName }
            };

            return await _client.PostAsync("/api/productos/upload", content);
        }
    }
}