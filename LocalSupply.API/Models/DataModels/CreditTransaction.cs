namespace LocalSupply.API.Models.DataModels;

public class CreditTransaction :Entity
{
    public Guid UserId { get; set; }
    public decimal Amount { get; set; } 
    public string Reason { get; set; } = "";
    public Guid? RelatedRequestId { get; set; }
}