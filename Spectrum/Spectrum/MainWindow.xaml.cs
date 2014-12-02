using System;
using System.Windows.Media;

namespace Spectrum {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public MainWindow() {
            InitializeComponent();
        }

        private ColourSpace _currentColourSpace = new Srgb();

        private void ConvertButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var newColor = SpectrumToColor();
            SelectedColorCanvas.Background = newColor;
        }

        private Brush SpectrumToColor()
        {
            var r = _currentColourSpace.ConversionMatrix[0][0] * SpectrumPlot.X + _currentColourSpace.ConversionMatrix[0][1] * SpectrumPlot.Y +
           _currentColourSpace.ConversionMatrix[0][2] * SpectrumPlot.Z;
            var g = _currentColourSpace.ConversionMatrix[1][0] * SpectrumPlot.X + _currentColourSpace.ConversionMatrix[1][1] * SpectrumPlot.Y +
                    _currentColourSpace.ConversionMatrix[1][2] * SpectrumPlot.Z;
            var b = _currentColourSpace.ConversionMatrix[2][0] * SpectrumPlot.X + _currentColourSpace.ConversionMatrix[2][1] * SpectrumPlot.Y +
                    _currentColourSpace.ConversionMatrix[2][2] * SpectrumPlot.Z;

            //If has negative values - move inside gamut
            var w = Math.Min(0, Math.Min(r, Math.Min(g, b)));
            w = -w;

            if (w > 0)
            {
                r += w;
                g += w;
                b += w;
            }

            //normalize to 1
            var max = Math.Max(r, Math.Max(g, b));
            r /= max;
            g /= max;
            b /= max;

            r *= 255;
            g *= 255;
            b *= 255;
            return new SolidColorBrush(new Color { R = (byte)r, G = (byte)g, B = (byte)b, A = 255});
        }

        private void ColourSpaceComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var index = ColourSpaceComboBox.SelectedIndex;

            switch (index)
            {
                case 0:
                    _currentColourSpace = new Srgb();
                    break;
                case 1:
                    _currentColourSpace = new AdobeRgb();
                    break;
                default:
                    _currentColourSpace = new Srgb();
                    break;
            }
        }
    }
}
