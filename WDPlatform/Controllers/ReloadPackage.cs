using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WDPlatform.Controllers
{
    public class ReloadPackage
    {
        public List<CardsAH.Card> cards { get; set; }
        public CardsAH.Card question {get;set;}
        public Dictionary<string,int> scores {get;set;}
        public string questioner { get; set; }
    }
}