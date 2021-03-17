using BlueModas.Domain.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlueModas.Application.Cart
{
    public class AddCustomerInformation
    {
        private ISession _session;

        public AddCustomerInformation(ISession session)
        {
            _session = session;
        }

        public class Request
        {
            [Required]
            public string Name { get; set; }
            [Required]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }
            [Required]
            [DataType(DataType.PhoneNumber)]
            public string PhoneNumber { get; set; }

            [Required]
            public string Address { get; set; }
            [Required]
            public string City { get; set; }
            [Required]
            public string State { get; set; }
            [Required]
            public string Neighborhood { get; set; }
            [Required]
            public string ZipCode { get; set; }
        }

        public void Do(Request request)
        {
            var customerInformation = new CustomerInformation
            {
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                City = request.City,
                State = request.State,
                Neighborhood = request.Neighborhood,
                ZipCode = request.ZipCode,
            };

            var stringObject = JsonConvert.SerializeObject(customerInformation);

            _session.SetString("customer-info", stringObject);
        }
    }
}
