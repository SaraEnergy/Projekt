using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RechnerTecknik
{
    class Interrupt
    {
        //damit Labels vom MainWindow benützt werden können
        static MainWindow mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;

        public static void CallTimerInterrupt()
        {
            if ((Registerspeicher.getRegisterWert(Registerspeicher.INTCON) & 0xA4) == 0xA4)
            {
                Stack.myStack.Push(MainWindow.CommandCounter); // und in Stack für Rücksprungadresse gespeichert
                Commands.stackAsString = MainWindow.CommandCounter.ToString("X2") + Commands.stackAsString;
                mainWin.stackBox.Text = Commands.stackAsString;

                MainWindow.CommandCounter = 4;
                mainWin.InterruptLabel.Content = "Timer Interrupt";
            }
        }

        internal static void CallRB0Interrupt()
        {
            if ((Registerspeicher.getRegisterWert(Registerspeicher.INTCON) & 0x92) == 0x92)
            {
                Stack.myStack.Push(MainWindow.CommandCounter); // und in Stack für Rücksprungadresse gespeichert
                Commands.stackAsString = MainWindow.CommandCounter.ToString("X2") + Commands.stackAsString;
                mainWin.stackBox.Text = Commands.stackAsString;

                MainWindow.CommandCounter = 4;
                mainWin.InterruptLabel.Content = "RB0 Interrupt";
            }
        }

        internal static void CallRB47Interrupt()
        {
            if ((Registerspeicher.getRegisterWert(Registerspeicher.INTCON) & 0x89) == 0x89)
            {
                Stack.myStack.Push(MainWindow.CommandCounter); // und in Stack für Rücksprungadresse gespeichert
                Commands.stackAsString = MainWindow.CommandCounter.ToString("X2") + Commands.stackAsString;
                mainWin.stackBox.Text = Commands.stackAsString;

                MainWindow.CommandCounter = 4;
                mainWin.InterruptLabel.Content = "RB4-RB7 Interrupt";
            }
        }
    }
}
