using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models.Entities;

public class SellerInfo : BaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Website { get; set; }
    public string Logo { get; set; }
    public string Banner { get; set; }
    public string Facebook { get; set; }
    public string Twitter { get; set; }
    public string Instagram { get; set; }
    public string Linkedin { get; set; }
    public string Youtube { get; set; }
    public string Tiktok { get; set; }
    public string Snapchat { get; set; }
    public string Pinterest { get; set; }
    public string Reddit { get; set; }
    public string Tumblr { get; set; }
    public string Whatsapp { get; set; }
    public string Telegram { get; set; }
    public string Signal { get; set; }
    public string Viber { get; set; }
    public string Wechat { get; set; }
    public string Line { get; set; }
    public string Weibo { get; set; }
    public string Vk { get; set; }
    public string Skype { get; set; }
    public string Discord { get; set; }
    public string Twitch { get; set; }
    public string Clubhouse { get; set; }
    public string Patreon { get; set; }
    public string Cashapp { get; set; }
    public string Paypal { get; set; }
    public string Venmo { get; set; }
    public string Zelle { get; set; }
    public string Applepay { get; set; }
    public string Googlepay { get; set; }
    public string Samsungpay { get; set; }

    public Guid RequestId { get; set; }
    public bool IsVerified { get; set; }

    // Navigation properties
    [ForeignKey("RequestId")]
    public SellerUpgradeRequest Request { get; set; }

    // TODO: Add stripe and paypal account details
}