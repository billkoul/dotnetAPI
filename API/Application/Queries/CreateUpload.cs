using MediatR;

namespace API.Application.Queries
{
	public class CreateUpload : IRequest<int>
	{
        public int Id { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public string OriginalName { get; set; }
    }
}
