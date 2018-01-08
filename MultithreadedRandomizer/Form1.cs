using System;
using System.Collections.Generic;
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
        DatabaseManager databaseManager;
        //OleDbConnection connection = new OleDbConnection();
        const string allowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz#@$^*()";
        static Random rnd = new Random();
        bool generatorActive = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            databaseManager = new DatabaseManager(DatabaseInfo.path);

            Exception ex;
            ex = databaseManager.checkConnection();
            if (ex != null)
                MessageBox.Show("Error connecting database: " + ex);

            /*
            try
            {
                connection.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Colibri\source\repos\MultithreadedRandomizer\MultithreadedRandomizer\MultithreadedRandomizer\App_data\randomStrings.mdb";
                connection.Open();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting db: " + ex);
            }
            */
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int treadsValue;
            start.Enabled = false;
            if (!string.IsNullOrEmpty(threadsAmount.Text))
            {
                if (!int.TryParse(threadsAmount.Text, out treadsValue))
                {
                    validator.Text = "Value must be a number!";
                }
                else
                {
                    if (treadsValue < 2)
                    {
                        validator.Text = "Value must be more than 2!";

                    }
                    else if (treadsValue > 15)
                    {
                        validator.Text = "Value must be less than 16!";

                    }
                    else
                    {
                        validator.Text = "";
                        start.Enabled = true;
                    }
                }
            }
            else
            {
                validator.Text = "";
            }
        }

        

        private void start_Click(object sender, EventArgs e)
        {
            start.Enabled = false;
            stop.Enabled = true;
            databaseManager.openConnction();
            Int32 threadsValue = Int32.Parse(threadsAmount.Text);

            generatorActive = true;


            for (int i = 0; i < threadsValue; i++)
            {        
                createThread();
            }

           
        }
        private void stop_Click(object sender, EventArgs e)
        {
            start.Enabled = true;
            stop.Enabled = false;
            databaseManager.closeConnection();
            generatorActive = false;
        }
        /// Functions 
        
        void createThread()
        {
            Thread t = new Thread(startGeneration);

            
            /*
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = DatabaseInfo.path;
            lock (connection) ;
            */

            t.Start();

            
        }
        
        void startGeneration()
        {

            ThreadSafeRandom tsr = new ThreadSafeRandom();
            int threadID = Thread.CurrentThread.ManagedThreadId;
            string generatedString;
            bool firstExecution = true;


            while (generatorActive)
            {
                if (firstExecution)
                {
                    firstExecution = false;
                    Thread.Sleep(tsr.Next(500, 2000));
                    if (!generatorActive)
                        Thread.CurrentThread.Abort();
                }

                generatedString = generateRandomString();

                databaseManager.addItem("INSERT INTO randomStrings (ThreadID, GenerationTime, Data) VALUES (" + threadID + ",'" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss") + "', '" + generatedString + "')");

                //rsi.updateItem(DateTime.Now, generateRandomString());
                ListViewItem item = new ListViewItem(threadID.ToString());

                if (listView1.InvokeRequired)
                {
                    listView1.Invoke(new MethodInvoker(delegate
                    {
                        item.SubItems.Add(generatedString);
                        listView1.Items.Add(item);
                    }));
                }
                else
                {
                    item.SubItems.Add(generatedString);
                    listView1.Items.Add(item);
                }

                
                Thread.Sleep(tsr.Next(500, 2000));

            }
            firstExecution = true;
            //con.Close();
            
        }

        internal static string generateRandomString()
        {
            int strLenght = rnd.Next(6, 16);
            char[] chars = new char[strLenght];
            for (int i = 0; i < strLenght; i++)
                chars[i] = allowedChars[rnd.Next(0, allowedChars.Length)];

            return new string(chars);
        }

        int generateRandomInt(int min, int max)
        {
            Random rnd = new Random();
            return rnd.Next(min, max);

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


    }

}
