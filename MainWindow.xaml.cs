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
        //Dictionary<int, Pixel> pixels = new Dictionary<int, Pixel>();
        Dictionary<int, Dot> pixels = new Dictionary<int, Dot>();
        //Dictionary<int, Pixel> subPixels = new Dictionary<int, Pixel>();
        Dictionary<int, Dot> targets = new Dictionary<int, Dot>();
        //Grid myGrid;
        int tick = 0;
        //Canvas myCanvas;
        Random random = new Random();
        System.Windows.Threading.DispatcherTimer myTimer = new System.Windows.Threading.DispatcherTimer();
        //Topology Topology = new Topology(4, 4, 0.1, 4);
        //NeuronNetwork NN;
        //BloopBrain brain = new BloopBrain();
        //NN AI;

        public MainWindow()
        {
            InitializeComponent();
            //brain.GenerationRandomBrain();
            //myCanvas = new Canvas() { Background = Brushes.Gray, ClipToBounds = false };
            //Grid.SetColumn(myCanvas, 0);
            //Grid.SetRow(myCanvas, 1);
            //mainGrid.Children.Add(myCanvas);
            //MN = new MainController(myCanvas, pixels, targets);
            //NN = new NeuronNetwork(Topology);
            //AI = new NN(5, 8, 4);

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

        //private void Fight(Pixel pixel1, Pixel pixel2)
        //{
        //    if (pixel1.Id != pixel2.Id)
        //    {
        //        if (pixel1.Healh + pixel1.Attack > pixel2.Healh + pixel2.Attack)
        //        {
        //            pixel1.Healh -= pixel2.Attack;
        //            pixel2.Healh -= pixel1.Attack;
        //        }
        //        else
        //        {
        //            pixel2.Healh -= pixel1.Attack;
        //            pixel1.Healh -= pixel2.Attack;
        //        }
        //    }
        //}

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
                tick++;
                pixelCount.Text = tick.ToString();
                var random = new Random();
                //var x = int.Parse(sizeX.Text);
                //var y = int.Parse(sizeY.Text);
                //var target = targets.Values.FirstOrDefault();
                //var inputSignals = new double[pixels.Count];
                //if (x > 100 || y > 100)
                //    throw new ArgumentException();

                if (tick % 1000 == 0 & pixels.Where(x => x.Value.Health > 0 & x.Value.Score >= 1000).Count() >= 2)
                {
                    var parents = pixels
                        .OrderByDescending(x => x.Value.Score)
                        .Where(x => x.Value.Health > 0)
                        .Take(2)
                        .Select(x => x.Value)
                        .ToList();
                    var result = parents[0].Brain.Crossover(parents[1].Brain);
                    result[0].Mutate();
                    bornDots(result);
                }

                foreach (var pixel in pixels.Values.Where(x => x.Health > 0))
                {
                    //if (tick % 500 == 0)
                    //    pixel.Brain.Mutate();

                    foreach (var target in targets.Values)
                    {
                        var distance = Math.Sqrt(Math.Pow(target.X - pixel.X, 2) + Math.Pow(target.Y - pixel.Y, 2));

                        pixel.Brain.inputLayers[0] = (float)pixel.X;
                        pixel.Brain.inputLayers[1] = (float)pixel.Y;
                        pixel.Brain.inputLayers[2] = (float)distance;
                        pixel.Brain.inputLayers[3] = pixel.Brain.fitness;
                        pixel.Brain.inputLayers[4] = pixel.Health;
                        pixel.Brain.ping();

                        //var result = pixel.Brain.FeedForward(new float[] { (float)pixel.X, (float)pixel.Y, (float)distance });

                        for (int i = 0; i < pixel.Brain.outputLayers.Length; i++)
                        {
                            if (Math.Round(pixel.Brain.outputLayers[i]) == 1)
                            {
                                if (i == 0)
                                {
                                    //var step = GenerateStep(pixel, 1, 0);
                                    if (!IsWall(pixel, 1, 0))
                                        pixel.X += 1;
                                    else
                                    {
                                        pixel.Brain.fitness -= 0.1f;
                                        pixel.Brain.Mutate();
                                    }
                                }
                                if (i == 1)
                                {
                                    //var step = GenerateStep(pixel, -1, 0);
                                    if (!IsWall(pixel, 1, 0))
                                        pixel.X -= 1;
                                    else
                                    {
                                        pixel.Brain.fitness -= 0.1f;
                                        pixel.Brain.Mutate();
                                    }
                                }
                                if (i == 2)
                                {
                                    //var step = GenerateStep(pixel, 0, 1);
                                    if (!IsWall(pixel, 0, 1))
                                        pixel.Y += 1;
                                    else
                                    {
                                        pixel.Brain.fitness -= 0.1f;
                                        pixel.Brain.Mutate();
                                    }
                                }
                                if (i == 3)
                                {
                                    //var step = GenerateStep(pixel, 0, -1);
                                    if (!IsWall(pixel, 0, -1))
                                        pixel.Y -= 1;
                                    else
                                    {
                                        pixel.Brain.fitness -= 0.1f;
                                        pixel.Brain.Mutate();
                                    }
                                }
                            }
                        }
                        Canvas.SetTop(pixel.Show(), pixel.Y);
                        Canvas.SetLeft(pixel.Show(), pixel.X);
                        pixel.RectForm = new Rect(Canvas.GetLeft(pixel.Show()), Canvas.GetTop(pixel.Show()), pixel.Show().Width, pixel.Show().Height);
                        if (pixel.RectForm.IntersectsWith(target.RectForm))
                        {
                            target.Show().Stroke = Brushes.Black;
                            pixel.Health += 100;
                            pixel.Score++;
                            pixel.Show().ToolTip = ToolTip = new ToolTip() { Content = $"{pixel.Score}" };
                            var newDistance = Math.Sqrt(Math.Pow(target.X - pixel.X, 2) + Math.Pow(target.Y - pixel.Y, 2));
                            if (newDistance <= distance)
                                pixel.Brain.fitness += 0.1f;
                        }
                        pixel.Health--;
                        //if (pixel.Health <= 0)
                        //    pixel.Show().Fill = Brushes.Black;
                        target.Show().Stroke = Brushes.White;
                    }
                }
                var deads = pixels.Values.Where(x => x.Health <= 0).Select(x => x.Id);
                foreach (var key in deads)
                {
                    myCanvas.Children.Remove(pixels[key].Show());
                    pixels.Remove(key);
                }
                #region
                //if (newDistance == distance)
                //    pixel.Brain.fitness++;

                //var index = pixel.Brain.Layers.Last().Neurons.IndexOf(neuron);
                //if (index == 0)
                //{
                //    var step = GenerateStep(pixel, 1, 0);
                //    pixel.X = step.Item1;
                //}
                //if (index == 1)
                //{
                //    var step = GenerateStep(pixel, -1, 0);
                //    pixel.X = step.Item1;
                //}
                //if (index == 2)
                //{
                //    var step = GenerateStep(pixel, 0, 1);
                //    pixel.Y = step.Item2;
                //}
                //if (index == 3)
                //{
                //    var step = GenerateStep(pixel, 0, -1);
                //    pixel.Y = step.Item2;
                //}
                //pixel.Move();
                //pixel.RectForm = new Rect(pixel.X, pixel.Y, 25, 25);
                //if (pixel.RectForm.IntersectsWith(target.RectForm))
                //    pixel.Score++;

                //var newDistance = Math.Sqrt(Math.Pow(target.X - pixel.X, 2) + Math.Pow(target.Y - pixel.Y, 2));
                //if (newDistance > distance)
                //    pixel.Brain.Learn(new double[] { 0 }, new double[,] { { pixel.X, pixel.Y, newDistance, pixel.Score } }, 1);
                //else
                //    pixel.Brain.Learn(new double[] { 1 }, new double[,] { { pixel.X, pixel.Y, newDistance, pixel.Score } }, 1);
                #endregion

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
                tick = 0;
                myTimer.Stop();
                pixels.Clear();
                targets.Clear();
                myCanvas.Children.Clear();
                //myCanvas.Width = mainGrid.RowDefinitions[1].ActualHeight;
                //myCanvas.Height = mainGrid.ColumnDefinitions[0].ActualWidth;
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
                //myCanvas.Width = mainGrid.RowDefinitions[1].ActualHeight;
                //myCanvas.Height = mainGrid.ColumnDefinitions[0].ActualWidth;
                var dotCount = int.Parse(sizeX.Text);
                for (int i = 0; i < dotCount; i++)
                {
                    var brain = new BloopBrain();
                    brain.GenerationRandomBrain();
                    var dot = new Dot(new Rectangle()
                    {
                        Height = 25,
                        Width = 25,
                        Fill = new SolidColorBrush(Color.FromRgb(0, (byte)random.Next(100, 256), 0))
                    }, "", brain)
                    { Y = random.Next(0, (int)myCanvas.ActualHeight), X = random.Next(0, (int)myCanvas.ActualWidth) };

                    pixels.Add(dot.Id, dot);
                    Canvas.SetTop(dot.Show(), dot.Y);
                    Canvas.SetLeft(dot.Show(), dot.X);
                    myCanvas.Children.Add(dot.Show());
                }
                //makeFood(1);
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
                var food = new Dot(new Rectangle()
                {
                    Height = 25,
                    Width = 25,
                    Fill = new SolidColorBrush(Color.FromRgb(200, 0, 0))
                }, "food", null)
                { Y = random.Next(0, (int)myCanvas.ActualHeight), X = random.Next(0, (int)myCanvas.ActualWidth) };
                targets.Add(food.Id, food);
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
                var dot = new Dot(new Rectangle()
                {
                    Height = 25,
                    Width = 25,
                    Fill = new SolidColorBrush(Color.FromRgb(0, (byte)random.Next(100, 256), 0))
                }, "", brains[i])
                { Y = random.Next(0, (int)myCanvas.ActualHeight), X = random.Next(0, (int)myCanvas.ActualWidth) };
                pixels.Add(dot.Id, dot);
                Canvas.SetTop(dot.Show(), dot.Y);
                Canvas.SetLeft(dot.Show(), dot.X);
                myCanvas.Children.Add(dot.Show());
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

        private void myCanvas_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!addPixel.IsEnabled)
                addPixel.IsEnabled = true;
            myTimer.Stop();
            var point = e.GetPosition(myCanvas);
            var food = new Dot(new Rectangle()
            {
                Height = 25,
                Width = 25,
                Fill = new SolidColorBrush(Color.FromRgb(200, 0, 0))
            }, "food", null)
            { Y = point.Y, X = point.X };
            targets.Add(food.Id, food);
            Canvas.SetTop(food.Show(), food.Y);
            Canvas.SetLeft(food.Show(), food.X);
            myCanvas.Children.Add(food.Show());
            food.RectForm = new Rect(Canvas.GetLeft(food.Show()), Canvas.GetTop(food.Show()), food.Show().Width, food.Show().Height);
            myTimer.Start();
        }
    }
}