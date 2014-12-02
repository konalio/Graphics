﻿namespace Spectrum {
    class AppleRgb : ColourSpace {
        public AppleRgb()
        {
            ConversionMatrix = new Matrix3X3(
                2.9515373, -1.2894116, -0.4738445, 
                -1.0851093, 1.9908566, 0.0372026, 
                0.0854934, -0.2694964, 1.0912975
                );
        }
    }
}
