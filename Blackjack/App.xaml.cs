using Blackjack.Commands;
using Blackjack.Views;
using Blackjack.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;
using Blackjack.Models;
using Blackjack.Services;

namespace Blackjack
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var gameWindow = serviceProvider.GetService<GameWindow>();
            SetVMControls(gameWindow);
            
            gameWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<GameWindow>();
            services.AddScoped<GameViewModel>();
            services.AddHttpClient();
            services.AddScoped<GameCommands>();
            services.AddScoped<ApiGameService>();
            services.AddScoped<ApiDealService>();
        }

        private void SetVMControls(GameWindow gameWindow)
        {
            GameViewModel gameViewModel = (GameViewModel) MainWindow.DataContext;
            SetDealerImages(gameViewModel, gameWindow);
            SetPlayerImages(gameViewModel, gameWindow);
            SetSplitImages(gameViewModel, gameWindow);
            gameViewModel.SetProperties();
        }

        private void SetDealerImages(GameViewModel gameViewModel, GameWindow gameWindow)
        {
            gameViewModel.DealerImages = gameWindow.DealerImages;
            gameViewModel.Card1Dealer = gameWindow.Card1Dealer;
            gameViewModel.Card2Dealer = gameWindow.Card2Dealer;
            gameViewModel.Card3Dealer = gameWindow.Card3Dealer;
            gameViewModel.Card4Dealer = gameWindow.Card4Dealer;
            gameViewModel.Card5Dealer = gameWindow.Card5Dealer;
            gameViewModel.Card6Dealer = gameWindow.Card6Dealer;


        }
        private void SetPlayerImages(GameViewModel gameViewModel, GameWindow gameWindow)
        {
            gameViewModel.PlayerImages = gameWindow.PlayerImages;
            gameViewModel.Card1Player = gameWindow.Card1Player;
            gameViewModel.Card2Player = gameWindow.Card2Player;
            gameViewModel.Card3Player = gameWindow.Card3Player;
            gameViewModel.Card4Player = gameWindow.Card4Player;
            gameViewModel.Card5Player = gameWindow.Card5Player;
            gameViewModel.Card6Player = gameWindow.Card6Player;

        }

        private void SetSplitImages(GameViewModel gameViewModel, GameWindow gameWindow)
        {
            gameViewModel.CardLeft1 = gameWindow.CardLeft1;
            gameViewModel.CardLeft2 = gameWindow.CardLeft2;
            gameViewModel.CardLeft3 = gameWindow.CardLeft3;
            gameViewModel.CardRight1 = gameWindow.CardRight1;
            gameViewModel.CardRight2 = gameWindow.CardRight2;
            gameViewModel.CardRight3 = gameWindow.CardRight3;
        }
       
    }
}
;
