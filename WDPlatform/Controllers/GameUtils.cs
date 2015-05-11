using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDPlatform.Controllers
{
    public class GameUtils
    {
        public static Dictionary<long, Game> currentGames = new Dictionary<long, Game>();

        static Game getCurrentGame(long number) {
            return currentGames[number];
        }
    }
}