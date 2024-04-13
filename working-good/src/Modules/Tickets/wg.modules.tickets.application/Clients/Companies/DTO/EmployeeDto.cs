namespace wg.modules.tickets.application.Clients.Companies.DTO;

public class EmployeeDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsActive { get; set; }
}