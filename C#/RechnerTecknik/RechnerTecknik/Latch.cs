using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RechnerTecknik
{
    public class Latch
    {
        static MainWindow mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;

        static byte inhaltPortB;
        static byte inhaltTrisB;
        public static void LeseTrisBPortBInhalt()
        {
           inhaltPortB = Registerspeicher.getRegisterWert(Registerspeicher.PORTB); //Inhalt PortB wird vom Speicher geholt
           inhaltTrisB = Registerspeicher.getRegisterWert(0x86); //vom Speicher an Stelle 0x86 TRISB Register Wert wird geholt
        }

        public static void PruefTrisB()
        {
            Check7BitTrisB();
            Check6BitTrisB();
            Check5BitTrisB();
            Check4BitTrisB();
            Check3BitTrisB();
            Check2BitTrisB();
            Check1BitTrisB();
            Check0BitTrisB();  
        }
        
        public static void Check7BitTrisB()
        {
            LeseTrisBPortBInhalt();
            if ((byte)(inhaltTrisB & 0x80) == 0x80) // 7.Bit steht 1 => INPUT
            {
                if (mainWin.RB7.IsChecked == true) //wenn RB7 Checkbox einen Hacken hat schreibe 1 an 7.Bit in PortB
                {
                    Registerspeicher.speicher[Registerspeicher.PORTB] |= 0x80;
                    Registerspeicher.labels[Registerspeicher.PORTB].Content = Registerspeicher.speicher[Registerspeicher.PORTB].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters

                }
                if (mainWin.RB7.IsChecked == false) //wenn RB7 Checkbox keinen Hacken hat, schreibe 0 an 7.Bit in PortB
                {
                    Registerspeicher.speicher[Registerspeicher.PORTB] &= 0x7F;
                    Registerspeicher.labels[Registerspeicher.PORTB].Content = Registerspeicher.speicher[Registerspeicher.PORTB].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                }
            }
            if ((byte)(inhaltTrisB & 0x80) == 0x00) // 7.Bit steht 0 => OUTPUT
            {
                if ((byte)(inhaltPortB & 0x80) == 0x80) //wenn 7.Bit in PortB 1 ist, wird Checkbox RB7 gesetzt
                {
                    mainWin.RB7.IsChecked = true;
                }
                if ((byte)(inhaltPortB & 0x80) == 0x00) //wenn 7.Bit in PortB 0 ist, wird Checkbox RB7 auf 0 gesetzt
                {
                    mainWin.RB7.IsChecked = false;
                }
            }
        }

        public static void Check6BitTrisB()
        {
            LeseTrisBPortBInhalt();
            if ((byte)(inhaltTrisB & 0x40) == 0x40) // 6.Bit steht 1 => INPUT
            {
                if (mainWin.RB6.IsChecked == true)
                {
                    Registerspeicher.speicher[Registerspeicher.PORTB] |= 0x40;
                    Registerspeicher.labels[Registerspeicher.PORTB].Content = Registerspeicher.speicher[Registerspeicher.PORTB].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                }
                if (mainWin.RB6.IsChecked == false)
                {
                    Registerspeicher.speicher[Registerspeicher.PORTB] &= 0xBF;
                    Registerspeicher.labels[Registerspeicher.PORTB].Content = Registerspeicher.speicher[Registerspeicher.PORTB].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                }
            }
            if ((byte)(inhaltTrisB & 0x40) == 0x00) // 6.Bit steht 0 => OUTPUT
            {
                if ((byte)(inhaltPortB & 0x40) == 0x40) //RB6
                {
                    mainWin.RB6.IsChecked = true;
                }
                if ((byte)(inhaltPortB & 0x40) == 0x00) //RB6
                {
                    mainWin.RB6.IsChecked = false;
                }
            }
        }

        public static void Check5BitTrisB()
        {
            LeseTrisBPortBInhalt();
            if ((byte)(inhaltTrisB & 0x20) == 0x20) // 5.Bit steht 1 => INPUT
            {
                if (mainWin.RB5.IsChecked == true)
                {
                    Registerspeicher.speicher[Registerspeicher.PORTB] |= 0x20;
                    Registerspeicher.labels[Registerspeicher.PORTB].Content = Registerspeicher.speicher[Registerspeicher.PORTB].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                }
                if (mainWin.RB5.IsChecked == false)
                {
                    Registerspeicher.speicher[Registerspeicher.PORTB] &= 0xDF;
                    Registerspeicher.labels[Registerspeicher.PORTB].Content = Registerspeicher.speicher[Registerspeicher.PORTB].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                }
            }
            if ((byte)(inhaltTrisB & 0x20) == 0x00) // 5.Bit steht 0 => OUTPUT
            {
                if ((byte)(inhaltPortB & 0x20) == 0x20) //RB5
                {
                    mainWin.RB5.IsChecked = true;
                }
                if ((byte)(inhaltPortB & 0x20) == 0x00) //RB5
                {
                    mainWin.RB5.IsChecked = true;
                }
            }
        }

        public static void Check4BitTrisB()
        {
            LeseTrisBPortBInhalt();
            if ((byte)(inhaltTrisB & 0x10) == 0x10) // 4.Bit steht 1 => INPUT
            {
                if (mainWin.RB4.IsChecked == true)
                {
                    Registerspeicher.speicher[Registerspeicher.PORTB] |= 0x10;
                    Registerspeicher.labels[Registerspeicher.PORTB].Content = Registerspeicher.speicher[Registerspeicher.PORTB].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                }
                if (mainWin.RB4.IsChecked == false)
                {
                    Registerspeicher.speicher[Registerspeicher.PORTB] &= 0xEF;
                    Registerspeicher.labels[Registerspeicher.PORTB].Content = Registerspeicher.speicher[Registerspeicher.PORTB].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                }
            }
            if ((byte)(inhaltTrisB & 0x10) == 0x00) // 4.Bit steht 0 => OUTPUT
            {
                if ((byte)(inhaltPortB & 0x10) == 0x10) //RB4
                {
                    mainWin.RB4.IsChecked = true;
                }
                if ((byte)(inhaltPortB & 0x10) == 0x00) //RB4
                {
                    mainWin.RB4.IsChecked = false;
                }
            }
        }

        public static void Check3BitTrisB()
        {
            LeseTrisBPortBInhalt();
            if ((byte)(inhaltTrisB & 0x08) == 0x08) // 3.Bit steht 1 => INPUT
            {
                if (mainWin.RB3.IsChecked == true)
                {
                    Registerspeicher.speicher[Registerspeicher.PORTB] |= 0x08;
                    Registerspeicher.labels[Registerspeicher.PORTB].Content = Registerspeicher.speicher[Registerspeicher.PORTB].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                }
                if (mainWin.RB3.IsChecked == false)
                {
                    Registerspeicher.speicher[Registerspeicher.PORTB] &= 0xF7;
                    Registerspeicher.labels[Registerspeicher.PORTB].Content = Registerspeicher.speicher[Registerspeicher.PORTB].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                }
            }
            if ((byte)(inhaltTrisB & 0x08) == 0x00) // 3.Bit steht 0 => OUTPUT
            {
                if ((byte)(inhaltPortB & 0x08) == 0x08) //RB3
                {
                    mainWin.RB3.IsChecked = true;
                }
                if ((byte)(inhaltPortB & 0x08) == 0x00) //RB3
                {
                    mainWin.RB3.IsChecked = false;
                }
            }
        }

        public static void Check2BitTrisB()
        {
            LeseTrisBPortBInhalt();
            if ((byte)(inhaltTrisB & 0x04) == 0x04) // 2.Bit steht 1 => INPUT
            {
                if (mainWin.RB2.IsChecked == true)
                {
                    Registerspeicher.speicher[Registerspeicher.PORTB] |= 0x04;
                    Registerspeicher.labels[Registerspeicher.PORTB].Content = Registerspeicher.speicher[Registerspeicher.PORTB].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                }
                if (mainWin.RB2.IsChecked == false)
                {
                    Registerspeicher.speicher[Registerspeicher.PORTB] &= 0xFB;
                    Registerspeicher.labels[Registerspeicher.PORTB].Content = Registerspeicher.speicher[Registerspeicher.PORTB].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                }
            }
            if ((byte)(inhaltTrisB & 0x04) == 0x00) // 2.Bit steht 0 => OUTPUT
            {
                if ((byte)(inhaltPortB & 0x04) == 0x04) //RB2
                {
                    mainWin.RB2.IsChecked = true;
                }
                if ((byte)(inhaltPortB & 0x04) == 0x00) //RB2
                {
                    mainWin.RB2.IsChecked = false;
                }
            }
        }

        public static void Check1BitTrisB()
        {
            LeseTrisBPortBInhalt();
            if ((byte)(inhaltTrisB & 0x02) == 0x02) // 1.Bit steht 1 => INPUT
            {
                if (mainWin.RB1.IsChecked == true)
                {
                    Registerspeicher.speicher[Registerspeicher.PORTB] |= 0x02;
                    Registerspeicher.labels[Registerspeicher.PORTB].Content = Registerspeicher.speicher[Registerspeicher.PORTB].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                }
                if (mainWin.RB1.IsChecked == false)
                {
                    Registerspeicher.speicher[Registerspeicher.PORTB] &= 0xFD;
                    Registerspeicher.labels[Registerspeicher.PORTB].Content = Registerspeicher.speicher[Registerspeicher.PORTB].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                }
            }
            if ((byte)(inhaltTrisB & 0x02) == 0x00) // 1.Bit steht 0 => OUTPUT
            {
                if ((byte)(inhaltPortB & 0x02) == 0x02) //RB1
                {
                    mainWin.RB1.IsChecked = true;
                }
                if ((byte)(inhaltPortB & 0x02) == 0x00) //RB1
                {
                    mainWin.RB1.IsChecked = false;
                }
            }
        }

        public static void Check0BitTrisB()
        {
            LeseTrisBPortBInhalt();
            if ((byte)(inhaltTrisB & 0x01) == 0x01) // 0.Bit steht 1 => INPUT
            {
                if (mainWin.RB0.IsChecked == true)
                {
                    Registerspeicher.speicher[Registerspeicher.PORTB] |= 0x01;
                    Registerspeicher.labels[Registerspeicher.PORTB].Content = Registerspeicher.speicher[Registerspeicher.PORTB].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                }
                if (mainWin.RB0.IsChecked == false)
                {
                    Registerspeicher.speicher[Registerspeicher.PORTB] &= 0xFE;
                    Registerspeicher.labels[Registerspeicher.PORTB].Content = Registerspeicher.speicher[Registerspeicher.PORTB].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                }
            }
            if ((byte)(inhaltTrisB & 0x01) == 0x00) // 0.Bit steht 0 => OUTPUT
            {
                if ((byte)(inhaltPortB & 0x01) == 0x01) //RB0
                {
                    mainWin.RB0.IsChecked = true;
                }
                if ((byte)(inhaltPortB & 0x01) == 0x00) //RB0
                {
                    mainWin.RB0.IsChecked = false;
                }
            }
        }
    }
}
