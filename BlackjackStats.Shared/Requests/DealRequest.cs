using System;

namespace BlackjackStats.Shared.Requests

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
    public class DealRequest
    {
        public int ID { get; set; }
        public int GameID { get; set; }
        public DateTime DateTime { get; set; }
        public PlayerEnum Player { get; set; }
        public int Card { get; set; }
        public int SplitScore { get; set; }

        public DealAction Action { get; set; }
        public DealOutcome Outcome { get; set; }

        public GameRequest Game { get; set; }
    }
}
