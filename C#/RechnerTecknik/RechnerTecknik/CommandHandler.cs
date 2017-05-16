using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RechnerTecknik
{
    public class CommandHandler
    {
        //damit Labels vom MainWindow benützt werden können
        static MainWindow mainWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;

        public void ExecuteCommand(int commandAsNum, string commandAsString)
        {
            string myCom = FindOutCommand(commandAsNum, commandAsString);
            mainWin.CommandNameLabel.Content = myCom;
        }

        private string FindOutCommand(int commandToExecuteAsNum, string commandAsString)
        {
            if ((commandToExecuteAsNum & 0x3800) == 0x2000) //CALL
            {
                Commands.CALL(commandAsString);
                return "CALL";
            }
            else if ((commandToExecuteAsNum & 0x3800) == 0x2800) // GOTO
            {
                Commands.GOTO(commandAsString);
                return "GOTO";
            }
            else if ((commandToExecuteAsNum & 0x3FFF) == 0x0008) //RETURN
            {
                Commands.RETURN();
                return "RETURN";
            }
            else if ((commandToExecuteAsNum & 0x3F00) == 0x0700) //ADDWF
            {
                Commands.ADDWF(commandAsString);
                return "ADDWF";
            }
            else if ((commandToExecuteAsNum & 0x3F00) == 0x0500) //ANDWF
            {
                Commands.ANDWF(commandAsString);
                return "ANDWF";
            }
            else if ((commandToExecuteAsNum & 0x3F80) == 0x0180) //CLRF
            {
                Commands.CLRF(commandAsString);
                return "CLRF";
            }
            else if ((commandToExecuteAsNum & 0x3F80) == 0x0100) //CLRW
            {
                Commands.CLRW(commandAsString);
                return "CLRW";
            }
            else if ((commandToExecuteAsNum & 0x3F00) == 0x0900) //COMF
            {
                Commands.COMF(commandAsString);
                return "COMF";
            }
            else if ((commandToExecuteAsNum & 0x3F00) == 0x0300) //DECF
            {
                Commands.DECF(commandAsString);
                return "DECF";
            }
            else if ((commandToExecuteAsNum & 0x3F00) == 0x0B00) //DECFSZ
            {
                Commands.DECFSZ(commandAsString);
                return "DECFSZ";
            }
            else if ((commandToExecuteAsNum & 0x3F00) == 0x0A00) //INCF
            {
                Commands.INCF(commandAsString);
                return "INCF";
            }
            else if ((commandToExecuteAsNum & 0x3F00) == 0x0F00) //INCFSZ
            {
                Commands.INCFSZ(commandAsString);   
                return "INCFSZ";
            }
            else if ((commandToExecuteAsNum & 0x3F00) == 0x0400) //IORWF
            {
                Commands.IORWF(commandAsString);
                return "IORWF";
            }
            else if ((commandToExecuteAsNum & 0x3F00) == 0x0800) //MOVF
            {
                Commands.MOVF(commandAsString);
                return "MOVF";
            }
            else if ((commandToExecuteAsNum & 0x3F80) == 0x0080) //MOVWF
            {
                Commands.MOVWF(commandAsString);
                return "MOVWF";
            }
            else if ((commandToExecuteAsNum & 0x3F80) == 0x0000) //NOP
            {
                Commands.NOP();
                return "NOP";
            }
            else if ((commandToExecuteAsNum & 0x3F00) == 0x0D00) //RLF
            {
                Commands.RLF(commandAsString);
                return "RLF";
            }
            else if ((commandToExecuteAsNum & 0x3F00) == 0x0C00) //RRF
            {
                Commands.RRF(commandAsString);
                return "RRF";
            }
            else if ((commandToExecuteAsNum & 0x3F00) == 0x0200) //SUBWF
            {
                Commands.SUBWF(commandAsString);
                return "SUBWF";
            }
            else if ((commandToExecuteAsNum & 0x3F00) == 0x0E00) //SWAPF
            {
                Commands.SWAPF(commandAsString);
                return "SWAPF";
            }
            else if ((commandToExecuteAsNum & 0x3F00) == 0x0600) //XORWF
            {
                Commands.XORWF(commandAsString);
                return "XORWF";
            }
            else if ((commandToExecuteAsNum & 0x3C00) == 0x1000) //BCF
            {
                Commands.BCF(commandAsString);
                return "BCF";
            }
            else if ((commandToExecuteAsNum & 0x3C00) == 0x1400) //BSF
            {
                Commands.BSF(commandAsString);
                return "BSF";
            }
            else if ((commandToExecuteAsNum & 0x3C00) == 0x1800) //BTFSC
            {
                Commands.BTFSC(commandAsString);
                return "BTFSC";
            }
            else if ((commandToExecuteAsNum & 0x3C00) == 0x1C00) //BTFSS
            {
                Commands.BTFSS(commandAsString);
                return "BTFSS";
            }
            else if ((commandToExecuteAsNum & 0x3E00) == 0x3E00) //ADDLW
            {
                Commands.ADDLW(commandAsString);
                return "ADDLW";
            }
            else if ((commandToExecuteAsNum & 0x3F00) == 0x3900) //ANDLW
            {
                Commands.ANDLW(commandAsString);
                return "ANDLW";
            }
            else if ((commandToExecuteAsNum & 0x3FFF) == 0x0064)
            {
                //CLRWDT
                return "CLRWDT";
            }
            else if ((commandToExecuteAsNum & 0x3F00) == 0x3800) //IORLW
            {
                Commands.IORLW(commandAsString);
                return "IORLW";
            }
            else if ((commandToExecuteAsNum & 0x3C00) == 0x3000) //MOVLW
            {
                Commands.MOVLW(commandAsString);
                return "MOVLW";
            }
            else if ((commandToExecuteAsNum & 0x3FFF) == 0x0009)
            {
                //RETFIE
                return "RETFIE";
            }
            else if ((commandToExecuteAsNum & 0x3C00) == 0x3400) //RETLW
            {
                Commands.RETLW(commandAsString);
                return "RETLW";
            }
            
            else if ((commandToExecuteAsNum & 0x3FFF) == 0x0063)
            {
                //SLEEP
                return "SLEEP";
            }
            else if ((commandToExecuteAsNum & 0x3E00) == 0x3C00) //SUBLW
            {
                Commands.SUBLW(commandAsString);
                return "SUBLW";
            }
            else if ((commandToExecuteAsNum & 0x3F00) == 0x3A00) //XORLW
            {
                Commands.XORLW(commandAsString);
                return "XORLW";
            }
            else
            return "nothing";
        }
    }
}
