using System;
using System.Threading;
using System.Threading.Tasks;
using API.Application.Dto;
using Domain.Repositories;
using Domain.Models;
using API.Application.Queries;
using MediatR;

namespace API.Application.Handlers
{
    public class CreateUploadHandler : IRequestHandler<CreateUpload, int>
	{
		private readonly IUploadRepository _repo;

		public CreateUploadHandler(IUploadRepository repo)
		{
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
		}

		public async Task<int> Handle(CreateUpload request, CancellationToken cancellationToken)
		{
            var obj = request.Id > 0
                ? await _repo.Update<CreateUpload>(request, request.Id)
                : await _repo.Create<CreateUpload>(request);

			return obj;
        }

    }

}
