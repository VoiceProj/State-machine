using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace State_Machine
{
    enum States { odd, even };

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StreamReader myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                    {
                        string line;
                        // Read and display lines from the file until the end of 
                        // the file is reached.
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line != "" && line != "\n")
                            {
                                StateMachine stm = new StateMachine(line, this);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Let the user know what went wrong.
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }

    public class StateMachine
    {
        RichTextBox rtb;
        private string curStr = "";
        private States state;
        private int count = 0;

        public StateMachine(string str, Form1 myForm)
        {
            foreach (Control c in myForm.Controls)
            {
                if (c.Name == "richTextBox1")
                {
                    rtb = c as RichTextBox;
                }
            }
            curStr = str;
            state = States.even;
            Calculate();
            PrintResult();
        }

        private void switchState()
        {
            if (state == States.even)
                state = States.odd;
            else state = States.even;
        }

        private void Calculate()
        {
            for (int i = 0; i < curStr.Length; i++)
            {
                if (i<curStr.Length-1)
                switch (state)
                {
                    case States.even:
                        if (curStr[i] == '0' && curStr[i + 1] == '0')
                        {
                            i += 1;
                            count++;
                            switchState();
                        }
                        break;
                    case States.odd:
                        if (curStr[i] == '0' && curStr[i + 1] == '0')
                        {
                            i += 1;
                            count++;
                            switchState();
                        }
                        break;
                }
            }
        }

        private void PrintResult()
        {
            if (state == States.even)
            {
                rtb.AppendText(curStr + "  Error! Number of pair '00' is even!\n");                
            }
            else
            {
                rtb.AppendText(curStr + "  Everything is Ok, relax!\n");
            }
        }
    }
}
