using System;
using System.Collections.Generic;
using System.IO;
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

        public static string getCardsJson() {
            string json = "";
            string path = System.Web.Hosting.HostingEnvironment.MapPath("/assets/data/cards.json");
            json = File.ReadAllText(path);
            return json;
        }
    }
}