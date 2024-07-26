using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using ProductoImagenes.Data;
using ProductoImagenes.IntegrationTests.Services;
using ProductoImagenes.Services;

namespace ProductoImagenes.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ProductoDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            services.AddDbContext<ProductoDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });

            // Reemplazar el IBlobService real con el TestBlobService
            var blobServiceDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IBlobService));
            if (blobServiceDescriptor != null)
            {
                services.Remove(blobServiceDescriptor);
            }
            services.AddSingleton<IBlobService, TestBlobService>();

            // Configurar IConfiguration para pruebas
            services.AddSingleton<IConfiguration>(new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"BlobService:ContainerName", "test-container"}
                })
                .Build());
        });

        builder.UseEnvironment("Development");
    }
}