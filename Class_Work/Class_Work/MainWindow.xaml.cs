using System;
using System.IO;
using System.IO.Compression;
using System.Media;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using TimeSpan = System.TimeSpan;

namespace Class_Work
{
    class HockeyPuck
    {
        Rectangle Puck = new Rectangle();
        public double BeginSpeedX = 140;
        public double BeginSpeedY = 220;
        public double SpeedX = 140;
        public double SpeedY = 220;
        private double PuckStartPosX = 193;
        private double PuckStartPosY = 305;
        public double CurrentPosX = 0;
        public double CurrentPosY = 0;
        public HockeyPuck()
        {
            this.Puck.Height = 50;
            this.Puck.Width = 50;
            this.Puck.RadiusX = 100;
            this.Puck.RadiusY = 100;
            this.Puck.Stroke = Brushes.Black;
            this.Puck.StrokeThickness = 1;
            RadialGradientBrush myRadialGradientBrush = new RadialGradientBrush();
            myRadialGradientBrush.GradientStops.Add(
                new GradientStop(Colors.GreenYellow, 0.1));
            myRadialGradientBrush.GradientStops.Add(
                new GradientStop(Colors.LimeGreen, 0.70));
            Puck.Fill = myRadialGradientBrush;
            Canvas.SetLeft(this.Puck, PuckStartPosX);
            Canvas.SetTop(this.Puck, PuckStartPosY);
        }
        public Rectangle GPuck => this.Puck;
        public double GBeginPosX => PuckStartPosX;
        public double GbeginPosY => PuckStartPosY;
    }

    abstract class AbstractPlayer
    {
        public int Points = 0;
        protected int StickWidth = 25;
        protected int StickHeight = 25;
        protected Rectangle Stick = new Rectangle();
        protected string PlayerName = "";
        public Rectangle GStick => this.Stick;
        public string GName => this.PlayerName;
    }

    class MousePlayer : AbstractPlayer
    {
        public double MouseCordX = 0;
        public double MouseCordY = 0;
        public MousePlayer(string Name)
        {
            this.PlayerName = Name.ToString();
            Stick.Height = StickHeight;
            Stick.Width = StickWidth;
            Stick.Stroke = Brushes.Black;
            Stick.StrokeThickness = 2;
            Stick.RadiusX = 100;
            Stick.RadiusY = 100;
        }
    }

    class KeyPlayer : AbstractPlayer
    {
        public double KeyCordX = 0;
        public double KeyCordY = 0;
        public KeyPlayer(string Name)
        {
            this.PlayerName = Name.ToString();
            Stick.Height = StickHeight;
            Stick.Width = StickWidth;
            Stick.Stroke = Brushes.Black;
            Stick.StrokeThickness = 2;
            Stick.RadiusX = 100;
            Stick.RadiusY = 100;
        }
    }
    /// <summary>
    ///====================================================
    ///====================================================
    ///====================================================
    /// </summary>
    public partial class MainWindow : Window
    {
        MousePlayer player1;
        KeyPlayer player2;
        HockeyPuck puck = new HockeyPuck();
        int GameMod, Time, MaxScore;
        private DispatcherTimer AnimationTimer = new DispatcherTimer();
        public MainWindow(bool CanvasStyle, int GameModIndex, int Time, int WinScore, 
            SolidColorBrush TopStickColor, SolidColorBrush BottomStickColor )
        {
            InitializeComponent();
            this.GameMod = GameModIndex;
            this.MaxScore = WinScore;
            if (this.GameMod == 0)
            {
                player1 = new MousePlayer("MPlayer");
            }
            else if (this.GameMod == 1)
            {
                player1 = new MousePlayer("MPlayer");
                player2 = new KeyPlayer("KPlayer");

                Game.Children.Add(player2.GStick);
                Canvas.SetTop(this.player2.GStick, 100);
                Canvas.SetLeft(this.player2.GStick, 185);
                this.UpPlayerName.Content = $"Player: {player2.GName}";
                AnimationTimer.Tick += Bot_Move;
            }
            else if (this.GameMod == 2)
            {
                player1 = new MousePlayer("MPlayer");
                player2 = new KeyPlayer("KPlayer");

                Game.Children.Add(player2.GStick);
                Canvas.SetTop(this.player2.GStick, 100);
                Canvas.SetLeft(this.player2.GStick, 185);
                this.UpPlayerName.Content = $"Player: {player2.GName}";
                this.KeyDown += Key_Move;
            }

            this.DownPlayerName.Content = $"Player: {player1.GName}";

            Canvas.SetBottom(this.player1.GStick, 100);
            Canvas.SetLeft(this.player1.GStick, 185);

            this.MouseMove += Move_mouse;

            Game.Children.Add(player1.GStick);
            Game.Children.Add(puck.GPuck);

            //AnimationTimer Initialization
            AnimationTimer.Interval = TimeSpan.FromSeconds(0.05);
            AnimationTimer.Tick += Anim;
            AnimationTimer.Start();
            
        }

        private void MoveMouseLogick(MouseEventArgs e, MousePlayer MPlayer)
        {
            MPlayer.MouseCordX = e.GetPosition(Game).X;
            MPlayer.MouseCordY = e.GetPosition(Game).Y;
            if (MPlayer.MouseCordX >= 7 && MPlayer.MouseCordX <= 407
                && MPlayer.MouseCordY >= 305 && MPlayer.MouseCordY <= 576)
            {
                Canvas.SetLeft(player1.GStick, MPlayer.MouseCordX - 10);
                Canvas.SetTop(player1.GStick, MPlayer.MouseCordY - 10);
            }
            this.MouseCords.Content = $"X:{e.GetPosition(Game).X} Y:{e.GetPosition(Game).Y}";
        }

        private void Move_mouse(object sender, MouseEventArgs user_mouse)
        {
            MoveMouseLogick(user_mouse, player1);
        }

        private void KeyMoveLogic(KeyEventArgs e, KeyPlayer PKey)
        {
            PKey.KeyCordX = Canvas.GetLeft(PKey.GStick);
            PKey.KeyCordY = Canvas.GetTop(PKey.GStick);
            if (e.Key == Key.W)
            {
                Canvas.SetTop(PKey.GStick, PKey.KeyCordY - 10);
            }
            if (e.Key == Key.S)
            {
                Canvas.SetTop(PKey.GStick, PKey.KeyCordY + 10);
            }
            if (e.Key == Key.A)
            {
                Canvas.SetLeft(PKey.GStick, PKey.KeyCordX - 10);
            }
            if (e.Key == Key.D)
            {
                Canvas.SetLeft(PKey.GStick, PKey.KeyCordX + 10);
            }
        }
        private void Key_Move(object sender, KeyEventArgs e)
        {
            KeyMoveLogic(e, player2);
        }

        private void BotMoveLogick(KeyPlayer BBot)
        {

            Canvas.SetTop(player2.GStick, player2.KeyCordY);
            Canvas.SetLeft(player2.GStick, player2.KeyCordX);
        }

        private void Bot_Move(object sender, EventArgs e)
        {
            BotMoveLogick(player2);
        }

        private void AnimLogic(HockeyPuck tmp_puck, double ObjCordX, double ObjCordY)
        {
            if (ObjCordX > Canvas.GetLeft(puck.GPuck) - 10 && ObjCordX < Canvas.GetLeft(puck.GPuck) + puck.GPuck.Width + 10 &&
                ObjCordY > Canvas.GetTop(puck.GPuck) - 10 && ObjCordY < Canvas.GetTop(puck.GPuck) + puck.GPuck.Height + 10)
            {
                if (ObjCordX > Canvas.GetLeft(tmp_puck.GPuck) - 30
                    && ObjCordX < Canvas.GetLeft(tmp_puck.GPuck))
                {
                    tmp_puck.SpeedX = -tmp_puck.SpeedX;
                    SoundPlay(2);
                }

                else if (ObjCordX < Canvas.GetLeft(tmp_puck.GPuck) + tmp_puck.GPuck.Width + 30
                    && ObjCordX > Canvas.GetLeft(tmp_puck.GPuck) + tmp_puck.GPuck.Width)
                {
                    tmp_puck.SpeedX = -tmp_puck.SpeedX;
                    SoundPlay(2);
                }

                else if (ObjCordY > Canvas.GetTop(tmp_puck.GPuck) - 30
                    && ObjCordY < Canvas.GetTop(tmp_puck.GPuck))
                {
                    tmp_puck.SpeedY = -tmp_puck.SpeedY;
                    SoundPlay(2);
                }

                else if (ObjCordY < Canvas.GetTop(tmp_puck.GPuck) + tmp_puck.GPuck.Height + 30
                    && ObjCordY > Canvas.GetTop(tmp_puck.GPuck) + tmp_puck.GPuck.Height)
                {
                    tmp_puck.SpeedY = -tmp_puck.SpeedY;
                    SoundPlay(2);
                }
            }
        }

        private void Anim(object sender, EventArgs e)
        {
            this.puck.CurrentPosX = Canvas.GetLeft(this.puck.GPuck);
            this.puck.CurrentPosY = Canvas.GetTop(this.puck.GPuck);

            if (this.player1.MouseCordX >= 7 && this.player1.MouseCordX <= 407
                && this.player1.MouseCordY >= 305 && this.player1.MouseCordY <= 576)
            {
                AnimLogic(puck, this.player1.MouseCordX, this.player1.MouseCordY);
            }

            if (this.GameMod > 1)
                AnimLogic(puck, this.player2.KeyCordX, this.player2.KeyCordY);

            if (this.puck.CurrentPosX <= 0.0 || this.puck.CurrentPosX >= Game.ActualWidth - puck.GPuck.Width)
            {
                puck.SpeedX = -puck.SpeedX;
                SoundPlay(1);
            }

            if (this.puck.CurrentPosY <= 0.0 || this.puck.CurrentPosY >= Game.ActualHeight - puck.GPuck.Width)
            {
                puck.SpeedY = -puck.SpeedY;
                SoundPlay(1);
            }

            if (this.puck.CurrentPosY >= -10 && this.puck.CurrentPosY <= 10
                && this.puck.CurrentPosX >= Canvas.GetLeft(UpGates) - 10
                && this.puck.CurrentPosX <= Canvas.GetLeft(UpGates) + this.UpGates.Width + 10)
            {
                SoundPlay(3);
                if (GameMod > 1)
                {
                    ++player1.Points;
                    EndGame(MaxScore, player1.Points, player2.Points);
                    DownPlayerScore.Content = player1.Points.ToString();
                }
                else
                {
                    ++player1.Points;
                    EndGame(MaxScore, player1.Points);
                    DownPlayerScore.Content = player1.Points.ToString();
                }
                Canvas.SetLeft(puck.GPuck, puck.GBeginPosX);
                Canvas.SetTop(puck.GPuck, puck.GbeginPosY);
                this.puck.SpeedX = this.puck.BeginSpeedX;
                this.puck.SpeedY = this.puck.BeginSpeedY;
                return;
            }

            if (this.puck.CurrentPosY + puck.GPuck.Height >= 560 && this.puck.CurrentPosY + puck.GPuck.Height <= 570
                && this.puck.CurrentPosX >= Canvas.GetLeft(DownGates) - 10
                && this.puck.CurrentPosX <= Canvas.GetLeft(DownGates) + this.UpGates.Width + 10)
            {
                SoundPlay(3);
                if (GameMod > 1)
                {
                    ++this.player2.Points;
                    UpPlayerScore.Content = this.player2.Points.ToString();
                    EndGame(MaxScore, player1.Points, player2.Points);
                }
                else
                {
                    --this.player1.Points;
                    DownPlayerScore.Content = this.player1.Points.ToString();
                    EndGame(MaxScore, player1.Points);
                }
                Canvas.SetLeft(puck.GPuck, puck.GBeginPosX);
                Canvas.SetTop(puck.GPuck, puck.GbeginPosY);
                this.puck.SpeedX = this.puck.BeginSpeedX;
                this.puck.SpeedY = this.puck.BeginSpeedY;
                return;
            }

            this.puck.CurrentPosX += puck.SpeedX * AnimationTimer.Interval.TotalSeconds;
            this.puck.CurrentPosY += puck.SpeedY * AnimationTimer.Interval.TotalSeconds;

            if (this.puck.CurrentPosX > Game.ActualWidth - puck.GPuck.Width + 20 || this.puck.CurrentPosY > Game.ActualHeight - puck.GPuck.Height + 20 || this.puck.CurrentPosX < -20 || this.puck.CurrentPosY < -20)
            {
                Canvas.SetLeft(puck.GPuck, puck.GBeginPosX);
                Canvas.SetTop(puck.GPuck, puck.GbeginPosY);
                return;
            }
            else
            {
                Canvas.SetLeft(puck.GPuck, this.puck.CurrentPosX);
                Canvas.SetTop(puck.GPuck, this.puck.CurrentPosY);
                Cords.Content = $"Cords: X:{this.puck.CurrentPosX} Y:{this.puck.CurrentPosY}";
            }
            puck.SpeedX *= 1.001;
            puck.SpeedY *= 1.001;
        }
        private void SoundPlay(int SoundNum)
        {
            switch (SoundNum)
            {
                case 1:
                    using (MemoryStream fo = new MemoryStream(Properties.Resources.BoardSound))
                    using (GZipStream gz = new GZipStream(fo, CompressionMode.Decompress))
                        new SoundPlayer(gz).Play();
                    break;
                case 2:
                    using (MemoryStream fo = new MemoryStream(Properties.Resources.StickSound))
                    using (GZipStream gz = new GZipStream(fo, CompressionMode.Decompress))
                        new SoundPlayer(gz).Play();
                    break;
                case 3:
                    using (MemoryStream fo = new MemoryStream(Properties.Resources.GoalSound))
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

        public void EndGame(int Max, int BottomPlayerPoints, int TopPlayerPoints = 0)
        {
            if (BottomPlayerPoints == Max)
            {
                BottomWinner.Visibility = Visibility.Visible;
                TopLose.Visibility = Visibility.Visible;
                AnimationTimer.Stop();
                this.AnimationTimer.Tick -= Bot_Move;
                this.MouseMove -= Move_mouse;
                this.KeyDown -= Key_Move;
            }
            else if (TopPlayerPoints == Max)
            {
                BottomLose.Visibility = Visibility.Visible;
                TopWinner.Visibility = Visibility.Visible;
                AnimationTimer.Stop();
                this.AnimationTimer.Tick -= Bot_Move;
                this.MouseMove -= Move_mouse;
                this.KeyDown -= Key_Move;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new MenuWindow().Show();
            SoundPlay(4);
            AnimationTimer.Stop();
            this.AnimationTimer.Tick -= Bot_Move;
            this.MouseMove -= Move_mouse;
            this.KeyDown -= Key_Move;
            this.Close();
        }
    }
}