using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack.Models
{
    public static class ApiServiceDataModel
    {
        public const string ServiceURL = "https://localhost:44345/api/BlackjackStats/";

        public static int GameID { get; set; }

        public static DateTime GameStartDateTime { get; set; }

        public static int DealID { get; set; }
    }
}
