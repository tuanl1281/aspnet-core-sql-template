namespace Api.Template.Data.Entities.Common;

public class TenantBaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid TenantId { get; set; }
    
    public DateTime DateCreated { get; set; } = DateTime.Now;
    
    public DateTime DateUpdated { get; set; } = DateTime.Now;
}