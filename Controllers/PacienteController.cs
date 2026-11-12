using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CitasMedicas.Data;
using CitasMedicas.Models;

namespace CitasMedicas.Controllers
{
    public class PacientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PacientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var pacientes = await _context.Pacientes.ToListAsync();
            return View("~/Views/Home/Pacientes.cshtml", pacientes);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string Nombre, string Documento)
        {
            if (!string.IsNullOrWhiteSpace(Nombre) && !string.IsNullOrWhiteSpace(Documento))
            {
                var paciente = new Paciente
                {
                    Nombre = Nombre,
                    Documento = Documento
                };
                _context.Pacientes.Add(paciente);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string? Nombre, string? Documento)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                if (!string.IsNullOrWhiteSpace(Nombre))
                    paciente.Nombre = Nombre;
                if (!string.IsNullOrWhiteSpace(Documento))
                    paciente.Documento = Documento;

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}