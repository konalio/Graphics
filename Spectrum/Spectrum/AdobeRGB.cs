namespace Spectrum {
    class AdobeRgb : ColourSpace {
        public AdobeRgb()
        {
            ConversionMatrix = new Matrix3X3(2.041369, -0.5649464, -0.3446944, -0.9692660, 1.8760108, 0.041556, 0.0134474, -0.1183897, 1.0154096);
        }
    }
}
