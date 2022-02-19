using MediatR;

namespace API.Application.Queries
{
	public class RemoveUpload : IRequest<string>
	{
		public int Id { get; set; }
    }
}
