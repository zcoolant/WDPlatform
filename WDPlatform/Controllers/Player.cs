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
            playerIds = new List<string>();
            cards = new List<CardsAH.Card>();
            score = 0;
            currentSelected = new List<List<string>>();
        }
        public int score { get; set; }
        public List<string> playerIds { get; set; }
        public List<CardsAH.Card> cards { get; set; }
        public string userName { get; set; }
        public List<List<string>> currentSelected { get; set; }
    }
}