using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CitasMedicas.Data;
using CitasMedicas.Models;

namespace CitasMedicas.Controllers
{
    public class EstadisticasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstadisticasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var todasLasCitas = await _context.Citas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .ToListAsync();

            var citasActivas = todasLasCitas.Where(c => c.Estado == EstadoCita.Activa).ToList();
            var citasCanceladas = todasLasCitas.Where(c => c.Estado == EstadoCita.Cancelada).ToList();

            // 1. Paciente más recurrente (tomando en cuenta todo su historial)
            var topPacienteGrupo = todasLasCitas
                .GroupBy(c => c.PacienteId)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();

            if (topPacienteGrupo != null)
            {
                var paciente = topPacienteGrupo.First().Paciente;
                ViewBag.PacienteRecurrente = paciente?.NombreCompleto ?? "Sin datos";
                ViewBag.PacienteCantidad = topPacienteGrupo.Count();
            }
            else
            {
                ViewBag.PacienteRecurrente = "Sin registros";
                ViewBag.PacienteCantidad = 0;
            }

            // 2. Médico con mayor demanda (citas activas)
            var topMedicoGrupo = citasActivas
                .GroupBy(c => c.MedicoId)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();

            if (topMedicoGrupo != null)
            {
                var medico = topMedicoGrupo.First().Medico;
                ViewBag.MedicoTop = medico?.NombreCompleto ?? "Sin datos";
                ViewBag.MedicoEspecialidad = medico?.Especialidad ?? "";
                ViewBag.MedicoCantidad = topMedicoGrupo.Count();
            }
            else
            {
                ViewBag.MedicoTop = "Sin registros";
                ViewBag.MedicoEspecialidad = "-";
                ViewBag.MedicoCantidad = 0;
            }

            // 3. Especialidad más solicitada
            var topEspecialidad = citasActivas
                .Where(c => c.Medico != null)
                .GroupBy(c => c.Medico!.Especialidad)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();

            ViewBag.EspecialidadTop = topEspecialidad?.Key ?? "Sin registros";
            ViewBag.EspecialidadCantidad = topEspecialidad?.Count() ?? 0;

            // 4. Tasa de Citas Activas vs Canceladas
            int totalActivas = citasActivas.Count;
            int totalCanceladas = citasCanceladas.Count;
            int totalGeneral = todasLasCitas.Count;

            ViewBag.TotalActivas = totalActivas;
            ViewBag.TotalCanceladas = totalCanceladas;
            ViewBag.PorcentajeActivas = totalGeneral > 0 ? Math.Round((double)totalActivas / totalGeneral * 100, 1) : 0;
            ViewBag.PorcentajeCanceladas = totalGeneral > 0 ? Math.Round((double)totalCanceladas / totalGeneral * 100, 1) : 0;

            // 5. Motivo principal de cancelación
            var topMotivo = citasCanceladas
                .Where(c => !string.IsNullOrEmpty(c.MotivoCancelacion))
                .GroupBy(c => c.MotivoCancelacion)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();

            ViewBag.MotivoCancelacionTop = topMotivo?.Key ?? "Sin cancelaciones";
            ViewBag.MotivoCancelacionCantidad = topMotivo?.Count() ?? 0;

            // Tabla resumen por médico
            ViewBag.ResumenMedicos = await _context.Medicos
                .Select(m => new
                {
                    Nombre = m.Nombres + " " + m.Apellidos,
                    Especialidad = m.Especialidad,
                    Activas = _context.Citas.Count(c => c.MedicoId == m.Id && c.Estado == EstadoCita.Activa),
                    Canceladas = _context.Citas.Count(c => c.MedicoId == m.Id && c.Estado == EstadoCita.Cancelada)
                })
                .ToListAsync();

            return View("~/Views/Home/Estadisticas.cshtml");
        }
    }
}