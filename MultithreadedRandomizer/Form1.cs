using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Threading;


namespace MultithreadedRandomizer
{
    public partial class Form1 : Form
    {
        ThreadsManager threadsManager;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            threadsManager = new ThreadsManager();

            threadsManager.bindListView(listView1);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (isValid(threadsAmount.Text))
                start.Enabled = true;
            else
                start.Enabled = false;
        }

        private void start_Click(object sender, EventArgs e)
        {
            start.Enabled = false;
            stop.Enabled = true;
            threadsAmount.Enabled = false;

            threadsManager.activateGeneration();

            threadsManager.createThreads(int.Parse(threadsAmount.Text));

        }
        private void stop_Click(object sender, EventArgs e)
        {
            start.Enabled = true;
            stop.Enabled = false;
            threadsAmount.Enabled = true;
            threadsManager.deactivateGeneration();
        }


        ///***********************************************
        /// Functions 
        ///***********************************************

        ///*******************
        /// TextBox validation
        /// 

        public bool isValid(string text)
        {
            int textBoxValue;
            if (!string.IsNullOrEmpty(text))
            {
                if (!int.TryParse(threadsAmount.Text, out textBoxValue))
                {
                    validator.Text = "Value must be a number!";
                    return false;
                }
                else
                {
                    if (textBoxValue < 2)
                    {
                        validator.Text = "Value must be more than 2!";
                        return false;
                    }
                    else if (textBoxValue > 15)
                    {
                        validator.Text = "Value must be less than 16!";
                        return false;
                    }
                    else
                    {
                        validator.Text = "";
                        return true;
                    }
                }
            }
            else
            {
                validator.Text = "";
                return false;
            }
        }
    }
}
