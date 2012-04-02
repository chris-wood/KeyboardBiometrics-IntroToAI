using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeyCollectorGUI
{
    /// <summary>
    /// Interaction logic for UserControl2.xaml
    /// </summary>
    public partial class TestTextControl : UserControl
    {
        static Brush Good = Brushes.LightGreen;
        static Brush Bad = Brushes.LightSalmon;

        string sampleText = null;
        string userString = null;
        Run building = null;
        LinkedList<Run> runs = null;

        Logger logger = null;

        public TestTextControl()
        {
            InitializeComponent();

            testTextBox.Focus();

            // key processing events
            testTextBox.TextInput += new TextCompositionEventHandler(testText_PreviewTextInput);
            testTextBox.PreviewKeyDown += new KeyEventHandler(testTextBox_PreviewKeyDown);

            // get the text the user will type
            sampleText = untouched.Text;

            // start the keylogger
            logger = new Logger();

            // initialize the highlight state
            reset();

            // catch the exit event
            Application.Current.MainWindow.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);
        }

        ~TestTextControl()
        {
            // remove the exit event handler
            if (logger != null)
            {
                logger.stop();
                logger = null;
            }
            //Application.Current.MainWindow.Closing -= new System.ComponentModel.CancelEventHandler(MainWindow_Closing);
        }

        protected void reset()
        {
            // initialize the list of runs and the building run
            runs = new LinkedList<Run>();
            building = new Run(string.Empty);
            building.Background = Good;

            // initialize the userString
            userString = string.Empty;

            // clear the log
            logger.reset();

            // reflect the changes in the view
            updateParagraph();
        }

        protected void updateParagraph()
        {
            untouched.Text = sampleText.Substring(Math.Min(sampleText.Length, userString.Length));

            PWrapper.Inlines.Clear();

            foreach (Run r in runs)
            {
                PWrapper.Inlines.Add(r);
            }
            PWrapper.Inlines.Add(building);
            PWrapper.Inlines.Add(untouched);
        }

        protected void addCharacter(string c)
        {
            userString += c;
            Brush runType = null;

            if (userString.Length > sampleText.Length)
            {
                c = " ";
                runType = Bad;
            }
            else
            {
                c = sampleText[userString.Length - 1].ToString();
                if (userString[userString.Length - 1] == sampleText[userString.Length - 1])
                {
                    runType = Good;
                }
                else
                {
                    runType = Bad;
                }
            }

            if (runType != null)
            {
                if (building.Background == runType)
                {
                    building.Text += c;
                }
                else
                {
                    runs.AddLast(building);
                    building = new Run(c);
                    building.Background = runType;
                }
            }

            updateParagraph();
        }

        protected void deleteCharacter()
        {
            if (userString.Length >= 1)
            {
                userString = userString.Substring(0, userString.Length - 1);

                if (building.Text.Length == 0 && runs.Count > 0)
                {
                    building = runs.Last();
                    runs.RemoveLast();
                }

                if (building.Text.Length > 0)
                {
                    building.Text = building.Text.Substring(0, building.Text.Length - 1);
                }

                updateParagraph();
            }
        }

        protected void testText_PreviewTextInput(Object sender, TextCompositionEventArgs e)
        {
            if (e.Text != Convert.ToChar(27).ToString())    // ESCAPE should not be a character
            {
                addCharacter(e.Text);
            }
        }

        protected void testTextBox_PreviewKeyDown(Object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    addCharacter(" ");
                    break;
                case Key.Tab:
                    addCharacter("\t");
                    break;
                case Key.Back:
                    deleteCharacter();
                    break;
            }
        }

        protected void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (logger != null)
            {
                logger.stop();
                logger = null;
            }
        }

        private bool log_saveas()
        {
            // configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Keylog";    // default file name
            dlg.DefaultExt = ".log";    // default file extension
            dlg.Filter = "Log files|*.log|Text Documents|*.txt|All Files|*.*";  // file selection filters

            // show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // process save file dialog box results
            if (result == true)
            {
                // save the log
                using (StreamWriter logWriter = new StreamWriter(dlg.FileName))
                {
                    logWriter.Write(logger.Log);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            if (log_saveas())
            {
                Application.Current.MainWindow.Close();
            }
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            reset();
            testTextBox.Focus();
        }
    }
}
