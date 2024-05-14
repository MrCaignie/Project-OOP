using Newtonsoft.Json;
using Project_Judo;
using System;
using System.IO;
using System.Reflection.Emit;
using System.Windows; 
using System.Windows.Input; 
using System.Windows.Threading;

// git hub:
// https://github.com/MrCaignie/Project-OOP
//
namespace JudoScoreboard
{
    public partial class MainWindow : Window
    {
        // Variabelen voor bijhouden van scores en straffen van witte en blauwe judoka's
        private string whiteScore = "0";
        private string blueScore = "0";
        private string whiteFout = "0";
        private string blueFout = "0";

        // Variabelen voor bijhouden van timerstatus en tijd
        private bool timerRunning = false;
        private DispatcherTimer dispatcherTimer;
        private TimeSpan elapsedTime;
        private bool countingUp = false; // Vlag om aan te geven of de timer oploopt

        // Timer voor 20 seconden
        private DispatcherTimer twentySecondsTimer;
        private TimeSpan twentySecondsDuration = TimeSpan.FromSeconds(20);

        Mens persoon;

        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();
            InitializeTwentySecondsTimer();

        }

        // Initialiseer de hoofdtimer
        private void InitializeTimer()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += Timer_Tick;
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1); // Timer elke seconde laten tikken
        }

        // Initialiseer de timer voor 20 seconden
        private void InitializeTwentySecondsTimer()
        {
            twentySecondsTimer = new DispatcherTimer();
            twentySecondsTimer.Tick += TwentySecondsTimer_Tick;
            twentySecondsTimer.Interval = TimeSpan.FromSeconds(1); // Timer elke seconde laten tikken
        }

        // Start de hoofdtimer
        private void StartTimer()
        {
            // Stel de verstreken tijd in op 4 minuten (240 seconden)
            elapsedTime = TimeSpan.FromSeconds(240);
            dispatcherTimer.Start();
            timerRunning = true;
        }

        // Stop de hoofdtimer
        private void StopTimer()
        {
            dispatcherTimer.Stop();
            timerRunning = false;
        }

        // Event handler voor het tick-event van de hoofdtimer
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Logica voor het bijwerken van de tijdweergave
            if (countingUp)
            {
                elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1));
                Timertxtblock.Text = elapsedTime.ToString(@"mm\:ss");
                PlusLabel.Visibility = Visibility.Visible; // Toon het "+" label
                GoldenScore.Visibility = Visibility.Visible;
            }
            else
            {
                if (elapsedTime.TotalSeconds > 0)
                {
                    elapsedTime = elapsedTime.Subtract(TimeSpan.FromSeconds(1));
                    Timertxtblock.Text = elapsedTime.ToString(@"mm\:ss");
                }
                else
                {
                    StopTimer();
                    Timertxtblock.Text = "00:00"; // Optioneel: update de UI om aan te geven dat de timer nul heeft bereikt
                }
            }
        }

        // Reset de hoofdtimer
        private void ResetTimer()
        {
            StopTimer();
            Timertxtblock.Text = "00:00";
        }

        // Start de timer voor 20 seconden
        private void StartTwentySecondsTimer()
        {
            twentySecondsTimer.Start();
            ProgressBar.Value = 0; // Reset de voortgangsbalk
        }

        // Stop de timer voor 20 seconden
        private void StopTwentySecondsTimer()
        {
            twentySecondsTimer.Stop();
        }

        // Event handler voor het tick-event van de timer voor 20 seconden
        public void TwentySecondsTimer_Tick(object sender, EventArgs e)
        {
            // Logica voor het bijwerken van de tijdweergave voor 20 seconden
            if (ProgressBar.Value < ProgressBar.Maximum)
            {
                ProgressBar.Value += 1; // Voeg 1 seconde toe aan de voortgangsbalk
                Timer20txtbx.Text = twentySecondsDuration.Subtract(TimeSpan.FromSeconds(ProgressBar.Value)).ToString(@"ss");
                Timer20txtbx.Visibility = Visibility.Visible;
                ProgressBar.Visibility = Visibility.Visible;
            }
            else
            {
                StopTwentySecondsTimer();
                ProgressBar.Value = 0; // Reset de voortgangsbalk
                Timer20txtbx.Text = "00:00";
                Timer20txtbx.Visibility = Visibility.Hidden;
                ProgressBar.Visibility = Visibility.Hidden;
            }
        }

        // Event handler voor toetsaanslagen
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Start de 20 seconden timer als de "X" toets wordt ingedrukt
            if (e.Key == Key.C)
            {
                StartTwentySecondsTimer();
            }
            else
            {
                StopTwentySecondsTimer();
                Timer20txtbx.Visibility = Visibility.Hidden;
                ProgressBar.Visibility = Visibility.Hidden;
            }

            bool canStartTimer = !string.IsNullOrEmpty(Weighttxtbx.Text) && !string.IsNullOrEmpty(Agetxtbx.Text) && !string.IsNullOrEmpty(geslachtlbl.Content as string);

            // Controleer of aan alle vereisten is voldaan om de timer te starten
            if (canStartTimer && e.Key == Key.T)
            {
                StartTimer();
            }
            else
            {
                StopTimer();
            }

            // Logica voor het afhandelen van toetsaanslagen
            switch (e.Key)
            {
                case Key.G:
                    if (elapsedTime.TotalSeconds <= 0)
                    {
                        countingUp = true; // Zet de vlag om te tellen op
                        elapsedTime = TimeSpan.FromSeconds(0); // Zet de verstreken tijd op nul
                        countingUp = true; // Zet de vlag om te tellen op
                        dispatcherTimer.Start();
                        timerRunning = true;
                    }
                    break;
                // Scores aanpassen
                case Key.A:
                    if (whiteScore == "0")
                        whiteScore = "1";
                    else if (whiteScore == "1")
                        whiteScore = "Ippon";
                    UpdateScore();
                    break;
                case Key.Q:
                    if (blueScore == "0")
                        blueScore = "1";
                    else if (blueScore == "1")
                        blueScore = "Ippon";
                    UpdateScore();
                    break;
                // Verlaag de score voor de witte judoka met 1
                case Key.Z:
                    if (whiteScore == "Ippon")
                        whiteScore = "1";
                    else if (whiteScore == "1")
                        whiteScore = "0";
                    UpdateScore();
                    break;
                // Verlaag de score voor de blauwe judoka met 1
                case Key.S:
                    if (blueScore == "Ippon")
                        blueScore = "1";
                    else if (blueScore == "1")
                        blueScore = "0";
                    UpdateScore();
                    break;
                // Verhoog het aantal fouten voor de witte judoka met 1
                case Key.E:
                    int whiteFoutCount = int.Parse(whiteFout);
                    whiteFoutCount++;
                    whiteFout = whiteFoutCount.ToString();
                    UpdateScore();
                    break;
                // Verhoog het aantal fouten voor de blauwe judoka met 1
                case Key.D:
                    int blueFoutCount = int.Parse(blueFout);
                    blueFoutCount++;
                    blueFout = blueFoutCount.ToString();
                    UpdateScore();
                    break;
                // Verlaag het aantal fouten voor de witte judoka met 1
                case Key.R:
                    int whiteFoutCount2 = int.Parse(whiteFout);
                    if (whiteFoutCount2 > 0)
                        whiteFoutCount2--;
                    whiteFout = whiteFoutCount2.ToString();
                    UpdateScore();
                    break;
                // Verlaag het aantal fouten voor de blauwe judoka met 1
                case Key.F:
                    int blueFoutCount2 = int.Parse(blueFout);
                    if (blueFoutCount2 > 0)
                        blueFoutCount2--;
                    blueFout = blueFoutCount2.ToString();
                    UpdateScore();
                    break;

                // Reset alle waarden als de "W" toets wordt ingedrukt
                case Key.W:
                    whiteFout = "0";
                    blueFout = "0";
                    whiteScore = "0";
                    blueScore = "0";
                    PlusLabel.Visibility = Visibility.Hidden;
                    GoldenScore.Visibility = Visibility.Hidden;
                    ResetTimer();
                    UpdateScore();
                    Timer20txtbx.Visibility = Visibility.Hidden;
                    ProgressBar.Visibility = Visibility.Hidden;
                    break;
                case Key.X:
                    SaveScoreDataToJson();
                    break;
                default:
                    break;
            }

            // Gewichtstekst aanpassen op basis van toetsaanslagen
            switch (e.Key)
            {
                case Key.H:
                    Weighttxtbx.Text = "-34 kg";
                    break;
                case Key.J:
                    Weighttxtbx.Text = "-72 kg";
                    break;
                case Key.K:
                    Weighttxtbx.Text = "-90 kg";
                    break;
                default:
                    break;
            }

            // Leeftijdstekst aanpassen op basis van toetsaanslagen
            switch (e.Key)
            {
                case Key.Y:
                    Agetxtbx.Text = "U21";
                    break;
                case Key.U:
                    Agetxtbx.Text = "U18";
                    break;
                case Key.I:
                    Agetxtbx.Text = "U15";
                    break;
                default:
                    break;
            }
            switch (e.Key)
            {
                case Key.O:
                    persoon = new Man();
                    // Hier wordt het geslacht van de persoon weergegeven in de Label
                    geslachtlbl.Content = $"{persoon.Geslacht}";
                    break;
                case Key.L:
                    persoon = new Vrouw();
                    // Hier wordt het geslacht van de persoon weergegeven in de Label
                    geslachtlbl.Content = $"{persoon.Geslacht}";
                    break;
                default:
                    persoon = new Mens(); // Als er een andere toets wordt ingedrukt, wordt standaard Mens gebruikt
                    break;

            }
        }

        // Methode om de scoreweergave bij te werken
        private void UpdateScore()
        {
            // Bijwerkingslogica voor de scoreweergave
            WhiteScoreTextBlock.Text = whiteScore.ToString();
            BlueScoreTextBlock.Text = blueScore.ToString();

            WhiteFoutTextBlock.Text = whiteFout.ToString();
            BlueFoutTextBlock.Text = blueFout.ToString();
        }

        private void SaveScoreDataToJson()
{
    // Maak een nieuw ScoreData object met de huidige scoregegevens
    ScoreData scoreData = new ScoreData
    {
        WhiteScore = whiteScore,
        BlueScore = blueScore,
        WhiteFout = whiteFout,
        BlueFout = blueFout,
        ElapsedTime = elapsedTime // Toevoegen van elapsed time
    };

    try
    {
        // Converteer het object naar JSON-indeling
        string json = JsonConvert.SerializeObject(scoreData);

        // Voeg een nieuwe regel toe na elk gegeven
        json = json.Replace(", ", ",\n");

        // Bepaal het pad voor het JSON-bestand (bijvoorbeeld: "scores.json" in de huidige map)
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "scores.json");

        // Schrijf de JSON-gegevens naar het bestand
        File.WriteAllText(filePath, json);

        // Geef een melding dat de gegevens succesvol zijn opgeslagen
        MessageBox.Show("Scoregegevens zijn opgeslagen in scores.json", "Opslaan succesvol", MessageBoxButton.OK, MessageBoxImage.Information);
    }
    catch (Exception ex)
    {
        // Als er een fout optreedt, toon een foutmelding
        MessageBox.Show($"Fout bij het opslaan van scoregegevens: {ex.Message}", "Fout bij opslaan", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
    }
}
