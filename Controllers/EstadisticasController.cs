using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CitasMedicas.Data;

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
            var citasAgregadas = await _context.CitasAgregadas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .ToListAsync();

            var citasCanceladas = await _context.CitasCanceladas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .ToListAsync();

            // 1. Paciente más recurrente
            var todosPacientesIds = citasAgregadas.Select(c => c.PacienteId)
                .Concat(citasCanceladas.Select(c => c.PacienteId));

            var topPacienteGrupo = todosPacientesIds
                .GroupBy(id => id)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();

            if (topPacienteGrupo != null)
            {
                var paciente = await _context.Pacientes.FindAsync(topPacienteGrupo.Key);
                ViewBag.PacienteRecurrente = paciente?.Nombre ?? "Sin datos";
                ViewBag.PacienteCantidad = topPacienteGrupo.Count();
            }
            else
            {
                ViewBag.PacienteRecurrente = "Sin registros";
                ViewBag.PacienteCantidad = 0;
            }

            // 2. Médico con mayor demanda
            var topMedicoGrupo = citasAgregadas
                .GroupBy(c => c.MedicoId)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();

            if (topMedicoGrupo != null)
            {
                var medico = topMedicoGrupo.First().Medico;
                ViewBag.MedicoTop = medico?.Nombre ?? "Sin datos";
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
            var topEspecialidad = citasAgregadas
                .Where(c => c.Medico != null)
                .GroupBy(c => c.Medico!.Especialidad)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();

            ViewBag.EspecialidadTop = topEspecialidad?.Key ?? "Sin registros";
            ViewBag.EspecialidadCantidad = topEspecialidad?.Count() ?? 0;

            // 4. Tasa de Citas Activas vs Canceladas
            int totalActivas = citasAgregadas.Count;
            int totalCanceladas = citasCanceladas.Count;
            int totalGeneral = totalActivas + totalCanceladas;

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
                    Nombre = m.Nombre,
                    Especialidad = m.Especialidad,
                    Activas = _context.CitasAgregadas.Count(c => c.MedicoId == m.Id),
                    Canceladas = _context.CitasCanceladas.Count(c => c.MedicoId == m.Id)
                })
                .ToListAsync();

            return View("~/Views/Home/Estadisticas.cshtml");
        }
    }
}