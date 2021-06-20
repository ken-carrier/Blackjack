using Blackjack.ViewModels;

namespace Blackjack.Views
{
    public partial class GameWindow
    {
        public GameWindow(GameViewModel gameViewModel)
        {
            InitializeComponent();
            DataContext = gameViewModel; 
        }
        
        
    }
}