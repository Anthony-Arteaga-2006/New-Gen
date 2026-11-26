using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CitasMedicas.Data;
using CitasMedicas.Models;

namespace CitasMedicas.Controllers
{
    public class MedicosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var medicos = await _context.Medicos.ToListAsync();
            return View("~/Views/Home/Medicos.cshtml", medicos);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string Nombres, string Apellidos, string CI, string Especialidad, string Telefono)
        {
            if (!string.IsNullOrWhiteSpace(Nombres) &&
                !string.IsNullOrWhiteSpace(Apellidos) &&
                !string.IsNullOrWhiteSpace(CI) &&
                !string.IsNullOrWhiteSpace(Especialidad) &&
                !string.IsNullOrWhiteSpace(Telefono))
            {
                bool existeCI = await _context.Medicos.AnyAsync(m => m.CI == CI.Trim());
                if (!existeCI)
                {
                    var medico = new Medico
                    {
                        Nombres = Nombres.Trim(),
                        Apellidos = Apellidos.Trim(),
                        CI = CI.Trim(),
                        Especialidad = Especialidad.Trim(),
                        Telefono = Telefono.Trim()
                    };
                    _context.Medicos.Add(medico);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string? Nombres, string? Apellidos, string? CI, string? Especialidad, string? Telefono)
        {
            var medico = await _context.Medicos.FindAsync(id);
            if (medico != null)
            {
                if (!string.IsNullOrWhiteSpace(Nombres))
                    medico.Nombres = Nombres.Trim();
                if (!string.IsNullOrWhiteSpace(Apellidos))
                    medico.Apellidos = Apellidos.Trim();
                if (!string.IsNullOrWhiteSpace(CI))
                    medico.CI = CI.Trim();
                if (!string.IsNullOrWhiteSpace(Especialidad))
                    medico.Especialidad = Especialidad.Trim();
                if (!string.IsNullOrWhiteSpace(Telefono))
                    medico.Telefono = Telefono.Trim();

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var medico = await _context.Medicos.FindAsync(id);
            if (medico != null)
            {
                _context.Medicos.Remove(medico);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}