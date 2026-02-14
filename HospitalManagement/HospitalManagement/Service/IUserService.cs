using HospitalManagement.Models;
using HospitalManagement.ViewModel;

namespace HospitalManagement.Service
{
    public interface IUserService
    {
        public Users LoginService(string username, string password);
    }
}
