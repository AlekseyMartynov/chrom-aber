using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromAber {

    class Program {
        const int IMAGE_W = 1920;
        const int IMAGE_H = 1080;

        static void Main(string[] args) {
            var colors = InitColors();
            var eq = new SmoothCircleEq(IMAGE_W / 2, IMAGE_H / 2, 1.25 * (IMAGE_H / 2));

            double[,] 
                matrixR = null, 
                matrixG = null, 
                matrixB = null;

            Task.WaitAll(
                Task.Run(delegate {
                    matrixR = FillMatrix(colors, rgb => rgb.R, eq);
                }),
                Task.Run(delegate {
                    matrixG = FillMatrix(colors, rgb => rgb.G, eq);
                }),
                Task.Run(delegate {
                    matrixB = FillMatrix(colors, rgb => rgb.B, eq);
                })
            );

            var compressionFactor = 255 / matrixR.Cast<double>()
                .Concat(matrixG.Cast<double>())
                .Concat(matrixB.Cast<double>())
                .Max();

            var bitmap = new Bitmap(IMAGE_W, IMAGE_H);
            for(var x = 0; x < IMAGE_W; x++) {
                for(var y = 0; y < IMAGE_H; y++) {
                    var color = Color.FromArgb(
                        (int)(compressionFactor * matrixR[x, y]),
                        (int)(compressionFactor * matrixG[x, y]),
                        (int)(compressionFactor * matrixB[x, y])
                    );
                    bitmap.SetPixel(x, y, color);
                }
            }

            bitmap.Save("out.bmp", ImageFormat.Bmp);
        }

        static Rgb[] InitColors() {
            var rgbCalculator = new RgbCalculator();
            var planckLaw = new PlanckLaw(7000);

            return Enumerable.Range(380, 310)
                .Select(len => {
                    var radiation = planckLaw.Calc(len);
                    var rgb = rgbCalculator.Calc(len);
                    rgb.R *= radiation;
                    rgb.G *= radiation;
                    rgb.B *= radiation;
                    return rgb;
                })
                .ToArray();
        }

        static double[,] FillMatrix(Rgb[] colors, Func<Rgb, double> componentSelector, SmoothCircleEq eq) {
            var matrix = new double[IMAGE_W, IMAGE_H];

            for(var x = 0; x < IMAGE_W; x++) {
                for(var y = 0; y < IMAGE_H; y++) {
                    var score = eq.CalcBelongingScore(x, y);
                    if(score == 0)
                        continue;

                    for(var i = 0; i < colors.Length; i++) {
                        var aberration = (i - colors.Length / 2) / 2;
                        var defocusedX = x - aberration;
                        
                        if(defocusedX > 0 && defocusedX < IMAGE_W)
                            matrix[defocusedX, y] += score * componentSelector(colors[i]);
                    }

                }
            }

            return matrix;
        }

    }
}
