using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Threading;

namespace MultithreadedRandomizer
{
    public partial class Form1 : Form
    {
        OleDbConnection connection = new OleDbConnection();
        const string allowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz#@$^*()";
        static Random rnd = new Random();
        bool generatorActive = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
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
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            int treadsValue;
            if (!string.IsNullOrEmpty(threadsAmount.Text))
            {
                if (!int.TryParse(threadsAmount.Text, out treadsValue))
                {
                    validator.Text = "Value must be a number!";
                }
                else
                {
                    if (treadsValue < 2)
                        validator.Text = "Value must be more than 2!";
                    else if (treadsValue > 15)
                        validator.Text = "Value must be less than 16!";
                    else
                        validator.Text = "";
                }
            }
            else
            {
                validator.Text = "";
            }
        }

        

        private void start_Click(object sender, EventArgs e)
        {

            Int32 threadsValue = Int32.Parse(threadsAmount.Text);

            Thread[] threadsArray = new Thread[threadsValue];
            //RandomStringItem[] threadsItems = new RandomStringItem[threadsValue + 1];

            /*
            for (int i = 0; i < threadsValue; i++)
            {
                threadsItems[i] = new RandomStringItem(i + 1);
            }
            */

            generatorActive = true;

            for (int i = 0; i < threadsValue; i++)
            {
                threadsArray[i] = new Thread(() => startThredGeneration(i));
                threadsArray[i].Start();
            }
            /*
            Thread t = new Thread(print1);
            t.Start();

            connection.Open();
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            command.CommandText = "INSERT INTO randomStrings (GenerationTime) VALUES ('" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss") + "')";

            command.ExecuteNonQuery();
            MessageBox.Show("saved");
            connection.Close();
            */



            /*
            OleDbCommand command = new OleDbCommand();
            command.Connection = connection;
            command.CommandText = "INSERT INTO randomStrings (ThreadID, Time, Date) VALUES ('@ThreadID, @Time, @Date)";
            connection.Open();

            //int test = Int32.Parse(threadsAmount.Text);
            //DateTime dabar = DateTime.Now;
            //command.CommandText = "insert into randomStrings (ThreadID,Time,Date) values('1,1/11/2017,basd')";
            // command.CommandText = "insert into randomStrings (TreadID,Time,Date) values('" + test +"','" + dabar +"','" + "any text" + "')";

            command.Parameters.AddWithValue("@ThreadID", "1");
            command.Parameters.AddWithValue("@Time", "1/11/2017");
            command.Parameters.AddWithValue("@Date", "basd");


            int yy = command.ExecuteNonQuery();
            MessageBox.Show("Data saved");
            connection.Close();
            */
            /*
            command.CommandText = "select * from randomStrings where ID=1";
            OleDbDataReader reader = command.ExecuteReader();
            if (reader.Read())
                MessageBox.Show("parameter exists");
            else
                MessageBox.Show("No such parameter");

            */
            //connection.Close();
            /*
           int id = 0;
           DateTime dateNow = DateTime.Now;

           ListViewItem item = new ListViewItem(id.ToString());
           item.SubItems.Add(generateRandomString());
           listView1.Items.Add(item);
           */
        }
        private void stop_Click(object sender, EventArgs e)
        {
            generatorActive = false;
        }
        /// Functions
        /// 
        void startThredGeneration(int index)
        {
            RandomStringItem rsi = new RandomStringItem(index);
            while (generatorActive)
            {
                rsi.updateItem(DateTime.Now, generateRandomString());
                ListViewItem item = new ListViewItem(rsi.getId().ToString());

                if (listView1.InvokeRequired)
                {
                    listView1.Invoke(new MethodInvoker(delegate
                    {
                        item.SubItems.Add(rsi.getRandomString());
                        listView1.Items.Add(item);
                    }));
                }
                else
                {
                    item.SubItems.Add(rsi.getRandomString());
                    listView1.Items.Add(item);
                }
                Thread.Sleep(2000);
            }
        }

            /*
        void createTread(int id)
        {

            RandomStringItem thread = new RandomStringItem(id, DateTime.Now, generateRandomString());

            
        }
        */
        /*
        void addToList ()
        {
            ListViewItem item = new ListViewItem(id.ToString());

            if (listView1.InvokeRequired)
            {
                listView1.Invoke(new MethodInvoker(delegate
                {
                    item.SubItems.Add(thread.getRandomString());
                    listView1.Items.Add(item);
                }));
            }
            else
            {
                item.SubItems.Add(generateRandomString());
                listView1.Items.Add(item);
            }
        }
        */

        internal static string generateRandomString()
        {
            int strLenght = rnd.Next(6, 16);
            char[] chars = new char[strLenght];
            for (int i = 0; i < strLenght; i++)
                chars[i] = allowedChars[rnd.Next(0, allowedChars.Length)];

            return new string(chars);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        
    }
}
