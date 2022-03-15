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

        private bool IsWall(int x, int y)
        {
            if (x >= 0 & x <= myGrid.ColumnDefinitions.Count)
                if (y >= 0 & y <= myGrid.RowDefinitions.Count)
                    return false;
            return true;
        }

        private void ChangeColor(Pixel pixel)
        {
            var color = ((SolidColorBrush)pixel.Show().Fill).Color;
            if (color.G > 0)
            {
                //var gCanal = (color.G - color.G / pixel.Healh);
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
                VerticalAlignment = VerticalAlignment.Top,
                ShowGridLines = true
            };
            Grid.SetRow(myGrid, 1);
            Grid.SetColumn(myGrid, 0);

            foreach (var pixel in pixels)
            {
                if (pixel.Value.Healh > 0)
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
            mainGrid.Children.Add(myGrid);
            this.SizeToContent = SizeToContent.WidthAndHeight;
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
                        //Fight(pixel, tempPixel);
                        pixel.X += x;
                        pixel.Y += y;
                    }
                }
            }
            Move(pixel);
        }

        private void Run(Pixel meat, Pixel hunter)
        {
            for (int x = -1; x < 2; x++)
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 & y == 0 & IsWall(meat.X + x, meat.Y + y))
                        continue;
                    if (meat.X + x != hunter.X && meat.Y + y != hunter.Y)
                    {
                        meat.X += x;
                        meat.Y += y;
                        //hunter.X += x;
                        //hunter.Y += y;
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
                    //Hunt(pixel1);
                    //ChangeColor(pixel1);
                    //return pixel1;
                }
                else
                {
                    pixel2.Healh -= pixel1.Attack;
                    pixel1.Healh -= pixel2.Attack;
                    Run(pixel1, pixel2);
                    //Hunt(pixel2);
                    //ChangeColor(pixel2);
                    //return pixel2;
                }
            }
            //return pixel1;
        }

        private void Move(Pixel pixel)
        {
            var randomPos = new Random();
            var x = pixel.X + randomPos.Next(-1, 2);
            var y = pixel.Y + randomPos.Next(-1, 2);
            if (!IsWall(x, y))
            {
                var tempPixel = pixels
                            .Where(p => p.Value.X == x & p.Value.Y == y & p.Value.Id != pixel.Id)
                            .FirstOrDefault().Value;
                if (tempPixel != null && tempPixel.Healh > 0)
                {
                    Fight(pixel, tempPixel);
                    //tempPixel.X = x;
                    //tempPixel.Y = y;
                }
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
                {
                    Hunt(pixel.Value);
                }
            }

            foreach (var pixel in pixels)
            {
                pixel.Value.Healh--;
                ChangeColor(pixel.Value);
                if (pixel.Value.Healh <= 0 && pixel.Value.Healh > 0)
                    pixel.Value.Show().Fill = Brushes.Black;
                if (pixel.Value.Healh > 0 & !IsWall(pixel.Value.X, pixel.Value.Y))
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
            myTimer.Stop();
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

        private void speed_TextChanged(object sender, TextChangedEventArgs e)
        {
            myTimer.Interval = new TimeSpan(0, 0, 0, 0, int.Parse(speed.Text));
        }
    }
}
