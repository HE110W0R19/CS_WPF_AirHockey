using System;
using System.Collections.Generic;
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
            this.Close();
        }
    }
}
