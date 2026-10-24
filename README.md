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
  - SDK de .NET 8.0 o superior.
  - Visual Studio Code (con extensiones de C# y C# Dev Kit).

* Base de Datos y Servidor:
  - MariaDB Server (o MySQL 8.0+).
  - Servicio activo en puerto local (por defecto 3306).

* Paquetes de NuGet instalados en el proyecto:
  - Microsoft.EntityFrameworkCore (8.x)
  - Microsoft.EntityFrameworkCore.Tools (8.x)
  - Pomelo.EntityFrameworkCore.MySql (8.x)


--------------------------------------------------------------------
2. CONFIGURACIÓN Y CONEXIÓN
--------------------------------------------------------------------
* Cadena de conexión (appsettings.json):
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CitasMedicasDb;User=root;Password=tu_contraseña;"
  }

* Configuración de servicios (Program.cs):
  builder.Services.AddDbContext<ApplicationDbContext>(options =>
      options.UseMySql(
          builder.Configuration.GetConnectionString("DefaultConnection"),
          ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
      )
  );


--------------------------------------------------------------------
3. GUÍA DE MANEJO Y EJECUCIÓN DEL PROYECTO
--------------------------------------------------------------------

A. Iniciar la Aplicación:
   1. Abre la terminal en VS Code dentro de la carpeta del proyecto.
   2. Ejecuta el comando:
      dotnet run
   3. Abre la URL local generada (ej. http://localhost:5000) en el navegador.

B. Módulos del Sistema (CRUD completo en MariaDB):
   1. Médicos (Views/Home/Medicos.cshtml):
      - Ver/Listar: Muestra la lista de médicos registrados.
      - Agregar: Crea un registro con Nombre y Especialidad.
      - Actualizar: Modifica datos ingresando el ID del médico.
      - Eliminar: Borra el registro utilizando el ID.

   2. Pacientes (Views/Home/Pacientes.cshtml):
      - Ver/Listar: Lista de pacientes registrados.
      - Agregar: Registra Nombre y Documento.
      - Actualizar: Edita datos por ID.
      - Eliminar: Elimina paciente por ID.

   3. Citas Agregadas (Views/Home/CitasAgregadas.cshtml):
      - Ver/Listar: Visualiza las citas activas.
      - Agendar: Asigna Paciente, Médico, Fecha/Hora y Motivo.
      - Actualizar: Modifica la fecha o motivo mediante el ID de la cita.
      - Cancelar/Borrar: Remueve la cita activa.

   4. Citas Canceladas (Views/Home/CitasCanceladas.cshtml):
      - Historial independiente para almacenar las citas anuladas.


--------------------------------------------------------------------
4. MANTENIMIENTO Y COMANDOS ÚTILES
--------------------------------------------------------------------

* Detener la aplicación:
  En la terminal de VS Code presionar: Ctrl + C

* Actualizar cambios en la Base de Datos (Si cambias algún Modelo):
  1. Crear nueva migración:
     dotnet ef migrations add NombreDeLaMigracion
  2. Aplicar cambios a MariaDB:
     dotnet ef database update

* Limpiar y restrusturar la compilación (en caso de errores de caché):
  dotnet clean
  dotnet build

====================================================================

[RNF-03] RENDIMIENTO Y ENRUTAMIENTO
- Mapeo directo de rutas en el controlador ([Route]) para evitar errores 404.
- Ejecucion y despliegue rapido mediante comandos de consola (dotnet run).
====================================================================
