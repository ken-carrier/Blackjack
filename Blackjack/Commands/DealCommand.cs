using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Blackjack.Extensions;
using Blackjack.Models;
using Blackjack.Services;
using Blackjack.ViewModels;
using BlackjackStats.Shared.Requests;

namespace Blackjack.Commands
{
    public class DealCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        private readonly GameViewModel _viewModel; 
        private readonly ApiGameService _gameService;
        private readonly ApiDealService _dealService;
        public DealCommand(GameViewModel viewModel, ApiGameService gameService, ApiDealService dealService)
        {
            _viewModel = viewModel;
            _gameService = gameService;
            _dealService = dealService;
        }

        public bool CanExecute(object parameter)
        {
            if (_viewModel.BetPlaced)
                return false;

            return ValidateBetAmount();


        }
        private bool ValidateBetAmount()
        {
            int bet;
            if (int.TryParse(_viewModel.BetAmount, out bet))
            {
                if (bet is >= 100 and <= 500)
                {
                    return true;
                }
            }
            return false;
        }
        public void Execute(object parameter)
        {
           var cards = _viewModel.DealCards();
            _ = SaveDealGame(cards);
        }
        private async Task SaveDealGame(int[] cards)
        {
            var response =  await _gameService.SaveGameDeal();
            ApiServiceDataModel.GameID = response.GameID;
            ApiServiceDataModel.GameStartDateTime = response.StartDateTime;
            var outcome = GameExtension.GetDealOutcome(_viewModel.Player);
            var dealResponse = await _dealService.SaveDeal(cards[0], 0, DealAction.Deal, PlayerEnum.Player, outcome);
            ApiServiceDataModel.DealID = dealResponse.DealID;
            dealResponse = await _dealService.SaveDeal(cards[1], 0, DealAction.Deal, PlayerEnum.Player, outcome);
            ApiServiceDataModel.DealID = dealResponse.DealID;

        }


    }
}