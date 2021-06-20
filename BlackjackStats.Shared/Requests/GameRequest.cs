using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackjackStats.Shared.Requests
{
    public class GameRequest
    {
        public int ID { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        public string Ip { get; set; }

        public List<DealRequest> DealRequests { get; set; }
    }
}
