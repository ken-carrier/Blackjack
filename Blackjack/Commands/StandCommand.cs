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
    public class StandCommand : ICommand
    {
        private readonly GameViewModel _viewModel;
        private readonly ApiDealService _dealService;

        public StandCommand(GameViewModel model, ApiDealService dealService)
        {
            _viewModel = model;
            _dealService = dealService;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (_viewModel.BetPlaced)
            {
                return true;
            }
            return false;
        }

        public void Execute(object parameter)
        {
            _viewModel.Stand(false);
            _ = SaveDeal();
        }
        private async Task SaveDeal()
        {
            var outcome = GameExtension.GetDealOutcome(_viewModel.Player);
            var response = await _dealService.SaveDeal(0, 0, DealAction.Stand, PlayerEnum.Player, outcome);
            ApiServiceDataModel.DealID = response.DealID;
        }
    }
}