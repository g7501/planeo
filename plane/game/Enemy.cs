using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace plane.game
{
    internal class Enemy : GameObject
    {
        public Enemy(string tag, float height, float width, Brush fillColor, SolidColorBrush strokeColor) : base(tag, height, width, fillColor, strokeColor)
        {

        }
    }
}
