using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
    public class Review
    {
        [Key]
        public int ReviewID { get; set; }

        [Required(ErrorMessage = "Comment is required.")]
        [StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters.")]
        public string Comment { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [ForeignKey("Service")]
        [Required(ErrorMessage = "Service is required.")]
        public int ServiceID { get; set; }
        public Service Service { get; set; }

        [ForeignKey("User")]
        [Required(ErrorMessage = "User is required.")]
        public int UserID { get; set; }
        public User User { get; set; }
    }
}
