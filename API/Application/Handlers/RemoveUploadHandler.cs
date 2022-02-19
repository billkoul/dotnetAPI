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
	public class RemoveUploadHandler : IRequestHandler<RemoveUpload, string>
	{
		private readonly IUploadRepository _repo;

		public RemoveUploadHandler(IUploadRepository repo)
		{
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
		}

		public async Task<string> Handle(RemoveUpload request, CancellationToken cancellationToken)
		{
			var obj = await _repo.Remove(request.Id);

            return obj;
        }
    }

}
