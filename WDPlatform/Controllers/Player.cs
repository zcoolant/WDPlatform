using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDPlatform.Controllers
{
    public class Player
    {

        public Player(string userName)
        {
            this.userName = userName;
            this.hashCode = userName.GetHashCode();
            playerIds = new List<string>();
            cards = new List<CardsAH.Card>();
            score = 0;
            currentSelected = new List<List<CardsAH.Card>>();
        }
        public int score { get; set; }
        public int hashCode { get; set; }
        public List<string> playerIds { get; set; }
        public List<CardsAH.Card> cards { get; set; }
        public List<CardsAH.Card> newRoundCards { get; set; }
        public string userName { get; set; }
        public List<List<CardsAH.Card>> currentSelected { get; set; }
    }
}