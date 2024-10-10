using Microsoft.EntityFrameworkCore;
using OneStream.Api.Data;
using OneStream.Api.DataObjects;
using OneStream.Api.Services.Abstractions;

namespace OneStream.Api.Services
{
    public class PeopleRepo : IPeopleRepo
    {
        private readonly ApplicationDbContext _dbContext;

        public PeopleRepo(ApplicationDbContext dbDbContext)
        {
            _dbContext = dbDbContext;
        }

        public async Task<PersonDto[]> GetPeopleAsync()
        {
            var result = await _dbContext.People
                .Select(p => new PersonDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Email = p.Email
                })
                .ToArrayAsync();

            return result;
        }
    }
}
