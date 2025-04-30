# WebApplication1

Este es un proyecto basado en .NET 8 que utiliza Entity Framework Core para la gestión de datos y un API REST para manejar movimientos de inventario.

## Características

- API REST para consultar y gestionar movimientos de inventario.
- Soporte para filtros avanzados como rango de fechas, tipo de movimiento y número de documento.
- Integración con procedimientos almacenados en SQL Server.
- Configuración de CORS para permitir el consumo desde aplicaciones frontend.

## Requisitos previos

- .NET 8 SDK
- SQL Server
- Herramienta para probar APIs (como Postman o Swagger)

> [!Nota]  
> Swagger ya viene integrado.

## Instalación

1. Clona este repositorio
2. Restaura las dependencias:
3. Configura la cadena de conexión en el archivo `appsettings.json`. 
   ```json
	"ConnectionStrings": { "DefaultConnection": "Server=<TU_SERVIDOR>;Database=<TU_BASE_DE_DATOS>;User Id=<USUARIO>;Password=<CONTRASEÑA>;" }
   ```
4. Aplica las migraciones a la base de datos:

## Uso

1. Ejecuta la aplicación:
2. Accede a la API en tu navegador o herramienta de prueba en `http://localhost:5288/swagger`.

## Endpoints principales

- **POST /api/MovInventarios/FiltrarPorFechas**  
  Filtra movimientos de inventario por rango de fechas, tipo de movimiento y número de documento.

  **Ejemplo de cuerpo de solicitud:**
  ```json
	{ "fechaInicio": "1999-01-01T00:00:00", "fechaFin": "2025-12-31T23:59:59", "tipoMovimiento": "01", "nroDocumento": "1" }
  ```

- **GET /api/MovInventarios**  
  Obtiene todos los movimientos de inventario.

## Estructura del proyecto

- **Data/AppDbContext.cs**: Configuración del contexto de base de datos.
- **Models/**: Contiene las clases de modelo como `MovInventario` y `MovInventarioUbicacion`.
- **Controllers/**: Controladores para manejar las solicitudes API.
