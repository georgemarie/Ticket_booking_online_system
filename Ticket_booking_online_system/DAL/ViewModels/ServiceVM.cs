using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.ViewModels
{
    public class ServiceVM
    {
        public int ServiceID { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }

        public decimal Price { get; set; }
    }
}
