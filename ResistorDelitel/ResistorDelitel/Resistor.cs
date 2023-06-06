using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResistorDelitel
{
    public class Resistor
    {
        public long Resistance;
        /// <summary>
        /// Мощность рассеиваемая на резисторе
        /// </summary>
        public double P;
        public Resistor(long resistance)
        {
            Resistance = resistance;
        }
    }
}
