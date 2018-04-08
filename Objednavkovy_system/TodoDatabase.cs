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
            database.CreateTableAsync<Cart_Item>().Wait();
        }
        public Task<List<Game>> GetGames()
        {
            return database.Table<Game>().ToListAsync();
        }
        public Task<List<Cart_Item>> GetItems()
        {
            return database.Table<Cart_Item>().ToListAsync();
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
            return database.InsertOrReplaceAsync(item);
        }
        public Task<int> SaveCart(List<Cart_Item> l) 
        {
            return database.InsertAllAsync(l);   
        }
        public Task<int> Delete(Cart_Item item)
        {
            return database.DeleteAsync(item);
        }
    }
}
