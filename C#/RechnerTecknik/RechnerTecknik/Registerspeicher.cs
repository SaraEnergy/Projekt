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

        //private static void SwitchBanks(byte address, byte value)
        //{
        //    //Prüfen ob geswitched werden muss
        //    switch (address)
        //    {
        //        // Bank0 addressiert: auf Bank1 uebertragen
        //        case PCL:
        //        case STATUS:
        //        case FSR:
        //        case PCLATH:
        //        case INTCON:
        //            speicher[address + 0x80] = value;
        //            labels[address + 0x80].Content = value.ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
        //            break;
        //        // Bank1 addressiert: auf Bank0 uebertragen
        //        case PCL + 0x80:
        //        case STATUS + 0x80:
        //        case FSR + 0x80:
        //        case PCLATH + 0x80:
        //        case INTCON + 0x80:
        //            speicher[address - 0x80] = value;
        //            labels[address - 0x80].Content = value.ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
        //            break;
        //        // default
        //        default:
        //            break;
        //    }
        //}

        public static void setRegisterWert(byte address, byte value)
        {
            CheckBanks(address, value);
            speicher[address] = value;
            labels[address].Content = value.ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
            CopyBanks();
            //SwitchBanks(address, value);
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

        private static void CheckBanks(byte address, byte value)
        {
            byte InhaltStatus = getRegisterWert(STATUS);
            if ((InhaltStatus & 0x20) == 0x20) //Wenn im Statusregister das 5.Bit gesetzt ist
            {
                speicher[address + 0x80] = value;
                labels[address + 0x80].Content = value.ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
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

        //public static byte toogleBit(byte addr, byte bit)
        //{
        //    byte pow = (byte)Math.Pow(2, bit);
        //    if ((speicher[addr] & pow) == pow) // bit ist gesetzt
        //    {
        //        Console.WriteLine("Bit war gesetzt");
        //        speicher[addr] &= (byte)(0xff - pow);
        //        byte value = speicher[addr];
        //        labels[addr].Content = value.ToString("X2");
        //        CheckSwitchBanks(addr, value);
        //        return 0;
        //    }
        //    else // bit ist nicht gesetzt
        //    {
        //        Console.WriteLine("Bit war nicht gesetzt");
        //        speicher[addr] |= pow;
        //        byte value = speicher[addr];
        //        labels[addr].Content = value.ToString("X2");
        //        CheckSwitchBanks(addr, value);
        //        return 1;
        //    }
        //}

        //public static void setIndirect(byte indirct)
        //{
        //    set(indirect, indirct);
        //}

        //public static byte getIndirect()
        //{
        //    return speicher[indirect];
        //}

        //public static void setTMR0(byte tmr0)
        //{
        //    set(TMR0, tmr0);
        //}

        //public static byte getTMR0()
        //{
        //    return speicher[TMR0];
        //}

        //public static void setPCL(byte pcl)
        //{
        //    set(PCL, pcl);
        //}

        //public static byte getPCL()
        //{
        //    return speicher[PCL];
        //}

        //public static void setStatus(byte status)
        //{
        //    set(STATUS, status);
        //}

        //public static byte getStatus()
        //{
        //    return speicher[STATUS];
        //}

        //public static void setFSR(byte fsr)
        //{
        //    set(FSR, fsr);
        //}

        //public static byte getFSR()
        //{
        //    return speicher[FSR];
        //}

        //public static void setPortA(byte PortA)
        //{
        //    set(PORTA, PortA);
        //}

        //public static byte getPortA()
        //{
        //    return speicher[PORTA];
        //}

        //public static void setPortB(byte PortB)
        //{
        //    set(PORTB, PortB);
        //}

        //public static byte getPortB()
        //{
        //    return speicher[PORTB];
        //}

        //public static void setEEDATA(byte eedata)
        //{
        //    set(EEDATA, eedata);
        //}

        //public static byte getEEDATA()
        //{
        //    return speicher[EEDATA];
        //}

        //public static void setEEADR(byte eeadr)
        //{
        //    set(EEADR, eeadr);
        //}

        //public static byte getEEADR()
        //{
        //    return speicher[EEADR];
        //}

        //public static void setPCLATH(byte pclath)
        //{
        //    set(PCLATH, pclath);
        //}

        //public static byte getPCLATH()
        //{
        //    return speicher[PCLATH];
        //}

        //public static void setIntcont(byte intcon)
        //{
        //    set(INTCON, intcon);
        //}

        //public static byte getIntcont()
        //{
        //    return speicher[INTCON];
        //}



    }
}

