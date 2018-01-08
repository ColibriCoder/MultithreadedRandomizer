using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MultithreadedRandomizer
{
    struct DatabaseInfo
    {
        public static string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + System.Environment.CurrentDirectory + @"\App_data\randomStrings.mdb";
    }
}
