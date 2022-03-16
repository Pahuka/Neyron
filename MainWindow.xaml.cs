using System;
using System.Collections.Generic;
using System.Linq;
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
        Dictionary<int, Pixel> pixels = new Dictionary<int, Pixel>();
        Grid myGrid;
        System.Windows.Threading.DispatcherTimer myTimer = new System.Windows.Threading.DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            addPixel.IsEnabled = false;
            myTimer.Tick += Move;
            myTimer.Interval = new TimeSpan(0, 0, 0, 0, int.Parse(speed.Text));
        }

        private Tuple<int, int> IsWall(Pixel pixel, int x, int y)
        {
            var nextX = (x + pixel.X + myGrid.ColumnDefinitions.Count) % myGrid.ColumnDefinitions.Count;
            var nextY = (y + pixel.Y + myGrid.RowDefinitions.Count) % myGrid.RowDefinitions.Count;
            return Tuple.Create(nextX, nextY);
        }

        private void ChangeColor(Pixel pixel)
        {
            var color = ((SolidColorBrush)pixel.Show().Fill).Color;
            if (color.G > 0)
            {
                var gByte = pixel.Healh <= 0 ? (byte)0 : (byte)pixel.Healh;                    
                pixel.Show().Fill = new SolidColorBrush(Color.FromRgb(color.R, gByte, color.B));
            }
        }

        private void CreateGrid(int x, int y)
        {
            if (myGrid != null)
            {
                foreach (var pixel in pixels)
                {
                    myGrid.Children.Remove(pixel.Value.Show());
                }
                mainGrid.Children.Remove(myGrid);
            }

            myGrid = new Grid()
            {
                //VerticalAlignment = VerticalAlignment.Center,
                ShowGridLines = true
            };

            foreach (var pixel in pixels)
            {
                myGrid.Children.Add((UIElement)pixel.Value.Show());
            }

            for (int i = 0; i < x; i++)
            {
                //myGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(25) });
                myGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                //myGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int j = 0; j < y; j++)
            {
                //myGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(25) });
                myGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                //myGrid.RowDefinitions.Add(new RowDefinition());
            }
            Grid.SetRow(myGrid, 1);
            Grid.SetColumn(myGrid, 0);
            mainGrid.Children.Add(myGrid);
            //this.SizeToContent = SizeToContent.WidthAndHeight;
        }

        private void Hunt(Pixel pixel)
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 & y == 0)
                        continue;
                    var tempPixel = pixels
                        .Where(p => p.Value.X == pixel.X + x & p.Value.Y == pixel.Y + y)
                        .FirstOrDefault().Value;
                    if (tempPixel != null && tempPixel.Healh > 0)
                    {
                        var nextStep = IsWall(pixel, x, y);
                        pixel.X = nextStep.Item1;
                        pixel.Y = nextStep.Item2;
                    }
                }
            }
            CalculateMove(pixel);
        }

        private void Run(Pixel meat, Pixel hunter)
        {
            for (int x = -1; x < 2; x++)
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 & y == 0)
                        continue;
                    if (meat.X + x != hunter.X && meat.Y + y != hunter.Y)
                    {
                        var nextStep = IsWall(meat, x, y);
                        meat.X = nextStep.Item1;
                        meat.Y = nextStep.Item2;
                    }
                }
        }

        private void Eat(Pixel pixel)
        {
            for (int x = -1; x < 2; x++)
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 & y == 0)
                        continue;
                    var tempPixel = pixels
                            .Where(p => p.Value.X == pixel.X + x & p.Value.Y == pixel.Y + y)
                            .FirstOrDefault().Value;
                    if (tempPixel != null && tempPixel.Healh <= 0)
                    {
                        pixel.X += x;
                        pixel.Y += y;
                        pixel.Healh += 100;
                        pixels.Remove(tempPixel.Id);
                    }
                }
        }

        private void Fight(Pixel pixel1, Pixel pixel2)
        {
            if (pixel1.Id != pixel2.Id)
            {
                if (pixel1.Healh + pixel1.Attack > pixel2.Healh + pixel2.Attack)
                {
                    pixel1.Healh -= pixel2.Attack;
                    pixel2.Healh -= pixel1.Attack;
                    Run(pixel2, pixel1);
                }
                else
                {
                    pixel2.Healh -= pixel1.Attack;
                    pixel1.Healh -= pixel2.Attack;
                    Run(pixel1, pixel2);
                }
            }
        }

        private void CalculateMove(Pixel pixel)
        {
            var randomPos = new Random();
            var nextStep = IsWall(pixel, randomPos.Next(-1, 2), randomPos.Next(-1, 2));
            var tempPixel = pixels
                        .Where(p => p.Value.X == nextStep.Item1 & p.Value.Y == nextStep.Item2 & p.Value.Id != pixel.Id)
                        .FirstOrDefault().Value;
            if (tempPixel != null && tempPixel.Healh > 0)
            {
                Fight(pixel, tempPixel);
            }
            pixel.X = nextStep.Item1;
            pixel.Y = nextStep.Item2;
            if (pixel.Healh < 100)
                Eat(pixel);
        }

        private void Move(object o, EventArgs e)
        {
            try
            {
                var x = int.Parse(sizeX.Text);
                var y = int.Parse(sizeY.Text);
                if (x > 100 || y > 100)
                    throw new ArgumentException();

                CreateGrid(x, y);

                foreach (var pixel in pixels)
                {
                    if (pixel.Value.Show().Fill != Brushes.Black)
                    {
                        Hunt(pixel.Value);
                    }
                }

                foreach (var pixel in pixels)
                {
                    pixel.Value.Healh--;
                    ChangeColor(pixel.Value);
                    if (pixel.Value.Healh <= 0)
                        pixel.Value.Show().Fill = Brushes.Black;
                    if (pixel.Value.Healh > 0)
                    {
                        Grid.SetColumn((UIElement)pixel.Value.Show(), pixel.Value.X);
                        Grid.SetRow((UIElement)pixel.Value.Show(), pixel.Value.Y);
                    }
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
                CreateGrid(x, y);
                addPixel.IsEnabled = true;
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
                var random = new Random();
                for (int i = 0; i < int.Parse(pixelCount.Text); i++)
                {
                    var pixel = new Pixel(new Ellipse()
                    {
                        Height = myGrid.RowDefinitions[0].ActualHeight,
                        Width = myGrid.ColumnDefinitions[0].ActualWidth,
                        Fill = new SolidColorBrush(Color.FromRgb(0, (byte)random.Next(100, 256), 33))
                    }, random.Next(0, myGrid.ColumnDefinitions.Count), random.Next(0, myGrid.RowDefinitions.Count));
                    pixels.Add(pixel.Id, pixel);
                }
                myTimer.Start();
            }
            catch (Exception)
            {
                MessageBox.Show("Вводите только целое число", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

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