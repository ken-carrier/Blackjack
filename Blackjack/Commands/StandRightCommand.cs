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
    public class StandRightCommand : ICommand
    {
        private readonly GameViewModel _viewModel;
        private readonly ApiDealService _dealService;

        public StandRightCommand(GameViewModel viewModel, ApiDealService dealService)
        {
            this._viewModel = viewModel;
            _dealService = dealService;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (_viewModel.Player.SplitDeck.FinishedRight)
            {
                return false;
            }
            return true;
        }

        public void Execute(object parameter)
        {
            _viewModel.Player.SplitDeck.FinishedRight = true;
            if (_viewModel.Player.SplitDeck.FinishedLeft)
            {
                _viewModel.Stand(true);
            }
            _ = SaveDeal();

        }
        private async Task SaveDeal()
        {
            var outcome = GameExtension.GetRightDealOutcome(_viewModel.Player);

            var response = await _dealService.SaveDeal(0, 0, DealAction.StandRight, PlayerEnum.Player, outcome);
            ApiServiceDataModel.DealID = response.DealID;
        }
    }
}

