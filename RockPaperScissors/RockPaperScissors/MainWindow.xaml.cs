using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RockPaperScissors
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool AIGame;
        GameState RPS;
        public MainWindow()
        {
            InitializeComponent();
            
             RPS = new GameState();
            RPS.getWindow(this);
            
        }
        //Multi Player
        private void BeginButton_Click(object sender, RoutedEventArgs e)
        {
            AIGame = false;
            RPS.initialState();
            
        }
        //Single Player
        private void BeginAIButton_Click(object sender, RoutedEventArgs e)
        {
            AIGame = true;
            RPS.initialState();
        }
        public void TextChanger(TextBlock e, string s)
        {
            e.Text = s;
        }
        public void window_KeyPressed(object sender, KeyEventArgs e)
        {
            if(RPS.Player1Ready == true || RPS.Player2Ready == true) //Only processes inputs during correct ssection of the game
            {
                RPS.PlayerMove(sender, e);
            }
        }

    }
    public class GameState
    {
        
        private int Player1Choice;
        private int Player2Choice;
        public int PaperCount;
        public int RockCount;
        public int ScissorsCount;
        public int PreviousTurncount = 0;
        public int TurnCount;
        private int Winner;
        public string MostUsed;
        public bool Player1Ready;
        public bool Player2Ready;
        public MainWindow CurrentWindow; 
        Random random; 
        //Grabs the current window in order to access textblocks
        public void getWindow(MainWindow Window)
        {
            CurrentWindow = Window;
        }
        //Initialises the game for play.
        public void initialState()
        {
            PlayerChoiceSetter(1, 0);
            PlayerChoiceSetter(2, 0);
            Player1Ready = true;
            if (CurrentWindow.AIGame == false) Player2Ready = true;
            else { random = new Random(); }
            PreviousTurncount = TurnCount;
            TurnCount = 0;
            PaperCount = 0;
            RockCount = 0;
            ScissorsCount = 0;
            CurrentWindow.TextChanger(CurrentWindow.Turncount, TurnCount.ToString());
            CurrentWindow.TextChanger(CurrentWindow.Game_Over, "");
            CurrentWindow.TextChanger(CurrentWindow.Player1ReadyBox, "Player 1 has NOT picked");
            CurrentWindow.TextChanger(CurrentWindow.Player2ReadyBox, "Player 2 has NOT picked");
        }
         
        //Checks the usage of the three moves and sets the most used to the highest
        public void getHighestUsage()
        {
            if(RockCount > PaperCount)
            {
                if(ScissorsCount > RockCount)
                {
                     MostUsed = "Scissors";
                }
                MostUsed = "Rock";
            }
            else
            {
                if(ScissorsCount > PaperCount)
                {
                    MostUsed = "Scissors";
                }

                MostUsed = "Paper";
            }
            CurrentWindow.TextChanger(CurrentWindow.MostUsed, MostUsed);   
        }
        //Game end procedure, Sets the winner and turncounts.
        public void setWinner(int gameOutcome)
        {
            Winner = gameOutcome;
            CurrentWindow.TextChanger(CurrentWindow.PreviousWinner, "Player " + Winner.ToString());
            CurrentWindow.TextChanger(CurrentWindow.Turncount_Prev, TurnCount.ToString());
            CurrentWindow.TextChanger(CurrentWindow.Game_Over, "G A M E \nO V E R");
        }
        //Forcibly changes the player choice. Used during the game setup
        public void PlayerChoiceSetter(int playerNum, int move)
        {
            if (playerNum == 1)
            {
                Player1Choice = move;
            }
            else
            {
                Player2Choice = move;
            }
        }
        public int Player1ChoiceGet()
        {
            return Player1Choice;
        }
        public int Player2ChoiceGet()
        {
            return Player2Choice;
        }
        public void PlayerMove(object sender, KeyEventArgs e) // 1 is rock, 2 is paper, 3 is scissors
        {
            switch (e.Key)
            {
                //Player 1 Inputs
                case Key.Q:
                    if (Player1Ready == true)
                    {
                        PlayerChoiceSetter(1, 1);
                        Player1Ready = false;
                        RockCount++;
                    }
                    break;
                case Key.W:
                    if (Player1Ready == true)
                    {
                        PlayerChoiceSetter(1, 2);
                        Player1Ready = false;
                        PaperCount++;
                    }
                    break;
                case Key.E:
                    if (Player1Ready == true)
                    {
                        PlayerChoiceSetter(1, 3);
                        Player1Ready = false;
                        ScissorsCount++;
                    }
                    break;
                    //Player 2 Inputs
                case Key.I:
                    if (Player2Ready == true)
                    {
                        PlayerChoiceSetter(2, 1);
                        Player2Ready = false;
                        RockCount++;
                    }
                    break;
                case Key.O:
                    if (Player2Ready == true)
                    {
                        PlayerChoiceSetter(2, 2);
                        Player2Ready = false;
                        PaperCount++;
                    }
                    break;
                case Key.P:
                    if (Player2Ready == true)
                    {
                        PlayerChoiceSetter(2, 3);
                        Player2Ready = false;
                        ScissorsCount++;
                    }
                    break;
            }
            //Below prevents switching. All answers given are final, just like in the real game. A notification is given to ensure players are aware they've picked.
            if(Player1Ready == false)
            {
                CurrentWindow.TextChanger(CurrentWindow.Player1ReadyBox, "Player 1 has picked");
            }
            
            if (Player2Ready == false)
            {
                CurrentWindow.TextChanger(CurrentWindow.Player2ReadyBox, "Player 2 has picked");
            }

            if(Player1Ready == false && Player2Ready == false)
            {
                gameLogic();
            }
            
        }

        private void gameLogic() // 1 is player 1 win, 2 is player 2 win, 3 is draw;
        {
            
            int p1 = Player1ChoiceGet();
            int p2;
           if(CurrentWindow.AIGame == false) p2 = Player2ChoiceGet();
            else { p2 = COMPlayer(); } //If there is no player 2 this is where the COM places its choice
            int Outcome = 0;
            //Outcome table. Rock = 1, Paper = 2, Scissors = 3
            switch (p1)
            {
                case 1:
                    if(p2 == 1)
                    {
                        Outcome = 3;
                    }
                    if(p2 == 2)
                    {
                        Outcome = 2;
                    }
                    if(p2 == 3)
                    {
                        Outcome = 1;
                    }
                    break;
                case 2:
                    if (p2 == 1)
                    {
                        Outcome = 1;
                    }
                    if (p2 == 2)
                    {
                        Outcome = 3;
                    }
                    if (p2 == 3)
                    {
                        Outcome = 2;
                    }
                    break;
                    case 3:
                    if(p2 == 1)
                    {
                        Outcome = 2;
                    }
                    if(p2 == 2)
                    {
                        Outcome = 1;
                    }
                    if(p2 == 3)
                    {
                        Outcome = 3;
                    }
                    break;
            }
            TurnCount++;
            if (Outcome == 1 || Outcome == 2)
            {
                setWinner(Outcome);
                getHighestUsage();
            }
            else
            {
                // If there is a draw, it throws the game back to the selection section
                Player1Ready = true;
                CurrentWindow.TextChanger(CurrentWindow.Player1ReadyBox, "Player 1 has NOT picked");
                if (CurrentWindow.AIGame == false)
                {
                    Player2Ready = true;
                    CurrentWindow.TextChanger(CurrentWindow.Player2ReadyBox, "Player 2 has NOT picked");
                }
                CurrentWindow.TextChanger(CurrentWindow.Turncount, TurnCount.ToString());
                CurrentWindow.TextChanger(CurrentWindow.Game_Over, "DRAW! \nPick again!");

            }
        }
        //Very simple random number gen between 1 and 3 which corrosponds to Rock, paper and scissors respectively. 
        private int COMPlayer()
        {       
                return random.Next(1, 4);
        }
       
            
    }

    
}
