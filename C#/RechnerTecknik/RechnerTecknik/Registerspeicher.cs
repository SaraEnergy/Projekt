using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RechnerTecknik
{
    public class Registerspeicher
    {
        //damit Labels vom MainWindow benützt werden können
        static MainWindow mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;

        public static byte[] speicher = new byte[256];
        public static Label[] labels = new Label[256];
        public const byte indirect = 0x00;
        public const byte TMR0 = 0x01;
        public const byte PCL = 0x02;
        public const byte STATUS = 0x03;
        public const byte FSR = 0x04;
        public const byte PORTA = 0x05;
        public const byte PORTB = 0x06;
        public const byte EEDATA = 0x08;
        public const byte EEADR = 0x09;
        public const byte PCLATH = 0x0A;
        public const byte INTCON = 0x0B;

        public const byte C = 0;

        private static byte w;
        public static byte W
        {
            get {return w;}
            set {w = value; mainWin.WorkingRegisterLabel.Content = w.ToString("X2"); }
        }

        public static void initializeRegister()
        {
            for (int i = 0; i < 256; i++)
            {
                speicher[i] = 0x00;  // Überall wird null hingeschrieben;
                labels[i].Content = speicher[i].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
            }
            setRegisterWert(STATUS, 0x18);
            setRegisterWert(0x81, 0xFF); //Option_REG
            setRegisterWert(0x85, 0xFF); //TrisA
            setRegisterWert(0x86, 0xFF); //TrisB
        }

        public static void SetZeroFlag(byte workingRegister)
        {
            if (workingRegister == 0)
                {
                    speicher[STATUS] |= 0x04;  // Z-flag wird auf 1 gesetzt;
                    labels[STATUS].Content = speicher[STATUS].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                    mainWin.ZLabel.Content = "1";
                }
            else
                {
                    speicher[STATUS] &= 0xFB;   // Z-flag wird auf 0 gesetzt;
                    labels[STATUS].Content = speicher[STATUS].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                    mainWin.ZLabel.Content = "0";
                }
        }

        public static void SetCarryFlag(ushort tempWRegister, bool invert = false)
        {
            if (tempWRegister > 0xFF)
            {
                if (invert)
                {
                    speicher[STATUS] &= 0xFE;   // C-flag wird auf 0 gesetzt;
                    labels[STATUS].Content = speicher[STATUS].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                    mainWin.CLabel.Content = "0";
                }
                else
                {
                    speicher[STATUS] |= 0x01;  // C-flag wird auf 1 gesetzt;
                    labels[STATUS].Content = speicher[STATUS].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                    mainWin.CLabel.Content = "1";
                }
            }
            else
            {
                if (invert)
                {
                    speicher[STATUS] |= 0x01;  // C-flag wird auf 1 gesetzt;
                    labels[STATUS].Content = speicher[STATUS].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                    mainWin.CLabel.Content = "1";
                }
                else
                {
                    speicher[STATUS] &= 0xFE;   // C-flag wird auf 0 gesetzt;
                    labels[STATUS].Content = speicher[STATUS].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                    mainWin.CLabel.Content = "0";
                }
            }
        }

        public static void SetDigitCarryFlag(byte resultOfJustLow, bool invert = false)
        {
            if (resultOfJustLow > 0x0F)
            {
                if (invert)
                {
                    speicher[STATUS] &= 0xFD;   // DC-flag wird auf 0 gesetzt (& 1111 1101)
                    labels[STATUS].Content = speicher[STATUS].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                    mainWin.DCLabel.Content = "0";
                }
                else
                {
                    speicher[STATUS] |= 0x02;  // DC-flag wird auf 1 gesetzt (| 0000 0010)
                    labels[STATUS].Content = speicher[STATUS].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                    mainWin.DCLabel.Content = "1";
                }
            }
            else
            {
                if (invert)
                {
                    speicher[STATUS] |= 0x02;  // DC-flag wird auf 1 gesetzt (| 0000 0010)
                    labels[STATUS].Content = speicher[STATUS].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                    mainWin.DCLabel.Content = "1";
                }
                else
                {
                    speicher[STATUS] &= 0xFD;   // DC-flag wird auf 0 gesetzt (& 1111 1101)
                    labels[STATUS].Content = speicher[STATUS].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                    mainWin.DCLabel.Content = "0";
                }
            }
        }

        public static void setRegisterWert(byte address, byte value)
        {
            byte altOptionRegister = getRegisterWert(0x81); //OptionRegister wird wegen INTEDG ausgelesen
            byte altPortBRegister = getRegisterWert(PORTB); //PortBRegister wird wegen interrupt ausgelesen
            WriteToBank(address, value);
            CopyBanks();
            CheckIfRBOInterrupt(altOptionRegister);
            CheckIfRB47Interrupt(altPortBRegister);
        }

        private static void CheckIfRB47Interrupt(byte altPortBRegister)
        {
            byte neuPortBRegister = getRegisterWert(PORTB); //neuer PortBRegisterWert wird ausgelesen um RB4 - RB7 auf Veränderung zu prüfen
            string altPortBRegisterAsString = Convert.ToString(altPortBRegister);
            string neuPortBRegisterAsString = Convert.ToString(neuPortBRegister);
            if ((byte)(getRegisterWert(0x86) & 0x80) == 0x80) // 7.Bit steht 1 => INPUT
            {  try
                {
                    string altRB7 = altPortBRegisterAsString.Substring(0, 1);
                    string neuRB7 = neuPortBRegisterAsString.Substring(0, 1);
                    SetRB47Interrupt(altRB7, neuRB7);
                }
                catch (Exception){}
            }
            if ((byte)(getRegisterWert(0x86) & 0x40) == 0x40) // 6.Bit steht 1 => INPUT
            {
                try
                {
                    string altRB6 = altPortBRegisterAsString.Substring(1, 1);
                    string neuRB6 = neuPortBRegisterAsString.Substring(1, 1);
                    SetRB47Interrupt(altRB6, neuRB6);
                }
                catch (Exception) { }
            }
            if ((byte)(getRegisterWert(0x86) & 0x20) == 0x20) // 5.Bit steht 1 => INPUT
            {
                try
                {
                    string altRB5 = altPortBRegisterAsString.Substring(2, 1);
                    string neuRB5 = neuPortBRegisterAsString.Substring(2, 1);
                    SetRB47Interrupt(altRB5, neuRB5);
                }
                catch (Exception) { }
            }
            if ((byte)(getRegisterWert(0x86) & 0x10) == 0x10) // 4.Bit steht 1 => INPUT
            {
                try
                {
                    string altRB4 = altPortBRegisterAsString.Substring(3, 1);
                    string neuRB4 = neuPortBRegisterAsString.Substring(3, 1);
                    SetRB47Interrupt(altRB4, neuRB4);
                }
                catch (Exception) { }
            }
        }

        private static void SetRB47Interrupt(string altRB, string neuRB)
        {
            if (altRB != neuRB)
            {
                byte tempINTCON = getRegisterWert(INTCON);
                WriteToBank(PORTB, (byte)(tempINTCON | 0x01));
                CopyBanks();
                Interrupt.CallRB47Interrupt();
            }
        }

        private static void CheckIfRBOInterrupt(byte altOptionRegister)
        {
            byte neuOptionRegister = getRegisterWert(0x81); //neuer OptionRegisterWert wird ausgelesen um INTEDG auf Veränderung zu prüfen
            string altOptionRegisterAsString = Convert.ToString(altOptionRegister);
            string neuOptionRegisterAsString = Convert.ToString(neuOptionRegister);
            try
            {
                string altINTEDG = altOptionRegisterAsString.Substring(1, 1);
                string neuINTEDG = neuOptionRegisterAsString.Substring(1, 1);
                if (altINTEDG != neuINTEDG)
                {
                    byte tempPortB = getRegisterWert(PORTB);
                    WriteToBank(PORTB, (byte)(tempPortB | 0x01));
                    CopyBanks();
                    Interrupt.CallRB0Interrupt();
                }
            }
            catch (Exception)
            {

            }
            
        }

        public static void setRegisterBCF(byte address, byte value) // damit werte in beide Bänke geschrieben werden
        {
            speicher[address + 0x80] = value;
            labels[address + 0x80].Content = value.ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
 
            speicher[address] = value;
            labels[address].Content = value.ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
        }


        private static void CopyBanks()
        {
            byte InhaltStatus = getRegisterWert(STATUS);
            if ((InhaltStatus & 0x20) == 0x20) //Wenn im Statusregister das 5.Bit gesetzt ist
            {
                byte RegisterWertPCL = getRegisterWert(PCL);
                speicher[PCL + 0x80] = RegisterWertPCL;
                labels[PCL + 0x80].Content = RegisterWertPCL.ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                byte RegisterWertSTATUS = getRegisterWert(STATUS);
                speicher[STATUS + 0x80] = RegisterWertSTATUS;
                labels[STATUS + 0x80].Content = RegisterWertSTATUS.ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                byte RegisterWertFSR = getRegisterWert(FSR);
                speicher[FSR + 0x80] = RegisterWertFSR;
                labels[FSR + 0x80].Content = RegisterWertFSR.ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                byte RegisterWertPCLATH = getRegisterWert(PCLATH);
                speicher[PCLATH + 0x80] = RegisterWertPCLATH;
                labels[PCLATH + 0x80].Content = RegisterWertPCLATH.ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                byte RegisterWertINTCON = getRegisterWert(INTCON);
                speicher[INTCON + 0x80] = RegisterWertINTCON;
                labels[INTCON + 0x80].Content = RegisterWertINTCON.ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
            }
        }

        private static void WriteToBank(byte address, byte value)
        {
            byte InhaltStatus = getRegisterWert(STATUS);
            if ((InhaltStatus & 0x20) == 0x20) //Wenn im Statusregister das 5.Bit gesetzt ist, schreibe in Bank 1
            {
                speicher[address + 0x80] = value;
                labels[address + 0x80].Content = value.ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
            }
            else //schreibe in Bank 0
            {
                speicher[address] = value;
                labels[address].Content = value.ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
            }
        }

        public static byte getRegisterWert(byte address)
        {
            return speicher[address];
        }

        public static bool checkIfBitIsSet(byte address, byte bit) //prüfen ob bestimmtes Bit gesetzt ist
        {
            if (address == indirect)
                address = speicher[FSR];
            return (speicher[address] & (byte)Math.Pow(2, bit)) != 0;
        }

    
    }
}

