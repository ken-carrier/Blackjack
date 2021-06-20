using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Blackjack.Models;
using Blackjack.Services;
using Blackjack.ViewModels;
using BlackjackStats.Shared.Requests;

namespace Blackjack.Commands
{
    public class SplitCommand : ICommand
    {
        private readonly GameViewModel _viewModel;
        private readonly ApiDealService _dealService;

        public SplitCommand(GameViewModel model, ApiDealService dealService)
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
            if (_viewModel.BetPlaced && _viewModel.DoubleCards && _viewModel.Player.CurrentImage < 3)
            {
                return true;
            }
            return false;
        }

        public void Execute(object parameter)
        {
            int score;
            // Divide by two is the value of one of the double cards
            // Or if we have aces then score == 11
            if (_viewModel.Player.Aces <= 0)
            {
                score = _viewModel.Player.Score/2;
            }
            else
            {
                score = 11;
                _viewModel.Player.SplitDeck.AcesLeft = 1;
                _viewModel.Player.SplitDeck.AcesRight = 1;
            }

            FinishViewModelAndSave(score);
        }

        private void FinishViewModelAndSave(int score)
        {
            _viewModel.Player.SplitDeck.ScoreLeft = score;
            _viewModel.Player.SplitDeck.ScoreRight = score;

            _viewModel.SetSplitDeck(_viewModel.Player, true);
            _viewModel.DisplayPointsSplit(_viewModel.Player);
            _viewModel.SplitDeck = true;
            _ = SaveDeal(score);

        }

        private async Task SaveDeal(int splitScore)
        {
            var response = await _dealService.SaveDeal(0, splitScore, DealAction.Split, PlayerEnum.Player, DealOutcome.Under21);
            ApiServiceDataModel.DealID = response.DealID;
        }
    }
}
