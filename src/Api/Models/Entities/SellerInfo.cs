using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models.Entities;

public class SellerInfo : BaseEntity
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Name is too long. max 100 characters.")]
    public string Name { get; set; }

    [StringLength(3000, ErrorMessage = "Description is too long. max 3000 characters.")]
    public string Description { get; set; }

    [StringLength(200, ErrorMessage = "Address is too long. max 200 characters.")]
    public string Address { get; set; }

    [StringLength(20, ErrorMessage = "Phone number is too long. max 20 characters.")]
    public string Phone { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(100, ErrorMessage = "Email is too long. max 100 characters.")]
    public string Email { get; set; }

    [StringLength(200, ErrorMessage = "Website URL is too long. max 200 characters.")]
    public string Website { get; set; }

    [StringLength(2000, ErrorMessage = "Logo URL is too long. max 2000 characters.")]
    public string Logo { get; set; }

    [StringLength(2000, ErrorMessage = "Banner URL is too long. max 2000 characters.")]
    public string Banner { get; set; }
    
    [StringLength(2000, ErrorMessage = "URL is too long. max 2000 characters.")]
    [Url(ErrorMessage = "Invalid URL.")]
    public string Facebook { get; set; }

    [StringLength(2000, ErrorMessage = "URL is too long. max 2000 characters.")]
    [Url(ErrorMessage = "Invalid URL.")]
    public string Twitter { get; set; }
    
    [StringLength(2000, ErrorMessage = "URL is too long. max 2000 characters.")]
    [Url(ErrorMessage = "Invalid URL.")]
    public string Instagram { get; set; }
    
    [StringLength(2000, ErrorMessage = "URL is too long. max 2000 characters.")]
    [Url(ErrorMessage = "Invalid URL.")]
    public string Linkedin { get; set; }
    
    [StringLength(2000, ErrorMessage = "URL is too long. max 2000 characters.")]
    [Url(ErrorMessage = "Invalid URL.")]
    public string Youtube { get; set; }
    
    [StringLength(2000, ErrorMessage = "URL is too long. max 2000 characters.")]
    [Url(ErrorMessage = "Invalid URL.")]
    public string Tiktok { get; set; }
    
    [StringLength(2000, ErrorMessage = "URL is too long. max 2000 characters.")]
    [Url(ErrorMessage = "Invalid URL.")]
    public string Snapchat { get; set; }
    
    [StringLength(2000, ErrorMessage = "URL is too long. max 2000 characters.")]
    [Url(ErrorMessage = "Invalid URL.")]
    public string Pinterest { get; set; }
    
    [StringLength(2000, ErrorMessage = "URL is too long. max 2000 characters.")]
    [Url(ErrorMessage = "Invalid URL.")]
    public string Reddit { get; set; }
    
    [StringLength(2000, ErrorMessage = "URL is too long. max 2000 characters.")]
    [Url(ErrorMessage = "Invalid URL.")]
    public string Tumblr { get; set; }
    
    [StringLength(2000, ErrorMessage = "URL is too long. max 2000 characters.")]
    [Url(ErrorMessage = "Invalid URL.")]
    public string Whatsapp { get; set; }
    
    [StringLength(2000, ErrorMessage = "URL is too long. max 2000 characters.")]
    [Url(ErrorMessage = "Invalid URL.")]
    public string Telegram { get; set; }
    
    [StringLength(2000, ErrorMessage = "URL is too long. max 2000 characters.")]
    [Url(ErrorMessage = "Invalid URL.")]
    public string Signal { get; set; }
    
    [StringLength(2000, ErrorMessage = "URL is too long. max 2000 characters.")]
    [Url(ErrorMessage = "Invalid URL.")]
    public string Viber { get; set; }
    
    [StringLength(2000, ErrorMessage = "URL is too long. max 2000 characters.")]
    [Url(ErrorMessage = "Invalid URL.")]
    public string Wechat { get; set; }
    
    [StringLength(2000, ErrorMessage = "URL is too long. max 2000 characters.")]
    [Url(ErrorMessage = "Invalid URL.")]
    public string Line { get; set; }
    
    [StringLength(2000, ErrorMessage = "URL is too long. max 2000 characters.")]
    [Url(ErrorMessage = "Invalid URL.")]
    public string Weibo { get; set; }
    
    [StringLength(2000, ErrorMessage = "URL is too long. max 2000 characters.")]
    [Url(ErrorMessage = "Invalid URL.")]
    public string Vk { get; set; }
    
    [StringLength(2000, ErrorMessage = "URL is too long. max 2000 characters.")]
    [Url(ErrorMessage = "Invalid URL.")]
    public string Skype { get; set; }
    
    [StringLength(2000, ErrorMessage = "URL is too long. max 2000 characters.")]
    [Url(ErrorMessage = "Invalid URL.")]
    public string Discord { get; set; }
    
    [StringLength(2000, ErrorMessage = "URL is too long. max 2000 characters.")]
    [Url(ErrorMessage = "Invalid URL.")]
    public string Twitch { get; set; }
    
    [StringLength(2000, ErrorMessage = "URL is too long. max 2000 characters.")]
    [Url(ErrorMessage = "Invalid URL.")]
    public string Clubhouse { get; set; }
    
    [StringLength(2000, ErrorMessage = "URL is too long. max 2000 characters.")]
    [Url(ErrorMessage = "Invalid URL.")]
    public string Patreon { get; set; }
    
    
    [StringLength(20, ErrorMessage = "Cashapp is too long. max 20 characters.")]
    
    public string Cashapp { get; set; }
    
    [StringLength(20, ErrorMessage = "Paypal is too long. max 20 characters.")]
    public string Paypal { get; set; }
    
    [StringLength(20, ErrorMessage = "Venmo is too long. max 20 characters.")]
    public string Venmo { get; set; }
    
    [StringLength(20, ErrorMessage = "Zelle is too long. max 20 characters.")]
    public string Zelle { get; set; }
    
    [StringLength(20, ErrorMessage = "Applepay is too long. max 20 characters.")]
    public string Applepay { get; set; }
    
    [StringLength(20, ErrorMessage = "Googlepay is too long. max 20 characters.")]
    public string Googlepay { get; set; }
    
    [StringLength(20, ErrorMessage = "Samsungpay is too long. max 20 characters.")]
    public string Samsungpay { get; set; }

    public bool IsVerified { get; set; }

    // Navigation properties
    public ICollection<SellerUpgradeRequest> Requests { get; set; }

    // TODO: Add stripe and paypal account details
}