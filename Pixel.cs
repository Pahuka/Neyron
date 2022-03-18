using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Neyron
{
    class Pixel
    {
        private static int GlobalId { get; set; }
        public TextBox Clan { get; set; }
        public Ellipse Form { get; set; }
        public int Attack { get; set; }
        public int Healh { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Id { get; set; }

        public Pixel(Ellipse form, int x, int y)
        {
            X = x;
            Y = y;
            GlobalId++;
            Id = GlobalId;
            Healh = new Random().Next(100, 256);
            Form = form;
            Attack = new Random().Next(1, 21);
            Clan = new TextBox() { Text = $"{new Random().Next(1, 11)}", Height = 20, Width = 20, IsEnabled = false };
        }

        public void ChangeColor(Brush color)
        {
            Form.Fill = color;
        }

        public Ellipse Show()
        {
            return Form;
        }
    }
}