using HospitalManagement.Models;

namespace HospitalManagement.Repository
{
    public interface IUserRepository
    {
        public Users Login(string username, string password);
    }
}
