using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromAber {

    // Credits
    // http://stackoverflow.com/a/14917481
    // http://www.efg2.com/Lab/ScienceAndEngineering/Spectra.htm, Earl F. Glynn

    class RgbCalculator {
        
        public Rgb Calc(double len) {
            double factor;
            double r, g, b;

            if(len >= 380 && len < 440) {
                r = -(len - 440) / (440 - 380);
                g = 0;
                b = 1;
            } else if(len >= 440 && len < 490) {
                r = 0;
                g = (len - 440) / (490 - 440);
                b = 1;
            } else if(len >= 490 && len < 510) {
                r = 0;
                g = 1;
                b = -(len - 510) / (510 - 490);
            } else if(len >= 510 && len < 580) {
                r = (len - 510) / (580 - 510);
                g = 1;
                b = 0;
            } else if(len >= 580 && len < 645) {
                r = 1;
                g = -(len - 645) / (645 - 580);
                b = 0;
            } else if(len >= 645 && len < 781) {
                r = 1;
                g = 0;
                b = 0;
            } else {
                r = 0;
                g = 0;
                b = 0;
            }

            // Let the intensity fall off near the vision limits

            if(len >= 380 && len < 420) {
                factor = 0.3 + 0.7 * (len - 380) / (420 - 380);
            } else if(len >= 420 && len < 701) {
                factor = 1.0;
            } else if(len >= 701 && len < 781) {
                factor = 0.3 + 0.7 * (780 - len) / (780 - 700);
            } else {
                factor = 0;
            }

            return new Rgb(r * factor, g * factor, b * factor);
        }
    }

}
