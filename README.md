# New-Gen — Sistema de Gestión de Citas Médicas

Aplicación web desarrollada en **ASP.NET Core MVC (.NET 10.0)** conectada a **MySQL en la nube (Aiven)** o **MariaDB local** mediante **Entity Framework Core**. Permite administrar médicos, pacientes y citas médicas desde una interfaz unificada, registrando automáticamente cada movimiento en un historial de auditoría.

---

## 1. ¿Qué hace el programa?

El sistema centraliza la operación diaria de un consultorio o clínica en cuatro módulos integrados:

* **Gestión de Médicos:** Permite registrar, consultar, editar y eliminar profesionales de la salud junto con su especialidad.
* **Gestión de Pacientes:** Administra el registro de pacientes con su nombre completo y documento de identidad.
* **Gestión Unificada de Citas:** Presenta en una sola pantalla todas las citas del sistema con filtros rápidos (**Todas**, **Activas** y **Canceladas**), permitiendo agendar nuevas consultas, modificar horarios o anular citas indicando un motivo.
* **Historial de Movimientos (Auditoría):** Bitácora automática ubicada debajo de la tabla de citas que guarda cada acción realizada (`Creada`, `Editada`, `Cancelada`), el detalle del cambio y la fecha/hora exacta.

---

## 2. ¿Cómo funciona internamente?

### Arquitectura de Base de Datos (Herencia TPT)
El proyecto utiliza el patrón **Table-per-Type (TPT)** de Entity Framework Core sobre la base de datos `citas_medicas_db`:

1. **Tabla Base (`citas`):** Almacena los datos comunes de cualquier consulta (`PacienteId`, `MedicoId`, `FechaHora`, `Motivo` y `Estado`).
2. **Citas Activas (`citasagregadas`):** Hereda el `Id` de `citas`. Cuando se agenda una cita, se inserta aquí y se muestra con estado **Activa**.
3. **Citas Anuladas (`citascanceladas`):** Hereda el `Id` de `citas` y guarda `FechaHoraOriginal`, `FechaCancelacion` y `MotivoCancelacion`. Al cancelar una cita desde la web, el controlador la mueve automáticamente de `citasagregadas` a `citascanceladas` sin perder su registro base.
4. **Auditoría (`Historial`):** Cada vez que el usuario crea, edita o cancela una cita en la web, el controlador inserta automáticamente un registro vinculado a esa cita.

---

## 3. Requisitos del Sistema

* **Entorno de Desarrollo:**
  * SDK de **.NET 10.0**
  * Visual Studio Code (con extensiones *C#* y *C# Dev Kit*)
* **Base de Datos y Servidor:**
  * Instancia en la nube de **Aiven MySQL** (configuración principal del grupo) o **MariaDB / MySQL Server** local (a través de XAMPP).
* **Paquetes NuGet instalados:**
  * `Microsoft.EntityFrameworkCore`
  * `Microsoft.EntityFrameworkCore.Tools`
  * `Pomelo.EntityFrameworkCore.MySql`

---

## 4. Configuración y Conexión

* **Cadena de conexión en `appsettings.json`:**
  * *Opción A (Nube - Aiven MySQL):* Ya viene configurada en el proyecto para conectarse a la base compartida.
  * *Opción B (Local - XAMPP / MariaDB):* Si deseas crear una base de datos local desde cero, ajusta `appsettings.json` así:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost;Database=citas_medicas_db;User=root;Password=;"
    }
    ```

* **Registro del servicio en `Program.cs`:**
  ```csharp
  builder.Services.AddDbContext<ApplicationDbContext>(options =>
      options.UseMySql(
          builder.Configuration.GetConnectionString("DefaultConnection"),
          ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
      )
  );
  ```

---

## 5. Lista Completa de Comandos Terminal (.NET CLI)

Si deseas crear la base de datos desde cero en otro servidor o entorno local, puedes usar el script incluido en `Database/script_base_datos.sql` o ejecutar los siguientes comandos en la terminal de VS Code:

* **Instalar paquetes de la base de datos (si inicias un proyecto limpio):**
  ```bash
  dotnet add package Pomelo.EntityFrameworkCore.MySql
  dotnet add package Microsoft.EntityFrameworkCore.Tools
  ```

* **Configurar herramientas de Entity Framework Core:**
  ```bash
  dotnet new tool-manifest
  dotnet tool install dotnet-ef
  ```

* **Crear y aplicar migraciones para generar las tablas en MySQL / MariaDB:**
  ```bash
  dotnet ef migrations add InitialCreate
  dotnet ef database update
  ```
  *(Si usas manifiesto local de herramientas, ejecuta `dotnet tool run dotnet-ef database update`).*

* **Limpiar, restaurar y compilar el proyecto:**
  ```bash
  dotnet clean
  dotnet restore
  dotnet build
  ```

* **Iniciar y detener la aplicación web:**
  ```bash
  dotnet run
  ```
  *(Presiona `Ctrl + C` en la terminal para detener el servidor).*

---

## 6. Guía de Ejecución y Manejo

1. **Preparar entorno:** Si trabajas en local con XAMPP, inicia el servicio **MySQL** en el panel de control. Si trabajas conectado a **Aiven Cloud**, solo asegúrate de tener conexión a Internet.
2. **Ejecutar el sistema:** Abre una terminal en la carpeta raíz del proyecto, ejecuta `dotnet run` y abre en tu navegador la URL indicada (por ejemplo, `http://localhost:5000`).
3. **Flujo de trabajo en la web:**
   * **Médicos:** Registrar, listar, actualizar y eliminar médicos con su especialidad.
   * **Pacientes:** Administrar el registro de pacientes (nombre y documento).
   * **Citas:** Agendar nuevas consultas asociando paciente, médico, fecha/hora y motivo; filtrar por estado (*Todas*, *Activas*, *Canceladas*); editar citas o cancelarlas registrando el motivo.
   * **Historial de Movimientos:** Verificar en la tabla inferior la bitácora automática de cada operación realizada.
  
---

## 7. Advertencias y Buenas Prácticas de Seguridad (Qué NO hacer)

Dado que el equipo comparte una misma base de datos en la nube (**Aiven MySQL**), ten en cuenta las siguientes restricciones para evitar pérdida de datos o brechas de seguridad:

* **NO cambiar el repositorio a Público:** El archivo `appsettings.json` contiene las credenciales directas de conexión a Aiven. Mantén siempre el repositorio en modo **Private**; si en algún momento necesitas hacerlo público, elimina primero las contraseñas del historial o agrégalo al `.gitignore`.
* **NO modificar ni eliminar registros manualmente desde SQL sin respetar la herencia TPT:** Evita borrar filas directamente en `citasagregadas` o `citascanceladas` sin actualizar la tabla padre `citas` (y viceversa), ya que romperías la integridad de Entity Framework Core al cargar las vistas.
* **NO ejecutar `dotnet ef database update` con migraciones destructivas sobre producción:** Antes de aplicar cambios de esquema en la base compartida de Aiven, pruébalos en una base local (XAMPP/MariaDB) y avisa al equipo para no sobrescribir tablas existentes.
* **NO subir cambios sin hacer `git pull` previo:** Descarga siempre los últimos cambios de tus compañeros (`git pull origin main`) antes de hacer `git push` para evitar conflictos en los controladores o en el archivo `ApplicationDbContext.cs`.
* **NO eliminar registros de `Historial`:** La tabla `Historial` funciona como bitácora de auditoría del sistema; nunca debe limpiarse ni alterarse manualmente durante la operación normal.