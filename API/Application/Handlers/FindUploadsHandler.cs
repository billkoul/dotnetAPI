using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using API.Application.Dto;
using Domain.Repositories;
using Domain.Models;
using API.Application.Queries;
using MediatR;

namespace API.Application.Handlers
{
	public class FindUploadsHandler : IRequestHandler<FindUpload, List<UploadDto>>
	{
		private readonly IUploadRepository _repo;

		public FindUploadsHandler(IUploadRepository repo)
		{
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
		}

		public async Task<List<UploadDto>> Handle(FindUpload request, CancellationToken cancellationToken)
		{
			var obj = await _repo.Find<Upload>(request.Id);

			return MapToDto(obj);
		}

		private static List<UploadDto> MapToDto(IEnumerable<Upload> items)
        {
            return items.Select(e => new UploadDto
			{
                Id = e.Id, 
				FilePath = e.FilePath,
				FileType = e.FileType,
				OriginalName = e.OriginalName
            }).ToList();
        }

	}

}
