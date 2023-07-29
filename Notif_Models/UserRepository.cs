using System;
using System.Collections.Generic;
using System.Linq;

namespace Notif_Models
{
    public class UserRepository
    {
        public List<User> _users;

        public UserRepository()
        {
            _users = new List<User>
            {
                
            };
        }

        public User GetUserByName(string name)
        {
           
#pragma warning disable CS8603 
            return _users.FirstOrDefault(u => u.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
#pragma warning restore CS8603 
        }
    }
}
