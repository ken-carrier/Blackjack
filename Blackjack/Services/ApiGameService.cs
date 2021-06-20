using Blackjack.Models;
using BlackjackStats.Shared.Requests;
using BlackjackStats.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blackjack.Services
{
    public class ApiGameService
    {
        private readonly HttpClient _client;

        public ApiGameService(HttpClient client) 
        {
            _client = client;
        }

        public async Task<GameResponse> SaveGameDeal()
        {
            var gameRequest = new GameRequest() { ID = 0, StartDateTime = System.DateTime.Now, Ip = GetIp() };
            var content = JsonSerializer.Serialize(gameRequest);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var saveGameResult = await _client.PostAsync($"{ApiServiceDataModel.ServiceURL}SaveGame", bodyContent);
            var saveGameContent = await saveGameResult.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<GameResponse>(saveGameContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }

        public async Task<GameResponse> SaveGameEnd()
        {
            var gameRequest = new GameRequest() { ID = ApiServiceDataModel.GameID, StartDateTime = ApiServiceDataModel.GameStartDateTime,
                                                    EndDateTime = System.DateTime.Now, Ip = GetIp() };
            var content = JsonSerializer.Serialize(gameRequest);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var saveGameResult = await _client.PostAsync($"{ApiServiceDataModel.ServiceURL}SaveGame", bodyContent);
            var saveGameContent = await saveGameResult.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<GameResponse>(saveGameContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return response;
        }
        private string GetIp()
        {
            string strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            string ip = addr[1].ToString();
            return ip;
        }
    }
}
