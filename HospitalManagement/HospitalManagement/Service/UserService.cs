using HospitalManagement.Models;
using HospitalManagement.Repository;

namespace HospitalManagement.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }
        public Users LoginService(string username, string password)
        {
            return _repo.Login(username, password);
        }
    }
}
