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
                        Clients.Clients(game.players[userName].playerIds).reloadCards(game.players[userName].cards, game.currentQuestion, pscores,game.questioner);
                    }
                    else
                    {
                        Clients.Clients(game.getAllPlayerIds()).refreshPlayers(pscores);
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
                 startNewRoundAll(game);
             }
        }

        //Player submit a selection
        public void SubmitCards(long roomNumber, string userName, List<CardsAH.Card> selected)
        {
            Game game = GameUtils.currentGames[roomNumber];
            Player player = game.players[userName];
            player.cards.RemoveAll(c => selected.Exists(s => s.text == c.text));
            player.currentSelected.Add(selected);
            Clients.Clients(game.createrIds).addSelected(selected);
            Clients.Clients(game.players[game.questioner].playerIds).addSelected(selected);
        }

        //Questioner confirm a selection
        public void QuestionerConfirm(long roomNumber, string userName, string selected)
        {
            Game game = GameUtils.currentGames[roomNumber];
            string roundWinner = game.players.First(p => p.Value.currentSelected.Exists(cs=>cs[0].text==selected)).Key;
            foreach (var player in game.players.Values) {
                if (player.currentSelected.Count > 1 && player.userName != roundWinner) { 
                    //Bid person. Minus one score
                    player.score--;
                }
            }
            game.players[roundWinner].score++;
            game.questioner = roundWinner;
            List<CardsAH.Card> fullSelected = game.players[roundWinner].currentSelected.First(c=>c[0].text == selected);
            Clients.Clients(game.getAllPlayerIds()).showRoundWinner(roundWinner, fullSelected,game.currentQuestion);
            Clients.Clients(game.createrIds).showRoundWinner(roundWinner, fullSelected, game.currentQuestion);
            startNewRoundAll(game);
        }

        //start a brand new round for all
        private void startNewRoundAll(Game game) {
            game.reloadAll();
            foreach (var player in game.players.Values)
            {
                Clients.Clients(player.playerIds).reloadCards(player.newRoundCards, game.currentQuestion, game.getScores(), game.questioner);
            }
            Clients.Clients(game.createrIds).reloadCards(game.getSelectedFromAll(), game.currentQuestion, game.getScores(), game.questioner);
        }

    }
}