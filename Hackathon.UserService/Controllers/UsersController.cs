using Hackathon.UserService.Binders;
using Hackathon.UserService.DTOs;
using Hackathon.UserService.Models;
using Hackathon.UserService.Services;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon.UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(UsuarioService service) : ControllerBase
{
    [HttpPost("medico")]
    public async Task<IActionResult> CriarMedico([FromBody] MedicoDto dto)
    {
        await service.CriarMedicoAsync(dto);
        return Ok("Médico criado com sucesso.");
    }

    /// <summary>
    /// Pode fazer chamadas por especialidade: /api/users/medicos?especialidade=cardiologia
    /// </summary>
    /// <param name="especialidade"></param>
    /// <returns></returns>
    [HttpGet("medicos")]
    public async Task<IActionResult> ListarMedicos([ModelBinder(BinderType = typeof(EnumMemberModelBinder<EspecialidadeEnum>))] EspecialidadeEnum? especialidade)
    {
        var medicos = await service.ListarMedicosAsync();

        if (especialidade.HasValue)
            medicos = medicos.Where(m => m.Especialidade == especialidade.Value).ToList();

        return Ok(medicos);
    }

    [HttpPost("paciente")]
    public async Task<IActionResult> CriarPaciente([FromBody] PacienteDto dto)
    {
        await service.CriarPacienteAsync(dto);
        return Ok("Paciente criado com sucesso.");
    }

    [HttpGet("paciente/{cpf}")]
    public async Task<IActionResult> ObterPaciente(string cpf)
    {
        var pacienteDto = await service.ObterPacienteAsync(cpf);

        if (pacienteDto == null) return NotFound();
        return Ok(pacienteDto);
    }
}
