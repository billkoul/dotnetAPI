using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GSEAPI.Config.User
{
    public class UserService : IUserService
    {
        //Demo user authentication! This is not a real login mechanism, only for demonstration!
        public bool ValidateCredentials(string username, string password)
        {
            return username.Equals("demo") && password.Equals("demo");
        }
    }
}
