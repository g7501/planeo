﻿using System;
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
        TextBlock txtScore;
        Stopwatch stopwatch;
        //Keys 'A' and 'D'
        bool a;
        bool d;
        // Move the enemies to to left/right
        bool moveDirection = true, moveDown;
        //Enemies speed
        float plus = 2;
        //Start/pause game
        bool startGame;
        //Score of the game
        int score = 0;


        // Problems
        /*
         0- I spent 3 days just to find out how to use onKey up/down.
         1- I had the player movment inside the on key press which is making the player movement glichy
            -> TO solve this I made a bool and move the player inside the tick
            I had another problem with the movments is when the user presses 'A' the player will move to the
            left and will not stop till another key is pressed.
            -> To solve this one I had to call the class onKeyUp not only onKeyDown
         2- Making bullet for the player, first I made 7 bullets in the XAML and use them when the user 
            press space, but this have a lot of dropbacks. I tried to make an object form the code it
            took me so much time to make one bucause apperntlly thier was 2 classes to import in the same
            object and I was using the wrong one.
        3- I had problem with positioning the emeies and move them around
        4- The code was too missy and I had to orgnise it.
        5- I had a problem with reset the game. When I reset the game the object will be hidden
            So, they are still there but invisable.
            -> The solution for that is I made another class to clear the objects not only the canvas.
        6- If the player shoot a lost of bullets the game will start lagging because the bullet will keep
            going off screen.
            The soloution for this destroying the object form the game when it tach the edge of the schreen.
        - I had a problem with the text. When I restart the game the text box will be removed. This is because I am clearing the canvas canvas.children.clear
            -> The sloluotion for this will be removing it form XAML and make a new object form the code
            
        Problems not solved yet
            -Enemy spawn
            -When all enemies destried nothing happen
            -No message dissplay
            -Player can bullets are overpowerd need to put a cooldown
            -Maybe add another life for the player
            -No music/sound
       
        Good
            Bullets shape and colours
            player/enemy skins
            60 FPS. The game is smooth
            
            
            
         */
        public MainPage()
        {
            this.InitializeComponent();

            //I have added keyup to make the player move smother
            Window.Current.CoreWindow.KeyDown += KeyEventHandler;
            Window.Current.CoreWindow.KeyUp += KeyEventHandler;

            //make();
            spon();

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
            //if enter is pressed
            if ((Window.Current.CoreWindow.GetKeyState(VirtualKey.Enter) &
                 Windows.UI.Core.CoreVirtualKeyStates.Down)
                 == CoreVirtualKeyStates.Down)
            {
                startGame = true;
            }

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
            Canvas.SetTop(rec, Canvas.GetTop(rec) - player.getSpeed());
        }

        //THis mothod will help the ememy bullet move toword the player
        private void moveEnemyBullet(Rectangle rec)
        {
            Canvas.SetTop(rec, Canvas.GetTop(rec) + player.getSpeed());
        }

        // This is the time of the game
        private void Tick(object sender, object e)
        {
            //if this boolean is false don't start the game
            if (!startGame)
                return;

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
                Canvas.SetLeft(player.getRectangle(), Canvas.GetLeft(player.getRectangle()) - player.getSpeed());
            }

            if (d && Canvas.GetLeft(player.getRectangle()) + 80 < 1180)
            {
                Canvas.SetLeft(player.getRectangle(), Canvas.GetLeft(player.getRectangle()) + player.getSpeed());
            }

            //Ememies interaction with player bullet
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
                            score += 10;

                        txtScore.Text = "SCORE " + score;

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

            //enemies bullet interatction
            for (int i = 0; i < enemyBullets.Count; i++)
            {
                /*if (player == null)
                {
                    return;
                }*/

                // Removimg the enemy & the bullet on the collison
                if (checkCollision(enemyBullets[i].getRectangle(), player.getRectangle()))
                {
                    canvas.Children.Remove(enemyBullets[i].getRectangle());
                    canvas.Children.Remove(player.getRectangle());

                    //To remove the player from the game I need to destroy the boject
                   
                    startGame = false;

                    enemyBullets.RemoveAt(i);

                    clear();
                    spon();
                    return;
                }
                
                //Removing the bullet at the end of the screen
                if (Canvas.GetTop(enemyBullets[i].getRectangle()) > 800)
                {
                    canvas.Children.Remove(enemyBullets[i].getRectangle());
                    enemyBullets.RemoveAt(i);
                }
            }



            //Move the enemy to the right and appear from the left. + go down at the end of the rotation
            for (int i = 0; i < enemies.Count; i++)
            {   
                double x = Canvas.GetLeft(enemies[i].getRectangle());

                if (moveDirection)
                {
                    if (i == 0 && x > 1000)
                    {
                        moveDirection = !moveDirection;
                        plus = -plus;
                        moveDown = true;
                    }
                }

                Canvas.SetLeft(enemies[i].getRectangle(), x + plus);

                if(!moveDirection)
                {
                    if (i == enemies.Count - 1 && x < 200)
                    {
                        plus = -plus;
                        moveDirection = !moveDirection;
                    }
                }

                if(moveDown) {
                    Canvas.SetTop(enemies[i].getRectangle(), Canvas.GetTop(enemies[i].getRectangle()) + 5);
                }

                if(i == enemies.Count - 1) {
                    moveDown = false;
                }


                /*if(y + plus > 1200 - enemies[i].getRectangle().ActualWidth) {
                    Canvas.SetLeft(enemies[i].getRectangle(), 0);
                    Canvas.SetTop(enemies[i].getRectangle(), Canvas.GetTop(enemies[i].getRectangle()) + enemies[i].getRectangle().ActualHeight);
                }*/
            }

            //Time to make the enemy shoot
            stopwatch.Start();
            if (stopwatch.ElapsedMilliseconds > 500)
            {
                Bullet enemyBullet = new Bullet("enemy_bullet", 20, 5, new SolidColorBrush(Colors.White), new SolidColorBrush(Colors.Red));
                

                Random rnd = new Random();
                int random = rnd.Next(0, enemies.Count);

                //txtCount.Text = "random: " + random;

                if (random < enemies.Count)
                {
                    bulletPos(enemyBullet, enemies[random].getRectangle());
                    enemyBullets.Add(enemyBullet);
                    addGameObject(enemyBullet);
                    stopwatch.Restart();
                }
            }


        }

        // Making the enemy
        private void spon()
        {
            stopwatch = new Stopwatch();
            ImageBrush playerSkin = new ImageBrush();
            playerSkin.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/space.png"));
            player = new Player("player", 80, 80, playerSkin, new SolidColorBrush(Colors.Black));
            Canvas.SetTop(player.getRectangle(), 760);
            addGameObject(player);
            txtScore = new TextBlock();

            Canvas.SetLeft(txtScore,20);
            Canvas.SetTop(txtScore,20);
            canvas.Children.Add(txtScore);

            txtScore.Text = "SCORE ";
            txtScore.FontSize = 30;
            

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

        //Clear everything
        // I had a problem when I restart the game and not clean the objects (Hiden objects and reandom restarts...ect)
        private void clear() 
        {
            canvas.Children.Clear();
            stopwatch = null;
            txtScore = null;
            player = null;
            enemies.Clear();
            enemyBullets.Clear();
            bullets.Clear();
            score = 0;
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
