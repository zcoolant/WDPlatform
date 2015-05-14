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
            playersId = new Dictionary<string, string>();
            playersScore = new Dictionary<string, int>();
            createrIds = new List<string>();
        }

        public long getGameNumber() {
            return GameNumber;
        }
        public Dictionary<string,string> playersId { get; set; }
        public Dictionary<string, int> playersScore { get; set; }
        public string creater { get; set; }
        public List<string> createrIds {get;set;}
        public GameStatus status { get; set; }
    }

    public enum GameStatus {
        WAITING_CREATER,
        WAITING_PLAYER
    }
}