using System;
using System.Collections.Generic;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
using System.Xml;
using System.Globalization;

namespace SystemTrayApp.Components
{
    public partial class BubbleEffect : Window
    {
        private Random random = new Random();
        private DispatcherTimer timer;

        public BubbleEffect()
        {
            InitializeComponent();

            RenderTextAsSvg("SVG Text\n Hello World!");

            // Thiết lập Timer để tạo bọt khí mới mỗi 0.5 giây
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void RenderTextAsSvg(string text)
        {
            var lines = WrapTextToLines(text);
            var lineMarkup = new StringBuilder();
            for (int i = 0; i < lines.Count; i++)
            {
                var escapedLine = SecurityElement.Escape(lines[i]);
                lineMarkup.Append(i == 0
                    ? $"<tspan x='50%' dy='0'>{escapedLine}</tspan>"
                    : $"<tspan x='50%' dy='1.2em'>{escapedLine}</tspan>");
            }

            var fontSize = lines.Count > 2 ? "28" : "34";
            var (svgWidth, svgHeight) = CalculateSvgSize(lines, int.Parse(fontSize));

            Console.WriteLine($"SVG Size: {svgWidth} x {svgHeight}");

            string svgMarkup = $@"<svg xmlns='http://www.w3.org/2000/svg'
                    width='{svgWidth}' height='{svgHeight}' viewBox='0 0 {svgWidth} {svgHeight}'>
                    <defs>
                        <linearGradient id='bgGrad' x1='0%' y1='0%' x2='100%' y2='0%'>
                            <stop offset='0%' stop-color='#4facfe'/>
                            <stop offset='100%' stop-color='#00f2fe'/>
                        </linearGradient>
                    </defs>
                    <defs>
                        <linearGradient id='textGrad' x1='0%' y1='0%' x2='0%' y2='100%'>
                            <stop offset='0%' stop-color='#ff7e5f'/>
                            <stop offset='100%' stop-color='#feb47b'/>
                        </linearGradient>
                    </defs>
                    <rect width='100%' height='100%' fill='url(#bgGrad)' rx='12' ry='12'/>
                    <text y='40' text-anchor='middle' 
                          font-family='Segoe UI' font-size='{fontSize}' font-weight='bold'
                          fill='url(#textGrad)'>
                        {lineMarkup}
                    </text>
                </svg>";

            //         dominant-baseline='middle'    Gradient nằm phía sau nhưng chỉ hiển thị qua chữ (text như mask) — tạo hiệu ứng "text cutout" để nền gradient chỉ hiện trong lòng chữ:<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 600 160" width="600" height="160"> <defs> <linearGradient id="bgGrad" x1="0%" y1="0%" x2="100%" y2="0%"> <stop offset="0%" stop-color="#7b92ff"/> <stop offset="100%" stop-color="#00d4ff"/> </linearGradient> <!-- mask: chữ tạo vùng trong suốt --> <mask id="textMask"> <!-- toàn vùng mặc định đen = trong suốt cho mask, nên fill white cho vùng chữ --> <rect width="100%" height="100%" fill="black"/> <text x="50%" y="50%" text-anchor="middle" dominant-baseline="middle" font-family="Segoe UI, Arial" font-size="48" font-weight="700" fill="white"> {TEXT} </text> </mask>
            // </defs> <!-- nền gradient --> <rect width="100%" height="100%" fill="url(#bgGrad)"/> <!-- áp mask để chỉ hiển thị gradient trong chữ --> <rect width="100%" height="100%" fill="white" mask="url(#textMask)"/> </svg>

            var sr = new StringReader(svgMarkup);

            var xr = XmlReader.Create(sr);

            try
            {
                // 2. Cấu hình reader
                var settings = new WpfDrawingSettings
                {
                    IncludeRuntime = true,
                    TextAsGeometry = false // hoặc true nếu muốn text thành geometry
                };

                var reader = new FileSvgReader(settings);

                // 3. Đọc file SVG -> DrawingGroup
                var drawing = reader.Read(xr);
                if (drawing == null)
                {
                    // fallback: không đọc được
                    return;
                }

                // 4. Chuyển DrawingGroup thành Image (DrawingImage)
                var drawingImage = new DrawingImage(drawing);

                var image = new Image
                {
                    Source = drawingImage,
                    Width = drawing.Bounds.Width,   // hoặc đặt kích thước mong muốn
                    Height = drawing.Bounds.Height,
                    Stretch = Stretch.None
                };

                // 5. Đặt vị trí và thêm vào Canvas
                Canvas.SetLeft(image, 10);
                Canvas.SetTop(image, 10);
                MainCanvas.Children.Add(image);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu cần
                Console.WriteLine($"Error rendering SVG: {ex.Message}");
            }
        }

        private (double width, double height) CalculateSvgSize(List<string> lines, int charWidth = 10)
        {
            string longestLine = lines[0];
            foreach (var line in lines)
            {
                if (line.Length > longestLine.Length)
                    longestLine = line;
            }

            var formattedText = new FormattedText(
                longestLine,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Segoe UI"),
                charWidth,
                Brushes.Black,
                VisualTreeHelper.GetDpi(new ContainerVisual()).PixelsPerDip
            );

            double width = Math.Max(180, formattedText.Width + charWidth);
            double height = Math.Max(100, lines.Count * charWidth + charWidth);

            return (width, height);
        }

        private List<string> WrapTextToLines(string text)
        {
            var words = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var lines = new List<string>();

            foreach (var word in words)
            {
                lines.Add(word.Trim());
            }

            return lines.Count > 0 ? lines : new List<string> { "No text" };
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TaoBotKhi();
        }

        private void TaoBotKhi()
        {
            // 1. Tạo hình dáng bọt khí
            int size = random.Next(30, 60);
            Ellipse bubble = new Ellipse
            {
                Width = size,
                Height = size,
                Fill = new SolidColorBrush(Colors.White) { Opacity = 0.6 },
                Stroke = Brushes.LightBlue,
                StrokeThickness = 2
            };

            // 2. Định vị trí xuất hiện ngẫu nhiên ở đáy màn hình
            double startX = random.Next(0, (int)MainCanvas.ActualWidth - (int)bubble.Width);
            double startY = MainCanvas.ActualHeight;

            Canvas.SetLeft(bubble, startX);
            Canvas.SetTop(bubble, startY);

            MainCanvas.Children.Add(bubble);

            // 3. Thiết lập hiệu ứng dịch chuyển dọc (TranslateY)
            DoubleAnimation moveY = new DoubleAnimation
            {
                From = startY,
                To = -100, // Bay lên khỏi màn hình
                Duration = TimeSpan.FromSeconds(random.Next(4, 8)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };

            // 4. Thiết lập hiệu ứng mờ dần khi bay lên
            DoubleAnimation fade = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = moveY.Duration
            };

            // 5. Tạo hiệu ứng lắc lư ngang (TranslateX) nhẹ để bọt khí bay tự nhiên
            DoubleAnimation moveX = new DoubleAnimation
            {
                From = startX,
                To = startX + random.Next(-30, 30),
                Duration = TimeSpan.FromSeconds(moveY.Duration.TimeSpan.TotalSeconds / 2),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            // 6. Nhóm các hiệu ứng vào Storyboard
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(moveY);
            storyboard.Children.Add(fade);
            storyboard.Children.Add(moveX);

            // Gán đối tượng mục tiêu cho các Animation
            Storyboard.SetTarget(moveY, bubble);
            Storyboard.SetTargetProperty(moveY, new PropertyPath("(Canvas.Top)"));

            Storyboard.SetTarget(fade, bubble);
            Storyboard.SetTargetProperty(fade, new PropertyPath(UIElement.OpacityProperty));

            Storyboard.SetTarget(moveX, bubble);
            Storyboard.SetTargetProperty(moveX, new PropertyPath("(Canvas.Left)"));

            // Dọn dẹp bộ nhớ (xóa UI element) khi Animation kết thúc
            storyboard.Completed += (s, ev) =>
            {
                MainCanvas.Children.Remove(bubble);
            };

            // 7. Bắt đầu chạy hiệu ứng
            storyboard.Begin();
        }
    }
}