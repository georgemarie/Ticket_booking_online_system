using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Models
{
    public enum Country
    {
        Egypt,
        Spain,
        France,
        Germany,
        Italy,
        Switzerland,
        UK
    }
    public class Location
    {
        [Key]
        public int LocationID { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(100, ErrorMessage = "City cannot exceed 100 characters.")]
        [EnumDataType(typeof(Country))]
        public string City { get; set; }


        [Required(ErrorMessage = "Country is required.")]
        
        public string Country { get; set; }

        public ICollection<Service> Services { get; set; }
    }
}
