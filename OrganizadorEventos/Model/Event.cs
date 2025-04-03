using OrganizadorEventos.Enum;

namespace OrganizadorEventos.Model;

public class Event
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public decimal TotalPrice { get; set; } = 0;
    public int MaxParticipants { get; set; }

    public bool IsFinalized { get; set; }
    public DateTime CreatedAt { get; set; }

    public EventStatus Status { get; set; }
    public Guid? LocationId { get; set; }


    //Relacionamentos
    public Location? Location { get; set; }
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<EventCost> EventCosts { get; set; }
    public ICollection<Contact> Contacts { get; set; }
    public ICollection<EventParticipants> EventParticipants { get; set; } = new List<EventParticipants>();
}