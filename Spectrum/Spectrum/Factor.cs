using System.Windows;
using System.Windows.Shapes;

namespace Spectrum {
    public class Factor
    {
        public Point FactorPoint { get; set; }

        public Rectangle DrawnFactor { get; set; }

        public int Wavelength { get; set; }

        public double Intensity { get; set; }

        public Line LeftLine { get; set; }
        public Line RightLine { get; set; }

        public Factor(int wavelength)
        {
            Wavelength = wavelength;
            Intensity = Constants.MinIntensity;
            FactorPoint = new Point( (wavelength - Constants.MinWavelength) * 2, Constants.PlotHeight);
        }

    }
}
