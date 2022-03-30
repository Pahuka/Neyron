using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Neyron
{
    public class Pixel
    {
        private static int GlobalId { get; set; }
        //public Label Clan { get; set; }
        public Shape Form { get; set; }
        public Vector3 Position { get; set; }
        public Rect RectForm { get; set; }
        public string DotClass { get; set; }
        public int Score { get; set; }
        public AI PixelAI { get; set; }
        //public int Attack { get; set; }
        public int Health { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public int Id { get; set; }

        public Pixel(Shape form, string dotClass)
        {
            GlobalId++;
            Id = GlobalId;
            Health = 10000;
            Form = form;
            DotClass = dotClass;
            //Attack = new Random().Next(1, 21);
            //Clan = new Label() { 
            //    Content = $"{new Random().Next(1, 11)}",
            //    IsEnabled = false, 
            //    FontWeight = FontWeights.Bold,
            //    FontSize = Form.Width / 2,
            //    HorizontalAlignment = HorizontalAlignment.Center,
            //    VerticalAlignment = VerticalAlignment.Center
            //};
        }

        public void Move()
        {
            X += Position.X;
            Y += Position.Y;
        }

        public void ChangeColor(Brush color)
        {
            Form.Fill = color;
        }

        public Shape Show()
        {
            return Form;
        }
    }
}