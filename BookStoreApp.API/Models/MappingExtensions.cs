using BookStoreApp.API.Models.Author;
using BookStoreApp.API.Models.Book;

namespace BookStoreApp.API.Models
{
    public static class MappingExtensions
    {
        public static Data.Author MapToAuthor(this AuthorCreateDto authorDto)
        {
            return new Data.Author()
            {
                FirstName = authorDto.FirstName,
                LastName = authorDto.LastName,
                Bio = authorDto.Bio,
            };
        }

        public static Data.Author MapToAuthor(this AuthorUpdateDto authorUpdateDto)
        {
            return new Data.Author
            {
                Id = authorUpdateDto.Id,
                FirstName = authorUpdateDto.FirstName,
                LastName = authorUpdateDto.LastName,
                Bio = authorUpdateDto.Bio,
            };
        }

        public static AuthorReadOnlyDto MapToAuthorReadOnlyDto(this Data.Author author)
        {
            return new AuthorReadOnlyDto
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Bio = author.Bio,
            };

        }

        public static BookReadOnlyDto MapToBookReadOnlyDto(this Data.Book book)
        {
            return new BookReadOnlyDto
            {
                Id = book.Id,
                Isbn = book.Isbn,
                Price = book.Price,
                Summary = book.Summary,
                Title = book.Title,
                Year = book.Year,
                AuthorId = (int)book.AuthorId,
                AuthorName = $"{book.Author.FirstName} {book.Author.LastName}"
            };
        }

        public static Data.Book MapToBook(this BookCreateDto createBookDto)
        {
            return new Data.Book
            {
                AuthorId = createBookDto.AuthorId,
                Isbn = createBookDto.Isbn,
                Price = createBookDto.Price,
                Summary = createBookDto.Summary,
                Title = createBookDto.Title,
                Year = createBookDto.Year,
            };
        }

        public static Data.Book MapToBook(this BookUpdateDto updateBookDto)
        {
            return new Data.Book
            {
                AuthorId = updateBookDto.AuthorId,
                Id = updateBookDto.Id,
                Isbn = updateBookDto.Isbn,
                Price = updateBookDto.Price,
                Summary = updateBookDto.Summary,
                Title = updateBookDto.Title,
                Year = updateBookDto.Year
            };
        }
    }
}
