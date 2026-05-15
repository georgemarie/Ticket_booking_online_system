using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
    public class Service
    {
        [Key]
        public int ServiceID { get; set; }

        [Required(ErrorMessage = "Base Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Base Price must be greater than zero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BasePrice { get; set; }
        public string ServiceType { get; set; }

        [ForeignKey("Location")]
        [Required(ErrorMessage = "Location is required.")]
        public int LocationID { get; set; }
        [BindNever]
        public Location? Location { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }

}