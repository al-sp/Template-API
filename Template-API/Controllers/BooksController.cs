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
    /// Interacts with the Books
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public BooksController(
            IBookRepository bookRepository,
            ILoggerService logger,
            IMapper mapper
            )
        {
            _bookRepository = bookRepository;
            _logger = logger;
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
        /// Get all books
        /// </summary>
        /// <returns>A List of books</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooks()
        {
            string location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempted call");
                IList<Book> books = await _bookRepository.FindAll();
                var response = _mapper.Map<IList<BookDTO>>(books);
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Get an book by ID
        /// </summary>
        /// <param name="id">Book ID</param>
        /// <returns>A record book</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBook(int id)
        {
            string location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempted Call for id: {id}");

                Book book = await _bookRepository.FindById(id);

                if (book == null)
                {
                    _logger.LogWarn($"{location}: Record with id: {id} was not found");
                    return NotFound();
                }

                var response = _mapper.Map<BookDTO>(book);
                return Ok(response);
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Create an book
        /// </summary>
        /// <param name="bookDTO">Book</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateBook([FromBody] BookCreateDTO bookDTO)
        {
            string location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempted call");

                if (bookDTO == null)
                {
                    _logger.LogWarn($"{location}: Empty Request was submitted");
                    return BadRequest(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogWarn($"{location}: Record Data was incomplite");
                    return BadRequest(ModelState);
                }

                var book = _mapper.Map<Book>(bookDTO);
                var isSuccess = await _bookRepository.Create(book);

                if (!isSuccess)
                {
                    return InternalError($"{location}: Failed to create record");
                }

                return Created("Create", new { book });
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// Update an book
        /// </summary>
        /// <param name="id">book id</param>
        /// <param name="bookDTO">book</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookUpdateDTO bookDTO)
        {
            string location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempted call for id:");

                if (id < 1 || bookDTO == null || id != bookDTO.Id)
                {
                    _logger.LogWarn($"{location}: Empty Request was submitted");
                    return BadRequest(ModelState);
                }

                var isExists = await _bookRepository.IsExists(id);

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

                var book = _mapper.Map<Book>(bookDTO);
                var isSuccess = await _bookRepository.Update(book);

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
        /// Delete an book
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBook(int id)
        {
            string location = GetControllerActionNames();
            try
            {
                _logger.LogInfo($"{location}: Attempted call");

                if (id < 1)
                {
                    _logger.LogWarn($"{location}: Empty Request was submitted");
                    return BadRequest(ModelState);
                }

                bool isExists = await _bookRepository.IsExists(id);

                if (!isExists)
                {
                    _logger.LogWarn($"{location}: Record with id: {id} was not found");
                    return NotFound();
                }

                var book = await _bookRepository.FindById(id);
                bool isSuccess = await _bookRepository.Delete(book);

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
