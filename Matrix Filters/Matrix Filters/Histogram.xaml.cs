using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Matrix_Filters {
    /// <summary>
    /// Interaction logic for Histogram.xaml
    /// </summary>
    public partial class Histogram {
        public int NumberOfPixels { get; set; }
        public int MaximumNumberOfPixelForGivenColor { get; set; }
        public string MaximumStringified { get; set; }

        public static readonly int HistogramHeigth = 150;
        public static readonly int HistogramWidth = 256;

        public static readonly int PlotHeigth = 160;

        public Color LinesColor { get; set; }

        public List<Line> HistogramLines { get; set; }

        public Histogram(Color color) {
            InitializeComponent();

            LinesColor = color;
            HistogramLines = new List<Line>();
            MaximumStringified = "1";
            InitLines();
        }

        private void InitLines() {
            for (var i = 0; i < 256; i++) {
                var line = new Line { X1 = i, Y1 = PlotHeigth, X2 = i, Y2 = PlotHeigth, StrokeThickness = 1, Stroke = new SolidColorBrush(LinesColor) };
                HistogramLines.Add(line);
                PlotAreaCanvas.Children.Add(line);
            }
        }

        public unsafe void UpdateHistogram(WriteableBitmap image) {
            NumberOfPixels = image.PixelWidth * image.PixelHeight;

            var helperList = new int[256];

            using (var imageContext = image.GetBitmapContext(ReadWriteMode.ReadOnly)) {
                var width = imageContext.Width;
                var height = imageContext.Height;
                var pixels = imageContext.Pixels;
                var index = 0;

                for (var i = 0; i < height; i++) {
                    for (var j = 0; j < width; j++)
                    {
                        var value = 0;
                        var pixelColor = pixels[index];

                        if (LinesColor == Colors.Red) {
                            value = ((pixelColor >> 16) & 0x000000FF);
                        } else if (LinesColor == Colors.Green) {
                            value = ((pixelColor >> 8) & 0x000000FF);
                        } else if (LinesColor == Colors.Blue) {
                            value = ((pixelColor) & 0x000000FF);
                        }

                        helperList[value]++;
                        index++;
                    }
                }
            }

            MaximumNumberOfPixelForGivenColor = helperList.Max();
            MaximumStringified = MaximumNumberOfPixelForGivenColor.ToString(CultureInfo.InvariantCulture);

            CalculateLines(helperList);
        }

        private void CalculateLines(IList<int> values) {
            for (var i = 0; i < values.Count; i++) {
                var newYValue = PlotHeigth * (1 - (double)values[i] / MaximumNumberOfPixelForGivenColor);
                HistogramLines[i].Y2 = newYValue;
            }
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e) {
        }
    }
}
