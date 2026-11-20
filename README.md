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
  - MariaDB Server (a través de XAMPP u otro servicio local).
  - Aiven Cloud (para alojar la base de datos en la nube).

* Paquetes de NuGet instalados:
  - Microsoft.EntityFrameworkCore
  - Microsoft.EntityFrameworkCore.Tools
  - Pomelo.EntityFrameworkCore.MySql


--------------------------------------------------------------------
2. LISTA COMPLETA DE COMANDOS TERMINAL (DOTNET)
--------------------------------------------------------------------

* Configurar Herramientas de Entity Framework (Especialmente con XAMPP / Entornos Locales):
```
  dotnet new tool-manifest
  dotnet tool install dotnet-ef
  dotnet tool run dotnet-ef database update
```

* Agregar paquetes de la Base de Datos:
```
  dotnet add package Pomelo.EntityFrameworkCore.MySql
  dotnet add package Microsoft.EntityFrameworkCore.Tools
```

* Crear la estructura inicial de tablas (Migraciones):
```
  dotnet ef migrations add InitialCreate
```

* Aplicar y sincronizar las tablas con MariaDB/XAMPP:
 ``` 
  dotnet ef database update
```

* Iniciar y ejecutar la aplicación web:
```
  dotnet run
```
* Detener la ejecución del servidor:
  `Ctrl + C`

* Limpiar y re-compilar el proyecto (en caso de errores de caché):
```
  dotnet clean
  dotnet build
```

--------------------------------------------------------------------
3. CONFIGURACIÓN Y CONEXIÓN
--------------------------------------------------------------------
* Cadena de conexión en `appsettings.json` (Se conecta directo a la nube):
```
 {
  "ConnectionStrings": {
    "MariaDbConnection": "Server=TU_SERVIDOR;Port=TU_PUERTO;Database=citas_medicas_db;User=TU_USUARIO;Password=TU_CONTRASEÑA;"
  }
```
*Importante: Reemplazar los datos de ejemplo por las credenciales correspondientes. No publicar contraseñas reales en repositorios.

* Registro del servicio en `Program.cs`:
```
  builder.Services.AddDbContext<ApplicationDbContext>(options =>
      options.UseMySql(
          builder.Configuration.GetConnectionString("DefaultConnection"),
          ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
      )
  );
```

--------------------------------------------------------------------
4. GUÍA DE MANEJO DE LA APLICACIÓN
--------------------------------------------------------------------

1. Verificar que el servidor MariaDB esté disponible.
2. Si se utiliza Aiven Cloud, comprobar que el servicio esté activo y permita conexiones externas.
3. Ejecutar el proyecto con `dotnet run` y abrir la URL indicada (ej. http://localhost:5000).

4. Módulos integrados con persistencia en MariaDB:
   - Médicos: Registrar, listar, actualizar e ID de médicos.
   - Pacientes: Administrar nombre y documento de pacientes.
   - Citas Agregadas: Agendar citas asociando médicos, pacientes, fechas y motivos.
   - Citas Canceladas: Control de historial para registros anulados.

--------------------------------------------------------------------
5. BASE DE DATOS EN LA NUBE
--------------------------------------------------------------------
*El proyecto utiliza MariaDB alojado en Aiven Cloud para centralizar el almacenamiento de información.

*Esto permite:

    - Acceder a la base de datos desde diferentes equipos.
    - Mantener la información centralizada.
    - Evitar instalar el servidor MariaDB en cada equipo cliente.
    
*La conexión requiere las credenciales correctas y la configuración de seguridad correspondiente, incluyendo SSL/TLS cuando sea necesario.

--------------------------------------------------------------------
6. SEGURIDAD
--------------------------------------------------------------------
-No compartir las contraseñas de la base de datos.

-No publicar credenciales en GitHub.

-Utilizar variables de entorno o User Secrets para almacenar información sensible.

-Realizar copias de seguridad periódicas de la base de datos.

 

====================================================================
