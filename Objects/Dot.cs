using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows.Shapes;

namespace Neyron.Objects
{
    public class Dot
    {
        private static int GlobalId { get; set; }
        //public Vector3 Target { get; set; }
        public string DotClass { get; set; }
        public AI DotAI { get; set; }
        public int Attack { get; set; }
        public Vector2 Position { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public Ellipse Form { get; set; }
        public int Id { get; set; }

        public Dot(Ellipse form, string dotClass)
        {            
            GlobalId++;
            Id = GlobalId;
            Form = form;
            DotClass = dotClass;
            Position = new Vector2(X, Y);
        }

        public void Move()
        {
            X += Position.X;
            Y += Position.Y;
        }

        public Ellipse Show()
        {
            return Form;
        }
    }
}
