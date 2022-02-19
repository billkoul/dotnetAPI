using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GSEAPI.Config.User
{
    public class UserService : IUserService
    {
        public bool ValidateCredentials(string username, string password)
        {
            return username.Equals("eco") && password.Equals("ecoserve");
        }
    }
}
