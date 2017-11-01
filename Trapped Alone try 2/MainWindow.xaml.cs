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
using System.Threading;

namespace Trapped_Alone_try_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public struct Bullet
        {
            public double X;
            public double Y;
            public string Type;
            public string Alive;
            public int Size;
            public int DestinyX;
            public int DestinyY;
            public double Speed;
            public Ellipse ellipsebullet;
            SolidColorBrush colorOnBullet;
            public Bullet(int x, int y, string type, string alive, int destinyx, int destinyy, double speed, Ellipse ecBullet, SolidColorBrush bulletcolor)
            {
                X = x;
                Y = y;
                Type = type;
                Alive = alive;
                Size = 10;
                DestinyX = destinyx;
                DestinyY = destinyy;
                Speed = speed;
                ellipsebullet = ecBullet;// = new Ellipse();
                colorOnBullet = bulletcolor;
            }
        }

        Random ra = new Random();

        int Life = 3;

        int MousePositionX;
        int MousePositionY;

        int totalWidth = 525;
        int totalHeight = 350;

        int SniperX;
        int SniperY;

        bool Go = false;

        Rectangle skylt1 = new Rectangle();
        Rectangle skylt2 = new Rectangle();

        int bullets_alive = 0;
        Canvas MainCanvas = new Canvas();
        Canvas BulletsCanvas = new Canvas();
        Bullet[] bullets = new Bullet[1000];

        TextBlock ScoreTextBlock = new TextBlock();
        TextBlock LifeTextBlock = new TextBlock();

        SolidColorBrush SimpleBulletColor = new SolidColorBrush();
        SolidColorBrush SniperColor = new SolidColorBrush();
        SolidColorBrush CursorColor = new SolidColorBrush();
        SolidColorBrush BackGroundColor = new SolidColorBrush();
        SolidColorBrush RemoverBulletColor = new SolidColorBrush();
        SolidColorBrush LifeBulletColor = new SolidColorBrush();

        Ellipse Sniper = new Ellipse();
        Ellipse SniperEye = new Ellipse();

        Ellipse CursorDot = new Ellipse();

        public MainWindow()
        {
            InitializeComponent();
            SniperX = ra.Next(totalWidth);
            SniperY = ra.Next(totalHeight);
            Mouse.OverrideCursor = Cursors.None;
            Rectangle BackGround = new Rectangle();//background
            BackGroundColor.Color = System.Windows.Media.Color.FromArgb(255, 0, 0, 0);
            BackGround.Fill = BackGroundColor;
            BackGround.Width = 10000;
            BackGround.Height = 10000;
            MainCanvas.Children.Add(BackGround); // Lägger till Background

            #region Startup

            skylt1.Width = totalWidth / 1.5;
            skylt1.Height = totalHeight / 1.5;
            skylt1.Fill = Brushes.Green;
            Canvas.SetLeft(skylt1, totalWidth / 2 + 50);
            Canvas.SetTop(skylt1, totalHeight / 5 + 50);
            MainCanvas.Children.Add(skylt1);



            skylt2.Width = totalWidth / 1.5 - 20;
            skylt2.Height = totalHeight / 1.5 - 20;
            skylt2.Fill = Brushes.Yellow;
            Canvas.SetLeft(skylt2, totalWidth / 2 + 50 + 10);
            Canvas.SetTop(skylt2, totalHeight / 5 + 50 + 10);
            MainCanvas.Children.Add(skylt2);
            #endregion

            #region Text till Live och Score
            ScoreTextBlock.Text = "0";
            ScoreTextBlock.Foreground = CursorColor;
            Canvas.SetLeft(ScoreTextBlock, 10);
            Canvas.SetTop(ScoreTextBlock, 10);
            MainCanvas.Children.Add(ScoreTextBlock);


            LifeTextBlock.Text = "3";
            LifeTextBlock.Foreground = CursorColor;
            Canvas.SetLeft(LifeTextBlock, 10);
            Canvas.SetTop(LifeTextBlock, 20);
            MainCanvas.Children.Add(LifeTextBlock);
            #endregion

            SimpleBulletColor.Color = System.Windows.Media.Color.FromArgb(255, 255, 0, 0);      //Sets the color on the red simple bullets
            SniperColor.Color = System.Windows.Media.Color.FromArgb(255, 128, 0, 128);          //Sets the color on the purple Sniper
            CursorColor.Color = System.Windows.Media.Color.FromArgb(255, 255, 255, 255);        //Sets the color on the white cursor
            LifeBulletColor.Color = System.Windows.Media.Color.FromArgb(255, 127, 255, 0);      //Sets the color on the green lives
            RemoverBulletColor.Color = System.Windows.Media.Color.FromArgb(255, 133, 244, 244); //Sets the color on the Aqua Removers


            Draw_Sniper();

            MainCanvas.Children.Add(CursorDot);


            for (int i = 0; i < bullets.Length - 1; i++)
            {
                bullets[i].Alive = "Not Born";
                bullets[i].Size = 10;
            }

            this.Content = MainCanvas;

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1);
            dispatcherTimer.Start();

            System.Windows.Threading.DispatcherTimer Bullets = new System.Windows.Threading.DispatcherTimer();
            Bullets.Tick += Bullets_tick;
            Bullets.Interval = new TimeSpan(0, 0, 0, 0, 1);
            Bullets.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (Life > 0 && Go == true)
                randombullet();
            this.Content = MainCanvas;
        }

        private void Bullets_tick(object sender, EventArgs e)
        {
            if (Life > 0 && (Go == true))
            {
                for (int i = 0; i < bullets_alive; i++)
                {
                    if (bullets[i].Alive == "Alive")
                    {
                        double dbx = bullets[i].Speed * (bullets[i].X - bullets[i].DestinyX) / distance(bullets[i].X, bullets[i].Y, bullets[i].DestinyX, bullets[i].DestinyY);
                        double dby = bullets[i].Speed * (bullets[i].Y - bullets[i].DestinyY) / distance(bullets[i].X, bullets[i].Y, bullets[i].DestinyX, bullets[i].DestinyY);
                        bullets[i].X -= dbx;
                        bullets[i].Y -= dby;
                        Canvas.SetLeft(bullets[i].ellipsebullet, bullets[i].X - bullets[i].ellipsebullet.Width / 2);
                        Canvas.SetTop(bullets[i].ellipsebullet, bullets[i].Y - bullets[i].ellipsebullet.Height / 2);
                        if (distance(bullets[i].X, bullets[i].Y, bullets[i].DestinyX, bullets[i].DestinyY) <= bullets[i].Speed)
                        {
                            bullets[i].Alive = "Still Alive";
                        }
                    }

                    if (distance(MousePositionX, MousePositionY, bullets[i].X, bullets[i].Y) <= bullets[i].Size + 5 && (bullets[i].Alive == "Alive" || bullets[i].Alive == "Still"))
                    {
                        MainCanvas.Children.Remove(bullets[i].ellipsebullet);
                        if (bullets[i].Type == "Normal")
                        {
                            Life--;
                            LifeTextBlock.Text = Life.ToString();
                        }
                        else
                        if (bullets[i].Type == "Life")
                        {
                            Life++;
                            LifeTextBlock.Text = Life.ToString();
                        }
                        if (bullets[i].Type == "Remover")//shokran är bäst yay .....67ieukäåtlukfjk97ryfkguo08)
                        {
                            Life++;
                            for (int ii = 0; ii < bullets.Length; ii++)
                            {
                                if (ra.Next(3) == 0)
                                {
                                    MainCanvas.Children.Remove(bullets[ii].ellipsebullet);
                                    bullets[ii].Alive = "Dead";
                                }
                            }
                        }
                        bullets[i].Alive = "Dead!";
                    }
                }
                if (Life <= 0)
                {
                    Game_Over();
                }
            }
            Draw_Sniper_Eye();
        }

        private void Game_Over()
        {
            MessageBox.Show("You died!", "Meh");
            CursorColor.Color = System.Windows.Media.Color.FromArgb(100, 255, 255, 255); //Makes the cursor transparent
            MainCanvas.Children.Remove(CursorDot);
            MainCanvas.Children.Add(CursorDot);
            MainCanvas.Children.Add(skylt1);
            MainCanvas.Children.Add(skylt2);
            Go = false;
            Life = 3;
            for (int i = 0; i < bullets.Length; i++)
            {
                MainCanvas.Children.Remove(bullets[i].ellipsebullet);
            }
            bullets = new Bullet[1000];
        }

        private void Draw_Sniper()
        {
            Sniper.Fill = SniperColor;
            Sniper.Width = 20;
            Sniper.Height = 20;
            MainCanvas.Children.Add(Sniper);
            Canvas.SetLeft(Sniper, SniperX - Sniper.Width / 2);
            Canvas.SetTop(Sniper, SniperY - Sniper.Height / 2);
            Draw_Sniper_Eye(false);
        }


        private void Draw_Sniper_Eye(bool Update = true)
        {
            if (Update == false)
            {
                SniperEye.Fill = Brushes.White;
                SniperEye.Width = 5;
                SniperEye.Height = 5;
                MainCanvas.Children.Add(SniperEye);
                Canvas.SetLeft(Sniper, SniperX - Sniper.Width / 2);
                Canvas.SetTop(Sniper, SniperY - Sniper.Height / 2);
            }
            double dmx = MoveTo(5, SniperX - Sniper.Width / 2, SniperY - Sniper.Height / 2, MousePositionX, MousePositionY);
            double dmy = MoveTo(5, SniperY - Sniper.Height / 2, SniperX - Sniper.Width / 2, MousePositionY, MousePositionX);
            dmx *= -1;
            dmy *= -1;
            //dmx += Sniper.Width / 2;
            //dmy += Sniper.Height / 2;
            Canvas.SetLeft(SniperEye, SniperX - SniperEye.Width / 2 + dmx);
            Canvas.SetTop(SniperEye, SniperY - SniperEye.Height / 2 + dmy);
        }

        private double MoveTo(double Speed,double x1, double y1, double x2, double y2)
        {
            return Speed * (x1 - x2) / distance(x1, y1, x2, y2);
        }

        private void randombullet()
        {
            if (ra.Next(30) == 0)
            {
                createNewBullet(2);
            }
            else
            if (ra.Next(10) == 0)
            {
                for (int i = 0; i < 100 + ra.Next(50); i++)
                {
                    createNewBullet(3);
                }
            }
            else
                if (ra.Next(1000) == 0)
            {
                createNewBullet(4);
            }
            else
                createNewBullet(1);
        }

        private void createNewBullet(int type)
        {
            // type 1 = normal
            if (bullets_alive == bullets.Length)
                Array.Resize<Bullet>(ref bullets, bullets.Length + 100);

            bullets[bullets_alive].ellipsebullet = new Ellipse();

            bullets[bullets_alive].Alive = "Alive";
            if (type == 1)
            {
                bullets[bullets_alive].Type = "Normal";
                bullets[bullets_alive].X = SniperX;
                bullets[bullets_alive].Y = SniperY;
                bullets[bullets_alive].Size = 10;
                bullets[bullets_alive].DestinyX = MousePositionX;
                bullets[bullets_alive].DestinyY = MousePositionY;
                bullets[bullets_alive].Speed = 5;


                bullets[bullets_alive].ellipsebullet.Fill = SimpleBulletColor;
            }
            else
                if (type == 2)
            {
                bullets[bullets_alive].Type = "Life";
                bullets[bullets_alive].X = SniperX;
                bullets[bullets_alive].Y = SniperY;
                bullets[bullets_alive].Size = 5;
                bullets[bullets_alive].DestinyX = ra.Next(totalWidth);
                bullets[bullets_alive].DestinyY = ra.Next(totalHeight);
                bullets[bullets_alive].Speed = 3;


                bullets[bullets_alive].ellipsebullet.Fill = LifeBulletColor;
            }
            else
                if (type == 3)
            {

                bullets[bullets_alive].Type = "Normal";
                bullets[bullets_alive].X = SniperX;
                bullets[bullets_alive].Y = SniperY;
                bullets[bullets_alive].Size = 10;
                bullets[bullets_alive].DestinyX = ra.Next(totalWidth);
                bullets[bullets_alive].DestinyY = ra.Next(totalHeight);
                bullets[bullets_alive].Speed = 2;


                bullets[bullets_alive].ellipsebullet.Fill = SimpleBulletColor;

            }
            else
               if (type == 4)
            {
                bullets[bullets_alive].Type = "Remover";
                bullets[bullets_alive].X = SniperX;
                bullets[bullets_alive].Y = SniperY;
                bullets[bullets_alive].Size = 7;
                bullets[bullets_alive].DestinyX = ra.Next(totalWidth);
                bullets[bullets_alive].DestinyY = ra.Next(totalHeight);
                bullets[bullets_alive].Speed = 2;


                bullets[bullets_alive].ellipsebullet.Fill = RemoverBulletColor;
            }


            bullets[bullets_alive].ellipsebullet.Width = bullets[bullets_alive].Size;
            bullets[bullets_alive].ellipsebullet.Height = bullets[bullets_alive].Size;
            MainCanvas.Children.Add(bullets[bullets_alive].ellipsebullet);
            Canvas.SetLeft(bullets[bullets_alive].ellipsebullet, bullets[bullets_alive].X - bullets[bullets_alive].ellipsebullet.Width / 2);
            Canvas.SetTop(bullets[bullets_alive].ellipsebullet, bullets[bullets_alive].Y - bullets[bullets_alive].ellipsebullet.Height / 2);
            bullets_alive++;
            ScoreTextBlock.Text = bullets_alive.ToString();
            //bulletArray[bullets_alive] = bulletArray[bullets_alive];
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Point point = Mouse.GetPosition(MainCanvas);
            MousePositionX = (int)point.X;//System.Windows.Forms.Control.MousePosition.X;
            MousePositionY = (int)point.Y;//System.Windows.Forms.Control.MousePosition.Y;

            CursorDot.Fill = CursorColor;
            CursorDot.Width = 10;
            CursorDot.Height = 10;
            Canvas.SetLeft(CursorDot, MousePositionX - CursorDot.Width / 2);
            Canvas.SetTop(CursorDot, MousePositionY - CursorDot.Height / 2);
        }

        public double distance(double X1, double Y1, double X2, double Y2)
        {
            return Math.Sqrt(Math.Pow(Math.Abs(X1 - X2), 2) + Math.Pow(Math.Abs(Y1 - Y2), 2));
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            totalHeight = (int)((System.Windows.Controls.Panel)Application.Current.MainWindow.Content).ActualHeight;
            totalWidth = (int)((System.Windows.Controls.Panel)Application.Current.MainWindow.Content).ActualWidth;
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Go == false)
            {
                Go = true;
                MainCanvas.Children.Remove(skylt1);
                MainCanvas.Children.Remove(skylt2);
                CursorColor.Color = System.Windows.Media.Color.FromArgb(255, 255, 255, 255);//Sets the color on the white cursor
            }
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Go == true && (Life > 0))
            {
                Life = 0;
                Game_Over();
            }
        }
    }
}