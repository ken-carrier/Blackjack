using Blackjack.Models;
using BlackjackStats.Shared.Requests;
using BlackjackStats.Shared.Responses;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SR = BlackjackStats.Shared.Requests;

namespace Blackjack.Services
{
    public class ApiDealService
    {
        private readonly HttpClient _client;

        public ApiDealService(HttpClient client)
        {
            _client = client;
        }

        public async Task<DealResponse> SaveDeal(int card, int splitScore ,DealAction dealAction, SR.PlayerEnum player, DealOutcome dealOutcome)
        {
            var dealRequest = new DealRequest() { ID = 0, GameID = ApiServiceDataModel.GameID, DateTime = System.DateTime.Now,  Card = card, SplitScore =splitScore, 
                                                    Player=player, Action = dealAction, Outcome = dealOutcome};
            var content = JsonSerializer.Serialize(dealRequest);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var saveDealResult = await _client.PostAsync($"{ApiServiceDataModel.ServiceURL}SaveDeal", bodyContent);
            var saveDealContent = await saveDealResult.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<DealResponse>(saveDealContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }

    }
}
