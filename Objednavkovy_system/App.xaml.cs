using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Objednavkovy_system
{
    /// <summary>
    /// Interakční logika pro App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static TodoDatabase _database;
        public static TodoDatabase Database
        {
            get
            {
                if (_database == null)
                {
                    var fileHelper = new FileHelper();
                    _database = new TodoDatabase(fileHelper.GetLocalFilePath("Offline_backup.db3"));
                }
                return _database;
            }
        }
    }
}
