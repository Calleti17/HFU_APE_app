using Core.Models;

namespace Core.Services
{
    public class PersonService : IPersonService
    {
        private readonly ILocalStorage _localStorage;

        public PersonService(ILocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        public List<Person> Load()
        {
            throw new NotImplementedException();
        }

        public void Save(Person person)
        {
            if (person == null) throw new ArgumentNullException(nameof(person));

            _localStorage.SavePerson(person);
        }
    }
}
