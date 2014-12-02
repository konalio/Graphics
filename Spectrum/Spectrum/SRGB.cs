namespace Spectrum {
    public class Srgb : ColourSpace {
        public Srgb()
        {
            ConversionMatrix = new Matrix3X3(
                3.2404542, -1.5371385, -0.4985314,
                -0.969266, 1.8760108, 0.041556,
                0.0556434, -0.2040259, 1.0572252
            );
        }
    }
}
