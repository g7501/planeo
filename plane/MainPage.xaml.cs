using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.UI;
using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Windows.UI.Xaml.Media.Imaging;
using System.Drawing;
using Windows.UI.Xaml.Shapes;
using Rectangle = Windows.UI.Xaml.Shapes.Rectangle;
using plane.game;
using System.Diagnostics;



// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace plane
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        List<Bullet> bullets = new List<Bullet>();
        List<Enemy> enemies = new List<Enemy>();
        List<Bullet> enemyBullets = new List<Bullet>();
        Player player;

        Stopwatch stopwatch = new Stopwatch();

        bool a;
        bool d;
        bool fire;

        public MainPage()
        {
            this.InitializeComponent();

            ImageBrush playerSkin = new ImageBrush();
            playerSkin.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/space.png"));
            player = new Player("player", 80, 80, playerSkin, new SolidColorBrush(Colors.Black));
            Canvas.SetTop(player.getRectangle(), 760);
            addGameObject(player);

            //I have added keyup to make the player move smother
            Window.Current.CoreWindow.KeyDown += KeyEventHandler;
            Window.Current.CoreWindow.KeyUp += KeyEventHandler;

            make();

            _timer.Interval = TimeSpan.FromMilliseconds(0.01666666666);
            _timer.Tick += Tick;
            _timer.Start();            
        }
        //
        private void addGameObject(GameObject gb)
        {
            Canvas.SetTop(gb.getRectangle(), Canvas.GetTop(gb.getRectangle()));
            Canvas.SetLeft(gb.getRectangle(), Canvas.GetLeft(gb.getRectangle()));
            
            canvas.Children.Add(gb.getRectangle());
        }

        //This mothod will call the UpdateKeyState everytime a key got pressed
        private void KeyEventHandler(CoreWindow sender, KeyEventArgs args)
        { 
            UpdateKeyState();
        }
        //This mothod have all keys
        private void UpdateKeyState()
        {
            //Key A is down
            if ((Window.Current.CoreWindow.GetKeyState(VirtualKey.A) &
                 Windows.UI.Core.CoreVirtualKeyStates.Down)
                 == CoreVirtualKeyStates.Down)
            {
                a = true;
             //Key A is up
            }else if ((Window.Current.CoreWindow.GetKeyState(VirtualKey.A) &
                 Windows.UI.Core.CoreVirtualKeyStates.None)
                 == CoreVirtualKeyStates.None)
            {
                a = false;
            }
            //Key D is Down
            if ((Window.Current.CoreWindow.GetKeyState(VirtualKey.D) &
                 Windows.UI.Core.CoreVirtualKeyStates.Down)
                 == CoreVirtualKeyStates.Down)
            {
                d = true;
            //Key D is Up
            }else if ((Window.Current.CoreWindow.GetKeyState(VirtualKey.D) &
                Windows.UI.Core.CoreVirtualKeyStates.None)
                == CoreVirtualKeyStates.None)
            {
                d = false;
            }

            //Space is pressed
            if ((Window.Current.CoreWindow.GetKeyState(VirtualKey.Space) &
                Windows.UI.Core.CoreVirtualKeyStates.Down)
                == CoreVirtualKeyStates.Down)
            {
                /*
                SolidColorBrush blue = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 191, 255, 41));
                Brush red = new SolidColorBrush(Windows.UI.Color.FromArgb(23, 23, 234, 41));
                r = new Rectangle
                {
                    Tag = "Bullet",
                    Height = 20,
                    Width = 5,
                    Fill = blue,
                    Stroke = red,
                };
                Canvas.SetTop(r, Canvas.GetTop(player) - r.Height);
                Canvas.SetLeft(r, Canvas.GetLeft(player) + player.Width / 2);


                canvas.Children.Add(r);

                fire0 = true;
                */

                Bullet bullet = new Bullet("bullet", 20, 5, new SolidColorBrush(Colors.White), new SolidColorBrush(Colors.Blue));

                //bullet.setY(player.getY());
                //bullet.setX(player.getX());
                bullets.Add(bullet);
                bulletPos(bullet, player.getRectangle());
                addGameObject(bullet);  
            }
        }
        // This method will help postion the bullet near the player
        private void bulletPos(Bullet bullet, Rectangle rectangle)
        {
            Canvas.SetTop(bullet.getRectangle(), Canvas.GetTop(rectangle) -bullet.getRectangle().Height);
            Canvas.SetLeft(bullet.getRectangle(), Canvas.GetLeft(rectangle) + rectangle.Width / 2);
        }

        //This mehtod will help the bullet move to the top
        private void moveBullet(Rectangle rec)
        {
            Canvas.SetTop(rec, Canvas.GetTop(rec) - 10);
        }

        //THis mothod will help the ememy bullet move toword the player
        private void moveEnemyBullet(Rectangle rec)
        {
            Canvas.SetTop(rec, Canvas.GetTop(rec) + 10);
        }

        // This is the time of the game
        private void Tick(object sender, object e)
        {
            //Calling method move to move the bullet to the top
            for (int i = 0; i < bullets.Count; i++)
                moveBullet(bullets[i].getRectangle());

            for (int i = 0; i < enemyBullets.Count; i++)
            {
                moveEnemyBullet(enemyBullets[i].getRectangle());
            }

            // setting the TEXT to bulletcount for montaining
           // txtCount.Text = "count: " + butlletcount;

            //Player movement
            if (a && Canvas.GetLeft(player.getRectangle()) > 20)
            {
                Canvas.SetLeft(player.getRectangle(), Canvas.GetLeft(player.getRectangle()) - 8);
            }

            if (d && Canvas.GetLeft(player.getRectangle()) + 80 < 1180)
            {
                Canvas.SetLeft(player.getRectangle(), Canvas.GetLeft(player.getRectangle()) + 8);
            }

            //Ememies interaction
            for (int i = 0; i < bullets.Count; i++)
            {
                for (int y = 0; y < enemies.Count; y++)
                {       // Removimg the enemy & the bullet on the collison
                        if (checkCollision(bullets[i].getRectangle(), enemies[y].getRectangle()))
                        {
                            canvas.Children.Remove(enemies[y].getRectangle());
                            canvas.Children.Remove(bullets[i].getRectangle());
                            enemies.RemoveAt(y);
                            bullets.RemoveAt(i);

                        return;
                        }
                }
                //Removing the bullet at the end of the screen
                if (Canvas.GetTop(bullets[i].getRectangle()) < 0) 
                {
                    canvas.Children.Remove(bullets[i].getRectangle());
                    bullets.RemoveAt(i);
                }
            }

            //Move the enemy to the right and appear from the left. + go down at the end of the rotation
            float plus = 5;
            for (int i = 0; i < enemies.Count; i++)
            {
                double y = Canvas.GetLeft(enemies[i].getRectangle());
                Canvas.SetLeft(enemies[i].getRectangle(), y + plus);

                if(y + plus > 1200 - enemies[i].getRectangle().ActualWidth) {
                    Canvas.SetLeft(enemies[i].getRectangle(), 0);
                    Canvas.SetTop(enemies[i].getRectangle(), Canvas.GetTop(enemies[i].getRectangle()) + enemies[i].getRectangle().ActualHeight);
                }
            }

            //Time to make the enemy shoot
            stopwatch.Start();
            if(stopwatch.ElapsedMilliseconds < 2000)
            {
                

                //Canvas.GetLeft(enemies[0].getRectangle());
                txtCount.Text = "Time: " + stopwatch.ElapsedMilliseconds;

            }

            if (stopwatch.ElapsedMilliseconds > 6000)
            {
                Bullet enemyBullet = new Bullet("enemy_bullet", 20, 5, new SolidColorBrush(Colors.White), new SolidColorBrush(Colors.Red));
                txtCount.Text = "Time2: " + stopwatch.ElapsedMilliseconds;
                bulletPos(enemyBullet, enemies[1].getRectangle());
                enemyBullets.Add(enemyBullet);
                addGameObject(enemyBullet);
                stopwatch.Restart();
            }


        }

        // Making the enemy
        private void make()
        {
            int left = 800;

            for (int i = 0; i < 10; i++)
            {
                ImageBrush skin = new ImageBrush();
                skin.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/invader.png"));

                /*Rectangle enemy = new Rectangle
                {
                    Tag = "enemy",
                    Height = 45,
                    Width = 45,
                    Fill = skin,
                };*/

                Enemy enemy = new Enemy("enemy", 45, 45, skin, new SolidColorBrush(Colors.Black));
                enemies.Add(enemy);

                Canvas.SetTop(enemy.getRectangle(), 10);
                Canvas.SetLeft(enemy.getRectangle(), left);

                canvas.Children.Add(enemy.getRectangle());

                left -= 60;

            }
        }
        //Check if there is a collision
        public static bool checkCollision(Rectangle e1, Rectangle e2)
        {
            var r1 = e1.ActualWidth / 2;
            var x1 = Canvas.GetLeft(e1) + r1;
            var y1 = Canvas.GetTop(e1) + r1;
            var r2 = e2.ActualWidth / 2;
            var x2 = Canvas.GetLeft(e2) + r2;
            var y2 = Canvas.GetTop(e2) + r2;
            var d = new Vector2((float)(x2 - x1), (float)(y2 - y1));
            return d.Length() <= r1 + r2;
        }
    }
}
