using System;
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
        string sampleText = string.Empty;
        string userString = string.Empty;
        Run building = new Run(string.Empty);
        LinkedList<Run> runs = new LinkedList<Run>();

        Brush Good = Brushes.LightGreen;
        Brush Bad = Brushes.LightSalmon;

        public TestTextControl()
        {
            InitializeComponent();

            // key processing events
            testTextBox.TextInput += new TextCompositionEventHandler(testText_PreviewTextInput);
            testTextBox.PreviewKeyDown += new KeyEventHandler(testTextBox_PreviewKeyDown);

            // get the text the user will type
            sampleText = untouched.Text;

            building.Background = Good;

            //PWrapper.Inlines.Add(new Run("hello world"));

            //this.PreviewKeyDown += new KeyEventHandler(keyPressed);
            /*
            foreach( object o in LogicalTreeHelper.GetChildren(testTextBlock))
            {
                if (o is Run)
                {
                    var r = (Run)o;
                }
            }*/
        }

        protected void updateParagraph()
        {
            PWrapper.Inlines.Clear();

            foreach (Run r in runs)
            {
                PWrapper.Inlines.Add(r);
            }
            PWrapper.Inlines.Add(building);
            PWrapper.Inlines.Add(untouched);
        }

        protected void processUserString()
        {
            Console.WriteLine(userString);
            untouched.Text = sampleText.Substring(Math.Min(sampleText.Length, userString.Length));
            updateParagraph();
        }

        protected void addCharacter(string c)
        {
            userString += c;

            if (userString.Length > sampleText.Length)
            {
                // BAD
                c = " ";
                if (building.Background == Bad)
                {
                    building.Text += c;
                }
                else
                {
                    runs.AddLast(building);
                    building = new Run(c);
                    building.Background = Bad;
                }
            }
            else
            {
                c = "" + sampleText[userString.Length - 1];
                if (userString[userString.Length - 1] == sampleText[userString.Length - 1])
                {
                    // GOOD
                    if (building.Background == Good)
                    {
                        building.Text += c;
                    }
                    else
                    {
                        runs.AddLast(building);
                        building = new Run(c);
                        building.Background = Good;
                    }
                }
                else
                {
                    // BAD
                    if (building.Background == Bad)
                    {
                        building.Text += c;
                    }
                    else
                    {
                        runs.AddLast(building);
                        building = new Run(c);
                        building.Background = Bad;
                    }
                }
            }

            processUserString();
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

                processUserString();
            }
        }

        protected void testText_PreviewTextInput(Object sender, TextCompositionEventArgs e)
        {
            addCharacter(e.Text);
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
    }
}
