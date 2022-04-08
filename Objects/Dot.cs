using Neyron.Brain;
using Neyron.GNN;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Neyron.Objects
{
    public class Dot
    {
        private static int GlobalId { get; set; }
        //public Vector3 Target { get; set; }
        public string DotClass { get; set; }
        //public NeuronNetwork Brain { get; set; }
        public BloopBrain Brain { get; set; }
        public int Clan { get; set; }
        public float Speed { get; set; }
        public int Attack { get; set; }
        public int Neighbours { get; set; }
        public Vector2 Position { get; set; }
        public int Health { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public Shape Form { get; set; }
        public Rect RectForm { get; set; }
        public int Id { get; set; }

        public Dot()
        {

        }

        public Dot(Shape form, string dotClass, BloopBrain brain)
        {            
            GlobalId++;
            Id = GlobalId;
            Form = form;
            DotClass = dotClass;
            Brain = brain;
            Health = 2000;
            Attack = new Random().Next(1, 100);
            Speed = 1.0f;
            Clan = new Random().Next(1, 6);
            //var point = new Point(X, Y);
            //var size = new Size(form.Height, form.Width);
            //RectForm = new Rect();
        }

        public Vector2 GetVector()
        {
            Position = new Vector2((float)X, (float)Y);
            return Position;
        }

        public void Move(double x, double y)
        {
            var angle = Math.Atan2(X - x, Y - y);
            X -= Speed * Math.Sin(angle);
            Y -= Speed * Math.Cos(angle);
        }

        public Shape Show()
        {
            return Form;
        }
    }
}
