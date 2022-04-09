using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace plane.game
{
    internal class Player : GameObject
    {
        public Player(string tag, float height, float width, Brush fillColor, SolidColorBrush strokeColor) : base(tag, height, width, fillColor, strokeColor)
        {
        }

        
    }
}
