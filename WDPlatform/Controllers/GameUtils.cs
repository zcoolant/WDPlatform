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

        //Get back a no-dup card pack for start
        internal static List<string> getCardsPack() {
            HashSet<string> result = new HashSet<string>();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();
            for (int j = 0; j < 100; j++) {
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }
                
                result.Add(new String(stringChars));
            }
            return result.ToList();
        }

        internal static string getNewQuestion()
        {
            var random = new Random();
            string[] testc = {"How","is","I","OK","are","holy","nice","sad","he","you"};
            var stringChars = "";
            for (int i = 0; i < 3; i++)
                {
                    stringChars += testc[random.Next(testc.Length)];
                }
            return stringChars;
        }
    }
}