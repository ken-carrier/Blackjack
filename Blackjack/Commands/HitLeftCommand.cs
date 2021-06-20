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
    public class HitLeftCommand : ICommand
    {
        private readonly GameViewModel _viewModel;
        private readonly ApiDealService _dealService;

        public HitLeftCommand(GameViewModel viewModel, ApiDealService dealService)
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
            if (_viewModel.Player.SplitDeck.FinishedLeft)
            {
                return false;
            }
            return true;
        }

        public void Execute(object parameter)
        {
            int card =_viewModel.HitCardSplit(true);
            SplitDeck splitDeck = _viewModel.Player.SplitDeck;

            if (splitDeck.CurrentImageLeft >= splitDeck.ImagesLeft.Count)
            {
                SplitDeck(splitDeck, card);
            }
            _ = SaveDeal(card);

        }

        private void SplitDeck(SplitDeck splitDeck, int card)
        {
            splitDeck.FinishedLeft = true;

            if (splitDeck.FinishedRight)
            {
                _viewModel.Stand(true);
            }
        }

        private async Task SaveDeal(int card)
        {
            var outcome = GameExtension.GetLeftDealOutcome(_viewModel.Player);

            var response = await _dealService.SaveDeal(card, 0,DealAction.HitLeft, PlayerEnum.Player, outcome);
            ApiServiceDataModel.DealID = response.DealID;
        }
    }
}
