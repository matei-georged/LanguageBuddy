using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using LanguageBuddy.Models;

namespace LanguageBuddy.SignalR
{

    public class ConnectionMapping<T>
    {
        private readonly Dictionary<T, HashSet<string>> mapperConnHash =  
            new Dictionary<T, HashSet<string>>();

        public int Count
        {
            get
            {
                return mapperConnHash.Count;
            }
        }

        public void Add(T key, string connId)
        {
            lock (mapperConnHash)
            {
                HashSet<string> second_connections;
                if (!mapperConnHash.TryGetValue(key, out second_connections))
                {
                    second_connections = new HashSet<string>();
                    mapperConnHash.Add(key, second_connections);
                }

                lock (second_connections)
                {
                    second_connections.Add(connId);
                }
            }
        }

        public IEnumerable<string> GetConnections(T key)
        {
            HashSet<string> second_connections;
            if (mapperConnHash.TryGetValue(key, out second_connections))
            {
                return second_connections;
            }
            return Enumerable.Empty<string>();
        }

        public void Remove(T key, string connId)
        {
            lock (mapperConnHash)
            {
                HashSet<string> second_connections;
                if (!mapperConnHash.TryGetValue(key, out second_connections))
                {
                    return;
                }

                lock (second_connections)
                {
                    second_connections.Remove(connId);

                    if (second_connections.Count == 0)
                    {
                        mapperConnHash.Remove(key);
                    }
                }
            }
        }
    }

    public class ChatHub : Hub
    {
        
        static ConnectionMapping<string> mappingconn = new ConnectionMapping<string>();

        public void Connect(string userId)
        {
            string name = Context.User.Identity.Name;
            mappingconn.Add(name, Context.ConnectionId);
        }

        public void SendChatMessage(string who, string sender, string message, int isAudio = 0)
        {
            
            ApplicationDbContext db = new ApplicationDbContext();

            string rxIden = db.Users.Where( t => t.Id == who ).ToList().FirstOrDefault().Email;

            db.ChatMessages.Add( new ChatMessage
            {
              Message = message,
              ReceiverId = who,
              SenderId = sender,
              IsAudio = isAudio
            } );
            db.SaveChanges();

            string name = Context.User.Identity.Name;

            foreach (var connectionId in mappingconn.GetConnections(rxIden))
            {
                Clients.Client(connectionId).addNewMessageToPage(message, isAudio);
            }
        }

        public override Task OnConnected()
        {
            mappingconn.Add(Context.User.Identity.Name, Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stop)
        {
            mappingconn.Remove(Context.User.Identity.Name, Context.ConnectionId);
            return base.OnDisconnected(stop);
        }

        public override Task OnReconnected()
        {
            if (!mappingconn.GetConnections(Context.User.Identity.Name).Contains(Context.ConnectionId))
            {
                mappingconn.Add(Context.User.Identity.Name, Context.ConnectionId);
            }
            return base.OnReconnected();
        }

    }
}