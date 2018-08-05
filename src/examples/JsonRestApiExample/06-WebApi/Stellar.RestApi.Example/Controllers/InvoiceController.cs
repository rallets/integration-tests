using Stellar.RestApi.Example.Models;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Stellar.RestApi.Example.Controllers
{
    [RoutePrefix("v1/invoices")]
    public class InvoiceController : ApiController
    {
        private IRepository _repository;

        public InvoiceController(
            IRepository repository
            )
        {
            _repository = repository;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            var result = await _repository.GetAll<Invoice>();

            var response = new GetInvoicesResponse { Result = result };

            return Ok(response);
        }

        [Route("{InvoiceId}")]
        [HttpGet]
        public async Task<IHttpActionResult> Get([FromUri] Guid InvoiceId)
        {
            try
            {
                var result = await _repository.Get<Invoice>(InvoiceId);
                if (result == null)
                {
                    return NotFound();
                }

                var response = new GetInvoiceResponse { Result = result };

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
        public async Task<IHttpActionResult> Add(AddInvoiceRequest request)
        {
            try
            {
                if (request.Amount <= 0)
                {
                    return BadRequest();
                }

                var invoice = new Invoice
                {
                    Id = Guid.NewGuid(),
                    ClientId = request.ClientId,
                    OrderId = request.OrderId,
                    Description = request.Description,
                    Amount = request.Amount
                };

                var id = await _repository.Add(invoice);
                if (id == Guid.Empty)
                {
                    return BadRequest();
                }

                invoice = await _repository.Get<Invoice>(id);

                var response = new AddInvoiceResponse
                {
                    Result = invoice
                };

                return Ok(response);
            }
            catch (Exception)
            {
                // TODO: Log exception
            }
            return InternalServerError();
        }

        [Route("{InvoiceId}")]
        [HttpPut]
        public async Task<IHttpActionResult> Edit([FromUri] Guid InvoiceId, [FromBody] EditInvoiceRequest request)
        {
            try
            {
                if (request.Amount <= 0)
                {
                    return BadRequest();
                }

                var invoice = await _repository.Get<Invoice>(InvoiceId);
                if (invoice == null)
                {
                    return NotFound();
                }

                invoice.ClientId = request.ClientId;
                invoice.OrderId = request.OrderId;
                invoice.Description = request.Description;
                invoice.Amount = request.Amount;

                var rows = await _repository.Save(invoice);
                if (rows == 0)
                {
                    return BadRequest();
                }

                invoice = await _repository.Get<Invoice>(invoice.Id);

                var response = new EditInvoiceResponse
                {
                    Result = invoice
                };

                return Ok(response);
            }
            catch (Exception)
            {
                // TODO: Log exception
            }
            return InternalServerError();
        }

        [Route("{InvoiceId}")]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete([FromUri] Guid InvoiceId)
        {
            try
            {
                var invoice = await _repository.Get<Invoice>(InvoiceId);
                if (invoice == null)
                {
                    return NotFound();
                }

                var rows = await _repository.Remove<Invoice>(invoice.Id);
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