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

            var citas = await _context.CitasAgregadas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
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
                    Motivo = Motivo
                };
                _context.CitasAgregadas.Add(cita);
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
                var citaCancelada = new CitaCancelada
                {
                    PacienteId = cita.PacienteId,
                    MedicoId = cita.MedicoId,
                    FechaHoraOriginal = cita.FechaHora,
                    FechaCancelacion = DateTime.Now,
                    MotivoCancelacion = string.IsNullOrWhiteSpace(motivoCancelacion) ? "Cancelada desde citas agregadas" : motivoCancelacion
                };

                _context.CitasCanceladas.Add(citaCancelada);
                _context.CitasAgregadas.Remove(cita);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}