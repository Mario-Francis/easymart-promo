namespace EasyMartApp.ViewModel;

public class VerifyPhoneModel{
    public int UserId { get; set; }
    public string? Phone { get; set; }
    public string? MaskedPhone { get; set; }
    public string? Code { get; set; }
}