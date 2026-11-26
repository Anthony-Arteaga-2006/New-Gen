using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CitasMedicas.Data;
using CitasMedicas.Models;

namespace CitasMedicas.Controllers
{
    public class CitasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CitasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Vista 1: CITAS ACTIVAS (Listar, Agendar, Actualizar y Cancelar)
        public async Task<IActionResult> Activas()
        {
            var activas = await _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .Where(c => c.Estado == EstadoCita.Activa)
                .OrderBy(c => c.FechaHora)
                .ToListAsync();

            var pacientes = await _context.Pacientes
                .Select(p => new { p.Id, Texto = p.Nombres + " " + p.Apellidos + " (CI: " + p.CI + ")" })
                .ToListAsync();

            var medicos = await _context.Medicos
                .Select(m => new { m.Id, Texto = m.Nombres + " " + m.Apellidos + " - " + m.Especialidad })
                .ToListAsync();

            ViewBag.Pacientes = new SelectList(pacientes, "Id", "Texto");
            ViewBag.Medicos = new SelectList(medicos, "Id", "Texto");

            return View("~/Views/Home/CitasActivas.cshtml", activas);
        }

        // Vista 2: CITAS CANCELADAS + Historial de Auditoría
        public async Task<IActionResult> Canceladas()
        {
            var canceladas = await _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .Where(c => c.Estado == EstadoCita.Cancelada)
                .OrderByDescending(c => c.FechaCancelacion)
                .ToListAsync();

            ViewBag.Historial = await _context.Historial
                .Include(h => h.Cita)
                    .ThenInclude(c => c!.Paciente)
                .OrderByDescending(h => h.FechaRegistro)
                .ToListAsync();

            return View("~/Views/Home/CitasCanceladas.cshtml", canceladas);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int PacienteId, int MedicoId, DateTime FechaHora, string Motivo)
        {
            if (PacienteId > 0 && MedicoId > 0 && !string.IsNullOrWhiteSpace(Motivo))
            {
                var nuevaCita = new Cita
                {
                    PacienteId = PacienteId,
                    MedicoId = MedicoId,
                    FechaHora = FechaHora,
                    Motivo = Motivo.Trim(),
                    Estado = EstadoCita.Activa
                };

                _context.Citas.Add(nuevaCita);
                await _context.SaveChangesAsync(); // Dispara tr_auditoria_nueva_cita
            }
            return RedirectToAction(nameof(Activas));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, DateTime FechaHora, string? Motivo)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita != null && cita.Estado == EstadoCita.Activa)
            {
                cita.FechaHora = FechaHora;
                if (!string.IsNullOrWhiteSpace(Motivo))
                {
                    cita.Motivo = Motivo.Trim();
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Activas));
        }

        [HttpPost]
        public async Task<IActionResult> Cancelar(int id, string motivoCancelacion)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita != null && cita.Estado == EstadoCita.Activa)
            {
                string motivo = string.IsNullOrWhiteSpace(motivoCancelacion)
                    ? "Cancelada por el usuario"
                    : motivoCancelacion.Trim();

                // Ejecuta el Procedimiento Almacenado en MySQL
                await _context.Database.ExecuteSqlRawAsync(
                    "CALL sp_CancelarCita({0}, {1})", id, motivo);
            }
            return RedirectToAction(nameof(Activas));
        }
    }
}