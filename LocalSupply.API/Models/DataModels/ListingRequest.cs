namespace LocalSupply.API.Models.DataModels;

public class ListingRequest :Entity
{
    public Guid ListingId { get; set; }
    public Listing Listing { get; set; } = null!;
    public Guid BuyerId { get; set; }
    public User Buyer { get; set; } = null!;

    public RequestStatus Status { get; set; } = RequestStatus.Pending;
}
public enum RequestStatus
{ 
    Pending,
    Accepted,
    Rejected,
    Completed 
}
