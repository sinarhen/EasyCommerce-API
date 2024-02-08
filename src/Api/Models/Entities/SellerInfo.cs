namespace ECommerce.Models.Entities;

public class SellerInfo : BaseEntity
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; }
    public string CompanyDescription { get; set; }
    public string CompanyAddress { get; set; }
    public string CompanyPhone { get; set; }
    public string CompanyEmail { get; set; }
    public string CompanyWebsite { get; set; }
    public string CompanyLogo { get; set; }
    public string CompanyBanner { get; set; }
    public string CompanyFacebook { get; set; }
    public string CompanyTwitter { get; set; }
    public string CompanyInstagram { get; set; }
    public string CompanyLinkedin { get; set; }
    public string CompanyYoutube { get; set; }
    public string CompanyTiktok { get; set; }
    public string CompanySnapchat { get; set; }
    public string CompanyPinterest { get; set; }
    public string CompanyReddit { get; set; }
    public string CompanyTumblr { get; set; }
    public string CompanyWhatsapp { get; set; }
    public string CompanyTelegram { get; set; }
    public string CompanySignal { get; set; }
    public string CompanyViber { get; set; }
    public string CompanyWechat { get; set; }
    public string CompanyLine { get; set; }
    public string CompanyWeibo { get; set; }
    public string CompanyVk { get; set; }
    public string CompanySkype { get; set; }
    public string CompanyDiscord { get; set; }
    public string CompanyTwitch { get; set; }
    public string CompanyClubhouse { get; set; }
    public string CompanyPatreon { get; set; }
    public string CompanyCashapp { get; set; }
    public string CompanyPaypal { get; set; }
    public string CompanyVenmo { get; set; }
    public string CompanyZelle { get; set; }
    public string CompanyApplepay { get; set; }
    public string CompanyGooglepay { get; set; }
    public string CompanySamsungpay { get; set; }

    public bool IsVerified { get; set; }
    
    // TODO: Add stripe and paypal account details

    // Navigation properties
    public User User { get; set; }


}