using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
        private bool[] isSimple;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void GeneratePiramid_Click(object sender, RoutedEventArgs e)
        {
            limit = 0;
            FindSimpleNums();
            GenerateNumberTable();
        }

        private void FindSimpleNums()
        {
            limit = h * h;
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

        private void GenerateNumberTable()
        {
            canvas.Children.Clear();
            int lenMax = limit - (h - 1) * (h - 1); //количество колонок

            int gridWidth = lenMax * 35;
            int gridHeight = h * 21;
            int elemsCount = 1;
            int simpleCounter = 1;
            int beginX = gridWidth / 2 - 15;
            int currentY = 0;
            for (int j = 0; j < h; j++)
            {
                int currentX = beginX;
                for (int i = 0; i < elemsCount; i++)
                {
                    var tb = new TextBlock();
                    tb.Text = simpleCounter.ToString();  
                    tb.RenderTransform = new TranslateTransform { X = currentX, Y = currentY};
                    if(isSimple[simpleCounter])
                        tb.Background = new SolidColorBrush(Color.FromArgb(80, 255, 255, 0));
                    canvas.Children.Add(tb);
                    tb.Width = 30;
                    tb.Height = 20;
                    currentX += 30;
                    
                    simpleCounter++;
                }
                currentY += 20;
                elemsCount += 2;
                beginX = beginX - 30;
            }

            canvas.Background = new SolidColorBrush(Color.FromArgb(50, 50, 100, 150));
            canvas.Width = gridWidth;
            canvas.Height = gridHeight;
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
