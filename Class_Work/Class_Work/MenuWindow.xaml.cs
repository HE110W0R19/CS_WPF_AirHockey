using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Class_Work
{
    /// <summary>
    /// Логика взаимодействия для MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
        }

        public void ChangeLanguage()
        {
            this.GameName.Content = LangResource.air_hockey;
            this.NewGameButton.Content = LangResource.New_Game;
            this.SettingsButton.Content = LangResource.Settings;
            this.ExitButton.Content = LangResource.Exit;
        }

        private void LanguageSetting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
            new MainWindow().Show();
            SoundPlay(3);
            this.Close();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SoundPlay(1);
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            SoundPlay(1);
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
    }
}
