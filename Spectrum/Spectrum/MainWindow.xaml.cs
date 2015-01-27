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
            double r, g, b;
            CalculateRgbValues(out r, out g, out b);

            //If has negative values - move inside gamut
            RecalculateToGamut(ref r, ref g, ref b);

            //Normalize to [0:1]
            NormalizeRgb(ref r, ref g, ref b);

            //Create new colour with Alpha channel set to maximum
            return new SolidColorBrush(new Color { R = (byte)r, G = (byte)g, B = (byte)b, A = 255});
        }

        private void CalculateRgbValues(out double r, out double g, out double b)
        {
            r = _currentColourSpace.ConversionMatrix[0][0]*SpectrumPlot.X +
                _currentColourSpace.ConversionMatrix[0][1]*SpectrumPlot.Y +
                _currentColourSpace.ConversionMatrix[0][2]*SpectrumPlot.Z;
            g = _currentColourSpace.ConversionMatrix[1][0]*SpectrumPlot.X +
                _currentColourSpace.ConversionMatrix[1][1]*SpectrumPlot.Y +
                _currentColourSpace.ConversionMatrix[1][2]*SpectrumPlot.Z;
            b = _currentColourSpace.ConversionMatrix[2][0]*SpectrumPlot.X +
                _currentColourSpace.ConversionMatrix[2][1]*SpectrumPlot.Y +
                _currentColourSpace.ConversionMatrix[2][2]*SpectrumPlot.Z;
        }

        private static void NormalizeRgb(ref double r, ref double g, ref double b)
        {
            var max = Math.Max(r, Math.Max(g, b));
            r /= max;
            g /= max;
            b /= max;

            r *= 255;
            g *= 255;
            b *= 255;
        }

        private static void RecalculateToGamut(ref double r, ref double g, ref double b)
        {
            var w = Math.Min(0, Math.Min(r, Math.Min(g, b)));
            w = -w;

            if (!(w > 0)) return;

            r += w;
            g += w;
            b += w;
        }

        private void ColourSpaceComboBox_SelectionChanged(object sender,
            System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var index = ColourSpaceComboBox.SelectedIndex;
            var selectedType = (ColourSpaceType) ColourSpaceComboBox.SelectedItem;
            _currentColourSpace = ColourSpaceFactory.CreateColourSpace(selectedType);
        }

        private void ResetButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            SpectrumPlot.Reset();
        }
    }
}
