using System.Linq;
using System.Windows;

namespace ThuHaLe_COMP212_Sec001
{
    public partial class MainWindow : Window
    {
        private BaseballEntities1 db = new BaseballEntities1();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void DisplayAll_Click(object sender, RoutedEventArgs e)
        {
            PlayersDataGrid.ItemsSource = db.Players.ToList();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            // Check if the user entered an integer in the FirstNameTextBox
            if (!string.IsNullOrEmpty(FirstNameTextBox.Text))
            {
                if (int.TryParse(FirstNameTextBox.Text, out _))
                {
                    MessageBox.Show("Please enter a valid name (string) in the First Name field!");
                    return;
                }
                else
                {
                    PlayersDataGrid.ItemsSource = db.Players
                        .Where(p => p.FirstName.Contains(FirstNameTextBox.Text)).ToList();
                }
            }
            // Check if the user entered a Player ID in the PlayerIDTextBox
            else if (!string.IsNullOrEmpty(PlayerIDTextBox.Text))
            {
                if (int.TryParse(PlayerIDTextBox.Text, out int playerId))
                {
                    PlayersDataGrid.ItemsSource = db.Players
                        .Where(p => p.PlayerID == playerId).ToList();
                }
                else
                {
                    MessageBox.Show("Please enter a valid Player ID!");
                }
            }
        }

        private void DisplayBattingAverage_Click(object sender, RoutedEventArgs e)
        {
            var battingAverage = db.Players.Average(p => p.BattingAverage);
            DisplayBattingAverageBox.Text = $"Batting average of all the players\nBatting Average = {battingAverage:F3}";
        }

        private void HighestBattingScore_Click(object sender, RoutedEventArgs e)
        {
            // Find the player with the highest batting score
            var playerWithHighestScore = db.Players.OrderByDescending(p => p.BattingAverage).FirstOrDefault();

            if (playerWithHighestScore != null)
            {
                // Display the player's name and batting average
                HighestBattingScoreBox.Text = $"Player with Highest Batting Average is:\nPlayer Name: {playerWithHighestScore.FirstName} {playerWithHighestScore.LastName}\nBatting Average = {playerWithHighestScore.BattingAverage}";
            }
            else
            {
                // Handle the case where there are no players in the database
                HighestBattingScoreBox.Text = "No players found.";
            }
        }

        private void AllPlayersBattingAverage_Click(object sender, RoutedEventArgs e)
        {
            var allAverages = db.Players.Select(p => new
            {
                p.FirstName,
                p.LastName,
                p.BattingAverage
            }).ToList();

            AllPlayersBattingAverageBox.Clear();
            AllPlayersBattingAverageBox.AppendText("List of all Players and Batting Average:");
            foreach (var player in allAverages)
            {
                AllPlayersBattingAverageBox.AppendText(
                    $"\nPlayer Name:\n\t{player.FirstName} {player.LastName}\nBatting Average:\n\t{player.BattingAverage:F3}\n");
            }
        }

        private void ClearSearch_Click(object sender, RoutedEventArgs e)
        {
            FirstNameTextBox.Clear();
            PlayerIDTextBox.Clear();
            PlayersDataGrid.ItemsSource = null;
            DisplayBattingAverageBox.Clear();
            HighestBattingScoreBox.Clear();
            AllPlayersBattingAverageBox.Clear();
        }
    }
}

