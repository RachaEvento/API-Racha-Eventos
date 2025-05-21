using OrganizadorEventos.Enum;

namespace OrganizadorEventos.DTOs.Participantes;

public class ParticipanteDTO
{
    public Guid? Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Telefone { get; set; } = null!;
    public StatusParticipante Status { get; set; }
    public bool Ativo { get; set; }
}