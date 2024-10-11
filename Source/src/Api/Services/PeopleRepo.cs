using Microsoft.EntityFrameworkCore;
using OneStream.Api.Data;
using OneStream.Api.DataObjects;
using OneStream.Api.Services.Abstractions;

namespace OneStream.Api.Services
{
    /// <summary>
    /// Repository for the people.
    /// </summary>
    /// <param name="dbDbContext"></param>
    public class PeopleRepo(ApplicationDbContext dbDbContext) : IPeopleRepo
    {
        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public async Task<bool> UpdatePersonAsync(Guid id, EditPersonDto person)
        {
            if (id == Guid.Empty)
                return false;

            var found = await dbDbContext.People.FirstOrDefaultAsync(p => p.Id == id);
            if (found == null)
                return false;

            if (!string.IsNullOrEmpty(person.Name))
                found.Name = person.Name;
            if (!string.IsNullOrEmpty(person.Email))
                found.Email = person.Email;

            await dbDbContext.SaveChangesAsync();

            return true;
        }
    }
}
