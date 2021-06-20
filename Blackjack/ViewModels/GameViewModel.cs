using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Blackjack.Extensions;
using Blackjack.Models;
using Blackjack.Services;
using BlackjackStats.Shared.Requests;

namespace Blackjack.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        public Dictionary<int, BitmapImage[]> CardImages { get; private set; }
        public Player Player { get; set; }
        public Player Dealer { get; set; }

        public Grid PlayerImages { get; set; }

        public Grid DealerImages { get; set; }
        public TaskFactory TaskFactory { get; private set; }
        public string BetAmount { get; set; }
        public bool BetPlaced { get; set; }
        public bool DoubleCards { get; set; }
        public bool SplitDeck { get; set; }
        public Random Random { get; set; }

        public Image Card1Player { get; set; }
        public Image Card2Player { get; set; }

        public Image Card3Player { get; set; }
        public Image Card4Player { get; set; }
        public Image Card5Player { get; set; }
        public Image Card6Player { get; set; }

        public Image CardLeft1   { get; set; }
        public Image CardLeft2   { get; set; }
        public Image CardLeft3   { get; set; }
        public Image CardRight1  { get; set; }
        public Image CardRight2  { get; set; }
        public Image CardRight3 { get; set; }

        public Image Card1Dealer { get; set; }
        public Image Card2Dealer { get; set; }
        public Image Card3Dealer { get; set; }
        public Image Card4Dealer { get; set; }
        public Image Card5Dealer { get; set; }
        public Image Card6Dealer { get; set; }

        private Visibility _defaultVisibility;

        public Visibility DefaultVisibility
        {
            get => _defaultVisibility;
            set
            {
                _defaultVisibility = value;
                OnPropertyChanged("DefaultVisibility");
            }
          
        }

        private Visibility _splitVisibility;

        public Visibility SplitVisibility
        {
            get => _splitVisibility;
            set
            {
                _splitVisibility = value;
                OnPropertyChanged("SplitVisibility");
            }

        }

        Visibility _dealVisibility;
        public Visibility DealVisibility
        {
            get => _dealVisibility;
            set
            {
                _dealVisibility = value;
                OnPropertyChanged("DealVisibility");
            }
        }

        private string _result;
        public string Result {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged("Result");
            }
        }

        private int _dealerPoints;
        public int DealerPoints
        {
            get => _dealerPoints;
            set
            {
                _dealerPoints = value;
                OnPropertyChanged("DealerPoints");
            }
        }

        private string _playerPoints;
        public string PlayerPoints
        {
            get => _playerPoints;
            set
            {
                _playerPoints = value;
                OnPropertyChanged("PlayerPoints");
            }
        }
        int _playerMoney;
        public int PlayerMoney
        {
            get => _playerMoney;
            set
            {
                _playerMoney = value;
                OnPropertyChanged("PlayerMoney");
            }
        }

        int _dealerMoney;
        public int DealerMoney
        {
            get => _dealerMoney;
            set
            {
                _dealerMoney = value;
                OnPropertyChanged("DealerMoney");
            }
        }

        public GameCommands Commands { get; }

        private readonly ApiGameService _gameService;  // to update dealer game end
        private readonly ApiDealService _dealService;  // to update dealer deals

        public GameViewModel(GameCommands gameCommands, ApiGameService gameService, ApiDealService dealService)
        {
            DefaultVisibility = Visibility.Visible;
            SplitVisibility = Visibility.Hidden;
            Commands = gameCommands;
            Commands.BuildCommands(this);
            _gameService = gameService;
            _dealService = dealService;
        }

        public void SetProperties() 
        {
            Random = new Random();
            Player = new Player("Player", 1000, ImagesExtension.CreateImage("player"), 2);
            Player.CreateSplitDeck();
            AddSplitDeckCards(Player);
            Dealer = new Player("Dealer", 1000, ImagesExtension.CreateImage("dealer"), 2);
            DisplayMoney(Player, Dealer);
            
            CardImages = ImagesExtension.GetBlackJackCards();
            AddCards(Player, Dealer);
            Player.ShowBackside();
            Dealer.ShowBackside();
            BetAmount = "100";
        }

        private void AddCards(Player player, Player dealer)
        {
            AddImages(player, PlayerImages);
            AddImages(dealer, DealerImages);
        }

        private void AddImages(Player player, Grid grid)
        {
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(grid); i++)
            {
                var child = VisualTreeHelper.GetChild(grid, i);
                player.Images.Add((Image)child);
            }
        }

        private void AddSplitDeckCards(Player player)
        {
            player.SplitDeck.ImagesLeft.AddRange(new List<Image>() { Card3Player, Card5Player, CardLeft1, CardLeft2, CardLeft3 });
            player.SplitDeck.ImagesRight.AddRange(new List<Image>() { Card4Player, Card6Player, CardRight1, CardRight2, CardRight3 });
        }

        public int[] DealCards()
        {
            ResetBoard();

            int cardOne = Random.Next(2, 12); 
            int cardTwo = Random.Next(2, 12); 
            int[] initialCards = { cardOne, cardTwo };
            DealPlayerCards(cardOne, cardTwo);


            if (cardOne == cardTwo)
            {
                DoubleCards = true;
            }

            if (Player.Score > 21)
            {
                GameExtension.HasAces(Player);
                DisplayPoints(Player);
            }
            return initialCards;
        }

        private void ResetBoard()
        {
            // Reset the board
            GameExtension.ResetImages(Player, Dealer);
            GameExtension.ResetGame(Player, Dealer);
            ResetResult();
            SetSplitDeck(Player, false);
            BetPlaced = true;

        }

        public void ResetResult()
        {
            Result = string.Empty;
            DealerPoints = 0;
            PlayerPoints = "0";
            ResetSplitCards();
        }

        private void ResetSplitCards()
        {
            CardRight1.Source = null;
            CardRight2.Source = null;
            CardRight3.Source = null;
            CardLeft1.Source = null;
            CardLeft2.Source = null;
            CardLeft3.Source = null;
        }

        private void DealPlayerCards(int cardOne, int cardTwo)
        {
            GameExtension.AddAces(Player, cardOne);
            GameExtension.AddAces(Player, cardTwo);

            Player.Images[0].Source = ImagesExtension.RandomColorCard(CardImages.First(x => x.Key == cardOne).Value);
            Player.Images[1].Source = ImagesExtension.RandomColorCard(CardImages.First(x => x.Key == cardTwo).Value);
            Player.Score = cardOne + cardTwo;
            DisplayPoints(Player);

        }
        public void SetSplitDeck(Player player, bool activate)
        {
            if (activate)
            {
                DefaultVisibility = Visibility.Hidden;
                SplitVisibility = Visibility.Visible;
                return;
            }
                
            DefaultVisibility = Visibility.Visible;
            SplitVisibility = Visibility.Hidden;
            
        }

        public void DisplayPoints(Player player)
        {
            if (player.Name == "Dealer")
            {
                DealerPoints = player.Score;
            }
            else
            {
                PlayerPoints = player.Score.ToString();
            }
        }

       
        public int HitCard()
        {
            int card = Random.Next(2, 12);
            GameExtension.AddAces(Player, card);

            Player.Images[Player.CurrentImage].Source = ImagesExtension.RandomColorCard(CardImages.First(x => x.Key == card).Value);
            Player.CurrentImage++;
            Player.Score += card;
            
            if (Player.Score > 21 && !GameExtension.HasAces(Player))
            {
                BetPlaced = false;
                End(false);  
            }
            DisplayPoints(Player);
            return card;
        }
        public int HitCardSplit(bool leftDeck)
        {
            int card = Random.Next(2, 12);
            GameExtension.AddAcesSplit(Player, card, leftDeck);

            if (leftDeck)
            {
                HitLeftDeck(card);
            }
            else
            {
                HitRightDeck(card);
            }
            DisplayPointsSplit(Player);
            return card;
        }

        private void HitLeftDeck(int card)
        {
            Player.SplitDeck.ImagesLeft[Player.SplitDeck.CurrentImageLeft].Source =
                  ImagesExtension.RandomColorCard(CardImages.First(x => x.Key == card).Value);
            Player.SplitDeck.CurrentImageLeft++;
            Player.SplitDeck.ScoreLeft += card;

            if (Player.SplitDeck.ScoreLeft > 21 && !GameExtension.HasAcesSplit(Player, true))
            {
                Player.SplitDeck.FinishedLeft = true;
                if (Player.SplitDeck.FinishedRight)
                {
                    Stand(true);
                }
            }

        }

        private void HitRightDeck(int card)
        {
            Player.SplitDeck.ImagesRight[Player.SplitDeck.CurrentImageRight].Source =
                ImagesExtension.RandomColorCard(CardImages.First(x => x.Key == card).Value);
            Player.SplitDeck.CurrentImageRight++;
            Player.SplitDeck.ScoreRight += card;

            if (Player.SplitDeck.ScoreRight > 21 && !GameExtension.HasAcesSplit(Player, false))
            {
                Player.SplitDeck.FinishedRight = true;
                if (Player.SplitDeck.FinishedLeft)
                {
                    Stand(true);
                }
            }
        }
        public void DisplayPointsSplit(Player player)
        {
            PlayerPoints = player.SplitDeck.ScoreLeft + " : " + player.SplitDeck.ScoreRight;
        }

        public void Stand(bool isSplit)
        {
            SetDealerPlayerStand();

            if (Dealer.Score < 17 || (Dealer.Score > 21 && GameExtension.HasAces(Dealer)))
            {
                TaskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
                new Thread(() => HitCardDealer(isSplit)).Start();
            }
            else
            {
                End(isSplit);
            }
            BetPlaced = false;
       }

        private void SetDealerPlayerStand()
        {
            int cardOne = Random.Next(2, 12);
            int cardTwo = Random.Next(2, 12);
            int[] initialCards = new int[] {cardOne, cardTwo};
            
            GameExtension.AddAces(Dealer, cardOne);
            GameExtension.AddAces(Dealer, cardTwo);

            Dealer.Images[0].Source =
                ImagesExtension.RandomColorCard(CardImages.First(x => x.Key == cardOne).Value);
            Dealer.Images[1].Source =
                ImagesExtension.RandomColorCard(CardImages.First(x => x.Key == cardTwo).Value);

            Dealer.Score = cardOne + cardTwo;
            DisplayPoints(Dealer);
            _ = SaveDealerDeal(initialCards);
        }

        private async Task SaveDealerDeal(int[] cards)
        {
            var outcome = GameExtension.GetDealOutcome(Dealer);
            if (Dealer.Score >= 17 && outcome == DealOutcome.Under21)
            {
                outcome = DealOutcome.Dealer17Plus;
            }
            var response = await _dealService.SaveDeal(cards[0], 0, DealAction.Deal, PlayerEnum.Dealer, outcome);
            ApiServiceDataModel.DealID = response.DealID;
            
            response = await _dealService.SaveDeal(cards[1], 0, DealAction.Deal, PlayerEnum.Dealer, outcome);
            ApiServiceDataModel.DealID = response.DealID;

        }

        public void HitCardDealer(bool isSplit)
        {
            TaskFactory.StartNew(() => DealButton(false));
            while (Dealer.Score < 17 && Dealer.CurrentImage < Dealer.Images.Count())
            {
                HitCardDealer();
            }
            TaskFactory.StartNew(() => End(isSplit));
            TaskFactory.StartNew(() => DealButton(true));
            Thread.Yield();
        }
        public void HitCardDealer()
        {
            // Pretend to be thinking
            Thread.Sleep(1000);

            int card = Random.Next(2, 12);
            GameExtension.AddAces(Dealer, card);

            Dealer.Score += card;
            TaskFactory.StartNew(() => DisplayCard(card));

            if (Dealer.Score > 21)
            {
                GameExtension.HasAces(Dealer);
            }
            _ = SaveDealerHit(card);
        }

        private async Task SaveDealerHit(int card)
        {
            var outcome = GameExtension.GetDealOutcome(Dealer);
            if (Dealer.Score >= 17 && outcome == DealOutcome.Under21)
            {
                outcome = DealOutcome.Dealer17Plus;
            }

            var response = await _dealService.SaveDeal(card,0,DealAction.Hit,PlayerEnum.Dealer,outcome);
            ApiServiceDataModel.DealID = response.DealID;
        }

        private async Task SaveGameEnd()
        {
            await _gameService.SaveGameEnd();
            // new Game
            ApiServiceDataModel.GameID = 0;
            ApiServiceDataModel.GameStartDateTime = System.DateTime.MinValue;
        }

        private void DisplayCard(int card)
        {
            Dealer.Images[Dealer.CurrentImage].Source =
                ImagesExtension.RandomColorCard(CardImages.First(x => x.Key == card).Value);
            Dealer.CurrentImage++;
            DisplayPoints(Dealer);
        }

        private void End(bool isSplit)
        {
            if (isSplit)
            {
                EndGameSplit(Player, Dealer, Convert.ToInt16(BetAmount));
            }
            else
            {
                EndGame(Player, Dealer, Convert.ToInt16(BetAmount));
            }

            DisplayMoney(Player, Dealer);
            
            _ = SaveGameEnd();
        }

        public void EndGame(Player one, Player two, int bet)
        {
            Player winner = GameExtension.CalculateWinner(one, two);
            if (winner == null)
            {
                ShowResult("Draw!");
            }
            else
            {
                ShowResult(winner.Name + " won the game!");
                SetWinner(winner, one, two, bet);
            }
        }

        public void EndGameSplit(Player one, Player two, int bet)
        {
            Player winner = GameExtension.CalculateWinnerSplit(one, two);
            if (winner == null)
            {
                ShowResult("Draw!");
            }
            else
            {
                ShowResult(winner.Name + " won the game!");
                // Multiply by two because we doubled our wins / loses
                SetWinner(winner,one,two,bet);
            }


        }

        private void SetWinner(Player winner, Player one, Player two, int bet)
        {
            if (winner == one)
            {
                one.Money += bet * 2;
                two.Money -= bet * 2;
            }
            else
            {
                two.Money += bet * 2;
                one.Money -= bet * 2;
            }
        }

            
        public void DisplayMoney(Player one, Player two)
        {
            PlayerMoney = one.Money;
            DealerMoney = two.Money;
        }

        
        public void ShowResult(string result)
        {
            Result = result;
        }

        public void DealButton(bool show)
        {
            DealVisibility = show ? Visibility.Visible : Visibility.Hidden;
        }

    }
}