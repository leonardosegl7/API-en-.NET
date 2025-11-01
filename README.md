# API de Gestión de Clientes - .NET Core

API REST desarrollada en .NET Core 8.0 con Entity Framework Core para la gestión de clientes. Incluye autenticación mediante API KEY y operaciones CRUD completas.

## Características

- CRUD completo para entidad Cliente
- Autenticación mediante API KEY (Middleware personalizado)
- Validación de datos con Data Annotations
- Manejo de errores con códigos HTTP apropiados
- Base de datos SQLite con Entity Framework Core
- Documentación con Swagger/OpenAPI
- Logging de errores

## Tecnologías Utilizadas

- .NET Core 8.0
- Entity Framework Core
- SQLite
- Swagger/OpenAPI
- Middleware personalizado para seguridad

## Requisitos Previos

- [.NET SDK 8.0 o superior](https://dotnet.microsoft.com/download)
- Un editor de código (Visual Studio, VS Code, Rider, etc.)

## Instalación

1. Clonar el repositorio:
```bash
git clone <url-del-repositorio>
cd ClientesAPI
```

2. Restaurar las dependencias:
```bash
dotnet restore
```

3. Aplicar las migraciones y crear la base de datos:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

4. Ejecutar la aplicación:
```bash
dotnet run
```

La API estará disponible en: `http://localhost:5187` (el puerto puede variar)

## Estructura del Proyecto

```
ClientesAPI/
├── Controllers/
│   └── ClientesController.cs       # Controlador con endpoints CRUD
├── Data/
│   └── ApplicationDbContext.cs     # Contexto de Entity Framework
├── DTOs/
│   └── ClienteDto.cs                # Objetos de transferencia de datos
├── Middleware/
│   └── ApiKeyMiddleware.cs          # Middleware de autenticación
├── Models/
│   └── Cliente.cs                   # Entidad Cliente
├── Migrations/                      # Migraciones de EF Core
├── appsettings.json                 # Configuración
├── Program.cs                       # Punto de entrada
└── clientes.db                      # Base de datos SQLite
```

## Modelo de Datos

### Cliente
| Campo              | Tipo   | Descripción                    |
|--------------------|--------|--------------------------------|
| Id                 | int    | Identificador único (PK)       |
| Nombre             | string | Nombre del cliente             |
| CorreoElectronico  | string | Correo electrónico (único)     |
| Telefono           | string | Número de teléfono             |

## Endpoints de la API

### Autenticación
Todos los endpoints requieren el header `X-API-KEY` con el valor: `12345`

### Operaciones CRUD

#### 1. Listar todos los clientes
```http
GET /api/clientes
```

**Headers:**
```
X-API-KEY: 12345
```

**Respuesta exitosa (200 OK):**
```json
[
  {
    "id": 1,
    "nombre": "Juan Perez",
    "correoElectronico": "juan@example.com",
    "telefono": "4431234567"
  }
]
```

#### 2. Obtener un cliente por ID
```http
GET /api/clientes/{id}
```

**Headers:**
```
X-API-KEY: 12345
```

**Respuesta exitosa (200 OK):**
```json
{
  "id": 1,
  "nombre": "Juan Perez",
  "correoElectronico": "juan@example.com",
  "telefono": "4431234567"
}
```

**Respuesta error (404 Not Found):**
```json
{
  "error": "Cliente no encontrado"
}
```

#### 3. Crear un nuevo cliente
```http
POST /api/clientes
```

**Headers:**
```
Content-Type: application/json
X-API-KEY: 12345
```

**Body:**
```json
{
  "nombre": "Juan Perez",
  "correoElectronico": "juan@example.com",
  "telefono": "4431234567"
}
```

**Respuesta exitosa (201 Created):**
```json
{
  "id": 1,
  "nombre": "Juan Perez",
  "correoElectronico": "juan@example.com",
  "telefono": "4431234567"
}
```

**Respuesta error (409 Conflict):**
```json
{
  "error": "Ya existe un cliente con ese correo electronico"
}
```

#### 4. Actualizar un cliente
```http
PUT /api/clientes/{id}
```

**Headers:**
```
Content-Type: application/json
X-API-KEY: 12345
```

**Body:**
```json
{
  "nombre": "Juan Perez Actualizado",
  "correoElectronico": "juan.nuevo@example.com",
  "telefono": "4439876543"
}
```

**Respuesta exitosa (200 OK):**
```json
{
  "id": 1,
  "nombre": "Juan Perez Actualizado",
  "correoElectronico": "juan.nuevo@example.com",
  "telefono": "4439876543"
}
```

#### 5. Actualización parcial
```http
PATCH /api/clientes/{id}
```

Misma funcionalidad que PUT (se puede extender para actualizaciones parciales).

#### 6. Eliminar un cliente
```http
DELETE /api/clientes/{id}
```

**Headers:**
```
X-API-KEY: 12345
```

**Respuesta exitosa (204 No Content)**

## Códigos de Respuesta HTTP

| Código | Descripción                                      |
|--------|--------------------------------------------------|
| 200    | Operación exitosa                                |
| 201    | Recurso creado exitosamente                      |
| 204    | Operación exitosa sin contenido                  |
| 400    | Solicitud incorrecta (datos inválidos)           |
| 401    | No autorizado (API KEY inválida o ausente)       |
| 404    | Recurso no encontrado                            |
| 409    | Conflicto (recurso duplicado)                    |
| 500    | Error interno del servidor                       |

## Ejemplos de Uso

### Usando cURL

**Crear un cliente:**
```bash
curl -X POST http://localhost:5187/api/clientes \
  -H "Content-Type: application/json" \
  -H "X-API-KEY: 12345" \
  -d "{\"nombre\":\"Juan Perez\",\"correoElectronico\":\"juan@example.com\",\"telefono\":\"4431234567\"}"
```

**Listar clientes:**
```bash
curl -X GET http://localhost:5187/api/clientes \
  -H "X-API-KEY: 12345"
```

### Usando PowerShell

**Crear un cliente:**
```powershell
$headers = @{
    "Content-Type" = "application/json"
    "X-API-KEY" = "12345"
}
$body = @{
    nombre = "Juan Perez"
    correoElectronico = "juan@example.com"
    telefono = "4431234567"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5187/api/clientes" -Method Post -Headers $headers -Body $body
```

**Listar clientes:**
```powershell
$headers = @{
    "X-API-KEY" = "12345"
}

Invoke-RestMethod -Uri "http://localhost:5187/api/clientes" -Method Get -Headers $headers
```

## Validaciones Implementadas

- **Nombre:** Obligatorio, máximo 100 caracteres
- **Correo Electrónico:** Obligatorio, formato válido, único en la base de datos
- **Teléfono:** Obligatorio, formato válido

## Seguridad

### Middleware de API KEY

La API implementa un middleware personalizado que valida la presencia y validez de una API KEY en cada petición:

- **Header requerido:** `X-API-KEY`
- **Valor configurado:** `12345`
- **Configuración:** `appsettings.json` bajo la clave `ApiKey`

**Respuestas de error:**

Sin API KEY:
```json
{
  "error": "API Key no proporcionada"
}
```

API KEY inválida:
```json
{
  "error": "API Key invalida"
}
```

## Configuración

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=clientes.db"
  },
  "ApiKey": "12345"
}
```

**Para cambiar la API KEY:**
1. Modifica el valor de `ApiKey` en `appsettings.json`
2. Reinicia la aplicación
3. Usa el nuevo valor en el header `X-API-KEY`

## Documentación Swagger

Una vez ejecutada la aplicación, accede a:
```
http://localhost:5187/swagger
```

Swagger proporciona una interfaz interactiva para explorar y probar todos los endpoints de la API.

## Pruebas

### Probar la autenticación

**Sin API KEY (debe devolver 401):**
```bash
curl -X GET http://localhost:5187/api/clientes
```

**Con API KEY incorrecta (debe devolver 401):**
```bash
curl -X GET http://localhost:5187/api/clientes \
  -H "X-API-KEY: clave-incorrecta"
```

**Con API KEY correcta (debe devolver 200):**
```bash
curl -X GET http://localhost:5187/api/clientes \
  -H "X-API-KEY: 12345"
```

## Logs

Los logs se generan automáticamente en la consola durante la ejecución. Para ver logs detallados, modifica el nivel en `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information"
    }
  }
}
```

## Manejo de Errores

La API implementa un manejo robusto de errores:

- **Validaciones:** Se retornan mensajes descriptivos con código 400
- **Recursos no encontrados:** Código 404 con mensaje explicativo
- **Conflictos:** Código 409 para recursos duplicados (correos)
- **Errores internos:** Código 500 con logging automático

## Despliegue

### Variables de entorno para producción

Antes de desplegar en producción:

1. Cambia la API KEY a un valor seguro
2. Usa una base de datos más robusta (SQL Server, PostgreSQL)
3. Habilita HTTPS
4. Configura CORS si es necesario

### Comando para publicar

```bash
dotnet publish -c Release -o ./publish
```

## Contribución

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/nueva-funcionalidad`)
3. Commit tus cambios (`git commit -am 'Agrega nueva funcionalidad'`)
4. Push a la rama (`git push origin feature/nueva-funcionalidad`)
5. Abre un Pull Request

## Licencia

Este proyecto es de código abierto y está disponible bajo la Licencia MIT.

## Autor

Desarrollado como parte de un ejercicio técnico para demostrar conocimientos en .NET Core, Entity Framework y desarrollo de APIs REST.

## Contacto

Para preguntas o sugerencias, por favor abre un issue en el repositorio.