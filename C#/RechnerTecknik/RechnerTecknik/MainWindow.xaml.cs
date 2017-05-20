using System;
using System.Collections.Generic;
using System.IO;
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

namespace RechnerTecknik
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> myCommandList = new List<string>(); //CommandListe alle Commands
        string myCommand =string.Empty; //Variable für einzelne Commands

        public static int numberOfCycles = 0; //Variable für ein 1 oder 2 Cycle Befeht

        private static int commandCounter = 0; //ProgrammCounter
        public static int CommandCounter
        {
          get {return commandCounter;}
          set {commandCounter = value;}
        }

        public MainWindow()
        {
            InitializeComponent();
            LoadSpeicherGrid();
            Registerspeicher.initializeRegister();
        }

        private void LoadSpeicherGrid()
        {
            for (int i = 0; i < 256; i++)
            {
                Registerspeicher.labels[i] = new Label();
                Registerspeicher.labels[i].SetValue(Grid.ColumnProperty, i % 8);
                Registerspeicher.labels[i].SetValue(Grid.RowProperty, i / 8);
                Registerspeicher.labels[i].Content = "00";
                this.SpeicherGrid.Children.Add(Registerspeicher.labels[i]);
            }
        }


        private void helpButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\Users\Sara\Projekt\C#\Documentation.pdf");
        }


        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            myCommandList.Clear();
            Stack.myStack.Clear();
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dialog.DefaultExt = ".lst";
            dialog.Filter = "Test documents (.lst)|*.lst";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dialog.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dialog.FileName;
                browseBox.Text = filename;

                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(System.IO.File.ReadAllText(filename));

                FlowDocument document = new FlowDocument(paragraph);
                flowDocReader.Document = document;

                // Open the file to read from.
                string[] readText = File.ReadAllLines(filename);
                foreach (string zeile in readText)
                {
                    if (!zeile.StartsWith(" "))
                    {
                        this.myCommandList.Add(zeile.Substring(5, 4));
                    }
                }
            }           
        }


        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (commandCounter == 0)
            {
                try
                {
                    this.myCommand = myCommandList.First();
                }
                catch (Exception)
                {
                    MessageBox.Show("Command not found");
                }
                ExecutingCommandTextBlock.Text = this.myCommand;

                commandCounter++;
                TIMER0.TimerCounter++; //externe Variable für TIMER0 wird hochgezählt

                ExecuteCommand(this.myCommand);
                CommandCounterLabel.Content = commandCounter; //Commandcounter wird ausgegeben
            }
            else
            {
                MessageBox.Show("Programm already started");
            }
        }


        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.myCommand = myCommandList.ElementAt(commandCounter);
            }
            catch (Exception)
            {
                MessageBox.Show("End of Commands");
            }
            ExecutingCommandTextBlock.Text = this.myCommand;

            commandCounter++;
            TIMER0.TimerCounter++; //externe Variable für TIMER0 wird hochgezählt

            ExecuteCommand(this.myCommand);
            CommandCounterLabel.Content = commandCounter; //Commandcounter wird ausgegeben
        }


        private void ExecuteCommand(string CommandToExecute)
        {
            CommandHandler commandHandler= new CommandHandler();


            int commandAsNum = Convert.ToInt32(CommandToExecute, 16);
            commandHandler.ExecuteCommand(commandAsNum, CommandToExecute);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Registerspeicher.initializeRegister();
            commandCounter = 0;

            ExecutingCommandTextBlock.Text = "start over again";
            CommandNameLabel.Content = "reseted";
            CommandCounterLabel.Content = commandCounter;

            Registerspeicher.W = 0;
            TIMER0.TimerCounter = 0; //reset, auf null gesetzt

        }

        private async void GoButton_Click(object sender, RoutedEventArgs e)
        {
            int BreakPosition = Convert.ToInt32(GoTextBox.Text);
            while (commandCounter <= BreakPosition)
            {
                numberOfCycles = 1;
                    try
                    {
                        this.myCommand = myCommandList.ElementAt(commandCounter);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("End of Commands");
                    }
                    ExecutingCommandTextBlock.Text = this.myCommand;
                    commandCounter++;
                    TIMER0.TimerCounter++; //externe Variable für TIMER0 wird hochgezählt

                    ExecuteCommand(this.myCommand);
                    CommandCounterLabel.Content = commandCounter;
                if (numberOfCycles == 2)
                {
                    await Task.Delay(getFrequenz());
                }
                await Task.Delay(getFrequenz());
            }
        }

        private int getFrequenz()
        {
            float QuarzFrequenz = float.Parse(freqBox.Text); //Quarzfrequenz auslesen
            float MicroSekunden = 4 * (1 / QuarzFrequenz); //Berechnen der Microsekunden
            float MilliSekunden = MicroSekunden / 1000; //Umrechnen in Millisekunden
            int MilliSekundenGerundet = Convert.ToInt32(MilliSekunden); //Rückgabewert muss int sein
            return MilliSekundenGerundet;
        }
    }
}
