using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CitasMedicas.Data;
using CitasMedicas.Models;

namespace CitasMedicas.Controllers
{
    public class CitasAgregadasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CitasAgregadasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Pacientes = new SelectList(_context.Pacientes, "Id", "Nombre");
            ViewBag.Medicos = new SelectList(_context.Medicos, "Id", "Nombre");

            // 1. Citas activas (tabla hija citasagregadas + padre citas)
            var citas = await _context.CitasAgregadas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .OrderBy(c => c.FechaHora)
                .ToListAsync();

            // 2. Citas canceladas (tabla hija citascanceladas + padre citas) para mostrarlas en la MISMA vista
            ViewBag.CitasCanceladas = await _context.CitasCanceladas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .OrderByDescending(c => c.FechaCancelacion)
                .ToListAsync();

            // 3. Historial de movimientos para mostrarlo en la MISMA vista
            ViewBag.Historial = await _context.Historial
                .Include(h => h.Cita)
                    .ThenInclude(c => c!.Paciente)
                .OrderByDescending(h => h.FechaRegistro)
                .ToListAsync();

            return View("~/Views/Home/CitasAgregadas.cshtml", citas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int PacienteId, int MedicoId, DateTime FechaHora, string Motivo)
        {
            if (PacienteId > 0 && MedicoId > 0 && !string.IsNullOrWhiteSpace(Motivo))
            {
                var cita = new CitaAgregada
                {
                    PacienteId = PacienteId,
                    MedicoId = MedicoId,
                    FechaHora = FechaHora,
                    Motivo = Motivo,
                    Estado = "Pendiente"
                };
                _context.CitasAgregadas.Add(cita);

                // Registro automático en Historial
                _context.Historial.Add(new Historial
                {
                    Cita = cita,
                    Accion = "Creada",
                    Detalle = $"Cita agendada para el {FechaHora:dd/MM/yyyy HH:mm} - Motivo: {Motivo}",
                    FechaRegistro = DateTime.Now
                });

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DateTime FechaHora, string? Motivo)
        {
            var cita = await _context.CitasAgregadas.FindAsync(id);
            if (cita != null)
            {
                cita.FechaHora = FechaHora;
                if (!string.IsNullOrWhiteSpace(Motivo))
                    cita.Motivo = Motivo;

                // Registro automático en Historial
                _context.Historial.Add(new Historial
                {
                    CitaId = cita.Id,
                    Accion = "Reprogramada",
                    Detalle = $"Cita actualizada al {FechaHora:dd/MM/yyyy HH:mm} - Motivo: {cita.Motivo}",
                    FechaRegistro = DateTime.Now
                });

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, string? motivoCancelacion)
        {
            var cita = await _context.CitasAgregadas.FindAsync(id);
            if (cita != null)
            {
                var motivoFinal = string.IsNullOrWhiteSpace(motivoCancelacion)
                    ? "Cancelada desde gestión de citas"
                    : motivoCancelacion;

                var citaCancelada = new CitaCancelada
                {
                    PacienteId = cita.PacienteId,
                    MedicoId = cita.MedicoId,
                    FechaHora = cita.FechaHora,
                    Motivo = cita.Motivo,
                    Estado = "Cancelada",
                    FechaHoraOriginal = cita.FechaHora,
                    FechaCancelacion = DateTime.Now,
                    MotivoCancelacion = motivoFinal
                };

                _context.CitasCanceladas.Add(citaCancelada);
                _context.CitasAgregadas.Remove(cita);

                // Registro automático en Historial vinculado a la cita cancelada
                _context.Historial.Add(new Historial
                {
                    Cita = citaCancelada,
                    Accion = "Cancelada",
                    Detalle = $"Cita cancelada. Motivo: {motivoFinal}",
                    FechaRegistro = DateTime.Now
                });

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}