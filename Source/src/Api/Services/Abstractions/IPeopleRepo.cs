using OneStream.Api.DataObjects;

namespace OneStream.Api.Services.Abstractions
{
    public interface IPeopleRepo
    {
        Task<PersonDto[]> GetPeopleAsync();

        Task<PersonDto> AddPersonAsync(PersonDto person);

        Task<bool> DeletePersonAsync(Guid id);

        Task<bool> UpdatePersonAsync(Guid id, PersonDto person);
    }
}
