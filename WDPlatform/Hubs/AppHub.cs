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

        //Create a room
        public long CreateRoom(string userName)
        {
            long randomNumber =  r.Next(9999);
            Game game = new Game(randomNumber);
            game.creater = userName;
            game.createrIds.Add(Context.ConnectionId);
            GameUtils.currentGames.Add(randomNumber, game);
            return randomNumber;
        }

        //Join a room
        public string JoinRoom(long roomNumber, string userName)
        {
            lock (joinLock)
            {
                if (GameUtils.currentGames.ContainsKey(roomNumber))
                {
                    Game game = GameUtils.currentGames[roomNumber];
                    game.addPlayer(userName, Context.ConnectionId);
                    Dictionary<string, int> pscores = game.getScores();
                    if (game.status.Equals(GameStatus.STARTED)) {
                        game.reload(userName);
                        Clients.Clients(game.players[userName].playerIds).reloadQuestion(game.currentQuestion);
                        Clients.Clients(game.players[userName].playerIds).reloadCards(game.players[userName].cards);
                    }
                    else
                    {
                        foreach (var player in game.players.Values)
                        {
                            Clients.Clients(player.playerIds).refreshPlayers(pscores);
                        }

                    }
                    Clients.Clients(game.createrIds).refreshPlayers(pscores);
                    return "ok";
                }
                else
                {
                    return "fail";
                }
            }
            
        }

        //Start the game
        public void StartGame(long roomNumber) { 
           
             Game game = GameUtils.currentGames[roomNumber];
             if (game.createrIds.Contains(Context.ConnectionId)) {
                 //This user is the creater
                 game.reloadAll();
                 foreach (var player in game.players.Values)
                 {
                     Clients.Clients(player.playerIds).reloadQuestion(game.currentQuestion);
                     Clients.Clients(player.playerIds).reloadCards(player.cards);
                 }
                 Clients.Clients(game.createrIds).reloadQuestion(game.currentQuestion);
             }
        }

        public void SelecteCards(long roomNumber, string userName, List<string> selected) {
            Game game = GameUtils.currentGames[roomNumber];
            Player player = game.players[userName];
            player.currentSelected.Add(selected);
            Clients.Clients(game.createrIds).addSelected(selected);
        }


    }
}