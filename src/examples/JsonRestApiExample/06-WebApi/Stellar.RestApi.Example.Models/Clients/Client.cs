using System;
using System.Collections.Generic;
using Stellar.RestApi.Example.Models.Common;

namespace Stellar.RestApi.Example.Models
{
    public abstract class ClientBase
    {
        public string Name { get; set; }

        public Country Country { get; set; }
    }

    public class Client : ClientBase, IIdentifier
    {
        public Guid Id { get ; set ; }
    }

    public class GetClientRequest
    {
        public Guid Id { get; set; }
    }

    public class GetClientResponse
    {
        public Client Result { get; set; }
    }

    public class GetClientsResponse
    {
        public List<Client> Result { get; set; }
    }

    public class AddClientRequest: ClientBase
    {
    }

    public class AddClientResponse
    {
        public Client Result { get; set; }
    }

    public class EditClientRequest: ClientBase
    {
    }

    public class EditClientResponse
    {
        public Client Result { get; set; }
    }
}
