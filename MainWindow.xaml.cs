using Neyron.Brain;
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
        //Dictionary<int, Pixel> pixels = new Dictionary<int, Pixel>();
        Dictionary<int, Dot> pixels = new Dictionary<int, Dot>();
        //Dictionary<int, Pixel> subPixels = new Dictionary<int, Pixel>();
        Dictionary<int, Dot> targets = new Dictionary<int, Dot>();
        //Grid myGrid;
        Canvas myCanvas;
        Random random = new Random();
        System.Windows.Threading.DispatcherTimer myTimer = new System.Windows.Threading.DispatcherTimer();
        Topology Topology = new Topology(5, 4, 0.1, 2);
        NeuronNetwork NN;
        //NN AI;
        //List<Tuple<double, double[]>> dataset;

        public MainWindow()
        {
            InitializeComponent();

            myCanvas = new Canvas();
            Grid.SetColumn(myCanvas, 0);
            Grid.SetRow(myCanvas, 1);
            mainGrid.Children.Add(myCanvas);
            //MN = new MainController(myCanvas, pixels, targets);
            NN = new NeuronNetwork(Topology);
            //AI = new NN(5, 8, 4);

            //addPixel.IsEnabled = false;
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

        private void ChangeColor(Pixel pixel)
        {
            var color = ((SolidColorBrush)pixel.Show().Fill).Color;
            if (color.G > 0)
            {
                var gByte = pixel.Healh <= 0 ? (byte)0 : (byte)pixel.Healh;
                pixel.Show().Fill = new SolidColorBrush(Color.FromRgb(color.R, gByte, color.B));
            }
        }

        //private void CreateGrid(int x, int y)
        //{
        //    if (myGrid != null)
        //    {
        //        foreach (var pixel in pixels)
        //        {
        //            myGrid.Children.Remove(pixel.Value.Show());
        //            myGrid.Children.Remove(pixel.Value.Clan);
        //        }
        //        mainGrid.Children.Remove(myGrid);
        //    }

        //    myGrid = new Grid() { ShowGridLines = true };

        //    foreach (var pixel in pixels)
        //    {
        //        myGrid.Children.Add(pixel.Value.Show());
        //        myGrid.Children.Add(pixel.Value.Clan);
        //    }

        //    for (int i = 0; i < x; i++)
        //    {
        //        myGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
        //    }
        //    for (int j = 0; j < y; j++)
        //    {
        //        myGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
        //    }
        //    Grid.SetRow(myGrid, 1);
        //    Grid.SetColumn(myGrid, 0);
        //    mainGrid.Children.Add(myGrid);
        //    //this.SizeToContent = SizeToContent.WidthAndHeight;
        //}

        //private Pixel Hunt(Pixel pixel)
        //{
        //    Pixel tempPixel = null;
        //    for (int x = -1; x < 2; x++)
        //    {
        //        for (int y = -1; y < 2; y++)
        //        {
        //            var nextStep = GenerateStep(pixel, x, y);
        //            if (x == 0 & y == 0)
        //                continue;
        //            tempPixel = pixels
        //                .Where(p => p.Value.X == nextStep.Item1 & p.Value.Y == nextStep.Item2)
        //                .FirstOrDefault().Value;
        //            if (tempPixel != null)
        //                break;
        //        }
        //    }
        //    return tempPixel;
        //}

        //private void Run(Pixel meat, Pixel hunter)
        //{
        //    for (int x = -1; x < 2; x++)
        //        for (int y = -1; y < 2; y++)
        //        {
        //            if (x == 0 & y == 0)
        //                continue;
        //            if (meat.X + x != hunter.X && meat.Y + y != hunter.Y)
        //            {
        //                var nextStep = GenerateStep(meat, x, y);
        //                meat.X = nextStep.Item1;
        //                meat.Y = nextStep.Item2;
        //            }
        //        }
        //}

        //private void Eat(Pixel pixel)
        //{
        //    for (int x = -1; x < 2; x++)
        //        for (int y = -1; y < 2; y++)
        //        {
        //            if (x == 0 & y == 0)
        //                continue;
        //            var tempPixel = pixels
        //                    .Where(p => p.Value.X == pixel.X + x & p.Value.Y == pixel.Y + y)
        //                    .FirstOrDefault().Value;
        //            if (tempPixel != null && tempPixel.Healh <= 0)
        //            {
        //                pixel.X += x;
        //                pixel.Y += y;
        //                pixel.Healh += 100;
        //                pixels.Remove(tempPixel.Id);
        //                if (pixel.Healh >= 200)
        //                    CreateDot(subPixels);
        //            }
        //        }
        //}

        private void Fight(Pixel pixel1, Pixel pixel2)
        {
            if (pixel1.Id != pixel2.Id)
            {
                if (pixel1.Healh + pixel1.Attack > pixel2.Healh + pixel2.Attack)
                {
                    pixel1.Healh -= pixel2.Attack;
                    pixel2.Healh -= pixel1.Attack;
                }
                else
                {
                    pixel2.Healh -= pixel1.Attack;
                    pixel1.Healh -= pixel2.Attack;
                }
            }
        }

        //private void CalculateMove(Pixel pixel)
        //{
        //    var randomPos = new Random();
        //    var nextStep = GenerateStep(pixel, randomPos.Next(-1, 2), randomPos.Next(-1, 2));
        //    var tempPixel = Hunt(pixel);
        //    if (tempPixel != null)
        //    {
        //        if (tempPixel.Healh > 0)
        //        {
        //            if (!tempPixel.Clan.Content.Equals(pixel.Clan.Content))
        //                Fight(pixel, tempPixel);
        //            ChangeColor(tempPixel);
        //        }
        //        else
        //        {
        //            pixel.Healh += 100;
        //            pixels.Remove(tempPixel.Id);
        //            if (pixel.Healh >= 200)
        //                CreateDot(subPixels);
        //        }
        //    }
        //    else
        //    {
        //        pixel.X = nextStep.Item1;
        //        pixel.Y = nextStep.Item2;
        //        pixel.Healh--;
        //    }
        //    ChangeColor(pixel);
        //}

        //private void Move(object o, EventArgs e)
        //{
        //    try
        //    {
        //        var x = int.Parse(sizeX.Text);
        //        var y = int.Parse(sizeY.Text);
        //        if (x > 100 || y > 100)
        //            throw new ArgumentException();

        //        foreach (var pixel in pixels)
        //        {
        //            if (pixel.Value.Healh > 0)
        //                CalculateMove(pixel.Value);
        //        }

        //        pixels = pixels.Concat(subPixels).ToDictionary(x => x.Key, x => x.Value);
        //        subPixels.Clear();

        //        CreateGrid(x, y);
        //        foreach (var pixel in pixels)
        //        {
        //            Grid.SetColumn(pixel.Value.Clan, pixel.Value.X);
        //            Grid.SetRow(pixel.Value.Clan, pixel.Value.Y);
        //            Grid.SetColumn(pixel.Value.Show(), pixel.Value.X);
        //            Grid.SetRow(pixel.Value.Show(), pixel.Value.Y);
        //        }
        //    }
        //    catch (ArgumentException)
        //    {
        //        MessageBox.Show("Указан слишком большой размер поля, вводите не больше 100 по оси X или Y", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //        myTimer.Stop();
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("Вводите координаты X и Y только в виде целых чисел", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //        myTimer.Stop();
        //    }
        //}

        //private void sizeButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var x = int.Parse(sizeX.Text);
        //        var y = int.Parse(sizeY.Text);
        //        if (x > 100 || y > 100)
        //            throw new ArgumentException();
        //        myTimer.Stop();
        //        pixels.Clear();
        //        CreateGrid(x, y);
        //        addPixel.IsEnabled = true;
        //    }
        //    catch (ArgumentException)
        //    {
        //        MessageBox.Show("Указан слишком большой размер поля, вводите не больше 100 по оси X или Y", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("Вводите координаты X и Y только в виде целых чисел", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        private void Move(object o, EventArgs e)
        {
            try
            {
                var random = new Random();
                var x = int.Parse(sizeX.Text);
                var y = int.Parse(sizeY.Text);
                var target = targets.Values.FirstOrDefault();
                //var inputSignals = new double[pixels.Count];
                if (x > 100 || y > 100)
                    throw new ArgumentException();

                foreach (var pixel in pixels.Values)
                {
                    var distance = Math.Sqrt(Math.Pow(target.X - pixel.X, 2) + Math.Pow(target.Y - pixel.Y, 2));
                    //var norm = NN.Normalization(new double[,] { { pixel.X, pixel.Y, target.X, target.Y, distance } });
                    //var scal = NN.Scalling(new double[,] { { pixel.X, pixel.Y, target.X, target.Y, distance } });
                    //var learnRs = NN.Learn(new double[] { 0 }, new double[,] { { pixel.X, pixel.Y, target.X, target.Y, distance } }, 100);
                    var neuron = NN.FeedForward(new double[] { pixel.X, pixel.Y, target.X, target.Y, distance });
                    //var neuron = AI.FeedForward(new float[] { 0.1f, (float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble() });

                    var index = NN.Layers.Last().Neurons.IndexOf(neuron);
                    if (index == 0)
                        pixel.X -= 5;
                    if (index == 1)
                        pixel.X += 5;
                    if (index == 2)
                        pixel.Y -= 5;
                    if (index == 3)
                        pixel.Y += 5;
                    //distance = Math.Sqrt(Math.Pow(target.X - pixel.X, 2) + Math.Pow(target.Y - pixel.Y, 2));
                    
                    //pixel.X += random.Next(-5, 6);
                    //pixel.Y += random.Next(-5, 6);
                }

                //pixels = pixels.Concat(subPixels).ToDictionary(x => x.Key, x => x.Value);
                //subPixels.Clear();

                //CreateGrid(x, y);
                foreach (var pixel in pixels)
                {
                    Canvas.SetTop(pixel.Value.Show(), pixel.Value.Y);
                    Canvas.SetLeft(pixel.Value.Show(), pixel.Value.X);
                }
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Указан слишком большой размер поля, вводите не больше 100 по оси X или Y", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                myTimer.Stop();
            }
            catch (Exception)
            {
                MessageBox.Show("Вводите координаты X и Y только в виде целых чисел", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                myTimer.Stop();
            }
        }

        private void sizeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var x = int.Parse(sizeX.Text);
                var y = int.Parse(sizeY.Text);
                if (x > 100 || y > 100)
                    throw new ArgumentException();
                myTimer.Stop();
                pixels.Clear();
                targets.Clear();
                myCanvas.Children.Clear();
                myCanvas.Width = mainGrid.RowDefinitions[1].ActualHeight;
                myCanvas.Height = mainGrid.ColumnDefinitions[0].ActualWidth;

                //var dot = new Dot(new Ellipse()
                //{
                //    Height = 25,
                //    Width = 25,
                //    Fill = new SolidColorBrush(Color.FromRgb(0, (byte)random.Next(100, 256), 33))
                //}, "")
                //{ Position = new Vector2(random.Next(0, (int)myCanvas.Height), random.Next(0, (int)myCanvas.Width)) };
                //dot.Move();
                //pixels.Add(dot.Id, dot);
                //Canvas.SetTop(dot.Show(), dot.X);
                //Canvas.SetLeft(dot.Show(), dot.Y);
                //myCanvas.Children.Add(dot.Show());

                //myTimer.Start();
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Указан слишком большой размер поля, вводите не больше 100 по оси X или Y", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Вводите координаты X и Y только в виде целых чисел", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void addPixel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                myTimer.Stop();

                myCanvas.Width = mainGrid.RowDefinitions[1].ActualHeight;
                myCanvas.Height = mainGrid.ColumnDefinitions[0].ActualWidth;

                var dot = new Dot(new Ellipse()
                {
                    Height = 25,
                    Width = 25,
                    Fill = new SolidColorBrush(Color.FromRgb(0, (byte)random.Next(100, 256), 0))
                }, "")
                { X = random.Next(0, (int)myCanvas.Height), Y = random.Next(0, (int)myCanvas.Width) };
                //dot.Move();
                pixels.Add(dot.Id, dot);
                Canvas.SetTop(dot.Show(), dot.Y);
                Canvas.SetLeft(dot.Show(), dot.X);
                myCanvas.Children.Add(dot.Show());

                var food = new Dot(new Ellipse()
                {
                    Height = 25,
                    Width = 25,
                    Fill = new SolidColorBrush(Color.FromRgb(200, 0, 0))
                }, "food")
                { X = random.Next(0, (int)myCanvas.Height), Y = random.Next(0, (int)myCanvas.Width) };
                //food.Move();
                targets.Add(food.Id, food);
                Canvas.SetTop(food.Show(), food.Y);
                Canvas.SetLeft(food.Show(), food.X);
                myCanvas.Children.Add(food.Show());

                //MN.CreateDots(int.Parse(pixelCount.Text));
                //var dif = NN.Learn(new double[] { 0, 1, 0, 1 }, new double[,] { { 0, 0, 0, 0, 0 }, { 1, 0, 0, 0, 0 }, { 1, 1, 1, 1, 1 }, { 0, 0, 0, 1, 1 } }, 10000);
                
                myTimer.Start();
            }
            catch (Exception)
            {
                MessageBox.Show("Вводите только целое число", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //private void addPixel_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        myTimer.Stop();

        //        for (int i = 0; i < int.Parse(pixelCount.Text); i++)
        //        {
        //            //CreateDot(pixels);
        //        }
        //        myTimer.Start();
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("Вводите только целое число", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        //private void CreateDot(Dictionary<int, Pixel> pixels)
        //{
        //    var random = new Random();
        //    var pixelSize = (myGrid.RowDefinitions[0].ActualHeight + myGrid.ColumnDefinitions[0].ActualWidth) / 2.5;
        //    var pixel = new Pixel(new Ellipse()
        //    {
        //        Height = pixelSize,
        //        Width = pixelSize,
        //        Fill = new SolidColorBrush(Color.FromRgb(0, (byte)random.Next(100, 256), 33))
        //    }, random.Next(0, myGrid.ColumnDefinitions.Count), random.Next(0, myGrid.RowDefinitions.Count));
        //    pixels.Add(pixel.Id, pixel);
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
    }
}