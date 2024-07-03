using MLZ.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MLZ.Core.Services
{
    public interface ILocalStorage
    {
        Task<Fisch?> TryLoad(int id);
        Task<bool> Save(Fisch fisch);
        Task<bool> Delete(Fisch fisch);
        Task<List<Fisch>> LoadAll();
        Task<bool> DeleteAll();
        Task Initialize();
    }
}
