using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template_UI.Models;

namespace Template_UI.Contracts
{
    public interface IAuthenticationRepository
    {
        public Task<bool> Registration(RegistrationModel user);
        public Task<bool> Login(LoginModel user);
        public Task Logout();
    }
}
