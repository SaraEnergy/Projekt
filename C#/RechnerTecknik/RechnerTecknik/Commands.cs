using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RechnerTecknik
{
    class Commands
    {
        static MainWindow mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
        static string stackAsString = string.Empty;

        public static void MOVLW(string commandAsString)
        {
            byte myLiteralAsByte = ConvertStringToByte(commandAsString, 2, 2); //Argument von Befehl trennen
            Registerspeicher.W = myLiteralAsByte; // Literal in w-Register schieben
        }

        public static void ANDLW(string commandAsString)
        {
            byte myLiteralAsByte = ConvertStringToByte(commandAsString, 2, 2); //Argument von Befehl trennen
            byte w = Registerspeicher.W;
            Registerspeicher.W = (byte)(w & myLiteralAsByte); // Verundung Literal mit w-Register
            Registerspeicher.SetZeroFlag(Registerspeicher.W);
        }

        public static void IORLW(string commandAsString)
        {
            byte myLiteralAsByte = ConvertStringToByte(commandAsString, 2, 2); //Argument von Befehl trennen
            byte w = Registerspeicher.W;
            Registerspeicher.W = (byte)(w | myLiteralAsByte); // inclusive Or Literal mit w
            Registerspeicher.SetZeroFlag(Registerspeicher.W);
        }

        public static void SUBLW(string commandAsString)
        {
            byte myLiteralAsByte = ConvertStringToByte(commandAsString, 2, 2); //Argument von Befehl trennen

            var w = Registerspeicher.W;
            var resultTemp = (ushort)(myLiteralAsByte - w);
            var result = (byte)resultTemp;

            Registerspeicher.W = (byte)(myLiteralAsByte - w); // subtrahier w vom Literal
            Registerspeicher.SetZeroFlag(Registerspeicher.W);
            Registerspeicher.SetCarryFlag(resultTemp, true);
            Registerspeicher.SetDigitCarryFlag((byte)((myLiteralAsByte & 0xF) - (w & 0xF)), true);
        }

        public static void XORLW(string commandAsString)
        {
            byte myLiteralAsByte = ConvertStringToByte(commandAsString, 2, 2); //Argument von Befehl trennen
            byte w = Registerspeicher.W;
            Registerspeicher.W = (byte)(w ^ myLiteralAsByte); // exclusive or w mit Literal
            Registerspeicher.SetZeroFlag(Registerspeicher.W);
        }

        public static void ADDLW(string commandAsString)
        {
            byte myLiteralAsByte = ConvertStringToByte(commandAsString, 2, 2); //Argument von Befehl trennen
            
            var w = Registerspeicher.W;
            var resultTemp = (ushort)(myLiteralAsByte + w);
            var result = (byte)resultTemp;

            Registerspeicher.W = (byte)(w + myLiteralAsByte); // addiere w mit Literal
            Registerspeicher.SetZeroFlag(Registerspeicher.W);
            Registerspeicher.SetCarryFlag(resultTemp);
            Registerspeicher.SetDigitCarryFlag((byte)((myLiteralAsByte & 0xF) + (w & 0xF)));

        }

        public static void GOTO(string commandAsString)
        {
            int myLiteralAsNum = Convert.ToInt32(commandAsString, 16); // string zu int konvertieren
            string binary = Convert.ToString(myLiteralAsNum, 2); // int zu Binärstring konvertieren
            string myLiteralbinary = binary.Substring(3, 11); //Argument, Adresse auslesen

            int addressToGo = Convert.ToInt32(myLiteralbinary, 2); //Binäradresse wird als Int gespeichert
            MainWindow.CommandCounter = addressToGo;
            MainWindow.numberOfCycles = 2;
        }

        public static void CALL(string commandAsString)
        {
            Stack.myStack.Push(MainWindow.CommandCounter); // und in Stack für Rücksprungadresse gespeichert
            stackAsString = MainWindow.CommandCounter.ToString("X2") + stackAsString; 

            mainWin.stackBox.Text = stackAsString;

            int myLiteralAsNum = Convert.ToInt32(commandAsString, 16); // string zu int konvertieren
            string binary = Convert.ToString(myLiteralAsNum, 2); // int zu Binärstring konvertieren
            string myLiteralbinary = binary.Substring(3, 11); //Argument, Adresse auslesen

            int addressToGo = Convert.ToInt32(myLiteralbinary, 2); //Binäradresse wird als Int gespeichert
            MainWindow.CommandCounter = addressToGo;
            MainWindow.numberOfCycles = 2;
        }

        public static void RETURN()
        {
            MainWindow.CommandCounter = Stack.myStack.Peek(); //Adresse wird vom Stack geholt
            MainWindow.numberOfCycles = 2;
        }

        public static void NOP()
        {
           //NIX TUN
        }

        public static void RETLW(string commandAsString)
        {
            MOVLW(commandAsString);
            RETURN();
            MainWindow.numberOfCycles = 2;
        }

        public static void MOVWF(string commandAsString)
        {
            int myLiteralAsNum = Convert.ToInt32(commandAsString, 16); // string zu int konvertieren
            string binary = Convert.ToString(myLiteralAsNum, 2); // int zu Binärstring konvertieren
            string myLiteralbinary = binary.Substring(1, 7); //Argument, Adresse f auslesen

            int addressToGo = Convert.ToInt32(myLiteralbinary, 2); //Binäradresse wird als Int gespeichert
            byte fAsByteUnchecked = Convert.ToByte(addressToGo);

            byte fAsByte = CheckIndirectAddressing(fAsByteUnchecked); //Indirekte Adressierung wird überprüft

            Registerspeicher.setRegisterWert(fAsByte, Registerspeicher.W);
        }

        public static void ADDWF(string commandAsString) 
        {
            int myLiteralAsNum = Convert.ToInt32(commandAsString, 16); // string zu int konvertieren
            string binary = Convert.ToString(myLiteralAsNum, 2); // int zu Binärstring konvertieren
            string fLiteralbinary = binary.Substring(4, 7); //Argument, Adresse f auslesen
            string dLiteralbinary =binary.Substring(3, 1); // Argument d auslesen ob 1 oder 0

            int fAsInt = Convert.ToInt32(fLiteralbinary, 2); //Binäradresse wird als Int gespeichert
            byte fAsByteUnchecked = Convert.ToByte(fAsInt);

            byte fAsByte = CheckIndirectAddressing(fAsByteUnchecked); //Indirekte Adressierung wird überprüft

            var fWertvar = Registerspeicher.getRegisterWert(fAsByte);
            var w = Registerspeicher.W;
            var resultTemp = (ushort)(fWertvar + w);
            var result = (byte)resultTemp;
           
            if (dLiteralbinary == "0")
            {
                Registerspeicher.W = result;
            }
            else if (dLiteralbinary == "1")
            {
                Registerspeicher.setRegisterWert(fAsByte, result);
            }
            else 
            {
                MessageBox.Show("Problem ADDWF - d is not defined");
            }
            Registerspeicher.SetZeroFlag(result);
            Registerspeicher.SetCarryFlag(resultTemp);
            Registerspeicher.SetDigitCarryFlag((byte)((fWertvar & 0xF) + (w & 0xF)));
        }

        public static void ANDWF(string commandAsString)
        {
            byte w = Registerspeicher.W;
            int myLiteralAsNum = Convert.ToInt32(commandAsString, 16); // string zu int konvertieren
            string binary = Convert.ToString(myLiteralAsNum, 2); // int zu Binärstring konvertieren
            string fLiteralbinary = binary.Substring(4, 7); //Argument, Adresse f auslesen
            string dLiteralbinary = binary.Substring(3, 1); // Argument d auslesen ob 1 oder 0

            int fAsInt = Convert.ToInt32(fLiteralbinary, 2); //Binäradresse wird als Int gespeichert
            byte fAsByteUnchecked = Convert.ToByte(fAsInt);

            byte fAsByte = CheckIndirectAddressing(fAsByteUnchecked); //Indirekte Adressierung wird überprüft

            byte fWert = Registerspeicher.getRegisterWert(fAsByte);

            byte result = (byte)(fWert & w);
            int tempResult = Convert.ToInt32(result);

            if (dLiteralbinary == "0")
            {
                Registerspeicher.W = result;
            }
            else if (dLiteralbinary == "1")
            {
                Registerspeicher.setRegisterWert(fAsByte, result);
            }
            else
            {
                MessageBox.Show("Problem ANDWF - d is not defined");
            }
            Registerspeicher.SetZeroFlag(result);
        }

        public static void CLRF(string commandAsString)
        {
            int myLiteralAsNum = Convert.ToInt32(commandAsString, 16); // string zu int konvertieren
            string binary = Convert.ToString(myLiteralAsNum, 2); // int zu Binärstring konvertieren
            string myLiteralbinary = binary.Substring(2, 7); //Argument, Adresse f auslesen

            int addressToGo = Convert.ToInt32(myLiteralbinary, 2); //Binäradresse wird als Int gespeichert
            byte fAsByteUnchecked = Convert.ToByte(addressToGo);

            byte fAsByte = CheckIndirectAddressing(fAsByteUnchecked); //Indirekte Adressierung wird überprüft

            Registerspeicher.setRegisterWert(fAsByte, 0x00); //Register an Adress f wird auf 0 gesetzt

            Registerspeicher.SetZeroFlag(0x00); //ZeroFlag wird auf 1 gesetzt
        }

        public static void COMF(string commandAsString)
        {
            int myLiteralAsNum = Convert.ToInt32(commandAsString, 16); // string zu int konvertieren
            string binary = Convert.ToString(myLiteralAsNum, 2); // int zu Binärstring konvertieren
            string fLiteralbinary = binary.Substring(5, 7); //Argument, Adresse f auslesen
            string dLiteralbinary = binary.Substring(4, 1); // Argument d auslesen ob 1 oder 0

            int fAsInt = Convert.ToInt32(fLiteralbinary, 2); //Binäradresse wird als Int gespeichert
            byte fAsByteUnchecked = Convert.ToByte(fAsInt);

            byte fAsByte = CheckIndirectAddressing(fAsByteUnchecked); //Indirekte Adressierung wird überprüft

            byte fWert = Registerspeicher.getRegisterWert(fAsByte);

            fWert = (byte)~fWert; //fWert wird complementiert/negiert

            if (dLiteralbinary == "0")
            {
                Registerspeicher.W = fWert;
            }
            else if (dLiteralbinary == "1")
            {
                Registerspeicher.setRegisterWert(fAsByte, fWert);
            }
            else
            {
                MessageBox.Show("Problem COMF - d is not defined");
            }
            Registerspeicher.SetZeroFlag(fWert);
        }

        public static void DECF(string commandAsString)
        {
            int myLiteralAsNum = Convert.ToInt32(commandAsString, 16); // string zu int konvertieren
            string binary = Convert.ToString(myLiteralAsNum, 2); // int zu Binärstring konvertieren
            string fLiteralbinary = binary.Substring(3, 7); //Argument, Adresse f auslesen
            string dLiteralbinary = binary.Substring(2, 1); // Argument d auslesen ob 1 oder 0

            int fAsInt = Convert.ToInt32(fLiteralbinary, 2); //Binäradresse wird als Int gespeichert
            byte fAsByteUnchecked = Convert.ToByte(fAsInt);

            byte fAsByte = CheckIndirectAddressing(fAsByteUnchecked); //Indirekte Adressierung wird überprüft

            byte fWert = Registerspeicher.getRegisterWert(fAsByte);

            fWert--; //decrementieren des f-Werts

            if (dLiteralbinary == "0")
            {
                Registerspeicher.W = fWert;
            }
            else if (dLiteralbinary == "1")
            {
                Registerspeicher.setRegisterWert(fAsByte, fWert);
            }
            else
            {
                MessageBox.Show("Problem COMF - d is not defined");
            }
            Registerspeicher.SetZeroFlag(fWert);
        }

        public static void INCF(string commandAsString)
        {
            int myLiteralAsNum = Convert.ToInt32(commandAsString, 16); // string zu int konvertieren
            string binary = Convert.ToString(myLiteralAsNum, 2); // int zu Binärstring konvertieren
            string fLiteralbinary = binary.Substring(5, 7); //Argument, Adresse f auslesen
            string dLiteralbinary = binary.Substring(4, 1); // Argument d auslesen ob 1 oder 0

            int fAsInt = Convert.ToInt32(fLiteralbinary, 2); //Binäradresse wird als Int gespeichert
            byte fAsByteUnchecked = Convert.ToByte(fAsInt);

            byte fAsByte = CheckIndirectAddressing(fAsByteUnchecked); //Indirekte Adressierung wird überprüft

            byte fWert = Registerspeicher.getRegisterWert(fAsByte);

            fWert++; //incrementieren des f-Werts

            if (dLiteralbinary == "0")
            {
                Registerspeicher.W = fWert;
            }
            else if (dLiteralbinary == "1")
            {
                Registerspeicher.setRegisterWert(fAsByte, fWert);
            }
            else
            {
                MessageBox.Show("Problem COMF - d is not defined");
            }
            Registerspeicher.SetZeroFlag(fWert);
        }

        public static void MOVF(string commandAsString)
        {
            int myLiteralAsNum = Convert.ToInt32(commandAsString, 16); // string zu int konvertieren
            string binary = Convert.ToString(myLiteralAsNum, 2); // int zu Binärstring konvertieren
            string fLiteralbinary = binary.Substring(5, 7); //Argument, Adresse f auslesen
            string dLiteralbinary = binary.Substring(4, 1); // Argument d auslesen ob 1 oder 0

            int fAsInt = Convert.ToInt32(fLiteralbinary, 2); //Binäradresse wird als Int gespeichert
            byte fAsByteUnchecked = Convert.ToByte(fAsInt);

            byte fAsByte = CheckIndirectAddressing(fAsByteUnchecked); //Indirekte Adressierung wird überprüft

            byte fWert = Registerspeicher.getRegisterWert(fAsByte);

            if (dLiteralbinary == "0")
            {
                Registerspeicher.W = fWert;
            }
            else if (dLiteralbinary == "1")
            {
                Registerspeicher.setRegisterWert(fAsByte, fWert);
            }
            else
            {
                MessageBox.Show("Problem MOVF - d is not defined");
            }
            Registerspeicher.SetZeroFlag(fWert);
        }

        public static void IORWF(string commandAsString)
        {
            int myLiteralAsNum = Convert.ToInt32(commandAsString, 16); // string zu int konvertieren
            string binary = Convert.ToString(myLiteralAsNum, 2); // int zu Binärstring konvertieren
            string fLiteralbinary = binary.Substring(4, 7); //Argument, Adresse f auslesen
            string dLiteralbinary = binary.Substring(3, 1); // Argument d auslesen ob 1 oder 0

            int fAsInt = Convert.ToInt32(fLiteralbinary, 2); //Binäradresse wird als Int gespeichert
            byte fAsByteUnchecked = Convert.ToByte(fAsInt);

            byte fAsByte = CheckIndirectAddressing(fAsByteUnchecked); //Indirekte Adressierung wird überprüft

            byte w = Registerspeicher.W;
            byte fWert = Registerspeicher.getRegisterWert(fAsByte);

            byte result = (byte)(w | fWert); //W wird mit f verodert
             
            if (dLiteralbinary == "0")
            {
                Registerspeicher.W = result;
            }
            else if (dLiteralbinary == "1")
            {
                Registerspeicher.setRegisterWert(fAsByte, result);
            }
            else
            {
                MessageBox.Show("Problem COMF - d is not defined");
            }
            Registerspeicher.SetZeroFlag(result);
        }

        public static void SUBWF(string commandAsString)
        {
            int myLiteralAsNum = Convert.ToInt32(commandAsString, 16); // string zu int konvertieren
            string binary = Convert.ToString(myLiteralAsNum, 2); // int zu Binärstring konvertieren
            string fLiteralbinary = binary.Substring(3, 7); //Argument, Adresse f auslesen
            string dLiteralbinary = binary.Substring(2, 1); // Argument d auslesen ob 1 oder 0

            int fAsInt = Convert.ToInt32(fLiteralbinary, 2); //Binäradresse wird als Int gespeichert
            byte fAsByteUnchecked = Convert.ToByte(fAsInt);

            byte fAsByte = CheckIndirectAddressing(fAsByteUnchecked); //Indirekte Adressierung wird überprüft

            var fWertvar = Registerspeicher.getRegisterWert(fAsByte);
            var w = Registerspeicher.W;
            var resultTemp = (ushort)(fWertvar - w);
            var result = (byte)resultTemp;
            
            if (dLiteralbinary == "0")
            {
                Registerspeicher.W = result;
            }
            else if (dLiteralbinary == "1")
            {
                Registerspeicher.setRegisterWert(fAsByte, result);
            }
            else
            {
                MessageBox.Show("Problem SUBWF - d is not defined");
            }
            Registerspeicher.SetZeroFlag(result);
            Registerspeicher.SetCarryFlag(resultTemp, true);
            Registerspeicher.SetDigitCarryFlag((byte)((fWertvar & 0xF) - (w & 0xF)), true);
        }

        public static void SWAPF(string commandAsString)
        {
            int myLiteralAsNum = Convert.ToInt32(commandAsString, 16); // string zu int konvertieren
            string binary = Convert.ToString(myLiteralAsNum, 2); // int zu Binärstring konvertieren
            string fLiteralbinary = binary.Substring(5, 7); //Argument, Adresse f auslesen
            string dLiteralbinary = binary.Substring(4, 1); // Argument d auslesen ob 1 oder 0

            int fAsInt = Convert.ToInt32(fLiteralbinary, 2); //Binäradresse wird als Int gespeichert
            byte fAsByteUnchecked = Convert.ToByte(fAsInt);

            byte fAsByte = CheckIndirectAddressing(fAsByteUnchecked); //Indirekte Adressierung wird überprüft

            byte fWert = Registerspeicher.getRegisterWert(fAsByte); //Inhalt aus adresse f holen

            string fWertHex = fWert.ToString("X2"); // als Hexwert auslesen

            string fWertA = fWertHex.Substring(0, 1); // Hexwert teilen 
            string fWertB = fWertHex.Substring(1, 1); // Hexwert teilen

            string swap = string.Concat(fWertB, fWertA); //beide substrings in getauschter Reihenfolge zusammenfügen
            int swapAsInt = Convert.ToInt32(swap, 16); //zu decimal konvertieren

            string swapDec = swapAsInt.ToString("D2");
            
            byte result = Byte.Parse(swapDec); //string wird als byte gespeichert

            if (dLiteralbinary == "0")
            {
                Registerspeicher.W = result;
            }
            else if (dLiteralbinary == "1")
            {
                Registerspeicher.setRegisterWert(fAsByte, result);
            }
            else
            {
                MessageBox.Show("Problem SUBWF - d is not defined");
            }
        }

        public static void XORWF(string commandAsString)
        {
            int myLiteralAsNum = Convert.ToInt32(commandAsString, 16); // string zu int konvertieren
            string binary = Convert.ToString(myLiteralAsNum, 2); // int zu Binärstring konvertieren
            string fLiteralbinary = binary.Substring(4, 7); //Argument, Adresse f auslesen
            string dLiteralbinary = binary.Substring(3, 1); // Argument d auslesen ob 1 oder 0

            int fAsInt = Convert.ToInt32(fLiteralbinary, 2); //Binäradresse wird als Int gespeichert
            byte fAsByteUnchecked = Convert.ToByte(fAsInt);

            byte fAsByte = CheckIndirectAddressing(fAsByteUnchecked); //Indirekte Adressierung wird überprüft

            byte w = Registerspeicher.W;
            byte fWert = Registerspeicher.getRegisterWert(fAsByte);

            byte result = (byte)(w ^ fWert); //W wird mit f verodert

            if (dLiteralbinary == "0")
            {
                Registerspeicher.W = result;
            }
            else if (dLiteralbinary == "1")
            {
                Registerspeicher.setRegisterWert(fAsByte, result);
            }
            else
            {
                MessageBox.Show("Problem COMF - d is not defined");
            }
            Registerspeicher.SetZeroFlag(result);
        }

        public static void CLRW(string commandAsString)
        {
            Registerspeicher.W = 0x00; //Register w wird auf 0 gesetzt

            Registerspeicher.SetZeroFlag(0x00); //ZeroFlag wird auf 1 gesetzt
        }

        public static void RLF(string commandAsString)
        {
            int myLiteralAsNum = Convert.ToInt32(commandAsString, 16); // string zu int konvertieren
            string binary = Convert.ToString(myLiteralAsNum, 2); // int zu Binärstring konvertieren
            string fLiteralbinary = binary.Substring(5, 7); //Argument, Adresse f auslesen
            string dLiteralbinary = binary.Substring(4, 1); // Argument d auslesen ob 1 oder 0

            int fAsInt = Convert.ToInt32(fLiteralbinary, 2); //Binäradresse wird als Int gespeichert
            byte fAsByteUnchecked = Convert.ToByte(fAsInt);

            byte fAsByte = CheckIndirectAddressing(fAsByteUnchecked); //Indirekte Adressierung wird überprüft

            ushort fWertvar = Registerspeicher.getRegisterWert(fAsByte);
            fWertvar <<= 1; //1 links schifften
            var result = (byte)fWertvar;

            if (Registerspeicher.checkIfBitIsSet(Registerspeicher.STATUS, Registerspeicher.C))
            {
                result |= 1; //Rechteste Bit wird gesetzt, wenn Carrybit gesetzt war
            }

            if ((fWertvar & 0x100) != 0) //Linkeste Bit wird ins CarryFlag geschoben
            {
                Registerspeicher.speicher[Registerspeicher.STATUS] |= 0x01;  // C-flag wird auf 1 gesetzt;
                Registerspeicher.labels[Registerspeicher.STATUS].Content = Registerspeicher.speicher[Registerspeicher.STATUS].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                mainWin.CLabel.Content = "1";
            }
            else
            {
                Registerspeicher.speicher[Registerspeicher.STATUS] &= 0xFE;   // C-flag wird auf 0 gesetzt;
                Registerspeicher.labels[Registerspeicher.STATUS].Content = Registerspeicher.speicher[Registerspeicher.STATUS].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                mainWin.CLabel.Content = "0";
            }

            if (dLiteralbinary == "0")
            {
                Registerspeicher.W = result;
            }
            else if (dLiteralbinary == "1")
            {
                Registerspeicher.setRegisterWert(fAsByte, result);
            }
            else
            {
                MessageBox.Show("Problem RLF - d is not defined");
            }
        }

        public static void RRF(string commandAsString) // Carry Flag hier noch Falsch!!!!!!!!!!!!!!!!!!!!!!!!!!
        {
            int myLiteralAsNum = Convert.ToInt32(commandAsString, 16); // string zu int konvertieren
            string binary = Convert.ToString(myLiteralAsNum, 2); // int zu Binärstring konvertieren
            string fLiteralbinary = binary.Substring(5, 7); //Argument, Adresse f auslesen
            string dLiteralbinary = binary.Substring(4, 1); // Argument d auslesen ob 1 oder 0

            int fAsInt = Convert.ToInt32(fLiteralbinary, 2); //Binäradresse wird als Int gespeichert
            byte fAsByteUnchecked = Convert.ToByte(fAsInt);

            byte fAsByte = CheckIndirectAddressing(fAsByteUnchecked); //Indirekte Adressierung wird überprüft

            ushort fWertvar = Registerspeicher.getRegisterWert(fAsByte);
            
            if (Registerspeicher.checkIfBitIsSet(Registerspeicher.STATUS, Registerspeicher.C))
            {
                fWertvar |= 0x100; //Linkeste Bit wird gesetzt, wenn Carrybit gesetzt war
                Console.WriteLine("fWertvar+: " + fWertvar.ToString("X2"));
            }

            if ((fWertvar & 0x01) != 0) //Rechteste Bit wird ins Carryflag geschoben
            {
                Registerspeicher.speicher[Registerspeicher.STATUS] |= 0x01;  // C-flag wird auf 1 gesetzt;
                Registerspeicher.labels[Registerspeicher.STATUS].Content = Registerspeicher.speicher[Registerspeicher.STATUS].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                mainWin.CLabel.Content = "1";
            }
            else
            {
                Registerspeicher.speicher[Registerspeicher.STATUS] &= 0xFE;   // C-flag wird auf 0 gesetzt;
                Registerspeicher.labels[Registerspeicher.STATUS].Content = Registerspeicher.speicher[Registerspeicher.STATUS].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters
                mainWin.CLabel.Content = "0";
            }

            fWertvar >>= 1; //1 rechts schifften
            var result = (byte)fWertvar;

            if (dLiteralbinary == "0")
            {
                Registerspeicher.W = result;
            }
            else if (dLiteralbinary == "1")
            {
                Registerspeicher.setRegisterWert(fAsByte, result);
            }
            else
            {
                MessageBox.Show("Problem RRF - d is not defined");
            }
        }

        public static void DECFSZ(string commandAsString)
        {
            int myLiteralAsNum = Convert.ToInt32(commandAsString, 16); // string zu int konvertieren
            string binary = Convert.ToString(myLiteralAsNum, 2); // int zu Binärstring konvertieren
            string fLiteralbinary = binary.Substring(5, 7); //Argument, Adresse f auslesen
            string dLiteralbinary = binary.Substring(4, 1); // Argument d auslesen ob 1 oder 0

            int fAsInt = Convert.ToInt32(fLiteralbinary, 2); //Binäradresse wird als Int gespeichert
            byte fAsByteUnchecked = Convert.ToByte(fAsInt);

            byte fAsByte = CheckIndirectAddressing(fAsByteUnchecked); //Indirekte Adressierung wird überprüft

            byte fWert = Registerspeicher.getRegisterWert(fAsByte);
            fWert--;

            if (fWert == 0)
            {
                Commands.NOP();
                MainWindow.CommandCounter++;
                MainWindow.numberOfCycles = 2;
            }
            else
            {
                if (dLiteralbinary == "0")
                {
                    Registerspeicher.W = fWert;
                }
                else if (dLiteralbinary == "1")
                {
                    Registerspeicher.setRegisterWert(fAsByte, fWert);
                }
                else
                {
                    MessageBox.Show("Problem RRF - d is not defined");
                }
            }
        }

        public static void INCFSZ(string commandAsString)
        {
            int myLiteralAsNum = Convert.ToInt32(commandAsString, 16); // string zu int konvertieren
            string binary = Convert.ToString(myLiteralAsNum, 2); // int zu Binärstring konvertieren
            string fLiteralbinary = binary.Substring(5, 7); //Argument, Adresse f auslesen
            string dLiteralbinary = binary.Substring(4, 1); // Argument d auslesen ob 1 oder 0

            int fAsInt = Convert.ToInt32(fLiteralbinary, 2); //Binäradresse wird als Int gespeichert
            byte fAsByteUnchecked = Convert.ToByte(fAsInt);

            byte fAsByte = CheckIndirectAddressing(fAsByteUnchecked); //Indirekte Adressierung wird überprüft

            byte fWert = Registerspeicher.getRegisterWert(fAsByte);
            fWert++;

            if (fWert == 0)
            {
                Commands.NOP();
                MainWindow.CommandCounter++;
                MainWindow.numberOfCycles = 2;
            }
            else
            {
                if (dLiteralbinary == "0")
                {
                    Registerspeicher.W = fWert;
                }
                else if (dLiteralbinary == "1")
                {
                    Registerspeicher.setRegisterWert(fAsByte, fWert);
                }
                else
                {
                    MessageBox.Show("Problem RRF - d is not defined");
                }
            }
        }

        public static void BCF(string commandAsString)
        {
            int myLiteralAsNum = Convert.ToInt32(commandAsString, 16); // string zu int konvertieren
            string binary = Convert.ToString(myLiteralAsNum, 2); // int zu Binärstring konvertieren
            string fLiteralbinary = binary.Substring(6, 7); //Argument, Adresse f auslesen
            string bLiteralbinary = binary.Substring(3, 3); // Argument d auslesen ob 1 oder 0

            int fAsInt = Convert.ToInt32(fLiteralbinary, 2); //Binäradresse wird als Int gespeichert
            byte fAsByteUnchecked = Convert.ToByte(fAsInt);

            byte fAsByte = CheckIndirectAddressing(fAsByteUnchecked); //Indirekte Adressierung wird überprüft

            byte fWert = Registerspeicher.getRegisterWert(fAsByte);

            int bAsInt = Convert.ToInt32(bLiteralbinary, 2); //B-binär wird als Int gespeichert
            byte result = 0;

            if (bAsInt == 0)
            {
                result = (byte) (fWert & 0xFE);
            }
            else if (bAsInt == 1)
            {
                result = (byte)(fWert & 0xFD);
            }
            else if (bAsInt == 2)
            {
                result = (byte)(fWert & 0xFB);
            }
            else if (bAsInt == 3)
            {
                result = (byte)(fWert & 0xF7);
            }
            else if (bAsInt == 4)
            {
                result = (byte)(fWert & 0xEF);
            }
            else if (bAsInt == 5)
            {
                result = (byte)(fWert & 0xDF);
            }
            else if (bAsInt == 6)
            {
                result = (byte)(fWert & 0xBF);
            }
            else if (bAsInt == 7)
            {
                result = (byte)(fWert & 0x7F);
            }
            else
            {
                MessageBox.Show("Problem BCF - b is not defined");
            }
            Registerspeicher.setRegisterBCF(fAsByte, result);
        }

        public static void BSF(string commandAsString)
        {
            int myLiteralAsNum = Convert.ToInt32(commandAsString, 16); // string zu int konvertieren
            string binary = Convert.ToString(myLiteralAsNum, 2); // int zu Binärstring konvertieren
            string fLiteralbinary = binary.Substring(6, 7); //Argument, Adresse f auslesen
            string bLiteralbinary = binary.Substring(3, 3); // Argument d auslesen ob 1 oder 0

            int fAsInt = Convert.ToInt32(fLiteralbinary, 2); //Binäradresse wird als Int gespeichert
            byte fAsByteUnchecked = Convert.ToByte(fAsInt);

            byte fAsByte = CheckIndirectAddressing(fAsByteUnchecked); //Indirekte Adressierung wird überprüft

            byte fWert = Registerspeicher.getRegisterWert(fAsByte);

            int bAsInt = Convert.ToInt32(bLiteralbinary, 2); //B-binär wird als Int gespeichert
            byte result = 0;

            if (bAsInt == 0)
            {
                result = (byte)(fWert | 0x01);
            }
            else if (bAsInt == 1)
            {
                result = (byte)(fWert | 0x02);
            }
            else if (bAsInt == 2)
            {
                result = (byte)(fWert | 0x04);
            }
            else if (bAsInt == 3)
            {
                result = (byte)(fWert | 0x08);
            }
            else if (bAsInt == 4)
            {
                result = (byte)(fWert | 0x10);
            }
            else if (bAsInt == 5)
            {
                result = (byte)(fWert | 0x20);
            }
            else if (bAsInt == 6)
            {
                result = (byte)(fWert | 0x40);
            }
            else if (bAsInt == 7)
            {
                result = (byte)(fWert | 0x80);
            }
            else
            {
                MessageBox.Show("Problem BSF - b is not defined");
            }
            Registerspeicher.setRegisterWert(fAsByte, result);
        }

        public static void BTFSC(string commandAsString)
        {
            int myLiteralAsNum = Convert.ToInt32(commandAsString, 16); // string zu int konvertieren
            string binary = Convert.ToString(myLiteralAsNum, 2); // int zu Binärstring konvertieren
            string fLiteralbinary = binary.Substring(6, 7); //Argument, Adresse f auslesen
            string bLiteralbinary = binary.Substring(3, 3); // Argument d auslesen ob 1 oder 0

            int fAsInt = Convert.ToInt32(fLiteralbinary, 2); //Binäradresse wird als Int gespeichert
            byte fAsByteUnchecked = Convert.ToByte(fAsInt);

            byte fAsByte = CheckIndirectAddressing(fAsByteUnchecked); //Indirekte Adressierung wird überprüft

            byte fWert = Registerspeicher.getRegisterWert(fAsByte);

            int bAsInt = Convert.ToInt32(bLiteralbinary, 2); //B-binär wird als Int gespeichert
            byte result = 0;

            if (bAsInt == 0)
            {
                if ((fWert & 0x01) == 0x00)
                {
                    MainWindow.CommandCounter++;
                    MainWindow.numberOfCycles = 2;
                }
            }
            else if (bAsInt == 1)
            {
                if ((fWert & 0x02) == 0x00)
                {
                    MainWindow.CommandCounter++;
                    MainWindow.numberOfCycles = 2;
                }
            }
            else if (bAsInt == 2)
            {
                if ((fWert & 0x04) == 0x00)
                {
                    MainWindow.CommandCounter++;
                    MainWindow.numberOfCycles = 2;
                }
            }
            else if (bAsInt == 3)
            {
                if ((fWert & 0x08) == 0x00)
                {
                    MainWindow.CommandCounter++;
                    MainWindow.numberOfCycles = 2;
                }
            }
            else if (bAsInt == 4)
            {
                if ((fWert & 0x10) == 0x00)
                {
                    MainWindow.CommandCounter++;
                    MainWindow.numberOfCycles = 2;
                }
            }
            else if (bAsInt == 5)
            {
                if ((fWert & 0x20) == 0x00)
                {
                    MainWindow.CommandCounter++;
                    MainWindow.numberOfCycles = 2;
                }
            }
            else if (bAsInt == 6)
            {
                if ((fWert & 0x40) == 0x00)
                {
                    MainWindow.CommandCounter++;
                    MainWindow.numberOfCycles = 2;
                }
            }
            else if (bAsInt == 7)
            {
                if ((fWert & 0x80) == 0x00)
                {
                    MainWindow.CommandCounter++;
                    MainWindow.numberOfCycles = 2;
                }
            }
            else
            {
                MessageBox.Show("Problem BTFSC - b is not defined");
            }
            Registerspeicher.setRegisterWert(fAsByte, result);
        }

        public static void BTFSS(string commandAsString)
        {
            int myLiteralAsNum = Convert.ToInt32(commandAsString, 16); // string zu int konvertieren
            string binary = Convert.ToString(myLiteralAsNum, 2); // int zu Binärstring konvertieren
            string fLiteralbinary = binary.Substring(6, 7); //Argument, Adresse f auslesen
            string bLiteralbinary = binary.Substring(3, 3); // Argument d auslesen ob 1 oder 0

            int fAsInt = Convert.ToInt32(fLiteralbinary, 2); //Binäradresse wird als Int gespeichert
            byte fAsByteUnchecked = Convert.ToByte(fAsInt);

            byte fAsByte = CheckIndirectAddressing(fAsByteUnchecked); //Indirekte Adressierung wird überprüft

            byte fWert = Registerspeicher.getRegisterWert(fAsByte);

            int bAsInt = Convert.ToInt32(bLiteralbinary, 2); //B-binär wird als Int gespeichert
            byte result = 0;

            if (bAsInt == 0)
            {
                if ((fWert & 0x01) == 0x01)
                {
                    MainWindow.CommandCounter++;
                    MainWindow.numberOfCycles = 2;
                }
            }
            else if (bAsInt == 1)
            {
                if ((fWert & 0x02) == 0x02)
                {
                    MainWindow.CommandCounter++;
                    MainWindow.numberOfCycles = 2;
                }
            }
            else if (bAsInt == 2)
            {
                if ((fWert & 0x04) == 0x04)
                {
                    MainWindow.CommandCounter++;
                    MainWindow.numberOfCycles = 2;
                }
            }
            else if (bAsInt == 3)
            {
                if ((fWert & 0x08) == 0x08)
                {
                    MainWindow.CommandCounter++;
                    MainWindow.numberOfCycles = 2;
                }
            }
            else if (bAsInt == 4)
            {
                if ((fWert & 0x10) == 0x10)
                {
                    MainWindow.CommandCounter++;
                    MainWindow.numberOfCycles = 2;
                }
            }
            else if (bAsInt == 5)
            {
                if ((fWert & 0x20) == 0x20)
                {
                    MainWindow.CommandCounter++;
                    MainWindow.numberOfCycles = 2;
                }
            }
            else if (bAsInt == 6)
            {
                if ((fWert & 0x40) == 0x40)
                {
                    MainWindow.CommandCounter++;
                    MainWindow.numberOfCycles = 2;
                }
            }
            else if (bAsInt == 7)
            {
                if ((fWert & 0x80) == 0x80)
                {
                    MainWindow.CommandCounter++;
                    MainWindow.numberOfCycles = 2;
                }
            }
            else
            {
                MessageBox.Show("Problem BTFSS - b is not defined");
            }
            Registerspeicher.setRegisterWert(fAsByte, result);
        }

        public static void sleep()
        {
            Registerspeicher.speicher[Registerspeicher.STATUS] &= 0xF7;   // PD wird auf 0 gesetzt;
            Registerspeicher.labels[Registerspeicher.STATUS].Content = Registerspeicher.speicher[Registerspeicher.STATUS].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters

            Registerspeicher.speicher[Registerspeicher.STATUS] |= 0x10;   // TO wird auf 1 gesetzt;
            Registerspeicher.labels[Registerspeicher.STATUS].Content = Registerspeicher.speicher[Registerspeicher.STATUS].ToString("X2"); //X2 prints the string as two uppercase hexadecimal characters

        }



        private static byte ConvertStringToByte(string commandAsString, int startIndex, int length)
        {
            string myLiteral = commandAsString.Substring(2, 2); //Argument, Literal auslesen
            int myLiteralAsNum = Convert.ToInt32(myLiteral, 16); // string zu int konvertieren
            byte myLiteralAsByte = Convert.ToByte(myLiteralAsNum);
            return myLiteralAsByte;
        }

        private static byte CheckIndirectAddressing(byte fAsByteUnchecked)
        {
            if (fAsByteUnchecked == 0x00) //wenn f = 0 hole Adresse aus FSR Register
            {
                byte FSRAdresse = Registerspeicher.getRegisterWert(Registerspeicher.FSR);
                return FSRAdresse;
            }
            else //Ansonsten gebe das gleiche wieder zurück
            {
                return fAsByteUnchecked; 
            }
        }

    }
}
