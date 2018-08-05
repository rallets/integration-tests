using Stellar.RestApi.Example.Models;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Stellar.RestApi.Example.Controllers
{
    [RoutePrefix("v1/clients")]
    public class ClientController : ApiController
    {
        private IRepository _repository;

        public ClientController(
            IRepository repository
            )
        {
            _repository = repository;
        }

        [Route("")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            var result = await _repository.GetAll<Client>();

            var response = new GetClientsResponse { Result = result };

            return Ok(response);
        }

        [Route("{ClientId}")]
        [HttpGet]
        public async Task<IHttpActionResult> Get([FromUri] Guid ClientId)
        {
            try
            {
                var result = await _repository.Get<Client>(ClientId);
                if (result == null)
                {
                    return NotFound();
                }

                var response = new GetClientResponse { Result = result };

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
        public async Task<IHttpActionResult> Add(AddClientRequest request)
        {
            try
            {
                var client = new Client
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Country = request.Country
                };

                var id = await _repository.Add(client);
                if (id == Guid.Empty)
                {
                    return BadRequest();
                }

                client = await _repository.Get<Client>(id);

                var response = new AddClientResponse
                {
                    Result = client
                };

                return Ok(response);
            }
            catch (Exception)
            {
                // TODO: Log exception
            }
            return InternalServerError();
        }

        [Route("{ClientId}")]
        [HttpPut]
        public async Task<IHttpActionResult> Edit([FromUri] Guid ClientId, [FromBody] EditClientRequest request)
        {
            try
            {
                var client = await _repository.Get<Client>(ClientId);
                if (client == null)
                {
                    return NotFound();
                }

                client.Name = request.Name;
                client.Country = request.Country;

                var rows = await _repository.Save(client);
                if (rows == 0)
                {
                    return BadRequest();
                }

                client = await _repository.Get<Client>(client.Id);

                var response = new EditClientResponse
                {
                    Result = client
                };

                return Ok(response);
            }
            catch (Exception)
            {
                // TODO: Log exception
            }
            return InternalServerError();
        }

        [Route("{ClientId}")]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete([FromUri] Guid ClientId)
        {
            try
            {
                var client = await _repository.Get<Client>(ClientId);
                if (client == null)
                {
                    return NotFound();
                }

                var rows = await _repository.Remove<Client>(client.Id);
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