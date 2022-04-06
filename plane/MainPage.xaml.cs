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



// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace plane
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        Rectangle r;

        //bullet counter
        int butlletcount = 0;
        //The player have 7 bullets
        bool fire0;
        bool fire1;
        bool fire2;
        bool fire3;
        bool fire4;
        bool fire5;
        bool fire6;

        bool a;
        bool d;

        public MainPage()
        {
            this.InitializeComponent();

            _timer.Interval = TimeSpan.FromMilliseconds(10);
            _timer.Tick += Tick;
            _timer.Start();

            make();

            //I have added keyup to make the player move smother
            Window.Current.CoreWindow.KeyDown += KeyEventHandler;
            Window.Current.CoreWindow.KeyUp += KeyEventHandler;

            make();
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

                butlletcount++;

                if (butlletcount == 1)
                {
                    fire0 = true;
                    bulletPos(bullet0);
                }

                if (butlletcount == 2)
                {
                    fire1 = true;
                    bulletPos(bullet1);
                }

                if (butlletcount == 3)
                {
                    fire2 = true;
                    bulletPos(bullet2);
                }

                if (butlletcount == 4)
                {
                    fire3 = true;
                    bulletPos(bullet3);
                }

                if (butlletcount == 5)
                {
                    fire4 = true;
                    bulletPos(bullet4);
                }

                if (butlletcount == 6)
                {
                    fire5 = true;
                    bulletPos(bullet5);
                }

                if (butlletcount == 7)
                {
                    fire6 = true;
                    bulletPos(bullet6);
                    butlletcount = 0;
                }
                //I had if butlletcount == 7 do butlletcount = 1;
                //I said why I am making a delay just remove it and put it in the last one
                //I forgot if  (butlletcount == 6) it should be 7 but I had 2*6 So I had more delay

            }
        }
        // This method will help postion the bullet near the player
        private void bulletPos(Windows.UI.Xaml.Shapes.Rectangle rec)
        {
            Canvas.SetTop(rec, Canvas.GetTop(player) - rec.Height);
            Canvas.SetLeft(rec, Canvas.GetLeft(player) + player.Width / 2);
        }

        //This mehtod will help the bullet move to the top
        private void moveBullet(Windows.UI.Xaml.Shapes.Rectangle rec)
        {
            Canvas.SetTop(rec, Canvas.GetTop(rec) - 10);
        }

        // This is the time of the game
        private void Tick(object sender, object e)
        {
            
            //if fire is true Calling move method
            if (fire0)
            {
                moveBullet(bullet0);
            }

            if (fire1)
            {
                moveBullet(bullet1);
            }

            if (fire2)
            {
                moveBullet(bullet2);
            }

            if (fire3)
            {
                moveBullet(bullet3);
            }

            if (fire4)
            {
                moveBullet(bullet4);
            }

            if (fire5)
            {
                moveBullet(bullet5);
            }

            if (fire6)
            {
                moveBullet(bullet6);
            }
            // setting the TEXT to bulletcount for montaining
            txtCount.Text = "count: " + butlletcount;

            //Player movement
            if (a && Canvas.GetLeft(player) > 20)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - 8);
            }

            if (d && Canvas.GetLeft(player) + 80 < 1180)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + 8);
            }

        }

        private void make()
        {
            int left = 800;

            for (int i = 0; i < 10; i++)
            {
                ImageBrush skin = new ImageBrush();

                Rectangle enemy = new Rectangle
                {
                    Tag = "enemy",
                    Height = 45,
                    Width = 45,
                    Fill = skin,
                };

                Canvas.SetTop(enemy, 10);
                Canvas.SetLeft(enemy, left);
                
                canvas.Children.Add(enemy);
                left -= 60;

                skin.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/invader.png"));
                
            }
            left = 800;

            for (int i = 0; i < 10; i++)
            {
                ImageBrush skin = new ImageBrush();

                Rectangle enemy = new Rectangle
                {
                    Tag = "enemy",
                    Height = 45,
                    Width = 45,
                    Fill = skin,
                };

                Canvas.SetTop(enemy, 80);
                Canvas.SetLeft(enemy, left);

                canvas.Children.Add(enemy);
                left -= 60;

                skin.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/invader.png"));

            }
        }
    }
}
