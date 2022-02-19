namespace API.Config
{
	public class CustomErrorCodes
	{
        public string InvalidVehicle { get; set; }
        public string InvalidDates { get; set; }
        public string InvalidJson { get; set; }
        public string InvalidRequest { get; set; }

        public CustomErrorCodes()
        {
            InvalidVehicle = @"ERR01";
            InvalidDates = @"ERR02";
            InvalidJson = @"ERR03";
            InvalidRequest = @"ERR04";
        }
    }
}
