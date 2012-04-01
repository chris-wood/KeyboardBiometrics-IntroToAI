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
        string userString = string.Empty;
        LinkedList<Run> runs = new LinkedList<Run>();

        public TestTextControl()
        {
            InitializeComponent();

            testTextBox.TextInput += new TextCompositionEventHandler(testText_PreviewTextInput);
            testTextBox.PreviewKeyDown += new KeyEventHandler(testTextBox_PreviewKeyDown);

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

        protected void processUserString()
        {
            Console.WriteLine(userString);
        }

        protected void testText_PreviewTextInput(Object sender, TextCompositionEventArgs e)
        {
            userString += e.Text;
            processUserString();
        }


        protected void testTextBox_PreviewKeyDown(Object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    userString += " ";
                    processUserString();
                    break;
                case Key.Back:
                    if (userString.Length > 0)
                    {
                        userString = userString.Substring(0, -2);
                        processUserString();
                    }
                    break;
            }
        }
    }
}
