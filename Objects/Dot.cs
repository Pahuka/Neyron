using Neyron.Brain;
using Neyron.GNN;
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
        //public NeuronNetwork Brain { get; set; }
        public BloopBrain Brain { get; set; }
        public int Attack { get; set; }
        //public Vector2 Position { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public Ellipse Form { get; set; }
        public int Id { get; set; }

        public Dot(Ellipse form, string dotClass, BloopBrain brain)
        {            
            GlobalId++;
            Id = GlobalId;
            Form = form;
            DotClass = dotClass;
            Brain = brain;
        }

        //public void Move()
        //{
        //    X += Position.X;
        //    Y += Position.Y;
        //}

        public Ellipse Show()
        {
            return Form;
        }
    }
}
