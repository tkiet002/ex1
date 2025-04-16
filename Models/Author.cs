using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(500)]
        public string Bio { get; set; }
        
        public virtual ICollection<Book> Books { get; set; }
    }
}