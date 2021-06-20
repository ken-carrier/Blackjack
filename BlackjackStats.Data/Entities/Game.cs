using System;
using System.Collections.Generic;

namespace BlackjackStats.Data.Entities
{
    public class Game
    {
        public int ID { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public string Ip { get; set; }

        public ICollection<Deal> Deals { get; set; }

    }
}
