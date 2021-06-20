using System.Windows.Input;
using Blackjack.Commands;
using Blackjack.Services;
using Blackjack.ViewModels;

namespace Blackjack.Models
{
    public class GameCommands
    {
        private ApiGameService _apiGameService { get; }
        private ApiDealService _apiDealService { get; }

        public ICommand DealCommand { get; private set; }
        public ICommand HitCommand { get; private set; }
        public ICommand StandCommand { get; private set; }
        public ICommand SplitCommand { get; private set; }
        public ICommand RulesCommand { get; private set; }
        public ICommand HitLeftCommand { get; private set; }
        public ICommand StandLeftCommand { get; private set; }
        public ICommand HitRightCommand { get; private set; }
        public ICommand StandRightCommand { get; private set; }

        public GameCommands(ApiGameService apiGameService, ApiDealService apiDealService)
        {
            _apiGameService = apiGameService;
            _apiDealService = apiDealService;
        }

        public void BuildCommands(GameViewModel viewModel)
        {
            DealCommand = new DealCommand(viewModel, _apiGameService, _apiDealService);
            HitCommand = new HitCommand(viewModel, _apiDealService);
            StandCommand = new StandCommand(viewModel, _apiDealService);
            SplitCommand = new SplitCommand(viewModel, _apiDealService);
            HitLeftCommand = new HitLeftCommand(viewModel, _apiDealService);
            StandLeftCommand = new StandLeftCommand(viewModel, _apiDealService);
            HitRightCommand = new HitRightCommand(viewModel, _apiDealService);
            StandRightCommand = new StandRightCommand(viewModel, _apiDealService);
        }
    }
}
