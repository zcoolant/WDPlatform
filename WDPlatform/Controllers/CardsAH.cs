using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDPlatform.Controllers
{
    public class CardsAH
    {
        public List<Card> blackCards { get; set; }
        public List<Card> whiteCards { get; set; }

        public class Card
        {
            public string text { get; set; }
            public int pick { get; set; }
        }
    }
}