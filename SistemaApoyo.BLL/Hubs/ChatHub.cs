using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;



namespace SistemaApoyo.BLL.Hubs
{
    public class ChatHub : Hub
    {
        
        public async Task UnirseAlGrupo(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        
        public async Task SalirDelGrupo(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        
        public async Task EnviarMensaje(string groupName, string mensaje)
        {
            await Clients.Group(groupName).SendAsync("RecibirMensaje", mensaje);
        }

    
        public async Task NuevoChat(string groupName, object chat)
        {
            await Clients.Group(groupName).SendAsync("NuevoChat", chat);
        }

        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
            
            await base.OnDisconnectedAsync(exception);
        }
    }
}
