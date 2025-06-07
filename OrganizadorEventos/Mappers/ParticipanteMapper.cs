using OrganizadorEventos.DTOs.Participantes;
using OrganizadorEventos.Enum;
using OrganizadorEventos.Model;

namespace OrganizadorEventos.Mappers;

public static class ParticipanteMapper
{
    public static ParticipanteDTO ToRequest(this Participante entity)
    {
        return new ParticipanteDTO
        {
            Id = entity.Id,
            Nome = entity.Contato.Nome,
            Email = entity.Contato.Email,
            Telefone = entity.Contato.Telefone,
            Ativo = entity.Contato.Ativo,
            Status = (StatusParticipante)entity.Status
        };
    }
}