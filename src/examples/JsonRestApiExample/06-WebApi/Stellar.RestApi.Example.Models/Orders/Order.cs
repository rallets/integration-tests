using System;
using System.Collections.Generic;
using Stellar.RestApi.Example.Models.Common;

namespace Stellar.RestApi.Example.Models
{
    public abstract class OrderBase
    {
        public Guid ClientId { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }
    }

    public class Order : OrderBase, IIdentifier
    {
        public Guid Id { get ; set ; }
    }

    public class GetOrderRequest
    {
        public Guid Id { get; set; }
    }

    public class GetOrderResponse
    {
        public Order Result { get; set; }
    }

    public class GetOrdersResponse
    {
        public List<Order> Result { get; set; }
    }

    public class AddOrderRequest : OrderBase
    {
        
    }

    public class AddOrderResponse
    {
        public Order Result { get; set; }
    }

    public class EditOrderRequest: OrderBase
    {
    }

    public class EditOrderResponse
    {
        public Order Result { get; set; }
    }
}
