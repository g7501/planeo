using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace plane
{
    class Bullet : GameObject {
        
        public Bullet(string tag, float height, float width, Brush fillColor, SolidColorBrush strokeColor) : base(tag, height, width, fillColor, strokeColor)
        {
    
        }

        public void moveUp(Bullet bullet, Rectangle rectangle) 
        {
            Canvas.SetTop(bullet.getRectangle(), Canvas.GetTop(rectangle) - bullet.getRectangle().Height);
            Canvas.SetLeft(bullet.getRectangle(), Canvas.GetLeft(rectangle) + rectangle.Width / 2);
        }

        public void moveDown(Rectangle rectangle) 
        {
            Canvas.SetTop(rectangle, Canvas.GetTop(rectangle) + rectangle.Height);
        }
    }
}
