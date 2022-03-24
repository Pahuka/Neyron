using Neyron.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

public class MainController
{
    public Canvas myCanvas;
    Dictionary<int, Dot> dots;
    Dictionary<int, Dot> foods;
    Random random = new Random();

    private int frame = 0;

    public MainController(Canvas canvas, Dictionary<int, Dot> dots, Dictionary<int, Dot> foods)
    {
        this.myCanvas = canvas;
        this.dots = dots;
        this.foods = foods;
    }

    // Start is called before the first frame update
    //public void Start()
    //{
    //    Evolution();
    //}

    public void CreateDots(int dotCount)
    {
        for (int i = 0; i < dotCount; i++)
        {
            var dot = new Dot(new Ellipse()
            {
                Height = 25,
                Width = 25,
                Fill = new SolidColorBrush(Color.FromRgb(0, (byte)random.Next(100, 256), 33))
            }, "");
            dot.DotAI = new AI(dot);
            dots.Add(dot.Id, dot);
            Canvas.SetTop(dot.Show(), dot.X);
            Canvas.SetLeft(dot.Show(), dot.Y);
            myCanvas.Children.Add(dot.Show());
        }
    }

    public void Evolution(object o, EventArgs e)
    {
        for (int i = 0; i < 100; i++)
        {
            Genome genome = new Genome(64);
            foreach (var item in dots.Values)
            {
                item.Position = new Vector2((float)random.NextDouble(), (float)random.NextDouble());
                item.DotClass = "bacterium";
                item.Move();
                item.DotAI.Init(genome);
                item.DotAI.FixedUpdate(foods.Values.ToArray());
                Canvas.SetTop(item.Show(), item.X);
                Canvas.SetLeft(item.Show(), item.Y);
            }
        }
        //for (int i = 0; i < 1000; i++)
        //{
        //    foreach (var item in foods.Values)
        //    {
        //        GameObject food = Instantiate(this.food, new Vector3(Random.Range(-area.x, area.x), Random.Range(-area.y, area.y), 0), Quaternion.identity);
        //        food.name = "food";
        //    }
        //}
    }

    //void FixedUpdate()
    //{
    //    if (frame % 1 == 0)
    //    {
    //        var food = Instantiate(this.food, new Vector3(Random.Range(-area.x, area.x), Random.Range(-area.y, area.y), 0), Quaternion.identity);
    //        food.name = "food";
    //    }
    //    frame++;
    //}

    // Update is called once per frame
    //void Update()
    //{

    //}
}