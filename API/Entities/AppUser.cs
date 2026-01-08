using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser //authentication and sending back information about cuurent user when they log in
    {
        public required string DisplayName { get; set; }
        public string? ImageUrl { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

        //Navigation property
        public Member Member { get; set; } = null!;
    }
}
