using OneStream.Api.DataObjects;

namespace OneStream.Api.Services.Abstractions
{
    public interface IPeopleRepo
    {
        Task<PersonDto[]> GetPeopleAsync();
    }
}
