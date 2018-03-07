using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        public Task<List<Game>> GetGames()
        {
            return database.Table<Game>().ToListAsync();
        }
        /*public Task<int> SaveGame(Game item)
        {
            //MessageBox.Show("Ulozen " + item.Name);
            if (item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }*/
        public Task<int> SaveGame(Game item)
        {
            //MessageBox.Show("Ulozen " + item.Name);
            return database.InsertOrReplaceAsync(item);
        }
    }
}
