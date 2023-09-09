namespace CrunchEconUI.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public ulong SteamId { get; set; }
        public int? AvatarImageId { get; set; }
        public string Color { get; set; }
        public DateTime CreateDate { get; set; }
        public string AvatarUrl { get; set; }
    }
}
