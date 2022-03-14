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
        List<Pixel> pixels = new List<Pixel>();
        static Grid myGrid;
        System.Windows.Threading.DispatcherTimer myTimer = new System.Windows.Threading.DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            myTimer.Tick += Move;
            myTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
        }

        private void CreateGrid(int x, int y)
        {
            if (myGrid != null)
            {
                foreach (var pixel in pixels)
                {
                    myGrid.Children.Remove(pixel.Show());
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
                myGrid.Children.Add((UIElement)pixel.Show());
            }

            for (int i = 0; i < x; i++)
            {
                myGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(20) });
            }
            for (int j = 0; j < y; j++)
            {
                myGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(20) });
            }
            this.SizeToContent = SizeToContent.WidthAndHeight;
            mainGrid.Children.Add(myGrid);
        }

        private void CalculateMove(Pixel pixel)
        {
            var randomPos = new Random();
            var x = pixel.X + randomPos.Next(-1, 2);
            var y = pixel.Y + randomPos.Next(-1, 2);
            if (x >= 0 & x <= myGrid.RowDefinitions.Count)
                if (y >= 0 & y <= myGrid.ColumnDefinitions.Count)
                {
                    pixel.X = x;
                    pixel.Y = y;
                }
        }

        private void Move(object o, EventArgs e)
        {
            CreateGrid(int.Parse(sizeX.Text), int.Parse(sizeY.Text));

            foreach (var pixel in pixels)
            {
                CalculateMove(pixel);
                Grid.SetColumn((UIElement)pixel.Show(), pixel.Y);
                Grid.SetRow((UIElement)pixel.Show(), pixel.X);
            }
        }

        private void sizeButton_Click(object sender, RoutedEventArgs e)
        {
            myTimer.Stop();
            pixels.Clear();
            CreateGrid(int.Parse(sizeX.Text), int.Parse(sizeY.Text));
        }

        private void addPixel_Click(object sender, RoutedEventArgs e)
        {
            pixels.Add(new Pixel(new Ellipse()
            {
                Height = 20,
                Width = 20,
                Fill = Brushes.Red
            }, 0, 0));

            myTimer.Start();
        }
    }
}
