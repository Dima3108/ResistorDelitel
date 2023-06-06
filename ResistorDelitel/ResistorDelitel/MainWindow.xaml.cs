using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ResistorDelitel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ComputeR_Click(object sender, RoutedEventArgs e)
        {

            Voltages voltages = new Voltages(inp_volt, out_volt);
            var r1r2 = Computes.ComputeR1R2(voltages);
            Output_ResistanceR1.Text = r1r2.R1.Resistance.ToString();
            Output_ResistanceR2.Text = r1r2.R2.Resistance.ToString();
            OutputVoltage_fact.Text = Computes.ComputeOutputeVoltage(voltages.InputeVoltage, r1r2).ToString();
        }
        private double inp_volt, out_volt;

        private void OutputVoltage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(Double.TryParse(OutputVoltage.Text,out double v))
            {
                out_volt = v;
            }
            //out_volt=double.Parse(OutputVoltage.Text);
        }

        private void InputVoltage_TextChanged(object sender, TextChangedEventArgs e)
        {
           //inp_volt=double.Parse(InputVoltage.Text);
           if(Double.TryParse(InputVoltage.Text,out double v))
            {
                inp_volt = v;
            }
        }
    }
}
