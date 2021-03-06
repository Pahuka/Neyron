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
        int tick = 0;
        int generation = 0;
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
            myTimer.Interval = new TimeSpan(0, 0, 0, 0, int.Parse(speed.Text));
        }

        //private Tuple<int, int> GenerateStep(Pixel pixel, int x, int y)
        //{
        //    var nextX = (x + pixel.X + myGrid.ColumnDefinitions.Count) % myGrid.ColumnDefinitions.Count;
        //    var nextY = (y + pixel.Y + myGrid.RowDefinitions.Count) % myGrid.RowDefinitions.Count;
        //    return Tuple.Create(nextX, nextY);
        //}

        private Tuple<double, double> GenerateStep(Dot pixel, float x, float y)
        {
            var nextX = (x + pixel.X + myCanvas.ActualWidth) % myCanvas.ActualWidth;
            var nextY = (y + pixel.Y + myCanvas.ActualHeight) % myCanvas.ActualHeight;
            return Tuple.Create(nextX, nextY);
        }

        private bool IsWall(Dot pixel, float x, float y)
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
                var gByte = pixel.Health <= 0 ? (byte)0 : (byte)pixel.Health;
                pixel.Show().Fill = new SolidColorBrush(Color.FromRgb(color.R, gByte, color.B));
            }
        }

        public double[,] Scalling(double[,] inputs)
        {
            var result = new double[inputs.GetLength(0), inputs.GetLength(1)];
            for (int column = 0; column < inputs.GetLength(1); column++)
            {
                var min = inputs[0, column];
                var max = inputs[0, column];
                for (int row = 1; row < inputs.GetLength(0); row++)
                {
                    var item = inputs[row, column];

                    if (item < min)
                        min = item;
                    if (item > max)
                        max = item;
                }
                var divider = max - min;
                for (int row = 1; row < inputs.GetLength(0); row++)
                {
                    result[row, column] = (inputs[row, column] - min) / divider;
                }
            }

            return result;
        }

        private float Normalization(double x)
        {
            if (x > 20f)
                return 1;
            else if (x < -20f)
                return -1f;
            else
            {
                var a = (float)Math.Exp(x);
                var b = (float)Math.Exp(-x);
                return (a - b) / (a + b);
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
            tick++;
            //pixelCount.Text = tick.ToString();
            var random = new Random();

            if (pixels.Where(x => x.Value.Health > 0).Count() == 0)
            //if (tick % 10000 == 0)
            {
                generation++;
                pixelCount.Text = generation.ToString();
                var result = new List<BloopBrain>();
                var parents = pixels
                    .OrderByDescending(x => x.Value.Brain.fitness)
                    .Take(10)
                    .Select(x => x.Value)
                    .ToArray();
                bestBrain.Text = parents.FirstOrDefault().Brain.fitness.ToString();

                foreach (var item in pixels)
                {
                    myCanvas.Children.Remove(item.Value.Show());
                }
                pixels.Clear();
                if (parents.Length >= 2)
                {
                    for (int i = 1; i < parents.Length; i++)
                    {
                        bornDots(parents[i - 1].Brain.Crossover(parents[i].Brain));
                    }
                }
                else
                {
                    bornDots(parents[0].Brain.Crossover(parents[0].Brain));
                }
                //bornDots(result);
            }

            foreach (var pixel in pixels.Values.Where(x => x.Health > 0))
            {
                var minDistance = double.MaxValue;
                var target = new Dot();
                foreach (var food in targets.Values)
                {
                    var distance = Math.Sqrt(Math.Pow(food.X - pixel.X, 2) + Math.Pow(food.Y - pixel.Y, 2));
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        target = food;
                    }
                }

                var minPixelDistance = double.MaxValue;
                var neighbour = new Dot();
                foreach (var item in pixels.Values.Where(x => x.Clan != pixel.Clan && x.Id != pixel.Id && x.Health > 0 && x.Attack <= pixel.Attack))
                {
                    var tempDistance = Math.Sqrt(Math.Pow(item.X - pixel.X, 2) + Math.Pow(item.Y - pixel.Y, 2));
                    if (tempDistance <= minPixelDistance)
                    {
                        minPixelDistance = tempDistance;
                        neighbour = item;
                    }
                }

                for (int i = 0; i < pixel.Brain.outputLayers.Length; i++)
                {
                    //if (Math.Round(pixel.Brain.outputLayers[i]) == 1)
                    //    pixel.Move(target.X, target.Y);
                    //else
                    //    pixel.Move(neighbour.X, neighbour.Y);

                    if (i == 0)
                    {
                        //    //if (!IsWall(pixel, pixel.Brain.outputLayers[i], 0))
                        //    //    pixel.X += pixel.Brain.outputLayers[i];
                        //    //var step = GenerateStep(pixel, pixel.Brain.outputLayers[i], 0);
                        //    //pixel.X = step.Item1;
                        //    //else
                        //    //{
                        //    //    pixel.Brain.fitness -= 0.01f;
                        //    //}
                        if (Math.Round(pixel.Brain.outputLayers[i]) == 1)
                        {
                            pixel.Move(target.X, target.Y);
                            //        //var step = GenerateStep(pixel, -1, 0);
                            //        //pixel.X = step.Item1;

                            //        //if (!IsWall(pixel, 1, 0))
                            //            pixel.Move(target.X, target.Y);
                            //        //else
                            //        //{
                            //        //    pixel.Brain.fitness -= 0.01f;
                            //        //}
                        }
                        else
                        {
                            pixel.Move(random.Next(0, (int)myCanvas.ActualWidth), random.Next(0, (int)myCanvas.ActualHeight));
                            //    //else if (Math.Round(pixel.Brain.outputLayers[i]) == -1)
                            //    //{
                            //var step = GenerateStep(pixel, random.Next(-1, 2), random.Next(-1, 2));
                            //pixel.X = step.Item1;
                            //pixel.Y = step.Item2;

                            //    //    if (!IsWall(pixel, -1, 0))
                            //    //        pixel.X -= 1;
                            //    //    else
                            //    //    {
                            //    //        pixel.Brain.fitness -= 0.01f;
                            //    //    }
                            //    //}
                        }
                    }
                    if (i == 1)
                    {
                        //    //if (!IsWall(pixel, 0, pixel.Brain.outputLayers[i]))
                        //    //    pixel.Y += pixel.Brain.outputLayers[i];
                        //    //var step = GenerateStep(pixel, 0, pixel.Brain.outputLayers[i]);
                        //    //pixel.Y = step.Item2;
                        //    //else
                        //    //{
                        //    //    pixel.Brain.fitness -= 0.01f;
                        //    //}
                        if (Math.Round(pixel.Brain.outputLayers[i]) == 1)
                        {
                            pixel.Move(neighbour.X, neighbour.Y);
                            //        //var step = GenerateStep(pixel, 0, -1);
                            //        //pixel.Y = step.Item2;

                            //        //if (!IsWall(pixel, 0, 1))
                            //            pixel.Move(neighbour.X, neighbour.Y);
                            //        //else
                            //        //{
                            //        //    pixel.Brain.fitness -= 0.01f;
                            //        //}
                        }
                        else
                        {
                            pixel.Move(random.Next(0, (int)myCanvas.ActualWidth), random.Next(0, (int)myCanvas.ActualHeight));
                            //    //else if (Math.Round(pixel.Brain.outputLayers[i]) == -1)
                            //    //{
                            //var step = GenerateStep(pixel, random.Next(-1, 2), random.Next(-1, 2));
                            //pixel.X = step.Item1;
                            //pixel.Y = step.Item2;

                            //    //    if (!IsWall(pixel, 0, -1))
                            //    //        pixel.Y -= 1;
                            //    //    else
                            //    //    {
                            //    //        pixel.Brain.fitness -= 0.01f;
                            //    //    }
                            //    //}
                        }
                    }
                }

                //pixel.RectForm = new Rect(Canvas.GetLeft(pixel.Show()), Canvas.GetTop(pixel.Show()), pixel.Show().Width, pixel.Show().Height);
                pixel.RectForm = new Rect(pixel.X, pixel.Y, pixel.Show().Width, pixel.Show().Height);
                var newFoodDistance = 0.0;
                if (pixel.RectForm.IntersectsWith(target.RectForm))
                {
                    //MakeIntersect(pixel, target, Brushes.White);
                    //target.Show().Stroke = Brushes.Black;

                    target.Health -= pixel.Attack;
                    pixel.Health += target.Health / pixel.Attack;
                    pixel.Brain.fitness += 0.05f;
                    //pixel.Show().ToolTip = ToolTip = new ToolTip() { Content = $"{pixel.Score}" };
                    newFoodDistance = Math.Sqrt(Math.Pow(target.X - pixel.X, 2) + Math.Pow(target.Y - pixel.Y, 2));
                    if (newFoodDistance <= minDistance)
                        pixel.Brain.fitness += 0.1f;
                    if (target.Health <= 0)
                    {
                        pixel.Speed = 0.5f;
                        myCanvas.Children.Remove(target.Show());
                        //targets.Clear();
                        targets.Remove(target.Id);
                        makeFood(1);
                    }
                }

                var pixelsCollide = pixels.Values.Where(x => x.Health > 0).Where(x => x.Clan != pixel.Clan && x.Id != pixel.Id & x.RectForm.IntersectsWith(pixel.RectForm));
                if (pixelsCollide.Count() != 0)
                {
                    //pixel.Brain.fitness -= 0.05f;
                    pixel.Neighbours = pixelsCollide.Count();
                    foreach (var item in pixelsCollide)
                    {
                        //MakeIntersect(pixel, item, Brushes.Gray);
                        item.Health -= pixel.Attack;
                        //pixel.Health += item.Health / pixel.Attack;
                        pixel.Brain.fitness += 0.001f;
                    }
                }
                pixel.Health--;
                Canvas.SetTop(pixel.Show(), pixel.Y);
                Canvas.SetLeft(pixel.Show(), pixel.X);


                pixel.Brain.inputLayers[0] = Normalization(pixel.X);
                pixel.Brain.inputLayers[1] = Normalization(pixel.Y);
                //pixel.Brain.inputLayers[0] = (float)newFoodDistance;
                //pixel.Brain.inputLayers[1] = (float)(minPixelDistance);
                //pixel.Brain.inputLayers[2] = (float)(pixel.Neighbours);
                pixel.Brain.inputLayers[2] = Normalization(newFoodDistance);
                pixel.Brain.inputLayers[3] = Normalization(minPixelDistance);
                pixel.Brain.inputLayers[4] = Normalization(pixel.Neighbours);
                pixel.Brain.inputLayers[5] = Normalization(pixel.Health);

                pixel.Brain.ping();
                target.Show().Stroke = Brushes.White;
            }

            var deads = pixels.Values
                .OrderBy(x => x.Brain.fitness)
                .Where(x => x.Health <= 0)
                .Select(x => x.Id);
            foreach (var key in deads)
            {
                myCanvas.Children.Remove(pixels[key].Show());
                //pixels.Remove(key);
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

        private void MakeIntersect(Dot pixel, Dot target, Brush color)
        {
            var g1 = pixel.Show().RenderedGeometry.GetWidenedPathGeometry(new Pen(Brushes.Transparent, 0.1));
            var g2 = target.Show().RenderedGeometry.GetWidenedPathGeometry(new Pen(Brushes.Transparent, 0.1));
            var comb = Geometry.Combine(g1, g2, GeometryCombineMode.Union, pixel.Show().GeometryTransform);
            //var c = new CombinedGeometry(GeometryCombineMode.Union, g1, g2);
            var bounds = comb.GetRenderBounds(new Pen(Brushes.Transparent, 0.1));
            var r = new RectangleGeometry() { Rect = bounds };
            var result = new Path() { Data = r, Stroke = color, Fill = color };
            Canvas.SetTop(result, target.Y);
            Canvas.SetLeft(result, target.X);
            myCanvas.Children.Add(result);
        }

        private void sizeButton_Click(object sender, RoutedEventArgs e)
        {
            tick = 0;
            generation = 0;
            addPixel.IsEnabled = false;
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
                    }, "hunter", brain)
                    {
                        Y = random.Next(0, (int)myCanvas.ActualHeight),
                        X = random.Next(0, (int)myCanvas.ActualWidth)
                    };

                    pixels.Add(dot.Id, dot);
                    Canvas.SetLeft(dot.Show(), dot.X - dot.Show().Width / 2);
                    Canvas.SetTop(dot.Show(), dot.Y - dot.Show().Height / 2);
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
                var food = new Dot(new Ellipse()
                {
                    Height = 25,
                    Width = 25,
                    Fill = new SolidColorBrush(Color.FromRgb(200, 0, 0))
                }, "food", null)
                {
                    Y = random.Next(0, (int)myCanvas.ActualHeight),
                    X = random.Next(0, (int)myCanvas.ActualWidth),
                };
                food.RectForm = new Rect(food.X - food.Show().Width / 2, food.Y - food.Show().Height / 2, 15, 15);
                targets.Add(food.Id, food);
                Canvas.SetLeft(food.Show(), food.X - food.Show().Width / 2);
                Canvas.SetTop(food.Show(), food.Y - food.Show().Height / 2);
                myCanvas.Children.Add(food.Show());
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
                }, "hunter", brains[i])
                { Y = random.Next(0, (int)myCanvas.ActualHeight), X = random.Next(0, (int)myCanvas.ActualWidth) };
                pixels.Add(dot.Id, dot);
                Canvas.SetLeft(dot.Show(), dot.X - dot.Show().Width / 2);
                Canvas.SetTop(dot.Show(), dot.Y - dot.Show().Height / 2);
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
            //myTimer.Stop();
            var point = e.GetPosition(myCanvas);
            var food = new Dot(new Ellipse()
            {
                Height = 25,
                Width = 25,
                Fill = new SolidColorBrush(Color.FromRgb(200, 0, 0))
            }, "food", null)
            {
                Y = point.Y,
                X = point.X
            };
            food.RectForm = new Rect(point.X - food.Show().Width / 2, point.Y - food.Show().Height / 2, 15, 15);
            targets.Add(food.Id, food);
            Canvas.SetLeft(food.Show(), point.X - food.Show().Width / 2);
            Canvas.SetTop(food.Show(), point.Y - food.Show().Height / 2);
            myCanvas.Children.Add(food.Show());
            //myTimer.Start();
        }
    }
}