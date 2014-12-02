using System.Collections.Generic;

namespace Spectrum {
    public struct ColorMatcher {
        public double X;
        public double Y;
        public double Z;
    }

    public class Matrix3X3
    {
        private readonly List<List<double>> _matrix;

        public Matrix3X3(double v1, double v2, double v3, double v4, double v5, double v6, double v7, double v8, double v9)
        {
            _matrix = new List<List<double>>
            {
                new List<double> {v1, v2, v3},
                new List<double> {v4, v5, v6},
                new List<double> {v7, v8, v9}
            };
        }

        public List<double> this[int key]
        {
            get { return _matrix[key]; }
        }
    }

    public static class Matricies {
        static Matricies()
        {
            ColorMatchers = new List<ColorMatcher>
            {
                new ColorMatcher {X = 0.0014, Y = 0.0000, Z = 0.0065}, new ColorMatcher {X = 0.0022, Y = 0.0001, Z = 0.0105}, new ColorMatcher {X = 0.0042, Y = 0.0001, Z = 0.0201},
                new ColorMatcher {X = 0.0076, Y = 0.0002, Z = 0.0362}, new ColorMatcher {X = 0.0143, Y = 0.0004, Z = 0.0679}, new ColorMatcher {X = 0.0232, Y = 0.0006, Z = 0.1102},
                new ColorMatcher {X = 0.0435, Y = 0.0012, Z = 0.2074}, new ColorMatcher {X = 0.0776, Y = 0.0022, Z = 0.3713}, new ColorMatcher {X = 0.1344, Y = 0.0040, Z = 0.6456},
                new ColorMatcher {X = 0.2148, Y = 0.0073, Z = 1.0391}, new ColorMatcher {X = 0.2839, Y = 0.0116, Z = 1.3856}, new ColorMatcher {X = 0.3285, Y = 0.0168, Z = 1.6230},
                new ColorMatcher {X = 0.3483, Y = 0.0230, Z = 1.7471}, new ColorMatcher {X = 0.3481, Y = 0.0298, Z = 1.7826}, new ColorMatcher {X = 0.3362, Y = 0.0380, Z = 1.7721},
                new ColorMatcher {X = 0.3187, Y = 0.0480, Z = 1.7441}, new ColorMatcher {X = 0.2908, Y = 0.0600, Z = 1.6692}, new ColorMatcher {X = 0.2511, Y = 0.0739, Z = 1.5281},
                new ColorMatcher {X = 0.1954, Y = 0.0910, Z = 1.2876}, new ColorMatcher {X = 0.1421, Y = 0.1126, Z = 1.0419}, new ColorMatcher {X = 0.0956, Y = 0.1390, Z = 0.8130},
                new ColorMatcher {X = 0.0580, Y = 0.1693, Z = 0.6162}, new ColorMatcher {X = 0.0320, Y = 0.2080, Z = 0.4652}, new ColorMatcher {X = 0.0147, Y = 0.2586, Z = 0.3533},
                new ColorMatcher {X = 0.0049, Y = 0.3230, Z = 0.2720}, new ColorMatcher {X = 0.0024, Y = 0.4073, Z = 0.2123}, new ColorMatcher {X = 0.0093, Y = 0.5030, Z = 0.1582},
                new ColorMatcher {X = 0.0291, Y = 0.6082, Z = 0.1117}, new ColorMatcher {X = 0.0633, Y = 0.7100, Z = 0.0782}, new ColorMatcher {X = 0.1096, Y = 0.7932, Z = 0.0573},
                new ColorMatcher {X = 0.1655, Y = 0.8620, Z = 0.0422}, new ColorMatcher {X = 0.2257, Y = 0.9149, Z = 0.0298}, new ColorMatcher {X = 0.2904, Y = 0.9540, Z = 0.0203},
                new ColorMatcher {X = 0.3597, Y = 0.9803, Z = 0.0134}, new ColorMatcher {X = 0.4334, Y = 0.9950, Z = 0.0087}, new ColorMatcher {X = 0.5121, Y = 1.0000, Z = 0.0057},
                new ColorMatcher {X = 0.5945, Y = 0.9950, Z = 0.0039}, new ColorMatcher {X = 0.6784, Y = 0.9786, Z = 0.0027}, new ColorMatcher {X = 0.7621, Y = 0.9520, Z = 0.0021},
                new ColorMatcher {X = 0.8425, Y = 0.9154, Z = 0.0018}, new ColorMatcher {X = 0.9163, Y = 0.8700, Z = 0.0017}, new ColorMatcher {X = 0.9786, Y = 0.8163, Z = 0.0014},
                new ColorMatcher {X = 1.0263, Y = 0.7570, Z = 0.0011}, new ColorMatcher {X = 1.0567, Y = 0.6949, Z = 0.0010}, new ColorMatcher {X = 1.0622, Y = 0.6310, Z = 0.0008},
                new ColorMatcher {X = 1.0456, Y = 0.5668, Z = 0.0006}, new ColorMatcher {X = 1.0026, Y = 0.5030, Z = 0.0003}, new ColorMatcher {X = 0.9384, Y = 0.4412, Z = 0.0002},
                new ColorMatcher {X = 0.8544, Y = 0.3810, Z = 0.0002}, new ColorMatcher {X = 0.7514, Y = 0.3210, Z = 0.0001}, new ColorMatcher {X = 0.6424, Y = 0.2650, Z = 0.0000},
                new ColorMatcher {X = 0.5419, Y = 0.2170, Z = 0.0000}, new ColorMatcher {X = 0.4479, Y = 0.1750, Z = 0.0000}, new ColorMatcher {X = 0.3608, Y = 0.1382, Z = 0.0000},
                new ColorMatcher {X = 0.2835, Y = 0.1070, Z = 0.0000}, new ColorMatcher {X = 0.2187, Y = 0.0816, Z = 0.0000}, new ColorMatcher {X = 0.1649, Y = 0.0610, Z = 0.0000},
                new ColorMatcher {X = 0.1212, Y = 0.0446, Z = 0.0000}, new ColorMatcher {X = 0.0874, Y = 0.0320, Z = 0.0000}, new ColorMatcher {X = 0.0636, Y = 0.0232, Z = 0.0000},
                new ColorMatcher {X = 0.0468, Y = 0.0170, Z = 0.0000}, new ColorMatcher {X = 0.0329, Y = 0.0119, Z = 0.0000}, new ColorMatcher {X = 0.0227, Y = 0.0082, Z = 0.0000},
                new ColorMatcher {X = 0.0158, Y = 0.0057, Z = 0.0000}, new ColorMatcher {X = 0.0114, Y = 0.0041, Z = 0.0000}, new ColorMatcher {X = 0.0081, Y = 0.0029, Z = 0.0000},
                new ColorMatcher {X = 0.0058, Y = 0.0021, Z = 0.0000}, new ColorMatcher {X = 0.0041, Y = 0.0015, Z = 0.0000}, new ColorMatcher {X = 0.0029, Y = 0.0010, Z = 0.0000},
                new ColorMatcher {X = 0.0020, Y = 0.0007, Z = 0.0000}, new ColorMatcher {X = 0.0014, Y = 0.0005, Z = 0.0000}, new ColorMatcher {X = 0.0010, Y = 0.0004, Z = 0.0000},
                new ColorMatcher {X = 0.0007, Y = 0.0002, Z = 0.0000}, new ColorMatcher {X = 0.0005, Y = 0.0002, Z = 0.0000}, new ColorMatcher {X = 0.0003, Y = 0.0001, Z = 0.0000},
                new ColorMatcher {X = 0.0002, Y = 0.0001, Z = 0.0000}, new ColorMatcher {X = 0.0002, Y = 0.0001, Z = 0.0000}, new ColorMatcher {X = 0.0001, Y = 0.0000, Z = 0.0000},
                new ColorMatcher {X = 0.0001, Y = 0.0000, Z = 0.0000}, new ColorMatcher {X = 0.0001, Y = 0.0000, Z = 0.0000}, new ColorMatcher {X = 0.0000, Y = 0.0000, Z = 0.0000}
            };
        }

        public static List<ColorMatcher> ColorMatchers;
    }
}
