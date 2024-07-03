using SQLite;
using Core.Services;
using MLZ.Core.Models;

namespace MLZ.Core.Services
{
    public class SqliteLocalStorage : ILocalStorage
    {
        private readonly SQLiteAsyncConnection _connection;

        public SqliteLocalStorage(LocalStorageSettings settings)
        {
            _connection = new SQLiteAsyncConnection(settings.DatabasePath);
            Initialize().ConfigureAwait(false);
        }

        public async Task Initialize()
        {
            await _connection.CreateTableAsync<Fisch>();
        }

        public async Task<bool> Delete(Fisch fisch)
        {
            return await _connection.DeleteAsync(fisch) == 1;
        }

        public Task<List<Fisch>> LoadAll()
        {
            return _connection.Table<Fisch>().ToListAsync();
        }

        public async Task<bool> DeleteAll()
        {
            return await _connection.DeleteAllAsync<Fisch>() > 0;
        }

        public async Task<bool> Save(Fisch fisch)
        {
            return await _connection.InsertOrReplaceAsync(fisch) == 1;
        }

        public async Task<Fisch?> TryLoad(int id)
        {
            return await _connection.FindAsync<Fisch?>(id);
        }
    }
}
