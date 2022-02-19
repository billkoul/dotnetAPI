using MediatR;
using API.Application.Dto;
using System.Collections.Generic;

namespace API.Application.Queries
{
	public class FindUpload : IRequest<List<UploadDto>>
	{
		public int Id { get; set; }
    }
}
