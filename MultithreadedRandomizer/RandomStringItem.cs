using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadedRandomizer
{
    struct RandomStringItem
    {
        int id;
        DateTime currentTime;
        string randomString;

        public RandomStringItem(int id)
        {
            this.id = id;
            currentTime = DateTime.Now;
            randomString = "";
        }

        public RandomStringItem(int id, DateTime currentTime, string randomString)
        {
            this.id = id;
            this.currentTime = currentTime;
            this.randomString = randomString;
        }

        public string getRandomString()
        {
            return randomString;
        }

        public void updateItem(DateTime now, string newString)
        {
            currentTime = now;
            randomString = newString;
        }

        public int getId()
        {
            return id;
        }

    }
}
