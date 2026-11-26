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
        public async Task<IActionResult> Create(string Nombres, string Apellidos, string CI, string Telefono, string Direccion)
        {
            if (!string.IsNullOrWhiteSpace(Nombres) &&
                !string.IsNullOrWhiteSpace(Apellidos) &&
                !string.IsNullOrWhiteSpace(CI) &&
                !string.IsNullOrWhiteSpace(Telefono) &&
                !string.IsNullOrWhiteSpace(Direccion))
            {
                bool existeCI = await _context.Pacientes.AnyAsync(p => p.CI == CI.Trim());
                if (!existeCI)
                {
                    var paciente = new Paciente
                    {
                        Nombres = Nombres.Trim(),
                        Apellidos = Apellidos.Trim(),
                        CI = CI.Trim(),
                        Telefono = Telefono.Trim(),
                        Direccion = Direccion.Trim()
                    };
                    _context.Pacientes.Add(paciente);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, string? Nombres, string? Apellidos, string? CI, string? Telefono, string? Direccion)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                if (!string.IsNullOrWhiteSpace(Nombres))
                    paciente.Nombres = Nombres.Trim();
                if (!string.IsNullOrWhiteSpace(Apellidos))
                    paciente.Apellidos = Apellidos.Trim();
                if (!string.IsNullOrWhiteSpace(CI))
                    paciente.CI = CI.Trim();
                if (!string.IsNullOrWhiteSpace(Telefono))
                    paciente.Telefono = Telefono.Trim();
                if (!string.IsNullOrWhiteSpace(Direccion))
                    paciente.Direccion = Direccion.Trim();

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