namespace API.Config
{
	public class CustomErrorCodes
	{
        public string InvalidJson { get; set; }
        public string InvalidRequest { get; set; }

        public CustomErrorCodes()
        {
            InvalidJson = @"ERR01";
            InvalidRequest = @"ERR02";
        }
    }
}
