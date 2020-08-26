using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Template_API.Contracts;
using Template_API.Data;
using Template_API.DTOs;

namespace Template_API.Controllers
{
    /// <summary>
    /// Endpoint used to interact with the Autors in the book store's database
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class AuthorsController : ControllerBase
    {
        private readonly ILoggerService _logger;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        public AuthorsController(
            ILoggerService logger, 
            IAuthorRepository authorRepository, 
            IMapper mapper
            )
        {
            _logger = logger;
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        private string GetControllerActionNames()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;
            return $"{controller} - {action}";
        }

        private ObjectResult InternalError(string message)
        {
            _logger.LogError($"{message}");
            return StatusCode(500, "Something went wrong. Please contact the Administrator");
        }

        /// <summary>
        /// Get all authors
        /// </summary>
        /// <returns>List of Authors</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthors()
        {
            string location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempted call");
                IList<Author> authors = await _authorRepository.FindAll();
                var response = _mapper.Map<IList<AuthorDTO>>(authors);
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Get an author by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Author record</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthor(int id)
        {
            string location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempted call for id: {id}");
                var author = await _authorRepository.FindById(id);

                if (author == null)
                {
                    _logger.LogWarn($"Record with id: {id} was not found");
                    return NotFound();
                }

                var response = _mapper.Map<AuthorDTO>(author);
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Create an author
        /// </summary>
        /// <param name="authorDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorCreateDTO authorDTO)
        {
            string location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempted call");

                if (authorDTO == null)
                {
                    _logger.LogWarn($"{location}: Empty request was submitted");
                    return BadRequest(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"{location}: Data was incomplite");
                    return BadRequest(ModelState);
                }

                var author = _mapper.Map<Author>(authorDTO);
                bool isSuccess = await _authorRepository.Create(author);

                if (!isSuccess)
                {
                    return InternalError($"{location}: Failed to create record");
                }

                return Created("Create", new { author });
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Update an author
        /// </summary>
        /// <param name="id"></param>
        /// <param name="authorDTO"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] AuthorUpdateDTO authorDTO)
        {
            string location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempted call for id:");

                if (id < 1 || authorDTO == null || id != authorDTO.Id)
                {
                    _logger.LogWarn($"{location}: Empty request was submitted");
                    return BadRequest(ModelState);
                }

                var isExists = await _authorRepository.IsExists(id);

                if (!isExists)
                {
                    _logger.LogWarn($"{location}: Record with id: {id} was not found");
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"{location}: Data was incomplite");
                    return BadRequest(ModelState);
                }

                var author = _mapper.Map<Author>(authorDTO);
                bool isSuccess = await _authorRepository.Update(author);

                if (!isSuccess)
                {
                    return InternalError($"{location}: Failed to update record");
                }

                return NoContent();
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Delete an author
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            string location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempted call");

                if (id < 1)
                {
                    _logger.LogWarn($"{location}: Empty request was submitted");
                    return BadRequest(ModelState);
                }

                var isExists = await _authorRepository.IsExists(id);

                if (!isExists)
                {
                    _logger.LogWarn($"{location}: Record with id: {id} was not found");
                    return NotFound();
                }

                var author = await _authorRepository.FindById(id);
                bool isSuccess = await _authorRepository.Delete(author);

                if (!isSuccess)
                {
                    return InternalError($"{location}: Failed to delete record");
                }

                return NoContent();
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }
    }
}
