using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using static BlueModas.Application.Cart.GetOrder;

namespace BlueModas.Application.Cart
{
    public class GetCustomerInformation
    {
        private ISession _session;

        public GetCustomerInformation(ISession session)
        {
            _session = session;
        }

        public class Response
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

        public Response Do()
        {
            var stringObject = _session.GetString("customer-info");

            if (String.IsNullOrEmpty(stringObject))
                return null;

            var customerInformation = JsonConvert.DeserializeObject<CustomerInformation>(stringObject);

            return new Response
            {
                Name = customerInformation.Name,
                Email = customerInformation.Email,
                PhoneNumber = customerInformation.PhoneNumber,
                Address = customerInformation.Address,
                City = customerInformation.City,
                State = customerInformation.State,
                Neighborhood = customerInformation.Neighborhood,
                ZipCode = customerInformation.ZipCode
            };
        }
    }
}
