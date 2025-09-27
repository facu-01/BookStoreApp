using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models.Book
{
    public class BookUpdateDto : BaseDto
    {
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        public int? Year { get; set; }
        [Required]
        [StringLength(50)]
        public string? Isbn { get; set; }

        [StringLength(250)]
        public string? Summary { get; set; }
        public string? ImageBase64 { get; set; }
        public string? ImageOringinalName { get; set; }
        [Required]
        public decimal? Price { get; set; }
        [Required]
        public int AuthorId { get; set; }
    }
}
