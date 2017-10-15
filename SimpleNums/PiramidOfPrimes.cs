using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SimpleNums
{
    class PiramidOfPrimes : DrawingVisual
    {
        public int H
        {
            get
            {
                return _h;
            }
            set
            {
                _h = value;
            }
        }
        private int _h;
        private int _lenNumberInText;
        private int _maxSymbols;


        private double Width => _maxSymbols * 8.4;
        private double Height => H * 16;


        public PiramidOfPrimes(int h)
        {
            H = h;
            UpdateText();
        }

        public void UpdateText()
        {
            Initialize();
            var text = GetText(H);
            DrawText(text);
        }

        public Size GetSize()
        {
            return new Size(Width, Height);
        }

        private void Initialize()
        {
            var countNumbersInLast = H * H - (H - 1) * (H - 1);
            _lenNumberInText = Nums.GetDigitNumber(H * H);
            _maxSymbols = countNumbersInLast * _lenNumberInText + (countNumbersInLast - 1);
        }

        private string GetText(int h)
        {
            var str = new StringBuilder();
            int n = 1;
            for (int i = 1; i <= h; i++)
            {
                //str.AppendSpaces(FindSpacesCount(i * 2 - 1));
                for (int j = 0; j < i * 2 - 1; j++)
                {
                    str.AppendWithSpaces(n++, _lenNumberInText);
                }
                str.AppendLine();
            }

            return str.ToString();
        }

        private int FindSpacesCount(int currentCountOfNumbers)
        {
            int spaces = _maxSymbols - (currentCountOfNumbers * _lenNumberInText) - (currentCountOfNumbers - 1);
            var startLine = spaces / 2;
            return startLine;
        }

        private void DrawText(string text)
        {
            using (DrawingContext dc = RenderOpen())
            {
                FormattedText ft = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                                                        new Typeface("Courier New"), 14, Brushes.Black);
                ft.TextAlignment = TextAlignment.Center;
                dc.DrawText(ft, new Point(Width / 2, 0));
            }
        }
    }

    public class VisualHost : UIElement
    {
        public Visual Visual { get; set; }

        public VisualHost(Visual visual)
        {
            Visual = visual;
        }

        protected override int VisualChildrenCount
        {
            get { return Visual != null ? 1 : 0; }
        }

        protected override Visual GetVisualChild(int index)
        {
            return Visual;
        }
    }

    public static class StringBuilderExtensions
    {
        public static void AppendWithSpaces(this StringBuilder text, int value, int lengthText)
        {
            int lost = lengthText - Nums.GetDigitNumber(value);
            AppendSpaces1(text, lost);
            text.Append(value);
            text.Append(" ");
        }

        public static void AppendSpaces1(this StringBuilder text, int count)
        {
            for (int i = 0; i < count; i++)
            {
                text.Append("д");
            }
        }
        public static void AppendSpaces(this StringBuilder text, int count)
        {
            for (int i = 0; i < count; i++)
            {
                text.Append("п");
            }
        }
    }

    public class RectangleDrawer
    {
        public void DrawRectangle(int value, int h, Canvas canvas)
        {
            var point = GetPoint(value, h);
            var rect = new Rectangle();
            rect.Fill = new SolidColorBrush(Colors.Yellow);
            
            //X
            var max_nums = h * h - (h - 1) * (h - 1);
            var max_digit = Nums.GetDigitNumber(h * h);
            var punct = canvas.Width / (max_nums * max_digit + max_nums - 1);
            var valueX = ((point.X-1) * max_digit) + point.X - 1;
            point.X = valueX * punct;

            //Y
            var line1 = canvas.Height / (h + 1);
            point.Y = (point.Y - 1) * line1;

            rect.Width = max_digit * punct;
            rect.Height = line1;

            Canvas.SetTop(rect, point.Y);
            Canvas.SetLeft(rect, point.X);
            canvas.Children.Add(rect);
        }

        public void DrawRectangleWithPosition(double x, double y, Color color, Canvas canvas)
        {
            var rect = new Rectangle();
            rect.Fill = new SolidColorBrush(color);
            rect.Width = 30;
            rect.Height = 15;
            Canvas.SetTop(rect, y);
            Canvas.SetLeft(rect, x);
            canvas.Children.Add(rect);
        }

        private Point GetPoint(int value, int h)
        {
            double x = 0;
            double y = 0;

            y = Math.Sqrt(value);
            if (!IsInteger(y))
            {
                y = (int)y + 1;
            }
            //x = 15 * ((h - y * y - (y - 1) * (y - 1)) / 2 + value - (y - 1) * (y - 1));
            //сколько числе в y-строке
            double n_y = y * y - (y - 1) * (y - 1);
            double n_h = h * h - (h - 1) * (h - 1);
            //сколько "пустых" чисел спереди:
            int n_empty = (int)((n_h - n_y) / 2);
            //какое по счету в ряду value
            double n_in_row = value - (y - 1) * (y - 1);
            //сложим пустые числа и предыдущее значение
            x = n_empty + n_in_row;
            //x = 3 * 10 * x;
            //y = 15 * y;
            return new Point(x, y);
        }

        private bool IsInteger(double num)
        {
            return Math.Truncate(num) == num;
        }
    }

}
