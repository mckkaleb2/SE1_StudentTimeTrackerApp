using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;


// NOTE: at some point, hopefully we can make it where we can directly pass the username, instead of finding it via the Http Query

/// <summary>
/// Sourced from https://www.c-sharpcorner.com/article/building-a-real-time-chat-application-with-signalr-in-blazor-net-9/ 
///</summary>

namespace StudentTimeTrackerApp.Hubs
{
    public class ChatHub : Hub
    {
        public const string HubUrl = "/chathub";



        /// <summary>
        /// broadcasts a message to all connected clients.
        /// </summary>
        /// <remarks>
        /// This uses a tutorial that focused on having SignalR's Hub on a server, in a split client-server
        /// model.
        /// The tutorial can be found at: 
        ///     https://www.c-sharpcorner.com/article/building-a-real-time-chat-application-with-signalr-in-blazor-net-9/
        /// And the repository can be found at:
        ///     https://github.com/dotnet/blazor-samples/blob/main/9.0/BlazorSignalRApp/BlazorSignalRApp/Hubs/ChatHub.cs
        /// </remarks>
        /// <param name="user"></param>
        /// <param name="message"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task SendMessage(string user, string message, int courseId)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message, courseId);
        }

        /*-------------------------------------------------*/

        /// <summary>
        /// cache with &lt; UserName,ConnectionIds &gt;
        /// </summary>
        /// <typeparam name="UserName"></typeparam>
        /// <typeparam name="ConnectionIds"></typeparam>
        /// <returns></returns>
        private static ConcurrentDictionary<string, List<string>> _connectedUsers = new ConcurrentDictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Handles broadcasting messages to all users or sending to a specific user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="recipient"></param>
        /// <param name="message">Body text or string of a message</param>
        /// <returns></returns>
        public async Task Broadcast(string username, string recipient, string message, int courseId)
        {
            if (string.IsNullOrWhiteSpace(recipient) || recipient == "GROUP") // changed from .IsNullOrEmpty
            {
                await Clients.All.SendAsync("Broadcast", username, message, courseId);
            }
            // Search for the recipient in the connected users dictionary
            else if (_connectedUsers.TryGetValue(recipient, out var connections) && connections.Count > 0)
            {
                //send to a specific user
                await Clients.Clients(connections).SendAsync("RecieveFromUser", username, recipient, message, courseId);
            }
            //await Clients.Caller.SendAsync();
        }

        public override async Task OnConnectedAsync()
        {
            // Retrieve the username from the query string
            var username = Context.GetHttpContext().Request.Query["username"];
            if (_connectedUsers.TryGetValue(username, out var connections))
            {
                connections.Add(Context.ConnectionId);
            }
            else
            {
                connections = new List<string>() { Context.ConnectionId };
            }
            // Add to or update the connected users dictionary
            _connectedUsers[username] = connections;
            Console.WriteLine($"{Context.ConnectionId}:${username} connected");

            await Clients.All.SendAsync("UpdateConnectedUsers", _connectedUsers.Keys);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception e)
        {
            var username = Context.GetHttpContext().Request.Query["username"];
            if (_connectedUsers.TryGetValue(username, out var connections) && connections.Contains(Context.ConnectionId))
            {
                connections.Remove(Context.ConnectionId);
                _connectedUsers[username] = connections;
                if (connections.Count == 0)
                {
                    _connectedUsers.Remove(username, out var removed);
                }
            }
            Console.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}:${username}");

            await Clients.All.SendAsync("UpdateConnectedUsers", _connectedUsers.Keys);
            await base.OnDisconnectedAsync(e);
        }
    }



}



