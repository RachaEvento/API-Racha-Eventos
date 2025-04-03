namespace OrganizadorEventos.Model;

public class Contact
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string UserId { get; set; } = string.Empty;
    public User User { get; set; }
    
    public ICollection<Event> Events { get; set; }
    public ICollection<EventParticipants> EventParticipants { get; set; } = new List<EventParticipants>();
}