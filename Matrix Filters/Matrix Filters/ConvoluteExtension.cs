using System.Windows.Media.Imaging;

namespace Matrix_Filters {
    public unsafe static class WriteableBitmapConvoluteExtension {
        public static WriteableBitmap Convolute(this WriteableBitmap bitmap, Filter convolutionFilter) {
            const int kernelHeight = 3;
            const int kernelWidth = 3;

            using (var sourceBitmapContext = bitmap.GetBitmapContext(ReadWriteMode.ReadOnly)) {
                var w = sourceBitmapContext.Width;
                var h = sourceBitmapContext.Height;
                var resultBitmap = BitmapFactory.New(w, h);

                using (var resultContext = resultBitmap.GetBitmapContext()) {
                    var sourcePixels = sourceBitmapContext.Pixels;
                    var resultPixels = resultContext.Pixels;
                    var index = 0;
                    const int kwh = kernelWidth >> 1;
                    const int khh = kernelHeight >> 1;

                    for (var y = 0; y < h; y++) {
                        for (var x = 0; x < w; x++) {
                            var a = 0;
                            var r = 0;
                            var g = 0;
                            var b = 0;

                            for (var kx = -kwh; kx <= kwh; kx++) {
                                var px = kx + x;
                                if (px < 0) {
                                    px = 0;
                                } else if (px >= w) {
                                    px = w - 1;
                                }

                                for (var ky = -khh; ky <= khh; ky++) {
                                    var py = ky + y;
                                    if (py < 0) {
                                        py = 0;
                                    } else if (py >= h) {
                                        py = h - 1;
                                    }

                                    var col = sourcePixels[py * w + px];
                                    var k = convolutionFilter.FilterMatrix[ky + kwh][kx + khh];
                                    a += ((col >> 24) & 0x000000FF) * k;
                                    r += ((col >> 16) & 0x000000FF) * k;
                                    g += ((col >> 8) & 0x000000FF) * k;
                                    b += ((col) & 0x000000FF) * k;
                                }
                            }

                            var ta = ((a / convolutionFilter.Divisor) + convolutionFilter.Shift);
                            var tr = ((r / convolutionFilter.Divisor) + convolutionFilter.Shift);
                            var tg = ((g / convolutionFilter.Divisor) + convolutionFilter.Shift);
                            var tb = ((b / convolutionFilter.Divisor) + convolutionFilter.Shift);

                            var ba = (byte)((ta > 255) ? 255 : ((ta < 0) ? 0 : ta));
                            var br = (byte)((tr > 255) ? 255 : ((tr < 0) ? 0 : tr));
                            var bg = (byte)((tg > 255) ? 255 : ((tg < 0) ? 0 : tg));
                            var bb = (byte)((tb > 255) ? 255 : ((tb < 0) ? 0 : tb));

                            resultPixels[index++] = (ba << 24) | (br << 16) | (bg << 8) | (bb);
                        }
                    }
                    return resultBitmap;
                }
            }
        }
    }
}
