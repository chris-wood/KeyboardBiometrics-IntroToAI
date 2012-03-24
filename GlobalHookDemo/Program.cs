using System;
using System.Windows.Forms;
using gma.System.Windows;
using System.Diagnostics;
using System.IO;            // 
		  

namespace GlobalHookDemo
{
    class MainForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.TextBox textBox;

        public MainForm()
        {
            InitializeComponent();
        }

        // THIS METHOD IS MAINTAINED BY THE FORM DESIGNER
        // DO NOT EDIT IT MANUALLY! YOUR CHANGES ARE LIKELY TO BE LOST
        void InitializeComponent()
        {
            this.textBox = new System.Windows.Forms.TextBox();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox.Font = new System.Drawing.Font("Courier New", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.textBox.Location = new System.Drawing.Point(4, 55);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(322, 340);
            this.textBox.TabIndex = 3;
            // 
            // buttonStop
            // 
            this.buttonStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStop.Location = new System.Drawing.Point(85, 3);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 1;
            this.buttonStop.Text = "Stop";
            this.buttonStop.Click += new System.EventHandler(this.ButtonStopClick);
            // 
            // buttonStart
            // 
            this.buttonStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStart.Location = new System.Drawing.Point(4, 3);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "Start";
            this.buttonStart.Click += new System.EventHandler(this.ButtonStartClick);
            // 
            // MainForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(328, 398);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonStart);
            this.Name = "MainForm";
            this.Text = "This application captures keystrokes";
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        
        // StreamWriter object for output log
        private static StreamWriter writer;
        [STAThread]
        public static void Main(string[] args)
        {
            string logfile;
            // was there a filename specified?
            logfile = args.Length == 0 ? "log" : args[0];
            writer = new StreamWriter(logfile);
            writer.WriteLine("Log Created on: " + DateTime.Now.ToShortDateString() + 
                " at " + DateTime.Now.ToShortTimeString() );
            writer.WriteLine("\nHH:mm:ss:ffff -- Event -- Decimal");
            
            Application.Run(new MainForm());
            writer.Close();

        }

        void ButtonStartClick(object sender, System.EventArgs e)
        {
            actHook.Start();
        }

        void ButtonStopClick(object sender, System.EventArgs e)
        {
            actHook.Stop();
        }


        UserActivityHook actHook;
        void MainFormLoad(object sender, System.EventArgs e)
        {
            actHook = new UserActivityHook(); // crate an instance with global hooks

            // hang on events
            actHook.KeyDown += new KeyEventHandler(MyKeyDown);
            actHook.KeyUp += new KeyEventHandler(MyKeyUp);
        }

        public void MyKeyDown(object sender, KeyEventArgs e)
        {
            LogWrite("KeyDown 	- " + e.KeyData.ToString());
            writer.WriteLine(DateTime.Now.ToString("HH:mm:ss:ffff") + "  KeyDown    " + e.KeyValue );
        }

        public void MyKeyUp(object sender, KeyEventArgs e)
        {
            LogWrite("KeyUp 		- " + e.KeyData.ToString());
            writer.WriteLine(DateTime.Now.ToString("HH:mm:ss:ffff") + "  KeyUp      " + e.KeyValue);
        }

        private void LogWrite(string txt)
        {
            textBox.AppendText(txt + Environment.NewLine);
            textBox.SelectionStart = textBox.Text.Length;
        }

    }
}
