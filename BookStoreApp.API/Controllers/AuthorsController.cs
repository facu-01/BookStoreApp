using BookStoreApp.API.Data;
using BookStoreApp.API.Models;
using BookStoreApp.API.Models.Author;
using BookStoreApp.API.Models.Book;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly BookStoreDbContext _context;

        public AuthorsController(BookStoreDbContext context)
        {
            _context = context;
        }

        // GET: api/Authors
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AuthorReadOnlyDto>>> GetAuthors()
        {
            return await _context.Authors.Select(e => e.MapToAuthorReadOnlyDto()).ToListAsync();
        }

        // GET: api/Authors/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<AuthorReadOnlyDto>> GetAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return author.MapToAuthorReadOnlyDto();
        }

        [HttpGet("{id}/books")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BookReadOnlyDto>>> GetAuthorBooks(int id)
        {
            var author = await _context.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == id);

            if (author == null)
            {
                return NotFound();
            }

            return author.Books.Select(b => b.MapToBookReadOnlyDto()).ToList();
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutAuthor(int id, AuthorUpdateDto authorUpdateDto)
        {
            if (id != authorUpdateDto.Id)
            {
                return BadRequest();
            }

            var author = authorUpdateDto.MapToAuthor();

            _context.Entry(author).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthorReadOnlyDto>> PostAuthor(AuthorCreateDto authorDto)
        {
            var author = authorDto.MapToAuthor();
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAuthor", new { id = author.Id }, author.MapToAuthorReadOnlyDto());
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AuthorExists(int id)
        {
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}
