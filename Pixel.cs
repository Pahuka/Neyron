using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Neyron
{
    class Pixel
    {
        private static int GlobalId { get; set; }
        private Ellipse form;
        public int Attack { get; set; }
        public int Healh { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Id { get; set; }

        public Pixel(Ellipse form, int x, int y)
        {
            this.form = form;
            X = x;
            Y = y;
            GlobalId++;
            Id = GlobalId;
            Healh = new Random().Next(100, 256);
            //Attack = new Random().Next(1, 51);
            Attack = 1;
        }

        public void ChangeColor(Brush color)
        {
            form.Fill = color;
        }

        public Ellipse Show()
        {
            return form;
        }
    }
}