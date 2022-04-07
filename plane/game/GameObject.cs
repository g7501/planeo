using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;

namespace plane.game
{
    class GameObject
    {
        protected Rectangle r = new Rectangle();
        
        public GameObject(string tag, float height, float width, Brush fillColor, SolidColorBrush strokeColor) 
        {
            r = new Rectangle
            {
                Tag = tag,
                Height = height,
                Width = width,
                Fill = fillColor,
                Stroke = strokeColor,
            };
        }

        public Rectangle getRectangle()
        {
            return r;
        }
    }
}
