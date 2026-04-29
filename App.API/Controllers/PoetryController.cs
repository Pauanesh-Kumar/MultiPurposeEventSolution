using App.Application.DTOs.Request;
using App.Application.DTOs.Response;

using App.Application.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace App.API.Controllers
{
    [EnableCors("AllowAll")]
    [Route("api/[controller]")]
    [ApiController]
    public class PoetryController : ControllerBase
    {
        private readonly IPoetryService _poetryService;

        public PoetryController(IPoetryService poetryService)
        {
            this._poetryService = poetryService;
        }


        // <summary>
        // GET api/<PoetryController>/5
        /// Gets all poems for a specific author
        /// </summary>
        /// <param name="authorId">The ID of the author</param>
        /// <returns>List of poems</returns>
        [HttpGet("{authorId}")]
        public async Task<ActionResult<List<PoemDto>>> GetPoems([FromQuery] int authorId)
        {
            var poems = await _poetryService.GetPoemDetailAsync(authorId);

            if (poems == null || !poems.Any())
            {
                return NotFound($"No poems found for author ID {authorId}.");
            }

            return Ok(poems);
        }

        // POST api/<PoetryController>
        [HttpPost]
        [Route("poetry")]
        public async Task<IActionResult> Save([FromBody] CreatePoemDto createPoemDto)
        {
            var userCreated = await _poetryService.SaveAsync(createPoemDto);

            return Ok(userCreated);
        }

        // PUT api/<PoetryController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Modify(int id, [FromBody] ModifyPoemDto modifyPoemDto)
        {
            var userModified = await _poetryService.UpdateAsync(modifyPoemDto);

            return Ok(userModified);
        }

        // DELETE api/<PoetryController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userDeleted = await _poetryService.DeleteAsync(id);

            return Ok(userDeleted);
        }
    }
}
