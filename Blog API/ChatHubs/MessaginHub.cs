using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Blog_API.ChatHubs
{
   
    public class MessaginHub : Hub
    {
        private readonly IHubContext<MessaginHub> _hubcontext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MessaginHub(IHubContext<MessaginHub> hubcontext , IHttpContextAccessor httpContextAccessor )
        {
            _hubcontext = hubcontext;
            _httpContextAccessor = httpContextAccessor;
        }

        public override Task OnConnectedAsync()
        {
            ClaimsIdentity? email = (ClaimsIdentity)_httpContextAccessor.HttpContext?.User.Identity;
            Console.WriteLine(email.Name);
            return base.OnConnectedAsync();
        }
        public async Task SendMessage(string user, string message)
       => await Clients.All.SendAsync("ReceiveMessage", "user1", "hellllo");

       
    }
}
