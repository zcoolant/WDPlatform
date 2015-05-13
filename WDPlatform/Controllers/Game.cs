using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDPlatform.Controllers
{
    public class Game
    {
        readonly long GameNumber;
        public Game(long number) {
            GameNumber = number;
            players = new Dictionary<string,string>();
            createrIds = new List<string>();
        }

        public long getGameNumber() {
            return GameNumber;
        }
        public Dictionary<string,string> players { get; set; }
        public string creater { get; set; }
        public List<string> createrIds {get;set;}
        public GameStatus status { get; set; }
    }

    public enum GameStatus {
        WAITING_CREATER,
        WAITING_PLAYER
    }
}