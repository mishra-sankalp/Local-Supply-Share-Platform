using NetTopologySuite.Geometries;

namespace LocalSupply.API.Models.DataModels;

public class Listing : Entity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string? ApproxQuantity { get; set; }
    public string? Price { get; set; }
    public string? PhotoUrl {get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public ProductStatus Status { get; set; } = ProductStatus.Active;
    public ICollection<ListingRequest> Requests { get; set; } = new List<ListingRequest>();
}

public enum ProductStatus
{
    Active,
    Sold,
    GivenFreeOfCost,
    Expired,
    Cancelled,
}