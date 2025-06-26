using Hackathon.ScheduleService.DTOs;
using Hackathon.ScheduleService.Models;
using Hackathon.ScheduleService.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.ScheduleService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AgendamentosController(AgendamentoService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAgendamentos()
    {
        var agendamentos = await service.GetAgendamentos();

        return Ok(agendamentos);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAgendamento(Agendamento agendamento)
    {
        await service.CreateAgendamento(agendamento);

        return Ok("Agendamento criado com sucesso.");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Editar(Guid id, [FromBody] DateTime novaData)
    {
        await service.EditarAgendamento(id, novaData);

        return Ok("Agendamento editado com sucesso.");

    }

    [HttpGet("medico/{crm}")]
    public async Task<IActionResult> GetByMedico(string crm, bool disponivel)
    {
        // Obtém os agendamentos filtrados pelo CRM do médico e disponíveis
        var agendamentos = await service.GetAgendamentos();
        var horarios = agendamentos
            .Where(a => a.MedicoCRM == crm && a.Disponivel == disponivel)
            .OrderBy(a => a.DataHora)
            .Select(a => new AgendamentoDto
            {
                MedicoCRM = a.MedicoCRM,
                DataHora = a.DataHora
            })
            .ToList();

        return Ok(horarios);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Deletar(Guid id)
    {
        try
        {
            await service.DeleteAgendamento(id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok("Agendamento deletado com sucesso.");
    }
}