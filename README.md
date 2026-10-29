# New-Gen
====================================================================
               CITAS MÉDICAS ASP.NET CORE - MARIADB
            REQUISITOS DEL SISTEMA Y GUÍA DE MANEJO
====================================================================

--------------------------------------------------------------------
1. REQUISITOS DEL SISTEMA
--------------------------------------------------------------------
Para ejecutar y desarrollar este proyecto necesitas:

* Entorno de Desarrollo:
  - SDK de .NET 10.0
  - Visual Studio Code (con extensiones de C# y C# Dev Kit).

* Base de Datos y Servidor:
  - MariaDB Server (o MySQL 8.0+).
  - Servicio activo en el puerto local 3306.

* Paquetes de NuGet recomendados/instalados (.NET 10 / Pomelo):
  - Microsoft.EntityFrameworkCore
  - Microsoft.EntityFrameworkCore.Tools
  - Pomelo.EntityFrameworkCore.MySql


--------------------------------------------------------------------
2. LISTA COMPLETA DE COMANDOS TERMINAL (DOTNET)
--------------------------------------------------------------------

* Crear o verificar el proyecto (si fuera desde cero):
  dotnet new mvc -o CitasMedicas

* Agregar paquetes de la Base de Datos a .NET 10:
  dotnet add package Pomelo.EntityFrameworkCore.MySql
  dotnet add package Microsoft.EntityFrameworkCore.Tools

* Crear la estructura inicial de tablas (Migraciones):
  dotnet ef migrations add InitialCreate

* Aplicar y sincronizar las tablas con MariaDB:
  dotnet ef database update

* Iniciar y ejecutar la aplicación web:
  dotnet run

* Detener la ejecución del servidor:
  Ctrl + C

* Limpiar y re-compilar el proyecto (en caso de errores de caché):
  dotnet clean
  dotnet build


--------------------------------------------------------------------
3. CONFIGURACIÓN Y CONEXIÓN
--------------------------------------------------------------------
* Cadena de conexión en `appsettings.json`:
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CitasMedicasDb;User=root;Password=tu_contraseña;"
  }

* Registro del servicio en `Program.cs`:
  builder.Services.AddDbContext<ApplicationDbContext>(options =>
      options.UseMySql(
          builder.Configuration.GetConnectionString("DefaultConnection"),
          ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
      )
  );


--------------------------------------------------------------------
4. GUÍA DE MANEJO DE LA APLICACIÓN
--------------------------------------------------------------------

1. Ejecutar el proyecto con `dotnet run` y abrir la URL indicada (ej. http://localhost:5000).

2. Módulos integrados con persistencia en MariaDB:
   - Médicos: Registrar, listar, actualizar e id de médicos.
   - Pacientes: Administrar nombre y documento de pacientes.
   - Citas Agregadas: Agendar citas asociando médicos, pacientes, fechas y motivos.
   - Citas Canceladas: Control de historial para registros anulados.

====================================================================
