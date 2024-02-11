namespace EasyMartApp.Models;

public class UserItem{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ItemId {get;set;}
    public DateTime DateRedeemed {get;set;}
}