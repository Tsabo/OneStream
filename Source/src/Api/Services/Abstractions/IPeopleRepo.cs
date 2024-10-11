using OneStream.Api.DataObjects;

namespace OneStream.Api.Services.Abstractions
{
    /// <summary>
    /// Interface for the people repository.
    /// </summary>
    public interface IPeopleRepo
    {
        /// <summary>
        /// Get all people
        /// </summary>
        /// <returns>An array of people</returns>
        Task<PersonDto[]> GetPeopleAsync();

        /// <summary>
        /// Add a person to the list of people.
        /// </summary>
        /// <param name="person">The person to add</param>
        /// <returns>Returns the inserted person</returns>
        Task<PersonDto> AddPersonAsync(PersonDto person);

        /// <summary>
        /// Delete a person from the list of people.
        /// </summary>
        /// <param name="id">The unique id of the person to delete.</param>
        /// <returns>True if successful, false if not.</returns>
        Task<bool> DeletePersonAsync(Guid id);

        /// <summary>
        /// Update a person in the list of people.
        /// </summary>
        /// <param name="id">The unique id of the person to modify.</param>
        /// <param name="person">The parameters update. If a parameter is null then it will not be modified.</param>
        /// <returns>True if successful, false if not.</returns>
        Task<bool> UpdatePersonAsync(Guid id, EditPersonDto person);
    }
}
