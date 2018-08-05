using Stellar.RestApi.Example.Models;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Stellar.RestApi.Example.Controllers
{
    [RoutePrefix("v1/orders")]
    public class OrderController : ApiController
    {
        private IRepository _repository;

        public OrderController(
            IRepository repository
            )
        {
            _repository = repository;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            var result = await _repository.GetAll<Order>();

            var response = new GetOrdersResponse { Result = result };

            return Ok(response);
        }

        [Route("{OrderId}")]
        [HttpGet]
        public async Task<IHttpActionResult> Get([FromUri] Guid OrderId)
        {
            try
            {
                var result = await _repository.Get<Order>(OrderId);
                if (result == null)
                {
                    return NotFound();
                }

                var response = new GetOrderResponse { Result = result };

                return Ok(response);
            }
            catch (Exception)
            {
                // TODO: Log exception
            }
            return InternalServerError();
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> Add(AddOrderRequest request)
        {
            try
            {
                if(request.Amount <= 0)
                {
                    return BadRequest();
                }

                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    Description = request.Description,
                    ClientId = request.ClientId,
                    Amount = request.Amount
                };

                var id = await _repository.Add(order);
                if (id == Guid.Empty)
                {
                    return BadRequest();
                }

                order = await _repository.Get<Order>(id);

                var response = new AddOrderResponse
                {
                    Result = order
                };

                return Ok(response);
            }
            catch (Exception)
            {
                // TODO: Log exception
            }
            return InternalServerError();
        }

        [Route("{OrderId}")]
        [HttpPut]
        public async Task<IHttpActionResult> Edit([FromUri] Guid OrderId, [FromBody] EditOrderRequest request)
        {
            try
            {
                if (request.Amount <= 0)
                {
                    return BadRequest();
                }

                var order = await _repository.Get<Order>(OrderId);
                if (order == null)
                {
                    return NotFound();
                }

                order.ClientId = request.ClientId;
                order.Description = request.Description;
                order.Amount = request.Amount;

                var rows = await _repository.Save(order);
                if (rows == 0)
                {
                    return BadRequest();
                }

                order = await _repository.Get<Order>(order.Id);

                var response = new EditOrderResponse
                {
                    Result = order
                };

                return Ok(response);
            }
            catch (Exception)
            {
                // TODO: Log exception
            }
            return InternalServerError();
        }

        [Route("{OrderId}")]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete([FromUri] Guid OrderId)
        {
            try
            {
                var order = await _repository.Get<Order>(OrderId);
                if (order == null)
                {
                    return NotFound();
                }

                var rows = await _repository.Remove<Order>(order.Id);
                if (rows == 0)
                {
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception)
            {
                // TODO: Log exception
            }
            return InternalServerError();
        }
    }
}