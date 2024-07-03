using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using MLZ.Core.Models;

namespace MLZ.Services
{
    public class FischDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public FischDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Fisch>().Wait();
        }

        public Task<List<Fisch>> GetFischeAsync()
        {
            return _database.Table<Fisch>().ToListAsync();
        }

        public Task<Fisch> GetFischAsync(int id)
        {
            return _database.Table<Fisch>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveFischAsync(Fisch fisch)
        {
            if (fisch.Id != 0)
            {
                return _database.UpdateAsync(fisch);
            }
            else
            {
                return _database.InsertAsync(fisch);
            }
        }

        public Task<int> DeleteFischAsync(Fisch fisch)
        {
            return _database.DeleteAsync(fisch);
        }
    }
}
