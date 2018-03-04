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

namespace State_Machine2
{
    enum States { title, open, indexNum, indexLet, close, Error};
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //try
                //{
                    using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
                    {
                        string line;
                        // Read and display lines from the file until the end of 
                        // the file is reached.
                        while ((line = sr.ReadLine()) != null)
                        {
                            StateMachine stm = new StateMachine(line, this);
                        }
                    }
                //}
                //catch (Exception ex)
                //{
                //    // Let the user know what went wrong.
                //    Console.WriteLine("The file could not be read:");
                //    Console.WriteLine(ex.Message);
                //}
            }
        }
    }

    internal class StateMachine
    {
        RichTextBox rtb;
        private States state;
        private string curStr;
        private Form1 myForm;

        public StateMachine(string str, Form1 form)
        {
            myForm = form;
            foreach (Control c in myForm.Controls)
            {
                if (c.Name == "richTextBox1")
                {
                    rtb = c as RichTextBox;
                }
            }
            curStr = str;            
            state = States.title;
            Calculate();
            PrintResult();
        }

        private void SetState(States st)
        {
            state = st;
        }

        private bool isLetterOrNumber(char c)
        {
            int a = (int)c;
            if ((a >= 97 && a <= 122) || (a >= 65 && a <= 90) || (a >= 48 && a <= 57))
                return true;
            else
                return false;
        }

        private bool isLetter(char c)
        {
            int a = (int)c;
            if ((a >= 97 && a <= 122) || (a >= 65 && a <= 90))
                return true;
            else
                return false;
        }

        private bool isNumber(char c)
        {
            int a = (int)c;
            if (a >= 48 && a <= 57)
                return true;
            else
                return false;
        }

        private void Calculate()
        {
            foreach (char c in curStr)
            {
                switch (state)
                {
                    case States.title:
                        if (isLetterOrNumber(c)) ;
                        else if ((int)c == 91)
                            SetState(States.open);
                        else
                            SetState(States.Error);
                        break;
                    case States.open:
                        if (isLetter(c))
                            SetState(States.indexLet);
                        else if (isNumber(c))
                            SetState(States.indexNum);
                        else
                            SetState(States.Error);
                        break;
                    case States.indexLet:
                        if (isLetter(c))
                            SetState(States.indexLet);
                        else if ((int)c==93)
                            SetState(States.close);
                        else
                            SetState(States.Error);
                        break;
                    case States.indexNum:
                        if (isNumber(c))
                            SetState(States.indexNum);
                        else if ((int)c == 93)
                            SetState(States.close);
                        else
                            SetState(States.Error);
                        break;
                    case States.close:
                        if ((int)c==91)
                            SetState(States.open);
                        else if (c==' ')
                            SetState(States.title);
                        else
                            SetState(States.Error);
                        break;
                }
            }
            if (state != States.close)
                SetState(States.Error);
        }

        private void PrintResult()
        {
            if (state != States.Error)
            {
                rtb.AppendText(curStr + "Everything is Ok!\n");
                rtb.ScrollToCaret();
            }
            else
            {
                rtb.AppendText(curStr + "Error!!!\n");
                rtb.ScrollToCaret();
            }


        }
    }
}
