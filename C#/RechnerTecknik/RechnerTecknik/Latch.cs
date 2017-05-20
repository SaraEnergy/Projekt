using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RechnerTecknik
{
    class Latch
    {
        static MainWindow mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;

        static byte inhaltPortB = Registerspeicher.getRegisterWert(Registerspeicher.PORTB);

        public void PortBOutput()
        {
            if ((byte)(inhaltPortB & 0x80) == 0x80) //RB7
            {
                mainWin.RB7.IsChecked = true;
            }
            else if ((byte)(inhaltPortB & 0x40) == 0x40) //RB6
            {
                mainWin.RB6.IsChecked = true;
            }
            else if ((byte)(inhaltPortB & 0x20) == 0x20) //RB5
            {
                mainWin.RB5.IsChecked = true;
            }
            else if ((byte)(inhaltPortB & 0x10) == 0x10) //RB4
            {
                mainWin.RB4.IsChecked = true;
            }
            else if ((byte)(inhaltPortB & 0x08) == 0x08) //RB3
            {
                mainWin.RB3.IsChecked = true;
            }
            else if ((byte)(inhaltPortB & 0x04) == 0x04) //RB2
            {
                mainWin.RB2.IsChecked = true;
            }
            else if ((byte)(inhaltPortB & 0x02) == 0x02) //RB1
            {
                mainWin.RB1.IsChecked = true;
            }
            else if ((byte)(inhaltPortB & 0x01) == 0x01) //RB0
            {
                mainWin.RB0.IsChecked = true;
            }
        }

        public void PortBInput()
        { 
            if (mainWin.RB7.IsChecked == true)
            {
                Registerspeicher.speicher[Registerspeicher.PORTB] |= 0x80;   // PD wird auf 0 gesetzt;
                Registerspeicher.labels[Registerspeicher.PORTB].Content = Registerspeicher.speicher[Registerspeicher.PORTB].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
            }
            else if (mainWin.RB6.IsChecked == true)
            {
                Registerspeicher.speicher[Registerspeicher.PORTB] |= 0x40;   // PD wird auf 0 gesetzt;
                Registerspeicher.labels[Registerspeicher.PORTB].Content = Registerspeicher.speicher[Registerspeicher.PORTB].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
            }
            else if (mainWin.RB5.IsChecked == true)
            {
                Registerspeicher.speicher[Registerspeicher.PORTB] |= 0x20;   // PD wird auf 0 gesetzt;
                Registerspeicher.labels[Registerspeicher.PORTB].Content = Registerspeicher.speicher[Registerspeicher.PORTB].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
            }
            else if (mainWin.RB4.IsChecked == true)
            {
                Registerspeicher.speicher[Registerspeicher.PORTB] |= 0x10;   // PD wird auf 0 gesetzt;
                Registerspeicher.labels[Registerspeicher.PORTB].Content = Registerspeicher.speicher[Registerspeicher.PORTB].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
            }
            else if (mainWin.RB3.IsChecked == true)
            {
                Registerspeicher.speicher[Registerspeicher.PORTB] |= 0x08;   // PD wird auf 0 gesetzt;
                Registerspeicher.labels[Registerspeicher.PORTB].Content = Registerspeicher.speicher[Registerspeicher.PORTB].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
            }
            else if (mainWin.RB2.IsChecked == true)
            {
                Registerspeicher.speicher[Registerspeicher.PORTB] |= 0x04;   // PD wird auf 0 gesetzt;
                Registerspeicher.labels[Registerspeicher.PORTB].Content = Registerspeicher.speicher[Registerspeicher.PORTB].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
            }
            else if (mainWin.RB1.IsChecked == true)
            {
                Registerspeicher.speicher[Registerspeicher.PORTB] |= 0x02;   // PD wird auf 0 gesetzt;
                Registerspeicher.labels[Registerspeicher.PORTB].Content = Registerspeicher.speicher[Registerspeicher.PORTB].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
            }
            else if (mainWin.RB0.IsChecked == true)
            {
                Registerspeicher.speicher[Registerspeicher.PORTB] |= 0x01;   // PD wird auf 0 gesetzt;
                Registerspeicher.labels[Registerspeicher.PORTB].Content = Registerspeicher.speicher[Registerspeicher.PORTB].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
            }

        }
    }
}
