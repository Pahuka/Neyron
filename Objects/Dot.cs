using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows.Shapes;

namespace Neyron.Objects
{
    class Dot
    {
        private static int GlobalId { get; set; }
        //public Vector3 Target { get; set; }
        public string DotClass { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public Ellipse Form { get; set; }
        public int Id { get; set; }

        public Dot(Ellipse form, string dotClass)
        {            
            GlobalId++;
            Id = GlobalId;
            Form = form;
            DotClass = dotClass;
            X = 0;
            Y = 0;
        }

        public Ellipse Show()
        {
            return Form;
        }
    }
}
