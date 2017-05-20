using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RechnerTecknik
{
    class TIMER0
    {
        static int TimerValue;

        private static int timerCounter = 0;
        public static int TimerCounter
        {
            get { return timerCounter; }
            set { timerCounter = value; checkTimer0(); }
        }

        public static void checkTimer0()
        {
            byte InhaltOptionRegister = Registerspeicher.getRegisterWert(0x81);

            if ((InhaltOptionRegister & 0x08) == 0x08) //Watchdog: Prescaler is assigned to the WDT
            {
                if ((InhaltOptionRegister & 0x07) == 0x00)
                {
                    //WTD Rate = 1:2
                }
                else if ((InhaltOptionRegister & 0x07) == 0x01)
                {
                    //WTD Rate = 1:2
                }
                else if ((InhaltOptionRegister & 0x07) == 0x02)
                {
                    //WTD Rate = 1:4
                }
                else if ((InhaltOptionRegister & 0x07) == 0x03)
                {
                    //WTD Rate = 1:8
                }
                else if ((InhaltOptionRegister & 0x07) == 0x04)
                {
                    //WTD Rate = 1:16
                }
                else if ((InhaltOptionRegister & 0x07) == 0x05)
                {
                    //WTD Rate = 1:32
                }
                else if ((InhaltOptionRegister & 0x07) == 0x06)
                {
                    //WTD Rate = 1:64
                }
                else if ((InhaltOptionRegister & 0x07) == 0x07)
                {
                    //WTD Rate = 1:128
                }
                else
                {
                    MessageBox.Show("Something wrong with Watchdog");
                }
            }
            else if ((InhaltOptionRegister & 0x08) == 0x00) //Timer0: Prescaler is assigned to the Timer0 module
            {
                if ((InhaltOptionRegister & 0x07) == 0x00)
                {
                    //TMR0 Rate = 1:2
                    TimerValue = 2;
                    SetTimer0();
                }
                else if ((InhaltOptionRegister & 0x07) == 0x01)
                {
                    //TMR0 Rate = 1:4
                    TimerValue = 4;
                    SetTimer0();
                }
                else if ((InhaltOptionRegister & 0x07) == 0x02)
                {
                    //TMR0 Rate = 1:8
                    TimerValue = 8;
                    SetTimer0();
                }
                else if ((InhaltOptionRegister & 0x07) == 0x03)
                {
                    //TMR0 Rate = 1:16
                    TimerValue = 16;
                    SetTimer0();
                }
                else if ((InhaltOptionRegister & 0x07) == 0x04)
                {
                    //TMR0 Rate = 1:32
                    TimerValue = 32;
                }
                else if ((InhaltOptionRegister & 0x07) == 0x05)
                {
                    //TMR0 Rate = 1:64
                    TimerValue = 64;
                    SetTimer0();
                }
                else if ((InhaltOptionRegister & 0x07) == 0x06)
                {
                    //TMR0 Rate = 1:128
                    TimerValue = 128;
                    SetTimer0();
                }
                else if ((InhaltOptionRegister & 0x07) == 0x07)
                {
                    //TMR0 Rate = 1:256
                    TimerValue = 256;
                    SetTimer0();
                }
                else
                {
                    MessageBox.Show("Something wrong with Timer0");
                }
            }
            else
            {
                MessageBox.Show("Something wrong with PrescalerBit in Option Register");
            }
        }

        static void SetTimer0()
        {
            if (timerCounter == TimerValue)
            {
                byte tempTMRO = Registerspeicher.getRegisterWert(Registerspeicher.TMR0);
                if (MainWindow.numberOfCycles == 2)
                {
                    tempTMRO++;
                    Registerspeicher.setRegisterWert(Registerspeicher.TMR0, tempTMRO);
                }
                tempTMRO++;
                Registerspeicher.setRegisterWert(Registerspeicher.TMR0, tempTMRO);
                timerCounter = 0;
            }
        }

    }
}
