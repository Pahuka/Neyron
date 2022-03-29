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
        public int Score { get; set; }
        //public Vector2 Position { get; set; }
        public int Health { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public Shape Form { get; set; }
        public Rect RectForm { get; set; }
        public int Id { get; set; }

        public Dot(Shape form, string dotClass, BloopBrain brain)
        {            
            GlobalId++;
            Id = GlobalId;
            Form = form;
            DotClass = dotClass;
            Brain = brain;
            Health = 30000;
            //var point = new Point(X, Y);
            //var size = new Size(form.Height, form.Width);
            //RectForm = new Rect();
        }

        public void Move()
        {
            //var point = new Point(X, Y);
            //var size = new Size(Form.Height, Form.Width);
            RectForm = new Rect(X, Y, 25, 25);
        }

        public Shape Show()
        {
            return Form;
        }
    }
}
