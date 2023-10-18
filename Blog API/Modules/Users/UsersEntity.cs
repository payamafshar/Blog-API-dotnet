namespace Blog_API.Modules.Users
{
    public class UsersEntity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
