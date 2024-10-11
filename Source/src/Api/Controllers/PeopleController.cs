using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneStream.Api.DataObjects;
using OneStream.Api.Services.Abstractions;

namespace OneStream.Api.Controllers
{
    /// <summary>
    /// Simplistic CRUD controller for managing people.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class PeopleController(ILogger<PeopleController> logger, IPeopleRepo peopleRepo) : ControllerBase
    {
        private readonly ILogger _logger = logger;

        /// <summary>
        /// Retrieve people that have been enrolled.
        /// </summary>
        /// <returns>An array of <see cref="PersonDto">PersonDto[]</see></returns>
        [HttpGet(Name = "GetPeople")]
        public async Task<IActionResult> GetPeople()
        {
            try
            {
                var result = await peopleRepo.GetPeopleAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        /// <summary>
        /// Add a person to the list of people.
        /// </summary>
        /// <param name="request">The person to add</param>
        /// <returns>Returns a <see cref="CreatedAtRouteResult"/> with the <see cref="PersonDto">PersonDto</see> object if successful.</returns>
        [HttpPost(Name = "AddPerson")]
        public async Task<IActionResult> AddPerson([FromBody] PersonDto request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var result = await peopleRepo.AddPersonAsync(request);
                return CreatedAtRoute("GetPeople", new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding person.");

                return StatusCode(400, ex.Message);
            }
        }

        /// <summary>
        /// Delete a person from the list of people.
        /// </summary>
        /// <param name="id">The unique id of the person to remove.</param>
        /// <returns>Returns true if successful, false if not.</returns>
        [HttpDelete("{id}", Name = "DeletePerson")]
        public async Task<IActionResult> DeletePerson(Guid id)
        {
            try
            {
                var result = await peopleRepo.DeletePersonAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }

        /// <summary>
        /// Update a person in the list of people.
        /// </summary>
        /// <param name="id">The unique id of the person to remove.</param>
        /// <param name="request">The <see cref="EditPersonDto">PersonDto</see> with the parameters to change.</param>
        /// <returns>Returns true if successful, false if not.</returns>
        [HttpPut("{id}", Name = "UpdatePerson")]
        public async Task<IActionResult> UpdatePerson(Guid id, [FromBody] EditPersonDto request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var result = await peopleRepo.UpdatePersonAsync(id, request);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
    }
}
