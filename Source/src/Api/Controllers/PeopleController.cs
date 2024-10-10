using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OneStream.Api.DataObjects;
using OneStream.Api.Services.Abstractions;

namespace OneStream.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly IPeopleRepo _peopleRepo;

        public PeopleController(IPeopleRepo peopleRepo)
        {
            _peopleRepo = peopleRepo;
        }

        [HttpGet(Name = "GetPeople")]
        public async Task<IActionResult> GetPeople()
        {
            try
            {
                var result = await _peopleRepo.GetPeopleAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
