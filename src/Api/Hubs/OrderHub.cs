using ECommerce.Config;
using Microsoft.AspNetCore.SignalR;

namespace ECommerce.Hubs;

public class OrderHub : Hub
{
    private readonly IUserIdProvider _userIdProvider;

    public OrderHub(IUserIdProvider userIdProvider)
    {
        _userIdProvider = userIdProvider;
    }
    
    public async Task SendOrderStatus(string orderId, string status)
    {
        await Clients.All.SendAsync(Channels.OrderCreated, orderId, status);
    }
}
