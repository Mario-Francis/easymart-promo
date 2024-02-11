namespace EasyMartApp.Models;

public class PromoCode{
    public int Id { get; set; }
    public string Code { get; set; }=null!;
    public int Point {get;set;}
    public int? ItemId {get;set;}
    public int? UserId {get;set;}
    public bool IsRedeemed {get;set;}
    public DateTime? DateClaimed { get; set; }
}