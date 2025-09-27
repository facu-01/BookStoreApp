using BookStoreApp.API.Data;
using BookStoreApp.API.Models;
using BookStoreApp.API.Models.Book;
using BookStoreApp.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly BookStoreDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BooksController(BookStoreDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: api/Books
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BookReadOnlyDto>>> GetBooks()
        {
            var books = await _context.Books.Include(q => q.Author)
                .ToListAsync();

            var booksDto = books.Select(b =>
            {
                var bookDto = b.MapToBookReadOnlyDto();
                bookDto.ImageUrl = GetCoverBookImgUrl(b.Image);
                return bookDto;
            }).ToList();

            return booksDto;
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<BookReadOnlyDto>> GetBook(int id)
        {
            var book = await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            var bookDto = book.MapToBookReadOnlyDto();
            bookDto.ImageUrl = GetCoverBookImgUrl(book.Image);

            return bookDto;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutBook(int id, BookUpdateDto bookUpdateDto)
        {
            if (id != bookUpdateDto.Id)
            {
                return BadRequest();
            }

            var book = bookUpdateDto.MapToBook();

            var entry = _context.Entry(book);
            entry.State = EntityState.Modified;

            if (bookUpdateDto.ImageBase64 != null && bookUpdateDto.ImageOringinalName != null)
            {

                DeleteCoverBookImg(book.Image);
                book.Image = await SaveCoverBookImg(bookUpdateDto.ImageBase64, bookUpdateDto.ImageOringinalName);
            }
            else
            {
                entry.Property(b => b.Image).IsModified = false;
            }


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
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

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BookReadOnlyDto>> PostBook(BookCreateDto bookDto)
        {
            var book = bookDto.MapToBook();

            book.Image = await SaveCoverBookImg(bookDto.ImageBase64, bookDto.ImageOringinalName);
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var author = await _context.Authors.FindAsync(book.AuthorId);

            book.Author = author;

            return CreatedAtAction("GetBook", new { id = book.Id }, book.MapToBookReadOnlyDto());
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            DeleteCoverBookImg(book.Image);

            _context.Books.Remove(book);

            await _context.SaveChangesAsync();

            return NoContent();
        }


        private async Task<string> SaveCoverBookImg(string imgBase64, string imgFileName)
        {

            var fileExtension = Path.GetExtension(imgFileName);
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "booksCoverImgs", fileName);
            byte[] image = Convert.FromBase64String(imgBase64);
            await System.IO.File.WriteAllBytesAsync(filePath, image);

            return fileName;
        }

        private void DeleteCoverBookImg(string? imgFileName)
        {
            if (imgFileName == null) return;
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "booksCoverImgs", imgFileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

        }

        private string? GetCoverBookImgUrl(string? imgFilename)
        {
            if (string.IsNullOrEmpty(imgFilename)) return null;
            return $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/booksCoverImgs/{imgFilename}";
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
