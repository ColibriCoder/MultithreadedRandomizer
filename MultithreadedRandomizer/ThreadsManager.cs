using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace MultithreadedRandomizer
{
    class ThreadsManager
    {
        DatabaseManager databaseManager;
        ListView destinationListView;

        private bool generatorActive = false;

        public ThreadsManager()
        {
            databaseManager = new DatabaseManager(DatabaseInfo.path);
            
            Exception ex;
            ex = databaseManager.checkConnection();
            if (ex != null)
                MessageBox.Show("Error connecting database: " + ex);

            databaseManager.openConnction();
        }

        public void bindListView(ListView listView)
        {
            destinationListView = listView;
        }

        public void activateGeneration()
        {
            generatorActive = true;
        }

        public void deactivateGeneration()
        {
            generatorActive = false;
        }

        public void createThreads(int threadsAmount)
        {
            for (int i = 0; i < threadsAmount; i++)
            {
                createThread();
            }
        }

        private void createThread()
        {
            Thread t = new Thread(startGeneration);
            t.Start();
        }

        void startGeneration()
        {
            Randomizer randomizer = new Randomizer();
            int threadID = Thread.CurrentThread.ManagedThreadId;
            string generatedString;
            bool firstExecution = true;

            while (generatorActive)
            {
                if (firstExecution)
                {
                    firstExecution = false;
                    Thread.Sleep(randomizer.GetRandomInt(500, 2000));
                    if (!generatorActive)
                    {
                        Thread.CurrentThread.Abort();
                        databaseManager.closeConnection();
                    }
                }

                generatedString = randomizer.generateRandomString(5, 10);

                databaseManager.addItem("INSERT INTO randomStrings (ThreadID, GenerationTime, Data) VALUES (" + threadID + ",'" + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss") + "', '" + generatedString + "')");

                ListViewItem item = new ListViewItem(threadID.ToString());

                if (destinationListView.InvokeRequired)
                {
                    destinationListView.Invoke(new MethodInvoker(delegate
                    {
                        if (destinationListView.Items.Count >= 20)
                            destinationListView.TopItem.Remove();

                        item.SubItems.Add(generatedString);
                        destinationListView.Items.Add(item);
                    }));
                }
                else
                {
                    item.SubItems.Add(generatedString);
                    destinationListView.Items.Add(item);
                }

                Thread.Sleep(randomizer.GetRandomInt(500, 2000));
            }
            firstExecution = true;
        }
    }
}
