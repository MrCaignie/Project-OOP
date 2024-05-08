using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project_Judo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Methode voor het bijwerken van de score
        public void UpdateScore(int whiteScore, int blueScore)
        {
            // Voorbeeld van het bijwerken van de score
            TextBlock whiteScoreTextBlock = FindName("WhiteScoreTextBlock") as TextBlock;
            if (whiteScoreTextBlock != null)
            {
                whiteScoreTextBlock.Text = $"Score: {whiteScore}";
            }

            TextBlock blueScoreTextBlock = FindName("BlueScoreTextBlock") as TextBlock;
            if (blueScoreTextBlock != null)
            {
                blueScoreTextBlock.Text = $"Score: {blueScore}";
            }
        }

        // Methode voor het bijwerken van de shido
        public void UpdateShido(int whiteShido, int blueShido)
        {
            // Voorbeeld van het bijwerken van de shido
            TextBlock whiteShidoTextBlock = FindName("WhiteShidoTextBlock") as TextBlock;
            if (whiteShidoTextBlock != null)
            {
                whiteShidoTextBlock.Text = $"Shido: {whiteShido}";
            }

            TextBlock blueShidoTextBlock = FindName("BlueShidoTextBlock") as TextBlock;
            if (blueShidoTextBlock != null)
            {
                blueShidoTextBlock.Text = $"Shido: {blueShido}";
            }
        }

        // Methode voor het bijwerken van de tijd
        public void UpdateTimer(string time)
        {
            // Voorbeeld van het bijwerken van de timer
            TextBlock timerTextBlock = FindName("TimerTextBlock") as TextBlock;
            if (timerTextBlock != null)
            {
                timerTextBlock.Text = time;
            }
        }

        // Event wanneer het venster wordt geladen
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialiseer initiële gegevens of voeg event handlers toe
        }
    }
}