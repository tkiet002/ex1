using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreAPI.Models
{
    public class Book
    {
        public int BookId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }
        
        public int AuthorId { get; set; }
        
        public DateTime? PublishedDate { get; set; }
        
        [ForeignKey("AuthorId")]
        public virtual Author Author { get; set; }
    }
}