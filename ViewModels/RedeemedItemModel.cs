using EasyMartApp.Models;

namespace EasyMartApp.ViewModel;

public class RedeemedItemModel{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ItemId { get; set; }
    public Item Item { get; set; }=default!;
    public DateTime DateRedeemed { get; set; }
}