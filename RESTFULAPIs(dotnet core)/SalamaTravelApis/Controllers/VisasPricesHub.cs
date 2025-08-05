using Microsoft.AspNetCore.SignalR;

namespace SalamaTravelApis.Controllers
{
    public class VisasPricesHub:Hub
    {
        public const string ClientsGroup = "ClientsGroup";

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, ClientsGroup);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, ClientsGroup);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
