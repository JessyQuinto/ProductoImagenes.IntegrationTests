# ProductoImagenes.IntegrationTests
# ProductoImagenes

ProductoImagenes es una API construida con .NET 8 que permite la gestión de productos y sus imágenes. Incluye operaciones CRUD para productos, carga de imágenes a Azure Blob Storage e integración con SQL Server utilizando Entity Framework Core. Este proyecto también cuenta con pruebas de integración para asegurar la funcionalidad del sistema.

## Características

- CRUD para productos
- Carga de imágenes a Azure Blob Storage
- Integración con SQL Server
- Pruebas de integración

## Requisitos

- .NET 8 SDK
- SQL Server
- Azure Blob Storage

## Configuración del Entorno de Desarrollo

### Clonar el Repositorio


git clone https://github.com/tuusuario/ProductoImagenes.git
cd ProductoImagenes
Configurar la Base de Datos
Asegúrate de tener SQL Server instalado y ejecutándose. Actualiza la cadena de conexión en 'appsettings.json':

##
Aquí tienes el README completo en formato Markdown dividido en dos partes debido a su extensión:

Parte 1
markdown
Copy code
# ProductoImagenes

ProductoImagenes es una API construida con .NET 8 que permite la gestión de productos y sus imágenes. Incluye operaciones CRUD para productos, carga de imágenes a Azure Blob Storage e integración con SQL Server utilizando Entity Framework Core. Este proyecto también cuenta con pruebas de integración para asegurar la funcionalidad del sistema.

## Características

- CRUD para productos
- Carga de imágenes a Azure Blob Storage
- Integración con SQL Server
- Pruebas de integración

## Requisitos

- .NET 8 SDK
- SQL Server
- Azure Blob Storage

## Configuración del Entorno de Desarrollo

### Clonar el Repositorio


git clone https://github.com/tuusuario/ProductoImagenes.git
cd ProductoImagenes
Configurar la Base de Datos
Asegúrate de tener SQL Server instalado y ejecutándose. Actualiza la cadena de conexión en appsettings.json:

json
Copy code
"ConnectionStrings": {
  "ProductosDB": "Server=(localdb)\\mssqllocaldb;Database=ProductosDB;Trusted_Connection=True;"
}
Configurar Azure Blob Storage
Configura la cadena de conexión de Azure Blob Storage en appsettings.json:

json
Copy code
"ConnectionStrings": {
  "StorageAccount": "DefaultEndpointsProtocol=https;AccountName=tuCuenta;AccountKey=tuClave;EndpointSuffix=core.windows.net"
},
"BlobService": {
  "ContainerName": "tu-contenedor"
}
Ejecutar Migraciones de la Base de Datos
bash
Copy code
dotnet ef database update
Ejecutar la Aplicación
bash
Copy code
dotnet run
La API estará disponible en https://localhost:5001.

shell
Copy code

### Parte 2


## Pruebas

### Ejecutar Pruebas de Integración


cd ProductoImagenes.IntegrationTests
dotnet test
Uso de la API
Endpoints
GET /api/productos - Listar todos los productos
GET /api/productos/{id} - Descargar un archivo de producto por ID
POST /api/productos/upload - Subir un nuevo archivo de producto
PUT /api/productos/{id} - Actualizar un archivo de producto existente
DELETE /api/productos/{id} - Eliminar un producto y su archivo
Ejemplo de Solicitud POST para Subir un Archivo
bash
Copy code
curl -X POST "https://localhost:5001/api/productos/upload" -F "file=@path/to/your/file.txt"
Contribuciones
Las contribuciones son bienvenidas. Por favor, abre un issue o envía un pull request para mejoras o correcciones.

Licencia
Este proyecto está licenciado bajo la MIT License.

Asegúrate de reemplazar tuusuario, tuCuenta, tuClave y tu-contenedor con los valores correspondientes de tu configuración. Además, puedes añadir más detalles según sea necesario para tu proyecto.
