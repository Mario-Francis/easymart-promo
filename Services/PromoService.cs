using EasyMartApp.Models;
using EasyMartApp.ViewModel;

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

    public PromoService()
    {
        if (!Initialized)
        {
            // add items
            Items.Add(1, new Item { Id = 1, Name = "Black BackPack", RequiredPoints = 12, Image = "bag.png" });
            Items.Add(2, new Item { Id = 2, Name = "Computer Mouse", RequiredPoints = 16, Image = "mouse.png" });
            Items.Add(3, new Item { Id = 3, Name = "Power Bank", RequiredPoints = 12, Image = "power_bank.png" });

            // add promo codes
            PromoCodes.Add(1, new PromoCode { Id = 1, Code = "A00001", Point = 4 });
            PromoCodes.Add(2, new PromoCode { Id = 2, Code = "A00002", Point = 2 });
            PromoCodes.Add(3, new PromoCode { Id = 3, Code = "A00003", Point = 3, ItemId = 1 });
            PromoCodes.Add(4, new PromoCode { Id = 4, Code = "A00004", Point = 4 });
            PromoCodes.Add(5, new PromoCode { Id = 5, Code = "A00005", Point = 6 });
            PromoCodes.Add(6, new PromoCode { Id = 6, Code = "A00006", Point = 1, ItemId = 2 });
            PromoCodes.Add(7, new PromoCode { Id = 7, Code = "A00007", Point = 1 });
            PromoCodes.Add(8, new PromoCode { Id = 8, Code = "A00008", Point = 7 });
            PromoCodes.Add(9, new PromoCode { Id = 9, Code = "A00009", Point = 3, ItemId = 3 });
            PromoCodes.Add(10, new PromoCode { Id = 10, Code = "A00010", Point = 4 });
            PromoCodes.Add(11, new PromoCode { Id = 11, Code = "A00011", Point = 4 });
            PromoCodes.Add(12, new PromoCode { Id = 12, Code = "A00012", Point = 5, ItemId = 1 });
            PromoCodes.Add(13, new PromoCode { Id = 13, Code = "A00013", Point = 2 });
            PromoCodes.Add(14, new PromoCode { Id = 14, Code = "A00014", Point = 3 });
            PromoCodes.Add(15, new PromoCode { Id = 15, Code = "A00015", Point = 9, ItemId = 2 });
            PromoCodes.Add(16, new PromoCode { Id = 15, Code = "A00016", Point = 16, ItemId = 3, UserId = 1 });

            // add sample user
            Users.Add(1, new User { Id = 1, Email = "johndoe@gmail.com", FirstName = "John", LastName = "Doe", Password = "12345", Phone = "0123456789", IsVerified = true });

            // add user promo codes
            UserPromoCodes.Add(1, new UserPromoCode { Id = 1, UserId = 1, PromoCodeId = 16, DateClaimed = DateTime.Now });
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
        var accumulatedPoint = userPromoCodes.Where(p => !p.PromoCode!.IsRedeemed).Sum(p => p.PromoCode!.Point);

        dashboard.AccumulatedPoint = accumulatedPoint;
        dashboard.UserPromoCodes = userPromoCodes;
        dashboard.EligibleItems = eligibleItems;
        dashboard.Items = Items.Values.ToList();

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

        PromoCodes[_code.Id].UserId = userId;
        PromoCodes[_code.Id].DateClaimed = DateTime.Now;

        lock (LockObject)
        {
            int id = UserPromoCodes.Keys.Count + 1;
            UserPromoCodes.Add(id, new UserPromoCode
            {
                Id = id,
                DateClaimed = DateTime.Now,
                UserId = userId,
                PromoCodeId = _code.Id
            });
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
                requiredPoint+=qualifyingPromoCode.PromoCode!.Point;
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
        qualifyingPromoCode.PromoCode.IsRedeemed=true;
        for (int i = 0; i < promoCodesToUpdate.Count; i++)
        {
            lock (LockObject)
            {
                promoCodesToUpdate[i].IsRedeemed = true;
            }
        }

        // add reddem item
        lock (LockObject)
        {
            int _id = UserItems.Count() + 1;
            UserItems.Add(userId, new UserItem
            {
                Id = _id,
                UserId = userId,
                ItemId = itemId,
                DateRedeemed = DateTime.Now
            });
        }
    }

    public List<RedeemedItemModel> GetUserRedeemedItems(int userId){
        var userItems = UserItems.Values.Where(v=>v.UserId == userId);
        List<RedeemedItemModel> result = new(userItems.Count());

        foreach(var userItem in userItems){
            result.Add(new RedeemedItemModel{
                Id = userItem.Id,
                UserId=userItem.UserId,
                ItemId=userItem.ItemId,
                Item = Items[userItem.ItemId],
                DateRedeemed=userItem.DateRedeemed
            });
        }

        return result;
    }
}