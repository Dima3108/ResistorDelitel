using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResistorDelitel
{
    public class Resistors
    {
        public Resistor R1, R2;
        public Resistors(Resistor r1, Resistor r2)
        {
            R1 = r1;
            R2 = r2;
        }
        public Resistors(long rr1,long rr2)
        {
            R1 = new Resistor(rr1);
            R2 = new Resistor(rr2);
        }
    }
}
