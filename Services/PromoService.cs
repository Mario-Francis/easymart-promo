using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using EasyMartApp.Models;
using EasyMartApp.ViewModel;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace EasyMartApp.Services;

public class PromoService
{
    private object LockObject;
    private static bool Initialized = false;
    // items
    private static Dictionary<int, Item> Items = new();
    // codes
    private static Dictionary<int, PromoCode> PromoCodes = new();
    private static Dictionary<int, User> Users = new();
    private static Dictionary<int, UserItem> UserItems = new();
    private static Dictionary<int, UserPromoCode> UserPromoCodes = new();
    private static Dictionary<int, List<UserPointBalance>> UserPointBalances = new();

    public PromoService()
    {
        if (!Initialized)
        {
            // add items
            Items.Add(1, new Item { Id = 1, Name = "Black BackPack", RequiredPoints = 12, Image = "bag.png" });
            Items.Add(2, new Item { Id = 2, Name = "Computer Mouse", RequiredPoints = 16, Image = "mouse.png" });
            Items.Add(3, new Item { Id = 3, Name = "Power Bank", RequiredPoints = 12, Image = "power_bank.png" });
            Items.Add(4, new Item { Id = 4, Name = "5k Airtime", RequiredPoints = 10, Image = "airtime.jpg" });

            // add promo codes
            PromoCodes.Add(1, new PromoCode { Id = 1, Code = "RVZI1G", Point = 4 });
            PromoCodes.Add(2, new PromoCode { Id = 2, Code = "59OW4M", Point = 2 });
            PromoCodes.Add(3, new PromoCode { Id = 3, Code = "UR6IVI", Point = 3, ItemId = 1 });
            PromoCodes.Add(4, new PromoCode { Id = 4, Code = "45YDM3", Point = 4 });
            PromoCodes.Add(5, new PromoCode { Id = 5, Code = "9I181F", Point = 6 });
            PromoCodes.Add(6, new PromoCode { Id = 6, Code = "SZI1PM", Point = 1, ItemId = 2 });
            PromoCodes.Add(7, new PromoCode { Id = 7, Code = "KAQE4M", Point = 1 });
            PromoCodes.Add(8, new PromoCode { Id = 8, Code = "XCFG3J", Point = 7 });
            PromoCodes.Add(9, new PromoCode { Id = 9, Code = "O512D3", Point = 3, ItemId = 3 });
            PromoCodes.Add(10, new PromoCode { Id = 10, Code = "BQOOQI", Point = 10 });
            PromoCodes.Add(11, new PromoCode { Id = 11, Code = "MZJXMY", Point = 4 });
            PromoCodes.Add(12, new PromoCode { Id = 12, Code = "YGDCLO", Point = 5, ItemId = 1 });
            PromoCodes.Add(13, new PromoCode { Id = 13, Code = "72GRW9", Point = 2 });
            PromoCodes.Add(14, new PromoCode { Id = 14, Code = "LQKRC1", Point = 3 });
            PromoCodes.Add(15, new PromoCode { Id = 15, Code = "AZ1QFR", Point = 9, ItemId = 2 });
            PromoCodes.Add(16, new PromoCode { Id = 16, Code = "A00016", Point = 16, ItemId = 3, UserId = 1 });
            PromoCodes.Add(17, new PromoCode { Id = 17, Code = "4OUJ9G", Point = 6 });
            PromoCodes.Add(18, new PromoCode { Id = 18, Code = "0RUW98", Point = 4, ItemId = 4 });
            PromoCodes.Add(19, new PromoCode { Id = 19, Code = "2WVL90", Point = 6 });
            PromoCodes.Add(20, new PromoCode { Id = 20, Code = "YMY6US", Point = 10 });
            PromoCodes.Add(21, new PromoCode { Id = 21, Code = "GRY4XM", Point = 5, ItemId = 4 });
            PromoCodes.Add(22, new PromoCode { Id = 22, Code = "KMMPDQ", Point = 4 });
            PromoCodes.Add(23, new PromoCode { Id = 23, Code = "HSZQWL", Point = 3 });
            PromoCodes.Add(24, new PromoCode { Id = 24, Code = "331NWS", Point = 8, ItemId = 1 });
            PromoCodes.Add(25, new PromoCode { Id = 25, Code = "U5Z1QZ", Point = 4 });
            PromoCodes.Add(26, new PromoCode { Id = 26, Code = "6TZI61", Point = 2 });
            PromoCodes.Add(27, new PromoCode { Id = 27, Code = "4K2XGG", Point = 8, ItemId = 2 });
            PromoCodes.Add(28, new PromoCode { Id = 28, Code = "QPF3KX", Point = 4 });
            PromoCodes.Add(29, new PromoCode { Id = 29, Code = "JBTZEF", Point = 10, ItemId = 3 });
            PromoCodes.Add(30, new PromoCode { Id = 30, Code = "EKUPL7", Point = 4 });
            PromoCodes.Add(31, new PromoCode { Id = 31, Code = "JWEBIC", Point = 4 });
            PromoCodes.Add(32, new PromoCode { Id = 32, Code = "CTICU7", Point = 3, ItemId = 4 });
            PromoCodes.Add(33, new PromoCode { Id = 33, Code = "PA7GQ1", Point = 7 });
            PromoCodes.Add(34, new PromoCode { Id = 34, Code = "SIH6ML", Point = 6 });
            PromoCodes.Add(35, new PromoCode { Id = 35, Code = "4UOSCG", Point = 2, ItemId = 1 });
            PromoCodes.Add(36, new PromoCode { Id = 36, Code = "Y80AQ7", Point = 4 });
            PromoCodes.Add(37, new PromoCode { Id = 37, Code = "JM45II", Point = 1 });
            PromoCodes.Add(38, new PromoCode { Id = 38, Code = "8JJSGS", Point = 3, ItemId = 2 });
            PromoCodes.Add(39, new PromoCode { Id = 39, Code = "Q3T7IJ", Point = 10 });
            PromoCodes.Add(40, new PromoCode { Id = 40, Code = "DDSC1D", Point = 4 });
            PromoCodes.Add(41, new PromoCode { Id = 41, Code = "W51RDI", Point = 7, ItemId = 3 });
            PromoCodes.Add(42, new PromoCode { Id = 42, Code = "KK8EAD", Point = 4 });
            PromoCodes.Add(43, new PromoCode { Id = 43, Code = "PR4C3X", Point = 4 });
            PromoCodes.Add(44, new PromoCode { Id = 44, Code = "I6RIU1", Point = 8, ItemId = 4 });
            PromoCodes.Add(45, new PromoCode { Id = 45, Code = "UY689Z", Point = 5 });
            PromoCodes.Add(46, new PromoCode { Id = 46, Code = "3GDQ8E", Point = 2 });
            PromoCodes.Add(47, new PromoCode { Id = 47, Code = "0D5325", Point = 4, ItemId = 1 });
            PromoCodes.Add(48, new PromoCode { Id = 48, Code = "UJNWP7", Point = 6 });
            PromoCodes.Add(49, new PromoCode { Id = 49, Code = "065M66", Point = 10 });
            PromoCodes.Add(50, new PromoCode { Id = 50, Code = "065M66", Point = 10 });

            // add sample user
            Users.Add(1, new User { Id = 1, Email = "johndoe@gmail.com", FirstName = "John", LastName = "Doe", Password = "12345", Phone = "0123456789", IsVerified = true });

            // add user promo codes
            UserPromoCodes.Add(1, new UserPromoCode { Id = 1, UserId = 1, PromoCodeId = 16, DateClaimed = DateTime.Now });

            // add user point balances
            UserPointBalances.Add(1, new List<UserPointBalance> { new UserPointBalance { ItemType = ItemEnum.PowerBank, Point = 16 } });
            Initialized = true;
        }
        LockObject = new object();
    }

    public User? GetUser(string email, string password)
    {
        var _user = Users.Values.Where(u => u.Email.ToLower() == email.ToLower() && u.Password == password).FirstOrDefault();
        return _user;
    }
    public User? GetUser(int userId)
    {
        Users.TryGetValue(userId, out User? _user);
        return _user;
    }
    public User? GetUser(string email)
    {
        User? _user = Users.Values.Where(u => u.Email == email).FirstOrDefault();
        return _user;
    }

    public DashboardModel? GetDashboard(int userId)
    {
        if (!Users.TryGetValue(userId, out User? _user))
        {
            return null;
        }

        var dashboard = new DashboardModel();

        var userPromoCodes = UserPromoCodes.Values.Where(p => p.UserId == userId).ToList();
        var eligibleItems = new List<Item>();
        for (int i = 0; i < userPromoCodes.Count; i++)
        {
            userPromoCodes[i].PromoCode = PromoCodes[userPromoCodes[i].PromoCodeId];
            if (!userPromoCodes[i].PromoCode!.IsRedeemed && userPromoCodes[i].PromoCode!.ItemId != null)
            {
                eligibleItems.Add(Items[userPromoCodes[i].PromoCode!.ItemId!.Value]);
            }
        }

        var pointBalances = new List<UserPointBalance>();
        if (UserPointBalances.ContainsKey(userId))
        {
            pointBalances = UserPointBalances[userId];
        }
        var _balances = new List<UserPointBalance>();
        int accumulatedPoint = 0;

        for (int i = 0; i < 5; i++)
        {
            UserPointBalance balance = null!;
            if (pointBalances.Any(b => b.ItemType == (ItemEnum)i))
            {
                balance = pointBalances.First(b => b.ItemType == (ItemEnum)i);
                if (i > 0)
                {
                    balance.Item = Items[i];
                }
            }
            else
            {
                balance = new UserPointBalance { ItemType = (ItemEnum)i, Point = 0, Item = i > 0 ? Items[i] : null };
            }
            accumulatedPoint += balance.Point;
            _balances.Add(balance);
        }

        dashboard.AccumulatedPoint = accumulatedPoint;
        dashboard.UserPromoCodes = userPromoCodes;
        dashboard.EligibleItems = eligibleItems.Distinct().ToList();
        dashboard.Items = Items.Values.ToList();
        dashboard.PointBalances = _balances;
        dashboard.RedeemedItemIds = UserItems.Values.Where(i => i.UserId == userId).Select(i => i.ItemId).ToList();

        return dashboard;
    }

    public List<UserPromoCode> GetAllUserPromoCodes(int userId)
    {
        var userPromoCodes = UserPromoCodes.Values.Where(p => p.UserId == userId).ToList();
        for (int i = 0; i < userPromoCodes.Count; i++)
        {
            userPromoCodes[i].PromoCode = PromoCodes[userPromoCodes[i].PromoCodeId];
        }

        return userPromoCodes;
    }

    public bool ClaimCode(int userId, string code)
    {
        var _code = PromoCodes.Values.Where(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase) && c.UserId == null).FirstOrDefault();
        if (_code == null)
        {
            return false;
        }

        lock (LockObject)
        {
            // update promo code
            PromoCodes[_code.Id].UserId = userId;
            PromoCodes[_code.Id].DateClaimed = DateTime.Now;

            // add user promo code
            int id = UserPromoCodes.Keys.Count + 1;
            UserPromoCodes.Add(id, new UserPromoCode
            {
                Id = id,
                DateClaimed = DateTime.Now,
                UserId = userId,
                PromoCodeId = _code.Id
            });

            // update balance
            List<UserPointBalance> balances = new List<UserPointBalance>();
            if (UserPointBalances.ContainsKey(userId))
            {
                balances = UserPointBalances[userId];
            }
            var type = _code.ItemId == null ? ItemEnum.Uncategorized : (ItemEnum)_code.ItemId.Value;
            if (balances.Any(b => b.ItemType == type))
            {
                var balance = balances.First(b => b.ItemType == type);
                balance.Point += _code.Point;
            }
            else
            {
                balances.Add(new UserPointBalance { ItemType = type, Point = _code.Point });
            }
            UserPointBalances[userId] = balances;
        }

        return true;
    }


    public User RegisterUser(User user)
    {
        lock (LockObject)
        {
            user.Id = Users.Values.Count() + 1;
            Users.Add(user.Id, user);
        }

        return user;
    }

    public void VerifyUser(int userId)
    {
        var user = Users[userId];
        lock (LockObject)
        {
            user.IsVerified = true;
            Users[userId] = user;
        }
    }

    public void RedeemItem(int userId, int itemId)
    {
        var userPromoCodes = UserPromoCodes.Values.Where(p => p.UserId == userId).ToList();
        UserPromoCode? qualifyingPromoCode = null;
        var promoCodesToUpdate = new List<PromoCode>();
        int requiredPoint = 0;

        for (int i = 0; i < userPromoCodes.Count; i++)
        {
            userPromoCodes[i].PromoCode = PromoCodes[userPromoCodes[i].PromoCodeId];
            if (qualifyingPromoCode == null && !userPromoCodes[i].PromoCode!.IsRedeemed && userPromoCodes[i].PromoCode!.ItemId == itemId)
            {
                qualifyingPromoCode = userPromoCodes[i];
                requiredPoint += qualifyingPromoCode.PromoCode!.Point;
            }
        }
        var qualifyingItem = Items[qualifyingPromoCode!.PromoCode!.ItemId!.Value];
        for (int i = 0; i < userPromoCodes.Count; i++)
        {
            if (!userPromoCodes[i].PromoCode!.IsRedeemed && userPromoCodes[i].Id != qualifyingPromoCode.Id)
            {
                if (requiredPoint < qualifyingItem.RequiredPoints)
                {
                    requiredPoint += userPromoCodes[i].PromoCode!.Point;
                    promoCodesToUpdate.Add(userPromoCodes[i].PromoCode!);
                }
                else
                {
                    break;
                }
            }
        }

        // update promo codes
        // qualifyingPromoCode.PromoCode.IsRedeemed = true;
        // for (int i = 0; i < promoCodesToUpdate.Count; i++)
        // {
        //     lock (LockObject)
        //     {
        //         promoCodesToUpdate[i].IsRedeemed = true;
        //     }
        // }

        // add reddem item
        lock (LockObject)
        {
            int _id = UserItems.Count() + 1;
            UserItems.Add(_id, new UserItem
            {
                Id = _id,
                UserId = userId,
                ItemId = itemId,
                DateRedeemed = DateTime.Now
            });
        }

        // update balance

        lock (LockObject)
        {
            var itemBalance = UserPointBalances[userId].FirstOrDefault(b => b.ItemType == (ItemEnum)itemId);
            var freeBalance = UserPointBalances[userId].FirstOrDefault(b => b.ItemType == ItemEnum.Uncategorized);

            if (((itemBalance?.Point ?? 0) + (freeBalance?.Point ?? 0)) >= qualifyingItem.RequiredPoints)
            {
                if (itemBalance!.Point >= qualifyingItem.RequiredPoints)
                {
                    itemBalance.Point -= qualifyingItem.RequiredPoints;
                }
                else
                {
                    int left = qualifyingItem.RequiredPoints - itemBalance.Point;
                    itemBalance.Point = 0;
                    freeBalance!.Point -= left;
                }
            }
        }

    }

    public List<RedeemedItemModel> GetUserRedeemedItems(int userId)
    {
        var userItems = UserItems.Values.Where(v => v.UserId == userId);
        List<RedeemedItemModel> result = new(userItems.Count());

        foreach (var userItem in userItems)
        {
            result.Add(new RedeemedItemModel
            {
                Id = userItem.Id,
                UserId = userItem.UserId,
                ItemId = userItem.ItemId,
                Item = Items[userItem.ItemId],
                DateRedeemed = userItem.DateRedeemed
            });
        }

        return result;
    }

    public async Task SendMail(string code, string email)
    {
        var mail = new MimeMessage();
        var from = new MailboxAddress("EASYMART PROMO PORTAL", "no_reply@mariofrancis.com.ng");
        mail.From.Add(from);

        mail.To.Add(MailboxAddress.Parse(email));

        mail.Subject = "EASYMART PROMO PORTAL - Email Verification OTP";

        BodyBuilder body = new BodyBuilder();
        body.TextBody = $"""
            Hi there,

            Your one-time email verification OTP is {code}. This will expire in 5 minutes.

            Bests regards,
            EasyMart Team.
        """;

        mail.Body = body.ToMessageBody();
        var smtp = new SmtpClient();
        smtp.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((object sender, X509Certificate? cert, X509Chain? chain, SslPolicyErrors errors) => true);


        await smtp.ConnectAsync("mail.mariofrancis.com.ng", 465, SecureSocketOptions.Auto);

        await smtp.AuthenticateAsync("no_reply@mariofrancis.com.ng", "henriofrancis");

        await smtp.SendAsync(mail);
        await smtp.DisconnectAsync(true);
    }

    public string GenerateCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        Random random = new Random();
        string result = new string(new char[length].Select(c => chars[random.Next(chars.Length)]).ToArray());
        return result;
    }
}