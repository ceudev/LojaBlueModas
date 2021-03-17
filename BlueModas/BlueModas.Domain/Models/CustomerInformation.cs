using System;
using System.Collections.Generic;
using System.Text;

namespace BlueModas.Domain.Models
{
    public class CustomerInformation
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }


        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Neighborhood { get; set; }
        public string ZipCode { get; set; }
    }
}
