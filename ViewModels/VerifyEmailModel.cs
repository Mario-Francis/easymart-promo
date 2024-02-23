namespace EasyMartApp.ViewModel;

public class VerifyEmailModel{
    public int UserId { get; set; }
    public string? Email { get; set; }
    public string? MaskedEmail { get; set; }
    public string? Code { get; set; }
    public string? ExpectedCode {get;set;}
}