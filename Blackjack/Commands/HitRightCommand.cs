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
    public class HitRightCommand : ICommand
    {
        private readonly GameViewModel _viewModel;
        private readonly ApiDealService _dealService;

        public HitRightCommand(GameViewModel viewModel, ApiDealService dealService)
        {
            _viewModel = viewModel;
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
           var card = _viewModel.HitCardSplit(false);

            SplitDeck splitDeck = _viewModel.Player.SplitDeck;

            if (splitDeck.CurrentImageRight >= splitDeck.ImagesRight.Count)
            {
                SplitDeck(splitDeck, card);
            }
            
            _ = SaveDeal(card);
        }

        private void SplitDeck(SplitDeck splitDeck, int card)
        {
            splitDeck.FinishedRight = true;

            if (splitDeck.FinishedLeft)
            {
                _viewModel.Stand(true);
            }
            
        }

        private async Task SaveDeal(int card)
        {
            var outcome = GameExtension.GetRightDealOutcome(_viewModel.Player);

            var response = await _dealService.SaveDeal(card, 0, DealAction.HitRight, PlayerEnum.Player, outcome);
            ApiServiceDataModel.DealID = response.DealID;
        }
    }
}
