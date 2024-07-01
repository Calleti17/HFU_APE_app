using Core.Models;

namespace Core.Services
{
    public interface IPersonService
    {
        void Save(Person person);
        List<Person> Load(); 
    }
}
