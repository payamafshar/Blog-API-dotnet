namespace Blog_API.Modules.Users.Dtos
{
    public class AuthenticationResponse
    {
        public string Username { get; set; }

        public string? Email { get; set; }

        public string token { get; set; }
    }
}
