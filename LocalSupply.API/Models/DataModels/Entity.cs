namespace LocalSupply.API.Models.DataModels;

public abstract class Entity : IAuditableEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
}