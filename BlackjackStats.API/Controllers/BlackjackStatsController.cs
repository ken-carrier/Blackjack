using BlackjackStats.Data;
using BlackjackStats.Data.Entities;
using BlackjackStats.Shared.Requests;
using BlackjackStats.Shared.Responses;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackjackStats.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlackjackStatsController : Controller
    {

        private BlackjackStatsContext _context { get; }
        public BlackjackStatsController(BlackjackStatsContext context)
        {
            _context = context;
        }

        [HttpPost("SaveGame")]
        public async Task<IActionResult> SaveGame(GameRequest gameRequest)
        {
            var game = new Game();

            try
            {
                if (gameRequest.ID == 0)
                {
                    game = AddNewGame(gameRequest);
                }
                else
                {
                    game = UpdateGame(gameRequest);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            return Ok(new GameResponse() { GameID = game.ID, StartDateTime = game.StartDateTime });
        }

        private Game UpdateGame(GameRequest gameRequest)
        {
            var game = new Game()
            {
                ID = gameRequest.ID,
                StartDateTime = gameRequest.StartDateTime,
                EndDateTime = gameRequest.EndDateTime,
                Ip = gameRequest.Ip,
            };

            _context.Games.Update(game);
            return game;

        }

        private Game AddNewGame(GameRequest gameRequest)
        {
            var game = new Game()
            {
                ID = 0,
                StartDateTime = gameRequest.StartDateTime,
                EndDateTime = System.DateTime.MinValue,
                Ip = gameRequest.Ip,
            };

            _context.Games.Add(game);
            return game;
        }

        [HttpPost("SaveDeal")]
        public async Task<IActionResult> SaveDeal(DealRequest dealRequest)
        {
            var deal = new Deal();
            try
            {
                deal = new Deal()
                {
                    ID = 0,
                    GameID = dealRequest.GameID,
                    Card = dealRequest.Card,
                    SplitScore = dealRequest.SplitScore,
                    DateTime = dealRequest.DateTime,
                    Player = dealRequest.Player == Shared.Requests.PlayerEnum.Player ? Data.Entities.PlayerEnum.Player : Data.Entities.PlayerEnum.Dealer,
                    Action = CobvertDealAction(dealRequest.Action),
                    Outcome = ConvertOutCome(dealRequest.Outcome)
                };
                _context.Deals.Add(deal);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }

            return Ok(new DealResponse() { DealID = deal.ID });
        }

        private Data.Entities.DealAction CobvertDealAction(Shared.Requests.DealAction action)
        {
            return action switch
            {
                Shared.Requests.DealAction.Deal => Data.Entities.DealAction.Deal,
                Shared.Requests.DealAction.Hit => Data.Entities.DealAction.Hit,
                Shared.Requests.DealAction.HitLeft => Data.Entities.DealAction.HitLeft,
                Shared.Requests.DealAction.HitRight => Data.Entities.DealAction.HitRight,
                Shared.Requests.DealAction.Split => Data.Entities.DealAction.Split,
                Shared.Requests.DealAction.Stand => Data.Entities.DealAction.Stand,
                Shared.Requests.DealAction.StandLeft => Data.Entities.DealAction.StandLeft,
                Shared.Requests.DealAction.StandRight => Data.Entities.DealAction.StandRight,
                _ => throw new NotImplementedException(),
            };
        }

        private Data.Entities.DealOutcome ConvertOutCome(Shared.Requests.DealOutcome outcome)
        {
            return outcome switch
            {
                Shared.Requests.DealOutcome.Dealer17Plus => Data.Entities.DealOutcome.Dealer17Plus,
                Shared.Requests.DealOutcome.Over21 => Data.Entities.DealOutcome.Over21,
                Shared.Requests.DealOutcome.Under21 => Data.Entities.DealOutcome.Under21,
                Shared.Requests.DealOutcome.At21 => Data.Entities.DealOutcome.At21,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
