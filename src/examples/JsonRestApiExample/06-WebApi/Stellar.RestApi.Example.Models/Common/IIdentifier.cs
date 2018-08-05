using System;

namespace Stellar.RestApi.Example.Models.Common
{
    public interface IIdentifier
    {
        Guid Id { get; set; }
    }
}
