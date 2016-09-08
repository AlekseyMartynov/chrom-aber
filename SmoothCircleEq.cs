using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromAber {

    class SmoothCircleEq {
        static readonly double[,] FILTER = new[,] {
            { 1.0/16, 2.0/16, 1.0/16  },
            { 2.0/16, 4.0/16, 2.0/16 },
            { 1.0/16, 2.0/16, 1.0/16 }
        };

        double
            _centerX,
            _centerY,
            _radius;

        public SmoothCircleEq(double centerX, double centerY, double radius) {
            _centerX = centerX;
            _centerY = centerY;
            _radius = radius;
        }

        public double CalcBelongingScore(double x, double y) {
            var result = 0.0;

            for(var dx = -1; dx <= 1; dx++) {
                for(var dy = -1; dy <= 1; dy++) {
                    var diffX = x + dx - _centerX;
                    var diffY = y + dy - _centerY;

                    var dist = Math.Sqrt(diffX * diffX + diffY * diffY);

                    if(dist <= _radius)
                        result += Math.Min(_radius - dist, 1) * FILTER[1 + dx, 1 + dy];                    
                }
            }

            if(result < 0)
                result = 0;

            return result;
        }
    }

}
