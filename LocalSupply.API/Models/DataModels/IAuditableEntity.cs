namespace LocalSupply.API.Models.DataModels;

public interface IAuditableEntity
{
    DateTime CreatedOn { get; set; }
    DateTime? UpdatedOn { get; set; }
}