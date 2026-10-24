using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CitasMedicas.Data;
using CitasMedicas.Models;

namespace CitasMedicas.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    // ==================== 1. MÉDICOS ====================
    public IActionResult Medicos()
    {
        var listaMedicos = _context.Medicos.ToList();
        return View(listaMedicos);
    }

    [HttpPost]
    public IActionResult AgregarMedico(string nombre, string especialidad)
    {
        if (!string.IsNullOrWhiteSpace(nombre) && !string.IsNullOrWhiteSpace(especialidad))
        {
            _context.Medicos.Add(new Medico { Nombre = nombre, Especialidad = especialidad });
            _context.SaveChanges();
        }
        return RedirectToAction("Medicos");
    }

    [HttpPost]
    public IActionResult ActualizarMedico(int id, string nombre, string especialidad)
    {
        var med = _context.Medicos.Find(id);
        if (med != null)
        {
            if (!string.IsNullOrWhiteSpace(nombre)) med.Nombre = nombre;
            if (!string.IsNullOrWhiteSpace(especialidad)) med.Especialidad = especialidad;
            _context.SaveChanges();
        }
        return RedirectToAction("Medicos");
    }

    [HttpPost]
    public IActionResult BorrarMedico(int id)
    {
        var med = _context.Medicos.Find(id);
        if (med != null)
        {
            _context.Medicos.Remove(med);
            _context.SaveChanges();
        }
        return RedirectToAction("Medicos");
    }

    // ==================== 2. PACIENTES ====================
    public IActionResult Pacientes()
    {
        var listaPacientes = _context.Pacientes.ToList();
        return View(listaPacientes);
    }

    [HttpPost]
    public IActionResult AgregarPaciente(string nombre, string documento)
    {
        if (!string.IsNullOrWhiteSpace(nombre) && !string.IsNullOrWhiteSpace(documento))
        {
            _context.Pacientes.Add(new Paciente { Nombre = nombre, Documento = documento });
            _context.SaveChanges();
        }
        return RedirectToAction("Pacientes");
    }

    [HttpPost]
    public IActionResult ActualizarPaciente(int id, string nombre, string documento)
    {
        var pac = _context.Pacientes.Find(id);
        if (pac != null)
        {
            if (!string.IsNullOrWhiteSpace(nombre)) pac.Nombre = nombre;
            if (!string.IsNullOrWhiteSpace(documento)) pac.Documento = documento;
            _context.SaveChanges();
        }
        return RedirectToAction("Pacientes");
    }

    [HttpPost]
    public IActionResult BorrarPaciente(int id)
    {
        var pac = _context.Pacientes.Find(id);
        if (pac != null)
        {
            _context.Pacientes.Remove(pac);
            _context.SaveChanges();
        }
        return RedirectToAction("Pacientes");
    }

    // ==================== 3. CITAS AGREGADAS ====================
    public IActionResult CitasAgregadas()
    {
        var listaCitas = _context.CitasAgregadas.ToList();
        return View(listaCitas);
    }

    [HttpPost]
    public IActionResult AgregarCita(string pacienteNombre, string medicoNombre, DateTime fecha, string motivo)
    {
        if (!string.IsNullOrWhiteSpace(pacienteNombre) && !string.IsNullOrWhiteSpace(medicoNombre))
        {
            _context.CitasAgregadas.Add(new Cita { PacienteNombre = pacienteNombre, MedicoNombre = medicoNombre, Fecha = fecha, Motivo = motivo });
            _context.SaveChanges();
        }
        return RedirectToAction("CitasAgregadas");
    }

    [HttpPost]
    public IActionResult ActualizarCita(int id, DateTime fecha, string motivo)
    {
        var cita = _context.CitasAgregadas.Find(id);
        if (cita != null)
        {
            cita.Fecha = fecha;
            if (!string.IsNullOrWhiteSpace(motivo)) cita.Motivo = motivo;
            _context.SaveChanges();
        }
        return RedirectToAction("CitasAgregadas");
    }

    [HttpPost]
    public IActionResult BorrarCita(int id)
    {
        var cita = _context.CitasAgregadas.Find(id);
        if (cita != null)
        {
            _context.CitasAgregadas.Remove(cita);
            _context.SaveChanges();
        }
        return RedirectToAction("CitasAgregadas");
    }

    // ==================== 4. CITAS CANCELADAS ====================
    public IActionResult CitasCanceladas()
    {
        var listaCanceladas = _context.CitasCanceladas.ToList();
        return View(listaCanceladas);
    }

    [HttpPost]
    public IActionResult AgregarCitaCancelada(string pacienteNombre, string medicoNombre, DateTime fecha, string motivo)
    {
        if (!string.IsNullOrWhiteSpace(pacienteNombre) && !string.IsNullOrWhiteSpace(medicoNombre))
        {
            _context.CitasCanceladas.Add(new Cita { PacienteNombre = pacienteNombre, MedicoNombre = medicoNombre, Fecha = fecha, Motivo = motivo });
            _context.SaveChanges();
        }
        return RedirectToAction("CitasCanceladas");
    }

    [HttpPost]
    public IActionResult ActualizarCitaCancelada(int id, string motivo)
    {
        var cita = _context.CitasCanceladas.Find(id);
        if (cita != null && !string.IsNullOrWhiteSpace(motivo))
        {
            cita.Motivo = motivo;
            _context.SaveChanges();
        }
        return RedirectToAction("CitasCanceladas");
    }

    [HttpPost]
    public IActionResult BorrarCitaCancelada(int id)
    {
        var cita = _context.CitasCanceladas.Find(id);
        if (cita != null)
        {
            _context.CitasCanceladas.Remove(cita);
            _context.SaveChanges();
        }
        return RedirectToAction("CitasCanceladas");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new CitasMEdicas.Models.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}