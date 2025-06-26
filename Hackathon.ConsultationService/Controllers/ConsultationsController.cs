using Hackathon.ConsultationService.Data;
using Hackathon.ConsultationService.DTOs;
using Hackathon.ConsultationService.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.ConsultationService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsultationsController(AppDbContext context) : ControllerBase
{
    [HttpPost]
    public IActionResult Create(CreateConsultationDto dto)
    {
        var consulta = new Consultation
        {
            MedicoCRM = dto.MedicoCRM,
            PacienteDocumento = dto.PacienteDocumento,
            DataHora = dto.DataHora,
            Status = "pendente"
        };

        context.Consultations.Add(consulta);
        context.SaveChanges();

        return Ok(consulta);
    }

    [HttpPut("{id}/resposta")]
    public IActionResult Responder(int id, [FromQuery] string status)
    {
        var consulta = context.Consultations.Find(id);
        if (consulta == null) return NotFound();

        if (status != "aceita" && status != "recusada")
            return BadRequest("Status inválido");

        consulta.Status = status;
        context.SaveChanges();

        return Ok(consulta);
    }

    [HttpPut("{id}/cancelar")]
    public IActionResult Cancelar(int id, [FromQuery] string justificativa)
    {
        var consulta = context.Consultations.Find(id);
        if (consulta == null) return NotFound();

        consulta.Status = "cancelada";
        consulta.JustificativaCancelamento = justificativa;
        context.SaveChanges();

        return Ok(consulta);
    }

    [HttpGet("medico/{crm}")]
    public IActionResult GetByMedico(string crm)
    {
        var consultas = context.Consultations
            .Where(c => c.MedicoCRM == crm)
            .ToList();

        return Ok(consultas);
    }
}
