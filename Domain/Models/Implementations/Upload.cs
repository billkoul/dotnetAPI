using System;

namespace Domain.Models
{
	public class Upload : IDto
    {
        public int Id { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public string OriginalName { get; set; }
    }
}
