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
                return StatusCode(500, ex.Message);
            }
        }

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
            catch (Exception e)
            {
                _logger.LogError(e, "Error adding person.");

                return BadRequest(e);
            }
        }

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
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}", Name = "UpdatePerson")]
        public async Task<IActionResult> UpdatePerson(Guid id, [FromBody] PersonDto request)
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
                return StatusCode(500, ex.Message);
            }
        }
    }
}
