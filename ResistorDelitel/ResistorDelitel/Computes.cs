using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResistorDelitel
{
    public class Computes
    {
        /// <summary>
        /// Расчет фактического выходного напряжения
        /// </summary>
        /// <param name="inpute_voltage">входное напряжение</param>
        /// <param name="resistors">параметры резисторов</param>
        /// <returns>фактическое выходное напряжение</returns>
        public static double ComputeOutputeVoltage(double inpute_voltage, Resistors resistors)
        {
            double coef = (double)((double)resistors.R2.Resistance / (double)(resistors.R1.Resistance + resistors.R2.Resistance));
            double fact_output = coef * inpute_voltage;
            return fact_output;
        }
        public static double ComputeOutputeVoltageDelta(Voltages voltages,Resistors resistors)
        {
            /* double coef = (resistors.R2.Resistance / (resistors.R1.Resistance + resistors.R2.Resistance));
             double fact_output = coef * voltages.InputeVoltage;*/
            double fact_output = ComputeOutputeVoltage(voltages.InputeVoltage, resistors);
            return fact_output-voltages.OutputeVoltage;
        }
        /// <summary>
        /// Сравнение эталонного и полученного результата
        /// </summary>
        /// <param name="outpute_volt">эталонное значение</param>
        /// <param name="fact_voltage">фактическое значение</param>
        /// <returns>резница между эталонным и фактическим значением (
        /// <c>стремится к нулю при стремление  fact_voltage к output_voltage</c>)</returns>
        public static double ApproximationEstimate(double outpute_volt,double fact_voltage)
        {
            double c = outpute_volt / fact_voltage;
            return c - 1.0;
        }
        /// <summary>
        /// Вычисление сопротивления второго резистора исходя из значения первого
        /// </summary>
        /// <param name="R1">значение сопротивления первого резистора</param>
        /// <param name="voltages">пара входного и выходного <c>(эталонного) значений напряжений</c></param>
        /// <param name="w">коэффициент делителя <c>Не используется в данный момент</c></param>
        /// <param name="init_R2">предпологаемое значение сопротивления</param>
        /// <returns>значение сопротивления резистора R2</returns>
        public static long ComputeR2(long R1,Voltages voltages,double w,long init_R2 = 1)
        {
            /*var r1 = new Resistor(R1);
            double init_coef = ComputeOutputeVoltageDelta(voltages, new Resistors(r1, new Resistor(init_R2)));
            double pred_coef, next_coef;
            if (init_R2 >= 2)
            {
                pred_coef = ComputeOutputeVoltageDelta(voltages, new Resistors(r1, new Resistor(init_R2 - 1)));
            }
            else
            {
                pred_coef = double.MaxValue - 0.88;
            }
            if (init_R2 <= long.MaxValue - 1)
            {
                next_coef = ComputeOutputeVoltageDelta(voltages, new Resistors(r1, new Resistor(init_R2 + 1)));
            }
            else
            {
                next_coef = double.MaxValue - 0.88;
            }
            double coef=init_coef;
            long R=init_R2;
            long pred_R = R - 1;
            long next_R = R + 1;
            if (pred_coef <= coef)
            {
                while (pred_coef <= coef&&pred_R>0)
                {
                    R = pred_R;
                    pred_R -=1;
                    coef=pred_coef;
                    pred_coef = ComputeOutputeVoltageDelta(voltages, new Resistors(r1, new Resistor(pred_R)));
                }
            }
            if(next_coef <= coef)
            {
                while(next_coef <= coef&&next_R<=1000000)
                {
                    R = next_R;
                    next_R++;
                    coef = next_coef;
                    next_coef = ComputeOutputeVoltageDelta(voltages, new Resistors(r1, new Resistor(next_R)));
                }
            }
            return R;*/
            //double init_r2 = ((double)R1 * w) / (1 - w);
            long R2 = (long)init_R2;
            long R2_cop = R2;
            long R2_1 = R2;
            //double delta_init = ComputeOutputeVoltageDelta(voltages, new Resistors(R1, R2_1));
            double approxsim = ApproximationEstimate(voltages.OutputeVoltage, ComputeOutputeVoltage(voltages.InputeVoltage, new Resistors(R1, R2)));
            while(--R2>0&&ApproximationEstimate(voltages.OutputeVoltage,ComputeOutputeVoltage(voltages.InputeVoltage,new Resistors(R1,R2)))<=approxsim)
            {
                R2_1 = R2;
                //delta_init = ComputeOutputeVoltageDelta(voltages, new Resistors(R1, R2));
                approxsim = ApproximationEstimate(voltages.OutputeVoltage, ComputeOutputeVoltage(voltages.InputeVoltage, new Resistors(R1, R2)));
            }
            R2 = R2_cop;
            /*while(++R2<10&& ApproximationEstimate(voltages.OutputeVoltage, ComputeOutputeVoltage(voltages.InputeVoltage, new Resistors(R1, R2))) < approxsim)
            {
                R2_1 = R2;
                //delta_init = ComputeOutputeVoltageDelta(voltages, new Resistors(R1, R2));
                approxsim = ApproximationEstimate(voltages.OutputeVoltage, ComputeOutputeVoltage(voltages.InputeVoltage, new Resistors(R1, R2)));

            }*/
            return R2_1;
        }
        /// <summary>
        /// Вычисление сопротивлений резисторов
        /// </summary>
        /// <param name="voltages">пара входного и выходного <c>(эталонного) значений напряжений</c></param>
        /// <param name="min_resistance">минимальное значение сопротивления резистора</param>
        /// <param name="max_resistance">максимальное значение сопротивления резистора</param>
        /// <returns>Параметры выходных резисторов</returns>
        public static Resistors ComputeR1R2(Voltages voltages,long min_resistance=1,long max_resistance=1000000)
        {
            double w = voltages.OutputeVoltage / voltages.InputeVoltage;//коэффициент делителя
            long min_r = min_resistance;
            long r2, r2_cur;
            double delta, delta_cur;
            long r1, r1_cur;
            r1 = min_resistance;
            double start_r2 = ((r1 * w) / (1 - w));
            while ((long)start_r2 <= 0)
            {
                min_r++;
                r1 = min_r;
                start_r2 = ((r1 * w) / (1 - w));
            }
            r2 = ComputeR2(r1, voltages,w,(long)start_r2 );

            delta = ApproximationEstimate(voltages.OutputeVoltage, ComputeOutputeVoltage(voltages.InputeVoltage, new Resistors(r1, r2)));
                //ComputeOutputeVoltageDelta(voltages, new Resistors(new Resistor(r1), new Resistor(r2)));
            for(long resistance_r1 = min_r; resistance_r1 <= max_resistance; resistance_r1++)
            {
                r1_cur = resistance_r1;
                start_r2 = ((r1_cur * w) / (1 - w));
                r2_cur =//(long)start_r2 
                    ComputeR2(r1_cur, voltages, (long)start_r2)
                                       ;
                //delta_cur = ComputeOutputeVoltageDelta(voltages, new Resistors(r1_cur, r2_cur));
                delta_cur = ApproximationEstimate(voltages.OutputeVoltage, ComputeOutputeVoltage(voltages.InputeVoltage, new Resistors(r1_cur, r2_cur)));
                if (delta_cur <= delta)
                {
                    delta = delta_cur;
                    r1 = r1_cur;
                    r2 = r2_cur;
                }
            }
            return new Resistors(r1, r2);
        }
    }
}
