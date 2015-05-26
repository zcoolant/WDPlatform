using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDPlatform.Controllers
{
    public class CardsAH
    {
        public List<Card> blackCards { get; set; }
        public List<string> whiteCards { get; set; }

        public class Card
        {
            public Card() { 
            }

            public Card(string info){
                this.text = info;
                this.pick = 0;
            }

            public Card(string info, int pick) {
                this.pick = pick;
                this.text = info;
            }

            public string text { get; set; }
            public int pick { get; set; }

            public bool Equals(Card c)
            {
                // If parameter is null return false:
                if ((object)c == null)
                {
                    return false;
                }

                // Return true if the fields match:
                return text == c.text;
            }

        }
    }
}