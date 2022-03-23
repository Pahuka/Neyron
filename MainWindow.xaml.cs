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
        Dictionary<int, Pixel> subPixels = new Dictionary<int, Pixel>();
        //Grid myGrid;
        Canvas myCanvas;
        System.Windows.Threading.DispatcherTimer myTimer = new System.Windows.Threading.DispatcherTimer();
        Topology Topology = new Topology(3, 2, 2, 1);
        NeuronNetwork NN;

        public MainWindow()
        {
            InitializeComponent();

            myCanvas = new Canvas();
            Grid.SetColumn(myCanvas, 0);
            Grid.SetRow(myCanvas, 1);
            mainGrid.Children.Add(myCanvas);
            NN = new NeuronNetwork(Topology);


            addPixel.IsEnabled = false;
            myTimer.Tick += Move;
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
                var inputSignals = new double[pixels.Count];
                if (x > 100 || y > 100)
                    throw new ArgumentException();

                foreach (var pixel in pixels.Values)
                {
                    var r = NN.FeedForward(new double[] { pixel.X, pixel.Y, random.NextDouble() });
                    if (pixel.X <= myCanvas.Height)
                    {
                        pixel.X += r[0].Output;
                        pixel.Y += r[1].Output;
                    }
                    if (pixel.Y > myCanvas.Width)
                    {
                        pixel.Y -= r[1].Output;
                        pixel.X -= r[0].Output;
                    }
                }

                //pixels = pixels.Concat(subPixels).ToDictionary(x => x.Key, x => x.Value);
                //subPixels.Clear();

                //CreateGrid(x, y);
                foreach (var pixel in pixels)
                {
                    //Grid.SetColumn(pixel.Value.Clan, pixel.Value.X);
                    //Grid.SetRow(pixel.Value.Clan, pixel.Value.Y);
                    Canvas.SetTop(pixel.Value.Show(), pixel.Value.X);
                    Canvas.SetLeft(pixel.Value.Show(), pixel.Value.Y);
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
                //var x = int.Parse(sizeX.Text);
                //var y = int.Parse(sizeY.Text);
                //if (x > 100 || y > 100)
                //    throw new ArgumentException();
                myTimer.Stop();
                pixels.Clear();
                myCanvas.Width = mainGrid.RowDefinitions[1].ActualHeight;
                myCanvas.Height = mainGrid.ColumnDefinitions[0].ActualWidth;
                var random = new Random();
                var dot = new Dot(new Ellipse()
                {
                    Height = 25,
                    Width = 25,
                    Fill = new SolidColorBrush(Color.FromRgb(0, (byte)random.Next(100, 256), 33))});
                pixels.Add(dot.Id, dot);

                myCanvas.Children.Add(dot.Show());
                myTimer.Start();
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
                
                for (int i = 0; i < int.Parse(pixelCount.Text); i++)
                {
                    //CreateDot(pixels);
                }
                myTimer.Start();
            }
            catch (Exception)
            {
                MessageBox.Show("Вводите только целое число", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

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