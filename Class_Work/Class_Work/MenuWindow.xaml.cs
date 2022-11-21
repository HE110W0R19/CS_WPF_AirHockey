using System;
using System.IO;
using System.IO.Compression;
using System.Media;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Class_Work
{
    /// <summary>
    /// Логика взаимодействия для MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        private bool IsMenu = false;
        private bool IsDefaultCanvas = true;
        private int GameModIndex = 0;
        private int GameSecTimer = 0;
        private int GameWinScore = 0;
        private SolidColorBrush TopPlayerColorBrush;
        private SolidColorBrush BottomPlayerColorBrush;
        public MenuWindow()
        {
            InitializeComponent();
            this.DefaultCanvas.IsChecked = true;
            this.GameModIndex = GameModBox.SelectedIndex;
            this.GameSecTimer = Convert.ToInt32(GameTimerTB.Text);
            this.GameWinScore = Convert.ToInt32(GameScoreTB.Text);
            this.BottomPlayerColorBrush = Brushes.Transparent;
            this.TopPlayerColorBrush = Brushes.Transparent;

            string FilePath = @"Scores\GameScore.txt";
            using (StreamReader sr = File.OpenText(FilePath))
            {
                string all_text = sr.ReadToEnd();
                sr.Close();
                string[] buf = all_text.Split(';');
                for(int i = buf.Length-1; i > 0; --i)
                {
                    this.LastGames.Text += buf[i] + "\n";
                }
            }
        }

        public void ChangeLanguage()
        {
            this.GameName.Content = LangResource.air_hockey;
            this.NewGameButton.Content = LangResource.New_Game;
            this.SettingsButton.Content = LangResource.Settings;
            this.ExitButton.Content = LangResource.Exit;
            this.Lastlabel.Content = LangResource.Last_Games;

            if (IsMenu == true)
            {
                this.SaveSettingsButton.Content = LangResource.Save;
                this.GoBackButton.Content = LangResource.GoBack;
                this.LabelGameSettings.Content = LangResource.Game_settings;
                this.LabelPlayerSettings.Content = LangResource.Player_settings;
                this.LabelCanvasStyle.Content = LangResource.Canvas_style;
                this.LabelGameModes.Content = LangResource.Game_modes;
                this.LabelGameTimer.Content = LangResource.Game_timer;
                this.LabelGameMaxScore.Content = LangResource.Win_score;
                this.LabelTopPlayer.Content = LangResource.Top_player;
                this.LabelBottomPLayer.Content = LangResource.Bottom_player;
                this.RealisticCanvas.Content = LangResource.Realistic;
                this.DefaultCanvas.Content = LangResource.Default;
                this.GameModBox.Items[0] = LangResource.One_player;
                this.GameModBox.SelectedIndex = 0;
                this.GameModBox.Items[1] = LangResource.Bot_vs_Player_Mouse_;
                this.GameModBox.Items[2] = LangResource._1_Key__VS_1_mouse_;
                this.LabelSecInfo.Content = LangResource._sec;
                this.LabelMaxCount.Content = LangResource._30___max_;
                this.SettingIsSaved.Content = LangResource.Settings_saved___;
                this.BottomPlayerColor.Items[5] = LangResource.Transparent;
                this.BottomPlayerColor.SelectedIndex = 5;
                this.TopPlayerColor.Items[5] = LangResource.Transparent;
                this.TopPlayerColor.SelectedIndex = 5;
            }
        }

        private void ButtonAnim(System.Windows.Controls.Button button_click)
        {
            TranslateTransform tran = new TranslateTransform();
            button_click.RenderTransform = tran;
            DoubleAnimation anim1 = new DoubleAnimation(0, 5, TimeSpan.FromSeconds(0.1));
            DoubleAnimation anim2 = new DoubleAnimation(5, 0, TimeSpan.FromSeconds(0.1));
            tran.BeginAnimation(TranslateTransform.YProperty, anim1);
            tran.BeginAnimation(TranslateTransform.YProperty, anim2);
        }

        private void LanguageSetting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SoundPlay(1);
            switch (LanguageSetting.SelectedIndex)
            {
                case 0:
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru");
                    this.GameName.FontSize = 54;
                    ChangeLanguage();
                    break;
                case 1:
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en");
                    this.GameName.FontSize = 72;
                    ChangeLanguage();
                    break;
                default:
                    break;
            }
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(this.IsDefaultCanvas, this.GameModIndex, this.GameSecTimer,
                this.GameWinScore, this.TopPlayerColorBrush, this.BottomPlayerColorBrush).Show();
            SoundPlay(3);
            this.Close();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SoundPlay(1);
            this.NewGameButton.Visibility = Visibility.Hidden;
            this.SettingsButton.Visibility = Visibility.Hidden;
            this.ExitButton.Visibility = Visibility.Hidden;
            this.LastGamesLabel.Visibility = Visibility.Hidden;
            this.Lastlabel.Visibility = Visibility.Hidden;

            this.LabelGameSettings.Visibility = Visibility.Visible;
            this.LabelCanvasStyle.Visibility = Visibility.Visible;
            this.LabelGameModes.Visibility = Visibility.Visible;
            this.LabelPlayerSettings.Visibility = Visibility.Visible;
            this.LabelTopPlayer.Visibility = Visibility.Visible;
            this.LabelBottomPLayer.Visibility = Visibility.Visible;
            this.LabelGameTimer.Visibility = Visibility.Visible;
            this.LabelMaxCount.Visibility = Visibility.Visible;
            this.LabelSecInfo.Visibility = Visibility.Visible;
            this.LabelGameMaxScore.Visibility = Visibility.Visible;
            this.SaveSettingsButton.Visibility = Visibility.Visible;
            this.GoBackButton.Visibility = Visibility.Visible;
            this.RealisticCanvas.Visibility = Visibility.Visible;
            this.DefaultCanvas.Visibility = Visibility.Visible;
            this.GameModBox.Visibility = Visibility.Visible;
            this.GameTimerTB.Visibility = Visibility.Visible;
            this.GameScoreTB.Visibility = Visibility.Visible;
            this.TopPlayerColor.Visibility = Visibility.Visible;
            this.BottomPlayerColor.Visibility = Visibility.Visible;
            this.IsMenu = true;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            SoundPlay(4);
            Thread.Sleep(300);
            Environment.Exit(0);
        }

        private void SoundPlay(int SoundNum)
        {
            switch (SoundNum)
            {
                case 1:
                    using (MemoryStream fo = new MemoryStream(Properties.Resources.MenuSelect))
                    using (GZipStream gz = new GZipStream(fo, CompressionMode.Decompress))
                        new SoundPlayer(gz).Play();
                    break;
                case 2:
                    using (MemoryStream fo = new MemoryStream(Properties.Resources.MenuUnSelect))
                    using (GZipStream gz = new GZipStream(fo, CompressionMode.Decompress))
                        new SoundPlayer(gz).Play();
                    break;
                case 3:
                    using (MemoryStream fo = new MemoryStream(Properties.Resources.StartGame))
                    using (GZipStream gz = new GZipStream(fo, CompressionMode.Decompress))
                        new SoundPlayer(gz).Play();
                    break;
                case 4:
                    using (MemoryStream fo = new MemoryStream(Properties.Resources.StopGame))
                    using (GZipStream gz = new GZipStream(fo, CompressionMode.Decompress))
                        new SoundPlayer(gz).Play();
                    break;
                default:
                    break;
            }
        }


        private void SaveSettingButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SoundPlay(1);
                ButtonAnim(SaveSettingsButton);

                SetPlayerColor(ref this.BottomPlayerColorBrush, BottomPlayerColor);
                SetPlayerColor(ref this.TopPlayerColorBrush, TopPlayerColor);

                this.GameModIndex = GameModBox.SelectedIndex;

                this.GameSecTimer = Convert.ToInt32(this.GameTimerTB.Text);
                this.GameWinScore = Convert.ToInt32(this.GameScoreTB.Text);

                if (IsDefaultCanvas)
                    this.IsDefaultCanvas = true;
                else
                    this.IsDefaultCanvas = false;

                this.SettingIsSaved.Visibility = Visibility.Visible;
            }
            catch (Exception)
            {
                this.GameTimerTB.Text = "86400";
                this.GameScoreTB.Text = "30";
                SaveSettingButton_Click(sender, e);
            }
        }

        private void SetPlayerColor(ref SolidColorBrush Player, ComboBox ColorBox)
        {
            switch (ColorBox.SelectedIndex)
            {
                case 0:
                    Player = Brushes.Red;
                    break;
                case 1:
                    Player = Brushes.Yellow;
                    break;
                case 2:
                    Player = Brushes.Lime;
                    break;
                case 3:
                    Player = Brushes.Blue;
                    break;
                case 4:
                    Player = Brushes.Magenta;
                    break;
                case 5:
                    Player = Brushes.Transparent;
                    break;
                default:
                    break;
            }
        }

        private void BottomPlayerColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SoundPlay(1);
        }

        private void TopPlayerColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SoundPlay(1);
        }

        private void GameModBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SoundPlay(1);
        }

        private void RealisticCanvas_Checked(object sender, RoutedEventArgs e)
        {
            SoundPlay(1);
            this.DefaultCanvas.IsChecked = false;
            this.IsDefaultCanvas = false;
        }

        private void DefaultCanvas_Checked(object sender, RoutedEventArgs e)
        {
            SoundPlay(1);
            this.RealisticCanvas.IsChecked = false;
            this.IsDefaultCanvas = true;
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            SoundPlay(2);
            this.NewGameButton.Visibility = Visibility.Visible;
            this.SettingsButton.Visibility = Visibility.Visible;
            this.ExitButton.Visibility = Visibility.Visible;
            this.LastGamesLabel.Visibility = Visibility.Visible;
            this.Lastlabel.Visibility = Visibility.Visible;

            this.LabelGameSettings.Visibility = Visibility.Hidden;
            this.LabelCanvasStyle.Visibility = Visibility.Hidden;
            this.LabelGameModes.Visibility = Visibility.Hidden;
            this.LabelPlayerSettings.Visibility = Visibility.Hidden;
            this.LabelTopPlayer.Visibility = Visibility.Hidden;
            this.LabelBottomPLayer.Visibility = Visibility.Hidden;
            this.LabelGameTimer.Visibility = Visibility.Hidden;
            this.LabelMaxCount.Visibility = Visibility.Hidden;
            this.LabelSecInfo.Visibility = Visibility.Hidden;
            this.LabelGameMaxScore.Visibility = Visibility.Hidden;
            this.SaveSettingsButton.Visibility = Visibility.Hidden;
            this.GoBackButton.Visibility = Visibility.Hidden;
            this.RealisticCanvas.Visibility = Visibility.Hidden;
            this.DefaultCanvas.Visibility = Visibility.Hidden;
            this.GameModBox.Visibility = Visibility.Hidden;
            this.GameTimerTB.Visibility = Visibility.Hidden;
            this.GameScoreTB.Visibility = Visibility.Hidden;
            this.TopPlayerColor.Visibility = Visibility.Hidden;
            this.BottomPlayerColor.Visibility = Visibility.Hidden;
            this.SettingIsSaved.Visibility = Visibility.Hidden;
            this.IsMenu = false;
        }

        private void GameTimerTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.GameTimerTB.Text.Length > 4)
            {
                this.GameTimerTB.Text = "86400";
                this.GameSecTimer = 86400;
            }
        }

        private void GameScoreTB_textChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (this.GameScoreTB.Text.Length == 0
                    || Convert.ToInt32(this.GameScoreTB.Text) > 30)
                {
                    this.GameScoreTB.Text = "30";
                    this.GameWinScore = 30;
                }
            }
            catch (Exception)
            {
                this.GameScoreTB.Text = "30";
            }
        }
    }
}
