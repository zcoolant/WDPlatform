using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDPlatform.Controllers
{
    public class Game
    {
        private List<string> allCards;
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
        public string question { get; set; }

        //Get this game's roomnumber
        public long getRoomNumber()
        {
            return RoomNumber;
        }

        //reload for everyone to default number
        public string reloadAll()
        {
            if (allCards == null) {
                allCards = GameUtils.getCardsPack();
            }
            foreach (Player player in players.Values)
            {
                if (player.cards.Count < defaultCardNumberPerP)
                {
                    player.cards.AddRange(draw(defaultCardNumberPerP - player.cards.Count));
                }
            }
            question = GameUtils.getNewQuestion();
            return question;
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

        //draw the top number of cards from allCards
        private List<string> draw(int number)
        {
            List<string> result = allCards.GetRange(0, number);
            allCards.RemoveRange(0, number);
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