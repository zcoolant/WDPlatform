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
            players = new List<string>();
        }

        public long getGameNumber() {
            return GameNumber;
        }
        public List<string> players { get; set; }
        public string creater { get; set; }
        public GameStatus status { get; set; }
    }

    public enum GameStatus {
        WAITING_CREATER,
        WAITING_PLAYER
    }
}