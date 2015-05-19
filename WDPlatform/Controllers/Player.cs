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
            cards = new List<string>();
            score = 0;
        }
        public int score { get; set; }
        public List<string> playerIds { get; set; }
        public List<string> cards { get; set; }
        public string userName { get; set; }
    }
}