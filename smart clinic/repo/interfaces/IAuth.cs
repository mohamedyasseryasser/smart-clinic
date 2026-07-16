using smart_clinic.viewmodels.Authviewmodel;
using smart_clinic.viewmodels.General;

namespace smart_clinic.services.interfaces
{
    public interface IAuth
    {
        Task<ResponseStatus<string>> LoginAsync(LoginViewModel model);
        Task LogoutAsync();
        Task<ResponseStatus<string>> ChangePasswordAsync(ChangePasswordViewModel model);
    }
}
