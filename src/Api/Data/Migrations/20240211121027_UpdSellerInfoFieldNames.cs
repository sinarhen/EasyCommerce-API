using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdSellerInfoFieldNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompanyZelle",
                table: "Sellers",
                newName: "Zelle");

            migrationBuilder.RenameColumn(
                name: "CompanyYoutube",
                table: "Sellers",
                newName: "Youtube");

            migrationBuilder.RenameColumn(
                name: "CompanyWhatsapp",
                table: "Sellers",
                newName: "Whatsapp");

            migrationBuilder.RenameColumn(
                name: "CompanyWeibo",
                table: "Sellers",
                newName: "Weibo");

            migrationBuilder.RenameColumn(
                name: "CompanyWechat",
                table: "Sellers",
                newName: "Wechat");

            migrationBuilder.RenameColumn(
                name: "CompanyWebsite",
                table: "Sellers",
                newName: "Website");

            migrationBuilder.RenameColumn(
                name: "CompanyVk",
                table: "Sellers",
                newName: "Vk");

            migrationBuilder.RenameColumn(
                name: "CompanyViber",
                table: "Sellers",
                newName: "Viber");

            migrationBuilder.RenameColumn(
                name: "CompanyVenmo",
                table: "Sellers",
                newName: "Venmo");

            migrationBuilder.RenameColumn(
                name: "CompanyTwitter",
                table: "Sellers",
                newName: "Twitter");

            migrationBuilder.RenameColumn(
                name: "CompanyTwitch",
                table: "Sellers",
                newName: "Twitch");

            migrationBuilder.RenameColumn(
                name: "CompanyTumblr",
                table: "Sellers",
                newName: "Tumblr");

            migrationBuilder.RenameColumn(
                name: "CompanyTiktok",
                table: "Sellers",
                newName: "Tiktok");

            migrationBuilder.RenameColumn(
                name: "CompanyTelegram",
                table: "Sellers",
                newName: "Telegram");

            migrationBuilder.RenameColumn(
                name: "CompanySnapchat",
                table: "Sellers",
                newName: "Snapchat");

            migrationBuilder.RenameColumn(
                name: "CompanySkype",
                table: "Sellers",
                newName: "Skype");

            migrationBuilder.RenameColumn(
                name: "CompanySignal",
                table: "Sellers",
                newName: "Signal");

            migrationBuilder.RenameColumn(
                name: "CompanySamsungpay",
                table: "Sellers",
                newName: "Samsungpay");

            migrationBuilder.RenameColumn(
                name: "CompanyReddit",
                table: "Sellers",
                newName: "Reddit");

            migrationBuilder.RenameColumn(
                name: "CompanyPinterest",
                table: "Sellers",
                newName: "Pinterest");

            migrationBuilder.RenameColumn(
                name: "CompanyPhone",
                table: "Sellers",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "CompanyPaypal",
                table: "Sellers",
                newName: "Paypal");

            migrationBuilder.RenameColumn(
                name: "CompanyPatreon",
                table: "Sellers",
                newName: "Patreon");

            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "Sellers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CompanyLogo",
                table: "Sellers",
                newName: "Logo");

            migrationBuilder.RenameColumn(
                name: "CompanyLinkedin",
                table: "Sellers",
                newName: "Linkedin");

            migrationBuilder.RenameColumn(
                name: "CompanyLine",
                table: "Sellers",
                newName: "Line");

            migrationBuilder.RenameColumn(
                name: "CompanyInstagram",
                table: "Sellers",
                newName: "Instagram");

            migrationBuilder.RenameColumn(
                name: "CompanyGooglepay",
                table: "Sellers",
                newName: "Googlepay");

            migrationBuilder.RenameColumn(
                name: "CompanyFacebook",
                table: "Sellers",
                newName: "Facebook");

            migrationBuilder.RenameColumn(
                name: "CompanyEmail",
                table: "Sellers",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "CompanyDiscord",
                table: "Sellers",
                newName: "Discord");

            migrationBuilder.RenameColumn(
                name: "CompanyDescription",
                table: "Sellers",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "CompanyClubhouse",
                table: "Sellers",
                newName: "Clubhouse");

            migrationBuilder.RenameColumn(
                name: "CompanyCashapp",
                table: "Sellers",
                newName: "Cashapp");

            migrationBuilder.RenameColumn(
                name: "CompanyBanner",
                table: "Sellers",
                newName: "Banner");

            migrationBuilder.RenameColumn(
                name: "CompanyApplepay",
                table: "Sellers",
                newName: "Applepay");

            migrationBuilder.RenameColumn(
                name: "CompanyAddress",
                table: "Sellers",
                newName: "Address");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Zelle",
                table: "Sellers",
                newName: "CompanyZelle");

            migrationBuilder.RenameColumn(
                name: "Youtube",
                table: "Sellers",
                newName: "CompanyYoutube");

            migrationBuilder.RenameColumn(
                name: "Whatsapp",
                table: "Sellers",
                newName: "CompanyWhatsapp");

            migrationBuilder.RenameColumn(
                name: "Weibo",
                table: "Sellers",
                newName: "CompanyWeibo");

            migrationBuilder.RenameColumn(
                name: "Wechat",
                table: "Sellers",
                newName: "CompanyWechat");

            migrationBuilder.RenameColumn(
                name: "Website",
                table: "Sellers",
                newName: "CompanyWebsite");

            migrationBuilder.RenameColumn(
                name: "Vk",
                table: "Sellers",
                newName: "CompanyVk");

            migrationBuilder.RenameColumn(
                name: "Viber",
                table: "Sellers",
                newName: "CompanyViber");

            migrationBuilder.RenameColumn(
                name: "Venmo",
                table: "Sellers",
                newName: "CompanyVenmo");

            migrationBuilder.RenameColumn(
                name: "Twitter",
                table: "Sellers",
                newName: "CompanyTwitter");

            migrationBuilder.RenameColumn(
                name: "Twitch",
                table: "Sellers",
                newName: "CompanyTwitch");

            migrationBuilder.RenameColumn(
                name: "Tumblr",
                table: "Sellers",
                newName: "CompanyTumblr");

            migrationBuilder.RenameColumn(
                name: "Tiktok",
                table: "Sellers",
                newName: "CompanyTiktok");

            migrationBuilder.RenameColumn(
                name: "Telegram",
                table: "Sellers",
                newName: "CompanyTelegram");

            migrationBuilder.RenameColumn(
                name: "Snapchat",
                table: "Sellers",
                newName: "CompanySnapchat");

            migrationBuilder.RenameColumn(
                name: "Skype",
                table: "Sellers",
                newName: "CompanySkype");

            migrationBuilder.RenameColumn(
                name: "Signal",
                table: "Sellers",
                newName: "CompanySignal");

            migrationBuilder.RenameColumn(
                name: "Samsungpay",
                table: "Sellers",
                newName: "CompanySamsungpay");

            migrationBuilder.RenameColumn(
                name: "Reddit",
                table: "Sellers",
                newName: "CompanyReddit");

            migrationBuilder.RenameColumn(
                name: "Pinterest",
                table: "Sellers",
                newName: "CompanyPinterest");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Sellers",
                newName: "CompanyPhone");

            migrationBuilder.RenameColumn(
                name: "Paypal",
                table: "Sellers",
                newName: "CompanyPaypal");

            migrationBuilder.RenameColumn(
                name: "Patreon",
                table: "Sellers",
                newName: "CompanyPatreon");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Sellers",
                newName: "CompanyName");

            migrationBuilder.RenameColumn(
                name: "Logo",
                table: "Sellers",
                newName: "CompanyLogo");

            migrationBuilder.RenameColumn(
                name: "Linkedin",
                table: "Sellers",
                newName: "CompanyLinkedin");

            migrationBuilder.RenameColumn(
                name: "Line",
                table: "Sellers",
                newName: "CompanyLine");

            migrationBuilder.RenameColumn(
                name: "Instagram",
                table: "Sellers",
                newName: "CompanyInstagram");

            migrationBuilder.RenameColumn(
                name: "Googlepay",
                table: "Sellers",
                newName: "CompanyGooglepay");

            migrationBuilder.RenameColumn(
                name: "Facebook",
                table: "Sellers",
                newName: "CompanyFacebook");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Sellers",
                newName: "CompanyEmail");

            migrationBuilder.RenameColumn(
                name: "Discord",
                table: "Sellers",
                newName: "CompanyDiscord");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Sellers",
                newName: "CompanyDescription");

            migrationBuilder.RenameColumn(
                name: "Clubhouse",
                table: "Sellers",
                newName: "CompanyClubhouse");

            migrationBuilder.RenameColumn(
                name: "Cashapp",
                table: "Sellers",
                newName: "CompanyCashapp");

            migrationBuilder.RenameColumn(
                name: "Banner",
                table: "Sellers",
                newName: "CompanyBanner");

            migrationBuilder.RenameColumn(
                name: "Applepay",
                table: "Sellers",
                newName: "CompanyApplepay");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Sellers",
                newName: "CompanyAddress");
        }
    }
}
