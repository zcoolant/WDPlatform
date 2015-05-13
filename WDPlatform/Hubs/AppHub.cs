using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using WDPlatform.Controllers;


namespace WDPlatform.Hubs
{

    public class AppHub : Hub
    {
        private Users connectedUsers = Users.Instance;
        private Random r = new Random();
        public static object joinLock = new object();
        
        public int GetUserCount() {
            Clients.Others.sendUserCount(connectedUsers.Count());
            return connectedUsers.Count();
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            connectedUsers.Add(Context.ConnectionId.ToString());
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            connectedUsers.Remove(Context.ConnectionId.ToString());
            GetUserCount();
            return base.OnDisconnected(stopCalled);
        }

        public override System.Threading.Tasks.Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public long CreateRoom(string userName)
        {
            long randomNumber =  r.Next(9999);
            Game game = new Game(randomNumber);
            game.creater = userName;
            game.createrIds.Add(Context.ConnectionId);
            GameUtils.currentGames.Add(randomNumber, game);
            return randomNumber;
        }

        public string JoinRoom(long roomNumber, string userName)
        {
            lock (joinLock)
            {
                if (GameUtils.currentGames.ContainsKey(roomNumber))
                {
                    Game game = GameUtils.currentGames[roomNumber];
                    game.players[userName] = Context.ConnectionId;
                    foreach(string id in game.players.Values){
                        Clients.Client(id).refreshGame(game);
                    }
                    return "ok";
                }
                else
                {
                    return "fail";
                }
            }
            
        }


    }
}