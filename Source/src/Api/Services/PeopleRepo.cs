using Microsoft.EntityFrameworkCore;
using OneStream.Api.Data;
using OneStream.Api.DataObjects;
using OneStream.Api.Services.Abstractions;

namespace OneStream.Api.Services
{
    public class PeopleRepo(ApplicationDbContext dbDbContext) : IPeopleRepo
    {
        public async Task<PersonDto[]> GetPeopleAsync()
        {
            var result = await dbDbContext.People
                .Select(p => new PersonDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Email = p.Email
                })
                .ToArrayAsync();

            return result;
        }

        public async Task<PersonDto> AddPersonAsync(PersonDto person)
        {
            if (person.Id == Guid.Empty)
                person.Id = Guid.NewGuid();

            dbDbContext.People.Add(new Person
            {
                Id = person.Id,
                Name = person.Name,
                Email = person.Email
            });

            await dbDbContext.SaveChangesAsync();

            return person;
        }

        public async Task<bool> DeletePersonAsync(Guid id)
        {
            if (id == Guid.Empty)
                return false;

            var found = await dbDbContext.People.FirstOrDefaultAsync(p => p.Id == id);
            if (found == null)
                return false;

            dbDbContext.People.Remove(found);

            await dbDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdatePersonAsync(Guid id, PersonDto person)
        {
            if (id == Guid.Empty)
                return false;

            var found = await dbDbContext.People.FirstOrDefaultAsync(p => p.Id == id);
            if (found == null)
                return false;

            found.Name = person.Name;
            found.Email = person.Email;

            await dbDbContext.SaveChangesAsync();

            return true;
        }
    }
}
