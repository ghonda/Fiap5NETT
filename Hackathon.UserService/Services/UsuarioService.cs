using AutoMapper;
using Hackathon.UserService.Data;
using Hackathon.UserService.DTOs;
using Hackathon.UserService.Models;

namespace Hackathon.UserService.Services;

public class UsuarioService(IUnitOfWork uow, IMapper mapper)
{
    public async Task CriarMedicoAsync(MedicoDto medicoDto)
    {
        var medico = mapper.Map<Medico>(medicoDto);
        await uow.Medicos.Adicionar(medico);
        await uow.CommitAsync();
    }

    public async Task<List<MedicoDto>> ListarMedicosAsync()
    {
        var medicos = await uow.Medicos.ListarTodosAsync();

        return mapper.Map<List<MedicoDto>>(medicos);
    }

    public async Task CriarPacienteAsync(PacienteDto pacienteDto)
    {
        var paciente = mapper.Map<Paciente>(pacienteDto);
        uow.Pacientes.Adicionar(paciente);
        await uow.CommitAsync();
    }

    public async Task<PacienteDto?> ObterPacienteAsync(string cpf)
    {
        var paciente = await uow.Pacientes.ObterPorCpfAsync(cpf);
        if (paciente == null) return null;
        return mapper.Map<PacienteDto>(paciente);
    }
}
