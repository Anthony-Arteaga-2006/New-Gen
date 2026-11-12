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
        public async Task<IActionResult> Create(string Nombre, string Especialidad)
        {
            if (!string.IsNullOrWhiteSpace(Nombre) && !string.IsNullOrWhiteSpace(Especialidad))
            {
                var medico = new Medico
                {
                    Nombre = Nombre,
                    Especialidad = Especialidad
                };
                _context.Medicos.Add(medico);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string? Nombre, string? Especialidad)
        {
            var medico = await _context.Medicos.FindAsync(id);
            if (medico != null)
            {
                if (!string.IsNullOrWhiteSpace(Nombre))
                    medico.Nombre = Nombre;
                if (!string.IsNullOrWhiteSpace(Especialidad))
                    medico.Especialidad = Especialidad;

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