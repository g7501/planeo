﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace plane
{
    internal class Player : GameObject
    {
        byte lifes;
        public Player(string tag, float height, float width, Brush fillColor, SolidColorBrush strokeColor) : base(tag, height, width, fillColor, strokeColor)
        {
        }

        public byte Lifes { get => lifes; set => lifes = value; }
    }
}
