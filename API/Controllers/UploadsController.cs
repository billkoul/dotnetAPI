using System.Collections.Generic;
using System.Threading.Tasks;
using API.Application.Dto;
using API.Config;
using API.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
	[ApiController]

	public class UploadsController : ControllerBase
	{
		private readonly IMediator _mediator;
		private readonly CustomErrorCodes _ce = new CustomErrorCodes();
		private readonly IFileUpdateSender fileUpdateSender;

		public UploadsController(IMediator mediator)
		{
			_mediator = mediator;
		}

        /// <summary>
        /// Returns all file upload entries
        /// </summary>
		//[ServiceFilter(typeof(ClientIpCheckActionFilter))]
		[HttpGet("{id}")]
        [Produces("application/json", Type = typeof(List<UploadDto>))]
		public async Task<IActionResult> Get(int id)
        {
            var query = new FindUpload {  };

			var result = await _mediator.Send(query);

			return Ok(result);
		}

        /// <summary>
        /// Creates file upload entry
        /// </summary>
		//[ServiceFilter(typeof(ClientIpCheckActionFilter))]
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] CreateUpload query)
		{
			//send new file details to RabbitMQ queue 
			await Task.Run(() => fileUpdateSender.SendFile(query)).ConfigureAwait(false);

			var result = await _mediator.Send(query);

            return Ok(result);
		}

		/// <summary>
		/// Removes  file upload entry
		/// </summary>
		[HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {

            if (id == 0)
                return Ok(_ce.InvalidRequest);

            var query = new RemoveUpload { Id = id };

            var result = await _mediator.Send(query);

            return Ok(result);
        }
	}
}
