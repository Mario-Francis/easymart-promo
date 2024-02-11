using EasyMartApp.Models;

namespace EasyMartApp.ViewModel;

public class DashboardModel
{
    public int AccumulatedPoint { get; set; }
    public List<UserPromoCode> UserPromoCodes { get; set; } = default!;
    public List<Item> EligibleItems { get; set; } = default!;
    public List<int> RedeemedItemIds { get; set; } = default!;
    public List<Item> Items { get; set; } = default!;
    public List<UserPointBalance> PointBalances { get; set; }=default!;
}