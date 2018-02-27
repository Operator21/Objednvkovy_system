using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objednavkovy_system
{
    public class TodoDatabase
    {
        private SQLiteAsyncConnection database;
        public TodoDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Game>().Wait();
        }
    }
}
