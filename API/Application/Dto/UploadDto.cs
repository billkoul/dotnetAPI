namespace API.Application.Dto
{
    public class UploadDto
    {
        public int Id { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public string OriginalName { get; set; }
    }
}