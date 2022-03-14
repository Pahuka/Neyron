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
            myTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
        }

        private void ChangeColor(Pixel pixel, bool isHungry)
        {
            var color = ((SolidColorBrush)pixel.Show().Fill).Color;
            if (color.G > 0)
            {
                var gCanal = isHungry ? (byte)(color.G - color.G / pixel.Healh) : (byte)(color.G + (byte)pixel.Healh);
                pixel.Show().Fill = new SolidColorBrush(Color.FromRgb(color.R, gCanal, color.B));
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
                VerticalAlignment = VerticalAlignment.Top,
                ShowGridLines = true
            };
            Grid.SetRow(myGrid, 1);
            Grid.SetColumn(myGrid, 0);

            foreach (var pixel in pixels)
            {
                myGrid.Children.Add((UIElement)pixel.Value.Show());
            }

            for (int i = 0; i < x; i++)
            {
                myGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(25) });
            }
            for (int j = 0; j < y; j++)
            {
                myGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(25) });
            }
            this.SizeToContent = SizeToContent.WidthAndHeight;
            mainGrid.Children.Add(myGrid);
        }

        private void Hunt(Pixel pixel)
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 & y == 0)
                        continue;
                    var tempPixel = pixels.Where(p => p.Value.X == x & p.Value.Y == y).FirstOrDefault().Value;
                    if (tempPixel != null && tempPixel.Show().Fill != Brushes.Black)
                    {
                        Eat(pixel, tempPixel);
                        pixel.X += x;
                        pixel.Y += y;
                    }
                }
            }
        }

        private void Eat(Pixel pixel1, Pixel pixel2)
        {
            if (pixel1.Id != pixel2.Id)
            {
                if (pixel1.Healh > pixel2.Healh)
                {
                    pixel1.Healh += pixel2.Healh;
                    ChangeColor(pixel1, false);
                    pixel2.Healh = 0;
                    pixel2.ChangeColor(Brushes.Black);
                }
                else
                {
                    pixel2.Healh += pixel1.Healh;
                    ChangeColor(pixel2, false);
                    pixel1.Healh = 0;
                    pixel1.ChangeColor(Brushes.Black);
                }
            }
        }

        private void CalculateMove(Pixel pixel)
        {
            var randomPos = new Random();
            Hunt(pixel);
            var x = pixel.X + randomPos.Next(-1, 2);
            var y = pixel.Y + randomPos.Next(-1, 2);
            if (x >= 0 & x <= myGrid.RowDefinitions.Count)
                if (y >= 0 & y <= myGrid.ColumnDefinitions.Count)
                {
                    var tempPixel = pixels.Where(p => p.Value.X == x & p.Value.Y == y).FirstOrDefault().Value;
                    if (tempPixel != null && tempPixel.Show().Fill != Brushes.Black)
                        Eat(pixel, tempPixel);
                    //pixel.Healh--;
                    //ChangeColor(pixel, true);
                    pixel.X = x;
                    pixel.Y = y;
                }
        }

        private void Move(object o, EventArgs e)
        {
            CreateGrid(int.Parse(sizeX.Text), int.Parse(sizeY.Text));

            foreach (var pixel in pixels)
            {
                if (pixel.Value.Show().Fill != Brushes.Black)
                    CalculateMove(pixel.Value);
            }

            foreach (var pixel in pixels)
            {
                pixel.Value.Healh--;
                ChangeColor(pixel.Value, true);
                if (pixel.Value.Healh <= 0 && pixel.Value.Show().Fill != Brushes.Black)
                    pixel.Value.Show().Fill = Brushes.Black;
                if (pixel.Value.Show().Fill != Brushes.Black)
                {
                    Grid.SetColumn((UIElement)pixel.Value.Show(), pixel.Value.Y);
                    Grid.SetRow((UIElement)pixel.Value.Show(), pixel.Value.X);
                }
            }
        }

        private void sizeButton_Click(object sender, RoutedEventArgs e)
        {
            myTimer.Stop();
            pixels.Clear();
            CreateGrid(int.Parse(sizeX.Text), int.Parse(sizeY.Text));
            addPixel.IsEnabled = true;
        }

        private void addPixel_Click(object sender, RoutedEventArgs e)
        {
            var random = new Random();
            var pixel = new Pixel(new Ellipse()
            {
                Height = 25,
                Width = 25,
                Fill = new SolidColorBrush(Color.FromRgb(0, 255, 33))
            }, random.Next(0, myGrid.ColumnDefinitions.Count), random.Next(0, myGrid.RowDefinitions.Count));
            pixels.Add(pixel.Id, pixel);

            myTimer.Start();
        }
    }
}
