using HospitalManagement.Models;
using HospitalManagement.ViewModel;

namespace HospitalManagement.Repository
{
    public interface IUserRepository
    {
        public Users Login(string username, string password);
    }
}
