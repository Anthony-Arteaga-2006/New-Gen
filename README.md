# New-Gen
====================================================================
           DOCUMENTO DE REQUISITOS Y REQUERIMIENTOS
            SISTEMA DE GESTION DE CITAS MEDICAS
====================================================================

PROYECTO: CitasMedicas
TECNOLOGIA: .NET 8 (ASP.NET Core MVC), Bootstrap 5, Razor Pages
ENTORNO: Visual Studio Code

--------------------------------------------------------------------
1. REQUERIMIENTOS FUNCIONALES (RF)
--------------------------------------------------------------------

[RF-01] NAVEGACION Y MENU PRINCIPAL
- Barra de navegacion superior persistente (_Layout.cshtml).
- Acceso directo a 6 secciones:
  1. Inicio
  2. Medicos
  3. Pacientes
  4. Citas Agregadas
  5. Citas Canceladas
  6. Privacidad

[RF-02] SECCION MEDICOS
- Ver / Listar: Mostrar lista de medicos registrados (ID, Nombre, Especialidad).
- Agregar: Formulario para registrar un nuevo medico.
- Actualizar: Formulario para modificar datos de un medico por ID.
- Borrar: Formulario para eliminar un medico por ID.

[RF-03] SECCION PACIENTES
- Ver / Listar: Mostrar lista de pacientes (ID, Nombre, Documento).
- Agregar: Formulario para registrar un nuevo paciente.
- Actualizar: Formulario para modificar datos de un paciente por ID.
- Borrar: Formulario para eliminar un paciente por ID.

[RF-04] SECCION CITAS AGREGADAS
- Ver / Listar: Mostrar citas agendadas (ID, Paciente, Medico, Fecha, Motivo).
- Agregar: Formulario para agendar una nueva cita.
- Actualizar: Formulario para cambiar la fecha o motivo por ID.
- Borrar: Formulario para cancelar/eliminar una cita activa por ID.

[RF-05] SECCION CITAS CANCELADAS
- Ver / Listar: Mostrar historial de citas canceladas.
- Agregar: Formulario para registrar una cita en la lista de canceladas.
- Actualizar: Formulario para modificar el motivo de cancelacion por ID.
- Borrar: Formulario para eliminar un registro del historial por ID.

[RF-06] INICIO Y PRIVACIDAD
- Inicio: Pantalla de bienvenida con accesos rapidos a las 4 secciones.
- Privacidad: Nota de confidencialidad y proteccion de datos medicos.


--------------------------------------------------------------------
2. REQUISITOS NO FUNCIONALES (RNF)
--------------------------------------------------------------------

[RNF-01] ARQUITECTURA
- Estructura basada en el patron MVC (Model-View-Controller).
- Controlador principal: HomeController.cs.
- Vistas en tecnologia Razor (.cshtml).

[RNF-02] INTERFAZ DE USUARIO (UI)
- Diseno responsivo con Bootstrap 5.
- Organizacion modular mediante pestañas interactivas (nav-tabs) para las acciones Ver, Agregar, Actualizar y Borrar en cada modulo.

[RNF-03] RENDIMIENTO Y ENRUTAMIENTO
- Mapeo directo de rutas en el controlador ([Route]) para evitar errores 404.
- Ejecucion y despliegue rapido mediante comandos de consola (dotnet run).
====================================================================
