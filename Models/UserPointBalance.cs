namespace EasyMartApp.Models;

public class UserPointBalance
{
    public ItemEnum ItemType { get; set; }
    public int Point { get; set; }
    public Item? Item { get; set; }
}