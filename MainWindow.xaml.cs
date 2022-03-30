using Neyron.Brain;
using Neyron.GNN;
using Neyron.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Neyron
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Dictionary<int, Dot> pixels = new Dictionary<int, Dot>();
        //Dictionary<int, Dot> targets = new Dictionary<int, Dot>();
        Dictionary<int, Pixel> targets = new Dictionary<int, Pixel>();
        Dictionary<int, Pixel> pixels = new Dictionary<int, Pixel>();
        int tick = 0;
        Random random = new Random();
        System.Windows.Threading.DispatcherTimer myTimer = new System.Windows.Threading.DispatcherTimer();
        //Topology Topology = new Topology(4, 4, 0.1, 4);
        //NeuronNetwork NN;
        //BloopBrain brain = new BloopBrain();
        //NN AI;

        public MainWindow()
        {
            InitializeComponent();

            addPixel.IsEnabled = false;
            myTimer.Tick += Move;
            //myTimer.Tick += MN.Evolution;
            myTimer.Interval = new TimeSpan(0, 0, 0, 0, int.Parse(speed.Text));
        }

        //private Tuple<int, int> GenerateStep(Pixel pixel, int x, int y)
        //{
        //    var nextX = (x + pixel.X + myGrid.ColumnDefinitions.Count) % myGrid.ColumnDefinitions.Count;
        //    var nextY = (y + pixel.Y + myGrid.RowDefinitions.Count) % myGrid.RowDefinitions.Count;
        //    return Tuple.Create(nextX, nextY);
        //}

        private Tuple<double, double> GenerateStep(Dot pixel, int x, int y)
        {
            var nextX = (x + pixel.X + myCanvas.ActualWidth) % myCanvas.ActualWidth;
            var nextY = (y + pixel.Y + myCanvas.ActualHeight) % myCanvas.ActualHeight;
            return Tuple.Create(nextX, nextY);
        }

        private bool IsWall(Dot pixel, int x, int y)
        {
            if (pixel.X + x <= myCanvas.ActualWidth & pixel.X + x >= 0.0)
                if (pixel.Y + y <= myCanvas.ActualHeight & pixel.Y + y >= 0.0)
                    return false;
            return true;
        }

        //private void ChangeColor(Pixel pixel)
        //{
        //    var color = ((SolidColorBrush)pixel.Show().Fill).Color;
        //    if (color.G > 0)
        //    {
        //        var gByte = pixel.Healh <= 0 ? (byte)0 : (byte)pixel.Healh;
        //        pixel.Show().Fill = new SolidColorBrush(Color.FromRgb(color.R, gByte, color.B));
        //    }
        //}

        private void Move(object o, EventArgs e)
        {
            tick++;
            pixelCount.Text = tick.ToString();

            //if (tick % 1000 == 0 & pixels.Where(x => x.Value.Health > 0 & x.Value.Score >= 1000).Count() >= 2)
            //{

            //}

            foreach (var pixel in pixels.Values.Where(x => x.Health > 0))
            {
                //var distance = Math.Sqrt(Math.Pow(target.X - pixel.X, 2) + Math.Pow(target.Y - pixel.Y, 2));

                //var collidePixels = new List<Pixel>();
                //pixel.RectForm = new Rect(Canvas.GetLeft(pixel.Show()), Canvas.GetTop(pixel.Show()), pixel.Show().Width, pixel.Show().Height);
                //foreach (var item in pixels.Values.Where(x => x.Id != pixel.Id))
                //{
                //    item.RectForm = new Rect(Canvas.GetLeft(item.Show()), Canvas.GetTop(item.Show()), item.Show().Width, item.Show().Height);
                //    if (pixel.RectForm.IntersectsWith(item.RectForm))
                //        collidePixels.Add(item);
                //}

                //pixel.PixelAI.Init(new Genome(64));
                pixel.PixelAI.FixedUpdate(pixels.Values.ToArray());

                if (pixel.DotClass != "food")
                {
                    Canvas.SetTop(pixel.Show(), pixel.Y);
                    Canvas.SetLeft(pixel.Show(), pixel.X);
                }

                //pixel.RectForm = new Rect(Canvas.GetLeft(pixel.Show()), Canvas.GetTop(pixel.Show()), pixel.Show().Width, pixel.Show().Height);
                //if (pixel.RectForm.IntersectsWith(target.RectForm))
                //{
                //    target.Show().Stroke = Brushes.Black;
                //    pixel.Health += 100;
                //    pixel.Score++;
                //    pixel.Show().ToolTip = ToolTip = new ToolTip() { Content = $"{pixel.Score}" };
                //    var newDistance = Math.Sqrt(Math.Pow(target.X - pixel.X, 2) + Math.Pow(target.Y - pixel.Y, 2));
                //}
                //pixel.Health--;
                ////if (pixel.Health <= 0)
                ////    pixel.Show().Fill = Brushes.Black;
                //target.Show().Stroke = Brushes.White;
            }

            var deads = pixels.Values.Where(x => x.Health <= 0).Select(x => x.Id);
            foreach (var key in deads)
            {
                myCanvas.Children.Remove(pixels[key].Show());
                pixels.Remove(key);
            }
        }

        #region
        //private void Move(object o, EventArgs e)
        //{
        //    tick++;
        //    pixelCount.Text = tick.ToString();

        //    if (tick % 1000 == 0 & pixels.Where(x => x.Value.Health > 0 & x.Value.Score >= 1000).Count() >= 2)
        //    {
        //        var parents = pixels
        //            .OrderByDescending(x => x.Value.Score)
        //            .Where(x => x.Value.Health > 0)
        //            .Take(2)
        //            .Select(x => x.Value)
        //            .ToList();
        //        var result = parents[0].Brain.Crossover(parents[1].Brain);
        //        result[0].Mutate();
        //        bornDots(result);
        //    }

        //    foreach (var pixel in pixels.Values.Where(x => x.Health > 0))
        //    {
        //        //if (tick % 500 == 0)
        //        //    pixel.Brain.Mutate();

        //        foreach (var target in targets.Values)
        //        {
        //            var distance = Math.Sqrt(Math.Pow(target.X - pixel.X, 2) + Math.Pow(target.Y - pixel.Y, 2));

        //            pixel.Brain.inputLayers[0] = (float)pixel.X;
        //            pixel.Brain.inputLayers[1] = (float)pixel.Y;
        //            pixel.Brain.inputLayers[2] = (float)distance;
        //            pixel.Brain.inputLayers[3] = pixel.Brain.fitness;
        //            pixel.Brain.inputLayers[4] = pixel.Health;
        //            pixel.Brain.ping();

        //            //var result = pixel.Brain.FeedForward(new float[] { (float)pixel.X, (float)pixel.Y, (float)distance });

        //            for (int i = 0; i < pixel.Brain.outputLayers.Length; i++)
        //            {
        //                if (Math.Round(pixel.Brain.outputLayers[i]) == 1)
        //                {
        //                    if (i == 0)
        //                    {
        //                        //var step = GenerateStep(pixel, 1, 0);
        //                        if (!IsWall(pixel, 1, 0))
        //                            pixel.X += 1;
        //                        else
        //                        {
        //                            pixel.Brain.fitness -= 0.1f;
        //                            pixel.Brain.Mutate();
        //                        }
        //                    }
        //                    if (i == 1)
        //                    {
        //                        //var step = GenerateStep(pixel, -1, 0);
        //                        if (!IsWall(pixel, 1, 0))
        //                            pixel.X -= 1;
        //                        else
        //                        {
        //                            pixel.Brain.fitness -= 0.1f;
        //                            pixel.Brain.Mutate();
        //                        }
        //                    }
        //                    if (i == 2)
        //                    {
        //                        //var step = GenerateStep(pixel, 0, 1);
        //                        if (!IsWall(pixel, 0, 1))
        //                            pixel.Y += 1;
        //                        else
        //                        {
        //                            pixel.Brain.fitness -= 0.1f;
        //                            pixel.Brain.Mutate();
        //                        }
        //                    }
        //                    if (i == 3)
        //                    {
        //                        //var step = GenerateStep(pixel, 0, -1);
        //                        if (!IsWall(pixel, 0, -1))
        //                            pixel.Y -= 1;
        //                        else
        //                        {
        //                            pixel.Brain.fitness -= 0.1f;
        //                            pixel.Brain.Mutate();
        //                        }
        //                    }
        //                }
        //            }
        //            Canvas.SetTop(pixel.Show(), pixel.Y);
        //            Canvas.SetLeft(pixel.Show(), pixel.X);
        //            pixel.RectForm = new Rect(Canvas.GetLeft(pixel.Show()), Canvas.GetTop(pixel.Show()), pixel.Show().Width, pixel.Show().Height);
        //            if (pixel.RectForm.IntersectsWith(target.RectForm))
        //            {
        //                target.Show().Stroke = Brushes.Black;
        //                pixel.Health += 100;
        //                pixel.Score++;
        //                pixel.Show().ToolTip = ToolTip = new ToolTip() { Content = $"{pixel.Score}" };
        //                var newDistance = Math.Sqrt(Math.Pow(target.X - pixel.X, 2) + Math.Pow(target.Y - pixel.Y, 2));
        //                if (newDistance <= distance)
        //                    pixel.Brain.fitness += 0.1f;
        //            }
        //            pixel.Health--;
        //            //if (pixel.Health <= 0)
        //            //    pixel.Show().Fill = Brushes.Black;
        //            target.Show().Stroke = Brushes.White;
        //        }
        //    }
        //    var deads = pixels.Values.Where(x => x.Health <= 0).Select(x => x.Id);
        //    foreach (var key in deads)
        //    {
        //        myCanvas.Children.Remove(pixels[key].Show());
        //        pixels.Remove(key);
        //    }
        //}
        #endregion

        private void sizeButton_Click(object sender, RoutedEventArgs e)
        {
            tick = 0;
            myTimer.Stop();
            pixels.Clear();
            targets.Clear();
            myCanvas.Children.Clear();
        }

        private void addPixel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myTimer.Stop();

                var dotCount = int.Parse(sizeX.Text);
                for (int i = 0; i < dotCount; i++)
                {
                    var pixel = new Pixel(new Rectangle()
                    {
                        Height = 25,
                        Width = 25,
                        Fill = new SolidColorBrush(Color.FromRgb(0, (byte)random.Next(100, 256), 0))
                    }, "bacterium")
                    { Y = random.Next(0, (int)myCanvas.ActualHeight), X = random.Next(0, (int)myCanvas.ActualWidth) };
                    pixel.PixelAI = new AI(pixel);
                    pixel.PixelAI.Init(new Genome(64));
                    pixel.Position = new Vector3((float)pixel.X, (float)pixel.Y, 0f);
                    pixels.Add(pixel.Id, pixel);
                    Canvas.SetTop(pixel.Show(), pixel.Y);
                    Canvas.SetLeft(pixel.Show(), pixel.X);
                    myCanvas.Children.Add(pixel.Show());
                }
                #region
                //for (int i = 0; i < dotCount; i++)
                //{
                //    var brain = new BloopBrain();
                //    brain.GenerationRandomBrain();
                //    var dot = new Dot(new Rectangle()
                //    {
                //        Height = 25,
                //        Width = 25,
                //        Fill = new SolidColorBrush(Color.FromRgb(0, (byte)random.Next(100, 256), 0))
                //    }, "", brain)
                //    { Y = random.Next(0, (int)myCanvas.ActualHeight), X = random.Next(0, (int)myCanvas.ActualWidth) };

                //    pixels.Add(dot.Id, dot);
                //    Canvas.SetTop(dot.Show(), dot.Y);
                //    Canvas.SetLeft(dot.Show(), dot.X);
                //    myCanvas.Children.Add(dot.Show());
                //}
                //makeFood(1);
                #endregion
                myTimer.Start();
            }
            catch (Exception)
            {
                MessageBox.Show("Вводите только целое число", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void makeFood(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var food = new Pixel(new Rectangle()
                {
                    Height = 25,
                    Width = 25,
                    Fill = new SolidColorBrush(Color.FromRgb(200, 0, 0))
                }, "food")
                { Y = random.Next(0, (int)myCanvas.ActualHeight), X = random.Next(0, (int)myCanvas.ActualWidth) };
                food.Position = new Vector3((float)food.X, (float)food.Y, 0f);
                pixels.Add(food.Id, food);
                Canvas.SetTop(food.Show(), food.Y);
                Canvas.SetLeft(food.Show(), food.X);
                myCanvas.Children.Add(food.Show());
                food.RectForm = new Rect(Canvas.GetLeft(food.Show()), Canvas.GetTop(food.Show()), food.Show().Width, food.Show().Height);
            }
        }

        private void bornDots(BloopBrain[] brains)
        {
            for (int i = 0; i < brains.Length; i++)
            {
                var dot = new Pixel(new Rectangle()
                {
                    Height = 25,
                    Width = 25,
                    Fill = new SolidColorBrush(Color.FromRgb(0, (byte)random.Next(100, 256), 0))
                }, "")
                { Y = random.Next(0, (int)myCanvas.ActualHeight), X = random.Next(0, (int)myCanvas.ActualWidth) };
                pixels.Add(dot.Id, dot);
                Canvas.SetTop(dot.Show(), dot.Y);
                Canvas.SetLeft(dot.Show(), dot.X);
                myCanvas.Children.Add(dot.Show());
            }
        }

        //private void makeFood(int count)
        //{
        //    for (int i = 0; i < count; i++)
        //    {
        //        var food = new Dot(new Rectangle()
        //        {
        //            Height = 25,
        //            Width = 25,
        //            Fill = new SolidColorBrush(Color.FromRgb(200, 0, 0))
        //        }, "food", null)
        //        { Y = random.Next(0, (int)myCanvas.ActualHeight), X = random.Next(0, (int)myCanvas.ActualWidth) };
        //        targets.Add(food.Id, food);
        //        Canvas.SetTop(food.Show(), food.Y);
        //        Canvas.SetLeft(food.Show(), food.X);
        //        myCanvas.Children.Add(food.Show());
        //        food.RectForm = new Rect(Canvas.GetLeft(food.Show()), Canvas.GetTop(food.Show()), food.Show().Width, food.Show().Height);
        //    }
        //}

        //private void bornDots(BloopBrain[] brains)
        //{
        //    for (int i = 0; i < brains.Length; i++)
        //    {
        //        var dot = new Dot(new Rectangle()
        //        {
        //            Height = 25,
        //            Width = 25,
        //            Fill = new SolidColorBrush(Color.FromRgb(0, (byte)random.Next(100, 256), 0))
        //        }, "", brains[i])
        //        { Y = random.Next(0, (int)myCanvas.ActualHeight), X = random.Next(0, (int)myCanvas.ActualWidth) };
        //        pixels.Add(dot.Id, dot);
        //        Canvas.SetTop(dot.Show(), dot.Y);
        //        Canvas.SetLeft(dot.Show(), dot.X);
        //        myCanvas.Children.Add(dot.Show());
        //    }
        //}

        private void speed_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                myTimer.Interval = new TimeSpan(0, 0, 0, 0, int.Parse(speed.Text));
            }
            catch (Exception)
            {
                MessageBox.Show("Вводите только целое число", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void myCanvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!addPixel.IsEnabled)
                addPixel.IsEnabled = true;
            myTimer.Stop();
            var point = e.GetPosition(myCanvas);
            var food = new Pixel(new Rectangle()
            {
                Height = 25,
                Width = 25,
                Fill = new SolidColorBrush(Color.FromRgb(200, 0, 0))
            }, "food")
            { Y = (float)point.Y, X = (float)point.X };
            food.Position = new Vector3((float)food.X, (float)food.Y, 0);
            food.PixelAI = new AI(food);
            food.PixelAI.Init(new Genome(64));
            food.Position = new Vector3((float)food.X, (float)food.Y, 0);
            pixels.Add(food.Id, food);
            Canvas.SetTop(food.Show(), food.Y);
            Canvas.SetLeft(food.Show(), food.X);
            myCanvas.Children.Add(food.Show());
            food.RectForm = new Rect(Canvas.GetLeft(food.Show()), Canvas.GetTop(food.Show()), food.Show().Width, food.Show().Height);
            //myTimer.Start();
        }

        //private void myCanvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    if (!addPixel.IsEnabled)
        //        addPixel.IsEnabled = true;
        //    myTimer.Stop();
        //    var point = e.GetPosition(myCanvas);
        //    var food = new Dot(new Rectangle()
        //    {
        //        Height = 25,
        //        Width = 25,
        //        Fill = new SolidColorBrush(Color.FromRgb(200, 0, 0))
        //    }, "food", null)
        //    { Y = point.Y, X = point.X };
        //    targets.Add(food.Id, food);
        //    Canvas.SetTop(food.Show(), food.Y);
        //    Canvas.SetLeft(food.Show(), food.X);
        //    myCanvas.Children.Add(food.Show());
        //    food.RectForm = new Rect(Canvas.GetLeft(food.Show()), Canvas.GetTop(food.Show()), food.Show().Width, food.Show().Height);
        //    myTimer.Start();
        //}
    }
}