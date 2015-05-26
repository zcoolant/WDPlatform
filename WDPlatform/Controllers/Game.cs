using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace WDPlatform.Controllers
{
    public class Game
    {
        public int defaultCardNumberPerP = 10;
        readonly long RoomNumber;
        public Game(long roomNumber)
        {
            RoomNumber = roomNumber;
            players = new Dictionary<string, Player>();

            createrIds = new List<string>();

        }

        public Dictionary<string, Player> players { get; set; }
        public string creater { get; set; }
        public List<string> createrIds { get; set; }
        public GameStatus status { get; set; }
        public CardsAH cardsAH { get; set; }
        public CardsAH.Card currentQuestion { get; set; }
        public string questioner { get; set; }

        //Get this game's roomnumber
        public long getRoomNumber()
        {
            return RoomNumber;
        }

        //Start this game
        public void startGame()
        {
            //load cardsAH
            String json = GameUtils.getCardsJson();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            CardsAH cah = jss.Deserialize<CardsAH>(json);
            cah.blackCards = cah.blackCards.OrderBy(a => Guid.NewGuid()).ToList();
            cah.whiteCards = cah.whiteCards.OrderBy(a => Guid.NewGuid()).ToList();
            this.cardsAH = cah;

            //Change status
            status = GameStatus.STARTED;
        }

        //reload for everyone to default number
        public void reloadAll()
        {
            if (cardsAH == null)
            {
                startGame();
            }
            foreach (Player player in players.Values)
            {
                player.newRoundCards = new List<CardsAH.Card>();
                if (player.cards.Count < defaultCardNumberPerP)
                {
                    player.newRoundCards.AddRange(draw(defaultCardNumberPerP - player.cards.Count));
                    player.cards.AddRange(player.newRoundCards);
                }
                player.currentSelected.Clear();
            }

            currentQuestion = cardsAH.blackCards[0];
            cardsAH.blackCards.RemoveAt(0);
        }

        //reload for specific user to default number. Only use when new user join
        public void reload(string userName)
        {
            if (cardsAH == null)
            {
                startGame();
            }
            Player player = this.players[userName];

            if (player.cards.Count < defaultCardNumberPerP)
            {
                player.newRoundCards = draw(defaultCardNumberPerP - player.cards.Count);
                player.cards.AddRange(player.newRoundCards);
            }
        }

        //Add a player to current game's players
        public void addPlayer(string userName, string sessionId)
        {
            if (players.ContainsKey(userName))
            {
                players[userName].playerIds.Add(sessionId);
            }
            else
            {
                Player player = new Player(userName);
                player.playerIds.Add(sessionId);
                players[userName] = player;
            }
            if (players.Count == 1) {
                questioner = userName;
            }
        }

        //return all plays and his score
        public Dictionary<string, int> getScores()
        {
            Dictionary<string, int> playerScores = new Dictionary<string, int>();
            foreach (var user in players.Keys)
            {
                playerScores.Add(user, players[user].score);
            }
            return playerScores;
        }

        public List<List<CardsAH.Card>> getSelectedFromAll (){
            List<List<CardsAH.Card>> result = new List<List<CardsAH.Card>>();
            foreach (var user in players.Keys)
            {
                result.AddRange(players[user].currentSelected);
            }
            return result;
        }

        public List<string> getAllPlayerIds() {
            List<string> ids = new List<string>();
            foreach (var player in players) {
                ids.AddRange(player.Value.playerIds);
            }
            return ids;
        }

        //draw the top number of cards from allCards
        private List<CardsAH.Card> draw(int number)
        {
            List<CardsAH.Card> result = new List<CardsAH.Card>();
            foreach (var card in cardsAH.whiteCards.GetRange(0, number)) {
                result.Add(new CardsAH.Card(card));
            }
            cardsAH.whiteCards.RemoveRange(0, number);
            return result;
        }
    }

    public enum GameStatus
    {
        WAITING_CREATER,
        WAITING_PLAYER,
        STARTED,
        END,
    }
}