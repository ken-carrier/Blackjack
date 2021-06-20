using System;

namespace BlackjackStats.Data.Entities
{
    public enum PlayerEnum
    {
        Player,
        Dealer
    }

    public enum DealAction
    {
        Deal,
        Hit,
        HitLeft,
        HitRight,
        Split,
        Stand,
        StandLeft,
        StandRight

    }

    public enum DealOutcome
    { 
        Under21,
        Over21,
        At21,
        Dealer17Plus
    }

    public class Deal
    {
        public int ID { get; set; }
        public int GameID { get; set; }
        public DateTime DateTime { get; set; }
        public PlayerEnum Player { get; set; }
        public int Card { get; set; }
        public int SplitScore { get; set; }
        public DealAction Action { get; set; }
        public DealOutcome Outcome { get; set; }

        public Game Game { get; set; }

    }
}
