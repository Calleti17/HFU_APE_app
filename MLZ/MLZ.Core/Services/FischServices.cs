using System.Collections.Generic;
using System.Threading.Tasks;
using MLZ.Core.Models;

namespace MLZ.Services
{
    public class FischService
    {
        private readonly FischDatabase _database;

        public FischService(FischDatabase database)
        {
            _database = database;
        }

        public Task<List<Fisch>> GetFischeAsync()
        {
            return _database.GetFischeAsync();
        }

        public Task<Fisch> GetFischAsync(int id)
        {
            return _database.GetFischAsync(id);
        }

        public Task<int> SaveFischAsync(Fisch fisch)
        {
            return _database.SaveFischAsync(fisch);
        }

        public Task<int> DeleteFischAsync(Fisch fisch)
        {
            return _database.DeleteFischAsync(fisch);
        }
    }
}
