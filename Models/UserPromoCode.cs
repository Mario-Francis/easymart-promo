namespace EasyMartApp.Models;

public class UserPromoCode{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PromoCodeId { get; set; }
    public DateTime DateClaimed {get;set;}
    public PromoCode? PromoCode { get; set; }
}