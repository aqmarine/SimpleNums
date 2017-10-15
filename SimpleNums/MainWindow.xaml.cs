using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace SimpleNums
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int h = 0;
        private int limit;
        private int lenNumberInText; //количество символов, которое будет занимать одно число, включая пробелы
        private int countNumbersInLast; //максимальное количество цифр в последней строке пирамиды
        private int maxSymbols;
        private bool[] isSimple;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void M(int h)
        {
            RectangleDrawer dr = new RectangleDrawer();
            dr.DrawRectangle(16, h, canvas);
            dr.DrawRectangleWithPosition(0, 0, Colors.Pink, canvas);
            dr.DrawRectangleWithPosition(0, 15, Colors.Green, canvas);

        }

        private void DrawPiramid(int h)
        {
            var p = new PiramidOfPrimes(h);
            canvas.Children.Add(new VisualHost(p));
            SetSize(p);
        }

        private void SetSize(PiramidOfPrimes p)
        {
            int MAX_WIDTH = 1200;
            int MAX_HEIGHT = 600;
            var size = p.GetSize();
            if (size.Width < MAX_WIDTH)
            {
                grid.Width = size.Width;
                canvas.Width = size.Width;
                this.Width = size.Width + 20;
            }
            else
            {
                grid.Width = size.Width;
                canvas.Width = size.Width;
                this.Width = MAX_WIDTH + 70;
            }
            if (size.Height < MAX_HEIGHT)
            {
                grid.Height = size.Height;
                canvas.Height = size.Height;
                this.Height = size.Height + 100;
            }
            else
            {
                grid.Height = size.Height;
                canvas.Height = size.Height;
                this.Height = MAX_HEIGHT + 100;
            }
        }

        private void GeneratePiramid_Click(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();
            //grid.Children.Clear();
            var sw = Stopwatch.StartNew();
            InitialValues();
            FindSimpleNums();
            DrawPiramid(h);
            M(h);
            //GenerateNumberTable2();
            this.Title = sw.ElapsedMilliseconds + "ms";
            //MessageBox.Show("the end");
        }

        private void FindSimpleNums()
        {

            isSimple = new bool[limit + 1];
            int sqrt = (int)Math.Sqrt(limit);
            for (int i = 0; i < limit; i++)
            {
                isSimple[i] = false;
            }

            //2,3,5 - заведомо простые числа
            if (h >= 2)
            {
                isSimple[2] = true;
                isSimple[3] = true;
            }
            if (h >= 3)
            {
                isSimple[5] = true;
            }


            int x2 = 0;
            for (int i = 1; i <= sqrt; i++)
            {
                x2 += 2 * i - 1;
                int y2 = 0;
                for (int j = 1; j <= sqrt; j++)
                {
                    y2 += 2 * j - 1;
                    int n = 4 * x2 + y2;
                    if (n <= limit && (n % 12 == 1 || n % 12 == 5))
                    {
                        isSimple[n] = true;
                    }
                    n -= x2;
                    if (n <= limit && (n % 12 == 7))
                    {
                        isSimple[n] = true;
                    }
                    n -= 2 * y2;
                    if (i > j && n <= limit && n % 12 == 11)
                    {
                        isSimple[n] = true;
                    }
                }
            }

            //отсеять квадраты
            for (int i = 5; i < sqrt; i++)
            {
                int n = i * i;
                for (int j = n; j < limit; j += n)
                {
                    isSimple[j] = false;
                }
            }
        }


        //private void GenerateNumberTable()
        //{
        //    canvas.Children.Clear();
        //    int gridWidth = countNumbersInLast * 35;
        //    int gridHeight = h * 21;
        //    int elemsCount = 1;
        //    int simpleCounter = 1;
        //    int beginX = gridWidth / 2 - 15;
        //    int currentY = 0;
        //    for (int j = 0; j < h; j++)
        //    {
        //        int currentX = beginX;
        //        for (int i = 0; i < elemsCount; i++)
        //        {
        //            var tb = new TextBlock();
        //            tb.Text = simpleCounter.ToString();
        //            tb.RenderTransform = new TranslateTransform { X = currentX, Y = currentY };
        //            if (isSimple[simpleCounter])
        //                tb.Background = new SolidColorBrush(Color.FromArgb(80, 255, 255, 0));
        //            canvas.Children.Add(tb);
        //            tb.Width = 30;
        //            tb.Height = 20;
        //            currentX += 30;

        //            simpleCounter++;
        //        }
        //        currentY += 20;
        //        elemsCount += 2;
        //        beginX = beginX - 30;
        //    }

        //    canvas.Background = new SolidColorBrush(Color.FromArgb(50, 50, 100, 150));
        //    canvas.Width = gridWidth;
        //    canvas.Height = gridHeight;
        //}

        //public void GenerateNumberTable2()
        //{
        //    textBlock.Text = "";
        //    //ScrollViewer sc = new ScrollViewer();
        //    //sc.Content = textBlock;
        //    textBlock.FontFamily = new FontFamily("Courier New");
        //    int elemsCount = 1;
        //    int simpleCounter = 1;
        //    var sb = new StringBuilder();
        //    var lastIsSimple = false;

        //    for (int j = 0; j < h; j++)
        //    {
        //        sb.AppendSpaces(FindSpacesCount(elemsCount));
        //        for (int i = 0; i < elemsCount; i++)
        //        {
        //            if (isSimple[simpleCounter])
        //            {
        //                if (!lastIsSimple)
        //                {
        //                    textBlock.Inlines.Add(sb.ToString());
        //                    //Strings.Add(sb.ToString());
        //                    sb.Clear();
        //                }
        //                lastIsSimple = true;
        //            }
        //            else
        //            {
        //                if (lastIsSimple)
        //                {
        //                    var run = new Run(sb.ToString());
        //                    run.Background = Brushes.Yellow;
        //                    textBlock.Inlines.Add(run);
        //                    sb.Clear();
        //                }
        //                lastIsSimple = false;

        //            }
        //            sb.AppendWithSpaces(simpleCounter, lenNumberInText);
        //            simpleCounter++;
        //        }
        //        if (sb != null)
        //        {
        //            textBlock.Inlines.Add(sb.ToString());
        //            sb.Clear();
        //        }
        //        textBlock.Inlines.Add(Environment.NewLine);

        //        elemsCount += 2;

        //    }
        //    //canvas.Children.Add(textBlock);
        //    //grid.Children.Add(textBlock);
        //    textBlock.Width = countNumbersInLast * lenNumberInText * 10 + 50;
        //    textBlock.Height = h * 22;
        //    grid.Width = countNumbersInLast * lenNumberInText * 10 + 50;
        //    grid.Height = h * 22;

        //}



        private void InitialValues()
        {
            limit = h * h;
            lenNumberInText = Nums.GetDigitNumber(limit);
            countNumbersInLast = limit - (h - 1) * (h - 1);
            maxSymbols = countNumbersInLast * lenNumberInText + (countNumbersInLast - 1);
        }

        private void PiramidHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(PiramidHeight.Text))
            {
                GeneratePiramid.IsEnabled = false;
            }
            else if (!Int32.TryParse(PiramidHeight.Text, out h))
            {
                MessageBox.Show("Введенное значение не является числом");
                PiramidHeight.Text = String.Empty;
                GeneratePiramid.IsEnabled = false;
            }
            else
            {
                GeneratePiramid.IsEnabled = true;
            }
        }
    }


}
