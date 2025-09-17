using BookStoreApp.API.Models.Author;
using BookStoreApp.API.Models.Book;

namespace BookStoreApp.API.Models
{
    public static class MappingExtensions
    {
        public static Data.Author MapToAuthor(this AuthorCreateDto authorDto)
        {
            var author = new Data.Author()
            {
                FirstName = authorDto.FirstName,
                LastName = authorDto.LastName,
                Bio = authorDto.Bio,
            };

            return author;

        }

        public static Data.Author MapToAuthor(this AuthorUpdateDto authorUpdateDto)
        {
            var author = new Data.Author()
            {
                Id = authorUpdateDto.Id,
                FirstName = authorUpdateDto.FirstName,
                LastName = authorUpdateDto.LastName,
                Bio = authorUpdateDto.Bio,
            };

            return author;

        }

        public static AuthorReadOnlyDto MapToAuthorReadOnlyDto(this Data.Author author)
        {
            var authorReadOnlyDto = new AuthorReadOnlyDto
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName,
                Bio = author.Bio,
            };

            return authorReadOnlyDto;

        }

        public static BookDto MapToBookDto(this Data.Book book)
        {
            var bookDto = new BookDto
            {
                Id = book.Id,
                Image = book.Image,
                Isbn = book.Isbn,
                Price = book.Price,
                Summary = book.Summary,
                Title = book.Title,
                Year = book.Year,
            };

            return bookDto;
        }



    }
}
