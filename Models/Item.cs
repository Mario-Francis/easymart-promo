namespace EasyMartApp.Models;

public class Item{
    public int Id { get; set; }
    public string Name { get; set; }=null!;
    public string Image { get; set; }=null!;
     public int RequiredPoints { get; set; }
}