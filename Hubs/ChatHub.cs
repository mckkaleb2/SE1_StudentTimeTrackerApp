using Microsoft.AspNetCore.SignalR;
using StudentTimeTrackerApp.Hubs;
using StudentTimeTrackerApp.Models.Entities;
using System.Collections.Concurrent;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;


// NOTE: at some point, hopefully we can make it where we can directly pass the username, instead of finding it via the Http Query

/// <summary>
/// Sourced from https://www.c-sharpcorner.com/article/building-a-real-time-chat-application-with-signalr-in-blazor-net-9/ 
///</summary>

namespace StudentTimeTrackerApp.Hubs
{
    public struct PersonCourse {         
        public string Person { get; set; }
        public int CourseId { get; set; }
        public PersonCourse(string person, int courseId)
        {
            Person = person;
            CourseId = courseId;
        }
    }


    


    public class ChatHub : Hub
    {
        public const string HubUrl = "/chathub";
        public const string GroupPrefix = "CourseGroup_";


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
        /// cache with &lt; UserName, &lt; recieverID, CourseID &gt &gt;
        /// </summary>
        /// <typeparam name="UserName"></typeparam>
        /// <typeparam name="PersonCourses"></typeparam>
        /// <returns></returns>
        private static ConcurrentDictionary<string, List<PersonCourse>> _userPersonCourses = new ConcurrentDictionary<string, List<PersonCourse>>(StringComparer.OrdinalIgnoreCase);

        public string? UpdateUserCoursePersons(string userId, PersonCourse personCourse)
        //public async Task<string?> UpdateUserCoursePersons(string userId, PersonCourse personCourse)
        {
            string? personA = null;

            if (_userPersonCourses.TryGetValue(userId, out var listA))
            {
                if (!listA.Contains(personCourse))
                {
                    listA.Add(personCourse); // add to a temp copy
                    personA = personCourse.Person;
                }
                else
                {
                    personA = personCourse.Person;
                }
            }
            else
            {
                listA = new List<PersonCourse>() { personCourse };
                personA = personCourse.Person;
            }
            // Add to or update the connected users dictionary
            _userPersonCourses[userId] = listA;
            //await Clients.All.SendAsync("UpdateConnectedUsers", _connectedUsers.Keys);

            return personA;
        }





        protected void RemoveUserCoursePersons(string userId, PersonCourse personCourse)
        {

            if (_userPersonCourses.TryGetValue(userId, out var listA))
            {
                if (listA.Contains(personCourse))
                {
                    listA.Remove(personCourse); // add to a temp copy
                    _userPersonCourses[userId] = listA;
                    if (listA.Count == 0)
                    {
                        _userPersonCourses.Remove(userId, out var removed);
                    }
                }
            }
            Console.WriteLine($"Disconnected {userId}:${personCourse.Person}_${personCourse.CourseId}");
            //await Clients.All.SendAsync("UpdateConnectedUsers", _connectedUsers.Keys);
            //await base.OnDisconnectedAsync(e); 

        }

        protected void RemoveUserCoursePersons(string userId, string recipientId, int courseId)
        {
            PersonCourse userCourse = new PersonCourse(userId, courseId);
            PersonCourse recipientCourse = new PersonCourse(recipientId, courseId);

            string? personA = null;

            if (_userPersonCourses.TryGetValue(userId, out var listA) && listA.Contains(recipientCourse))
            {
                listA.Remove(recipientCourse); // add to a temp copy
                _userPersonCourses[userId] = listA;
                if (listA.Count == 0)
                {
                    _userPersonCourses.Remove(userId, out var removed);
                }
            }
            Console.WriteLine($"Disconnected  {userId}:${recipientId}_${courseId}");
            //await Clients.All.SendAsync("UpdateConnectedUsers", _connectedUsers.Keys);
            //await base.OnDisconnectedAsync(e); 
            if (_userPersonCourses.TryGetValue(recipientId, out var listB) && listB.Contains(userCourse))
            {
                listB.Remove(userCourse); // add to a temp copy
                _userPersonCourses[userId] = listB;
                if (listB.Count == 0)
                {
                    _userPersonCourses.Remove(recipientId, out var removed);
                }
            }
            Console.WriteLine($"Disconnected {recipientId}:${userId}_${courseId}");

        }





        public async Task<string?> MakeGroupName(string userId, string recipientId, int courseId)
        {
            PersonCourse userCourse = new PersonCourse(userId, courseId);
            PersonCourse recipientCourse = new PersonCourse(recipientId, courseId);
            string? personA = null;
            string? personB = null;

            //if (_userPersonCourses.TryGetValue(userId, out var listA) && listA.Count > 0)
            //if (_userPersonCourses.TryGetValue(recipientId, out var listB) && listB.Count > 0)
            personA = await Task.Run(() => UpdateUserCoursePersons(userId, recipientCourse));
            personB = await Task.Run(() => UpdateUserCoursePersons(recipientId, userCourse));

            List<string> sortedPersons = new List<string>() { personA!, personB! };
            sortedPersons.Sort(StringComparer.OrdinalIgnoreCase);
            
            string lesser = sortedPersons[0];
            string greater = sortedPersons[1];

            string outputGroupName = $"{lesser}_{greater}_{courseId}";
        
            return outputGroupName; 
        }

        public async Task AddUserToGroupDirect(string userId, string recipientId, int courseId, string? displayName)
        {

            string? directGroup = await MakeGroupName(userId, recipientId, courseId);

            if (directGroup is not null && _connectedUsers.TryGetValue(userId, out var connections) && connections.Count > 0)
            {
                var connected = connections.ElementAt(0);
                await Groups.AddToGroupAsync(connected, directGroup);

                await Clients.OthersInGroup(directGroup)
                .SendAsync($"[Notice] {displayName} joined chat room.");

            }

        }


        public async Task RemoveUserFromGroupDirect(string userId, string recipientId, int courseId, string displayName)
        {


            string? directGroup = await MakeGroupName(userId, recipientId, courseId);

            if (directGroup is not null && _connectedUsers.TryGetValue(userId, out var connections) && connections.Count > 0)
            {
                var connected = connections.ElementAt(0);
                await Groups.RemoveFromGroupAsync(connected, directGroup);

                await Clients.OthersInGroup(directGroup)
                    .SendAsync($"[Notice] {displayName} left chat room.");
            }

        }



        /// <summary>
        /// Handles broadcasting messages to all users or sending to a specific user.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="recipient"></param>
        /// <param name="message">Body text or string of a message</param>
        /// <returns></returns>
        public async Task Broadcast(string username, string recipient, string message, int courseId)
        {
            string? destinationGroup = null;
            
            if (string.IsNullOrWhiteSpace(recipient) || recipient == "GROUP") // changed from .IsNullOrEmpty
            {
                if (recipient == "GROUP") destinationGroup = $"{GroupPrefix}{courseId}";
                else destinationGroup = await MakeGroupName(username, recipient, courseId);
                if (destinationGroup is not null)
                    await Clients.OthersInGroup(destinationGroup).SendAsync("Broadcast", username, message, courseId);
                else
                    await Clients.All.SendAsync("Broadcast", username, message, courseId);
            }
            // Search for the recipient in the connected users dictionary
            else if (_connectedUsers.TryGetValue(recipient, out var connections) && connections.Count > 0)
            {
                //send to a specific user
                //await Clients.Clients(connections).SendAsync("RecieveFromUser", username, recipient, message, courseId);
                if (recipient != "GROUP") destinationGroup = await MakeGroupName(username, recipient, courseId);
                //if (destinationGroup is not null)
                //    await Clients.Groups(destinationGroup).SendAsync("RecieveFromUser", username, recipient, message, courseId);
                if (_connectedUsers.TryGetValue(username, out var connectionIdExcept) && connectionIdExcept.Count > 0
                    && destinationGroup is not null)

                    await Clients.GroupExcept(destinationGroup, connectionIdExcept).SendAsync("RecieveFromUser", username, recipient, message, courseId);

                    
            }
            //await Clients.Caller.SendAsync();
        }

        /// <summary>
        /// https://www.codeproject.com/articles/SignalR-with-ASP-NET-One-to-one-and-Group-Chat-wit#comments-section
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task AddUserToGroupChat(string userId,  int courseId, string displayName)
        {
            //if (recipientId == "GROUP")
            //{
            string courseGroup = $"{GroupPrefix}{courseId}";
            if (_connectedUsers.TryGetValue(userId, out var connections) && connections.Count > 0)
            {
                var connected = connections.ElementAt(0);
                await Groups.AddToGroupAsync(connected, courseGroup);

                await Clients.OthersInGroup(courseGroup)
                .SendAsync($"[Notice] {displayName} joined chat room.");

            }
            //}
            //if (recipientId != "GROUP")
            //{
            //    string destinationGroup = string.Empty;
            //    destinationGroup = $"{userId}_{recipientId}_{courseId}";

            //    string courseGroup = $"{GroupPrefix}{courseId}";
            //    if (_connectedUsers.TryGetValue(userId, out var connections) && connections.Count > 0)
            //    {
            //        var connected = connections.ElementAt(0);
            //        await Groups.AddToGroupAsync(connected, courseGroup);
            //    }

            //}

        }
        
        public async Task RemoveUserFromGroupChat(string userId, int courseId, string displayName)
        {
            string courseGroup = $"{GroupPrefix}{courseId}";
            if (_connectedUsers.TryGetValue(userId, out var connections) && connections.Count > 0)
            {
                var connected = connections.ElementAt(0);
                await Groups.RemoveFromGroupAsync(connected, courseGroup);

                await Clients.OthersInGroup(courseGroup)
                    .SendAsync($"[Notice] {displayName} left chat room.");
            }

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



