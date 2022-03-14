using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;

namespace Neyron
{
    class Pixel
    {
        private Ellipse form;
        public int X { get; set; }
        public int Y { get; set; }

        public Pixel(Ellipse form, int x, int y)
        {
            this.form = form;
            X = x;
            Y = y;
        }

        public Ellipse Show()
        {
            return form;
        }
    }
}
