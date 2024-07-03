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

        public async Task<List<Person>> Load()
        {
            await _localStorage.Initialize();

            var people = await _localStorage.LoadAll();

            if (people.Count == 0)
            {
                people.Add(new Person());
            }

            return people;
        }

        public Task Save(Person person)
        {
            if (person == null) throw new ArgumentNullException(nameof(person));

            return _localStorage.Save(person);
        }
    }
}
