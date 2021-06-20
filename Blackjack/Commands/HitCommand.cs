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
    public class HitCommand : ICommand
    {
        private readonly GameViewModel _viewModel;
        private readonly ApiDealService _dealService;
        public HitCommand(GameViewModel model, ApiDealService apiDealService )
        {
            _viewModel = model;
            _dealService = apiDealService;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (_viewModel.Player.CurrentImage >= _viewModel.Player.Images.Count)
            {
                _viewModel.Stand(false);
                return false;
            }
            else if (_viewModel.BetPlaced)
            {
                return true;
            }
           
            return false;
        }

        public void Execute(object parameter)
        {
           var card = _viewModel.HitCard();
            _ = SaveDeal(card);
        }

        private async Task SaveDeal(int card)
        {
            var outcome = GameExtension.GetDealOutcome(_viewModel.Player);
            var response = await _dealService.SaveDeal(card, 0, DealAction.Hit, PlayerEnum.Player, outcome);
            ApiServiceDataModel.DealID = response.DealID;
        }
    }
}