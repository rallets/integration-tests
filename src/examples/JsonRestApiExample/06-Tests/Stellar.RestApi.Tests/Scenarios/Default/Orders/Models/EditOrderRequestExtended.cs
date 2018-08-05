using Stellar.RestApi.Example.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stellar.RestApi.Tests.Scenarios.Default.Clients.Models
{
    public class EditOrderRequestExtended : EditOrderRequest
    {
        public Guid Id { get; set; }
    }
}
