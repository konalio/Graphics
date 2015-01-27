namespace Spectrum {
    public enum ColourSpaceType
    {
        sRGB,
        AdobeRGB,
        AppleRGB,
        WideGamutRGB,
        PalSecamRGB
    }

    public abstract class ColourSpace
    {
        public Matrix3X3 ConversionMatrix;
    }
}
