using HospitalManagement.Models;

namespace HospitalManagement.Service
{
    public interface IUserService
    {
        public Users LoginService(string username, string password);
    }
}
