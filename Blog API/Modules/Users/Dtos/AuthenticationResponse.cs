namespace Blog_API.Modules.Users.Dtos
{
    public class AuthenticationResponse
    {
        public string Username { get; set; }

        public string? Email { get; set; }

        public string Token { get; set; }

        public DateTime TokenExpirationDateTime { get; set; }
        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpirationDateTime { get; set; }
    }
}
