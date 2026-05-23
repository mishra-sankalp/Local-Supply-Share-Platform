using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NetTopologySuite.Geometries;

namespace LocalSupply.API.Models.DataModels;
//Thinking :- in my project every user can be both receiver of the product or the one who posted the product
//so according to me it is best to introduce profile her only no need to make separate table
public class User : Entity
{
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string CountryCode { get; set; } = "91";
    public string PhoneNumber {get; set; }
    public string PasswordHash { get;set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? FcmToken { get; set; }
    
    public PaymentPreference PaymentPreference { get; set; }
    public string? UpiId { get; set; }             
    public decimal CreditBalance { get; set; } = 0;
}
public enum PaymentPreference 
{
    Credit,
    UPI
}