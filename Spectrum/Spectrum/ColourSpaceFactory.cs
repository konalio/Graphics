namespace Spectrum {
    public static class ColourSpaceFactory {
        public static ColourSpace CreateColourSpace(ColourSpaceType spaceType) {
            switch (spaceType) {
                case ColourSpaceType.sRGB:
                    return new Srgb();
                case ColourSpaceType.AdobeRGB:
                    return new AdobeRgb();
                case ColourSpaceType.AppleRGB:
                    return new AppleRgb();
                case ColourSpaceType.WideGamutRGB:
                    return new WideGamutRgb();
                case ColourSpaceType.PalSecamRGB:
                    return new PalSecamRgb();
                default:
                    return new Srgb();
            }

        }
    }
}
