using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CitasMedicas.Data;
using CitasMedicas.Models;

namespace CitasMedicas.Controllers
{
    public class CitasCanceladasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CitasCanceladasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Pacientes = new SelectList(_context.Pacientes, "Id", "Nombre");
            ViewBag.Medicos = new SelectList(_context.Medicos, "Id", "Nombre");

            var citasCanceladas = await _context.CitasCanceladas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .ToListAsync();

            return View("~/Views/Home/CitasCanceladas.cshtml", citasCanceladas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int PacienteId, int MedicoId, DateTime FechaHoraOriginal, string Motivo)
        {
            if (PacienteId > 0 && MedicoId > 0 && !string.IsNullOrWhiteSpace(Motivo))
            {
                var citaCancelada = new CitaCancelada
                {
                    PacienteId = PacienteId,
                    MedicoId = MedicoId,
                    FechaHoraOriginal = FechaHoraOriginal,
                    FechaCancelacion = DateTime.Now,
                    MotivoCancelacion = Motivo
                };
                _context.CitasCanceladas.Add(citaCancelada);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string Motivo)
        {
            var citaCancelada = await _context.CitasCanceladas.FindAsync(id);
            if (citaCancelada != null && !string.IsNullOrWhiteSpace(Motivo))
            {
                citaCancelada.MotivoCancelacion = Motivo;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var citaCancelada = await _context.CitasCanceladas.FindAsync(id);
            if (citaCancelada != null)
            {
                _context.CitasCanceladas.Remove(citaCancelada);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}