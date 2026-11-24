using Microsoft.AspNetCore.Mvc;
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

        // Redirige a la vista única de Citas
        public IActionResult Index()
        {
            return RedirectToAction("Index", "CitasAgregadas");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string Motivo)
        {
            var citaCancelada = await _context.CitasCanceladas.FindAsync(id);
            if (citaCancelada != null && !string.IsNullOrWhiteSpace(Motivo))
            {
                citaCancelada.MotivoCancelacion = Motivo;

                _context.Historial.Add(new Historial
                {
                    CitaId = citaCancelada.Id,
                    Accion = "Motivo Actualizado",
                    Detalle = $"Se actualizó el motivo de cancelación: {Motivo}",
                    FechaRegistro = DateTime.Now
                });

                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "CitasAgregadas");
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
            return RedirectToAction("Index", "CitasAgregadas");
        }
    }
}