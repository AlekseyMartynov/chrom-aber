using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromAber {

    class PlanckLaw {
        // http://physics.nist.gov/cgi-bin/cuu/Value?c1l
        const double C1L = 1.191042953e-16;

        // http://physics.nist.gov/cgi-bin/cuu/Value?c22ndrc
        const double C2 = 0.0143877736;
                          
        double _temp;

        public PlanckLaw(double temp) {
            _temp = temp;
        }

        public double Calc(double len) {
            // convert to meters
            len = 1e-9 * len;

            return C1L / len / len / len / (Math.Exp(C2 / len / _temp) - 1) / len / len;
        }
    }

}
